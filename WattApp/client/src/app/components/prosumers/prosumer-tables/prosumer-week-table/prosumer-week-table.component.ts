import { Component, Injectable, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ExportToCsv } from 'export-to-csv';
import { forkJoin } from 'rxjs';
import { WeekByDay } from 'src/app/models/devices.model';
import { AuthService } from 'src/app/services/auth.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import {DateAdapter} from '@angular/material/core';
import {
  MatDateRangeSelectionStrategy,
  DateRange,
  MAT_DATE_RANGE_SELECTION_STRATEGY,
  MatDatepickerInputEvent,
} from '@angular/material/datepicker';
import { FormControl, FormGroup } from '@angular/forms';
import { JwtToken } from 'src/app/utilities/jwt-token';


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
  selector: 'app-prosumer-week-table',
  templateUrl: './prosumer-week-table.component.html',
  styleUrls: ['./prosumer-week-table.component.css'],
  providers: [
    {
      provide: MAT_DATE_RANGE_SELECTION_STRATEGY,
      useClass: FiveDayRangeSelectionStrategy,
    },
  ],
})
export class ProsumerWeekTableComponent {
  maxDate:Date;
  list1:WeekByDay[] = [];
  list2:WeekByDay[] = [];
  mergedList: { day: number, month: string, year: number, consumption: number, production: number }[] = [];
  constructor(private deviceService:HistoryPredictionService,private authService:AuthService,private route:ActivatedRoute) {
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

  campaignOne: FormGroup = new FormGroup({
    start: new FormControl(),
    end: new FormControl()
  });

  sdate = this.campaignOne.value.start;
  send = this.campaignOne.value.end;

  ngOnInit(): void {
    let token=new JwtToken();
    const id = token.data.id as number;
  
    if((this.sdate == null && this.send == null) || (this.sdate != null && this.send == null)){
      forkJoin([
        this.deviceService.weekByDayUser(id, 2),
        this.deviceService.weekByDayUser(id, 1),
      ]).subscribe(([list1, list2]) => {
        this.list1 = list1;
        this.list2 = list2;
    });
    }
    else{
      const day1 = this.sdate.getDate();
          const month1 = this.sdate.getMonth()+1;
          const year1 = this.sdate.getFullYear();
          const day2 = this.send.getDate();
          const month2 = this.send.getMonth()+1;
          const year2 = this.send.getFullYear();
          let string1 = year1+'-'+month1+'-'+day1;
          let string2 = year2+'-'+month2+'-'+day2;

          forkJoin([
            this.deviceService.weekByDayUserFilter(string1,string2,id, 2),
            this.deviceService.weekByDayUserFilter(string1,string2,id, 1)
          ]).subscribe(([list1, list2]) => {
            this.list1 = list1;
            this.list2 = list2;
          });
    }
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