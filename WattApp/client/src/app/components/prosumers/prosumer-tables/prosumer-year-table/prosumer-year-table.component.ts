import { Component } from '@angular/core';
import { FormControl } from '@angular/forms';
import { MatDatepicker } from '@angular/material/datepicker';
import { ActivatedRoute } from '@angular/router';
import { ExportToCsv } from 'export-to-csv';
import moment, { Moment } from 'moment';
import { forkJoin, switchMap } from 'rxjs';
import { YearsByMonth } from 'src/app/models/devices.model';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { JwtToken } from 'src/app/utilities/jwt-token';
import {MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS} from '@angular/material-moment-adapter';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';
export const MY_FORMATS = {
  parse: {
    dateInput: 'YYYY',
  },
  display: {
    dateInput: 'YYYY',
    monthYearLabel: 'YYYY',
    monthYearA11yLabel: 'YYYY',
  },
};
@Component({
  selector: 'app-prosumer-year-table',
  templateUrl: './prosumer-year-table.component.html',
  styleUrls: ['./prosumer-year-table.component.css'],
  providers: [
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS]
    },
    { 
     provide: MAT_DATE_FORMATS, useValue: MY_FORMATS
    },
   ]
})
export class ProsumerYearTableComponent {

  maxYear = new Date();
  list1:YearsByMonth[]=[];
  list2:YearsByMonth[]=[];
  mergedList: {month: string, year: number, consumption: number, production: number }[] = [];
  datePipe: any;
  constructor(private deviceService:HistoryPredictionService,private route:ActivatedRoute) {
    this.date.valueChanges.subscribe((selectedDate : any) => {
      const arr1: any[] = [];
    arr1.push(Object.values(selectedDate)[4]);
    this.selectedDate=arr1[0];
    this.ngOnInit();
    });
  }

  date = new FormControl(moment());
  selectedDate : Date | undefined;
  setYear(year: Moment, datepicker: MatDatepicker<Moment>) {
    const ctrlValue = this.date.value!;
    ctrlValue.year(year.year());
    this.date.setValue(ctrlValue);
    datepicker.close();
  }

  ngOnInit(): void {
    let token=new JwtToken();
    const userId = token.data.id as number;
  
    if(this.selectedDate == undefined){
      forkJoin([
        this.deviceService.yearByMonthUser(userId, 2),
        this.deviceService.yearByMonthUser(userId, 1)
      ]).subscribe(([list1, list2]) => {
        this.list1 = list1;
        this.list2 = list2;
      });
    }
    else{
      const year = this.selectedDate.getFullYear();
      forkJoin([
        this.deviceService.monthbyDayUserFilter(year,userId, 2),
        this.deviceService.monthbyDayUserFilter(year,userId, 1)
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
        if (this.list1[i].month === this.list2[j].month && this.list1[i].year === this.list2[j].year) {
          this.mergedList.push({
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
    filename: 'consumption/production-year',
    quoteStrings: '"',
    useBom : true,
    decimalSeparator: '.',
    showLabels: true,
    useTextFile: false,
    headers: ['Month', 'Year', 'Consumption [kWh]', 'Production [kWh]', 'Exported Date '+formattedDate]
  };

  const csvExporter = new ExportToCsv(options);
  const csvData = csvExporter.generateCsv(this.mergedList);

  }
}
