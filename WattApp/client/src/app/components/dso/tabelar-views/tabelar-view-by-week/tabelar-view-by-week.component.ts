import { Component, Injectable, OnInit } from '@angular/core';
import { WeekByDay } from 'src/app/models/devices.model';
import { Settlement } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { DevicesService } from 'src/app/services/devices.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { saveAs } from 'file-saver';
import { ExportToCsv } from 'export-to-csv';
import { forkJoin } from 'rxjs';
import {DateAdapter} from '@angular/material/core';
import {
  MatDateRangeSelectionStrategy,
  DateRange,
  MAT_DATE_RANGE_SELECTION_STRATEGY,
  MatDatepickerInputEvent,
} from '@angular/material/datepicker';
import { FormControl, FormGroup } from '@angular/forms';


@Injectable()
export class FiveDayRangeSelectionStrategy<D> implements MatDateRangeSelectionStrategy<D> {
  constructor(private _dateAdapter: DateAdapter<D>) {}

  selectionFinished(date: D | null): DateRange<D> {
    return this._createFiveDayRange(date);
  }

  createPreview(activeDate: D | null): DateRange<D> {
    return this._createFiveDayRange(activeDate);
  }

  private _createFiveDayRange(date: D | null): DateRange<D> {
    if (date) {
      const start = this._dateAdapter.addCalendarDays(date, -4);
      const end = this._dateAdapter.addCalendarDays(date, 3);
      return new DateRange<D>(start, end);
    }

    return new DateRange<D>(null, null);
  }
}

@Component({
  selector: 'app-tabelar-view-by-week',
  templateUrl: './tabelar-view-by-week.component.html',
  styleUrls: ['./tabelar-view-by-week.component.css'],
  providers: [
    {
      provide: MAT_DATE_RANGE_SELECTION_STRATEGY,
      useClass: FiveDayRangeSelectionStrategy,
    },
  ],
})
export class TabelarViewByWeekComponent implements OnInit {

  maxDate: Date;
  list1:WeekByDay[] = [];
  list2:WeekByDay[] = [];
  settlements:Settlement[] = [];
  mergedList: { day: number, month: string, year: number, consumption: number, production: number }[] = [];

  constructor(private deviceService:HistoryPredictionService,private authService:AuthService) {
    this.maxDate = new Date();
    this.campaignOne.valueChanges.subscribe((value) => {
      this.sdate = value.start;
      this.send = value.end;
      if(this.send > this.maxDate){
        this.send = null;
      }
      this.ngOnInit();
    });
  }
  selectedOption: number = 0;


  campaignOne: FormGroup = new FormGroup({
    start: new FormControl(),
    end: new FormControl()
  });

  sdate = this.campaignOne.value.start;
  send = this.campaignOne.value.end;

  onOptionSelected() {
    this.ngOnInit();
  }
    ngOnInit(): void {

      this.authService.getlogInUser().subscribe(user=>{
        this.authService.getCityId(user.city).subscribe(number=>{
          this.authService.getSettlement(number).subscribe((settlement:Settlement[])=>{
            this.settlements = settlement;
          })
          if(this.selectedOption == 0 || (this.sdate == null && this.send == null) || (this.sdate != null && this.send == null)){
            forkJoin([
              this.deviceService.weekByDay(number, 2),
              this.deviceService.weekByDay(number, 1)
            ]).subscribe(([list1, list2]) => {
              this.list1 = list1;
              this.list2 = list2;
            });
          }
          else if(this.selectedOption == 0 && this.sdate != null && this.send != null){
            const day1 = this.sdate.getDate();
            const month1 = this.sdate.getMonth()+1;
            const year1 = this.sdate.getFullYear();
            const day2 = this.send.getDate();
            const month2 = this.send.getMonth()+1;
            const year2 = this.send.getFullYear();
            let string1 = year1+'-'+month1+'-'+day1;
            let string2 = year2+'-'+month2+'-'+day2;
  
            forkJoin([
              this.deviceService.weekByDayCityFilter(string1,string2,number, 2),
              this.deviceService.weekByDayCityFilter(string1,string2,number, 1)
            ]).subscribe(([list1, list2]) => {
              this.list1 = list1;
              this.list2 = list2;
            });
          }
          else if(this.selectedOption != 0 && this.sdate != null && this.send != null){
            const day1 = this.sdate.getDate();
            const month1 = this.sdate.getMonth()+1;
            const year1 = this.sdate.getFullYear();
            const day2 = this.send.getDate();
            const month2 = this.send.getMonth()+1;
            const year2 = this.send.getFullYear();
            let string1 = year1+'-'+month1+'-'+day1;
            let string2 = year2+'-'+month2+'-'+day2;
  
            forkJoin([
              this.deviceService.weekByDaySettlementFilter(string1,string2,number, this.selectedOption),
              this.deviceService.weekByDaySettlementFilter(string1,string2,number, this.selectedOption)
            ]).subscribe(([list1, list2]) => {
              this.list1 = list1;
              this.list2 = list2;
            });
          }
          else{
            forkJoin([
              this.deviceService.weekByDaySettlement(this.selectedOption, 2),
              this.deviceService.weekByDaySettlement(this.selectedOption, 1)
            ]).subscribe(([list1, list2]) => {
              this.list1 = list1;
              this.list2 = list2;
            });
          }
        })
      })
      
  }
  downloadCSV(): void {
    this.mergedList = [];
    for (let i = 0; i < this.list1.length; i++) {
      for (let j = 0; j < this.list2.length; j++) {
        if (this.list1[i].day === this.list2[j].day && this.list1[i].month === this.list2[j].month && this.list1[i].year === this.list2[j].year) {
          this.mergedList.push({
            day: this.list1[i].day,
            month: this.list1[i].month,
            year: this.list1[i].year,
            consumption: this.list1[i].energyUsageResult,
            production: this.list2[j].energyUsageResult
          });
          break;
        }
      }
  }
  const options = {
    fieldSeparator: ',',
    filename: 'consumption/production-week.csv',
    quoteStrings: '"',
    useBom : true,
    decimalSeparator: '.',
    showLabels: true,
    useTextFile: false,
    headers: ['Day', 'Month', 'Year', 'Consumption', 'Production']
  };

  const csvExporter = new ExportToCsv(options);
  const csvData = csvExporter.generateCsv(this.mergedList);

  }
}