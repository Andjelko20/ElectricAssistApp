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
      const start = this._dateAdapter.addCalendarDays(date, 0);
      const end = this._dateAdapter.addCalendarDays(date, 7);
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

  currentDate = new Date();
  maxDate = new Date(this.currentDate.getFullYear(), this.currentDate.getMonth(),this.currentDate.getDate()-7);
  list1:WeekByDay[] = [];
  list2:WeekByDay[] = [];
  settlements:Settlement[] = [];
  mergedList: { day: number, month: string, year: number, consumption: number, production: number }[] = [];
  datePipe: any;

  constructor(private deviceService:HistoryPredictionService,private authService:AuthService) {
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
            const selectElement = document.getElementById('dropdown') as HTMLSelectElement
            const selectedOptionName = selectElement.options[selectElement.selectedIndex].text;

            if (selectedOptionName === 'Total') {
              this.selectedOption = 0;
            } else {
              const selectedItem = this.settlements.find(item => item.name === selectedOptionName);
              if (selectedItem) {
                this.selectedOption = selectedItem.id;
              }
            }
          })
          if(this.selectedOption == 0 && (this.sdate == null && this.send == null) || (this.sdate != null && this.send == null)){
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
            let dayString1 = String(day1).padStart(2, '0');
            let monthString1 = String(month1).padStart(2, '0');
            const year1 = this.sdate.getFullYear();
            const day2 = this.send.getDate();
            const month2 = this.send.getMonth()+1;
            let dayString2 = String(day2).padStart(2, '0');
            let monthString2 = String(month2).padStart(2, '0');
            const year2 = this.send.getFullYear();
            let string1 = year1+'-'+monthString1+'-'+dayString1+' '+'00:00:00';
            let string2 = year2+'-'+monthString2+'-'+dayString2+' '+'00:00:00';
  
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
            let dayString1 = String(day1).padStart(2, '0');
            let monthString1 = String(month1).padStart(2, '0');
            const year1 = this.sdate.getFullYear();
            const day2 = this.send.getDate();
            const month2 = this.send.getMonth()+1;
            let dayString2 = String(day2).padStart(2, '0');
            let monthString2 = String(month2).padStart(2, '0');
            const year2 = this.send.getFullYear();
            let string1 = year1+'-'+monthString1+'-'+dayString1+' '+'00:00:00';
            let string2 = year2+'-'+monthString2+'-'+dayString2+' '+'00:00:00';
  
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
  const date = new Date();
  const formattedDate = this.datePipe.transform(date,'dd-MM-yyyy hh:mm:ss');
  const options = {
    fieldSeparator: ',',
    filename: 'consumption/production-week',
    quoteStrings: '"',
    useBom : true,
    decimalSeparator: '.',
    showLabels: true,
    useTextFile: false,
    headers: ['Day', 'Month', 'Year', 'Consumption [kWh]', 'Production [kWh]', 'Exported Date '+formattedDate]
  };

  const csvExporter = new ExportToCsv(options);
  const csvData = csvExporter.generateCsv(this.mergedList);

  }
}