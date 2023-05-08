import { Component, OnInit } from '@angular/core';
import { YearsByMonth } from 'src/app/models/devices.model';
import { Settlement } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { DevicesService } from 'src/app/services/devices.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { saveAs } from 'file-saver';
import { ExportToCsv } from 'export-to-csv';
import { forkJoin } from 'rxjs';
import {FormControl} from '@angular/forms';
import {MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS} from '@angular/material-moment-adapter';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';
import {MatDatepicker} from '@angular/material/datepicker';

import * as _moment from 'moment';
import {default as _rollupMoment, Moment} from 'moment';

const moment = _rollupMoment || _moment;

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
  selector: 'app-tabelar-view-by-year',
  templateUrl: './tabelar-view-by-year.component.html',
  styleUrls: ['./tabelar-view-by-year.component.css'],
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
export class TabelarViewByYearComponent implements OnInit{
  currentDate = new Date();
  maxYear = new Date(this.currentDate.getFullYear(), this.currentDate.getMonth()-1, 1);
  list1:YearsByMonth[]=[];
  list2:YearsByMonth[]=[];
  settlements:Settlement[] = [];
  mergedList: {month: string, year: number, consumption: number, production: number }[] = [];
  itemList: string[] = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Avg','Sep','Okt','Nov','Dec'];
  constructor(private deviceService:HistoryPredictionService,private authService:AuthService) {
    this.date.valueChanges.subscribe((selectedDate : any) => {
      const arr1: any[] = [];
    arr1.push(Object.values(selectedDate)[4]);
    this.selectedDate=arr1[0];
    this.ngOnInit();
    });
  }
  selectedOption: number = 0;

  onOptionSelected() {
    this.ngOnInit();
  }

  date = new FormControl(moment());
  selectedDate : Date | undefined;
  setYear(year: Moment, datepicker: MatDatepicker<Moment>) {
    const ctrlValue = this.date.value!;
    ctrlValue.year(year.year());
    this.date.setValue(ctrlValue);
    datepicker.close();
  }

  ngOnInit(): void {this.authService.getlogInUser().subscribe(user=>{
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
      if(this.selectedOption == 0 && this.selectedDate == undefined){
        forkJoin([
          this.deviceService.yearByMonth(number, 2),
          this.deviceService.yearByMonth(number, 1)
        ]).subscribe(([list1, list2]) => {
          this.list1 = list1;
          this.list2 = list2;

        });
      }
      else if(this.selectedOption == 0 && this.selectedDate != undefined){
        const year = this.selectedDate!.getFullYear();
        forkJoin([
          this.deviceService.monthbyDayCityFilter(year,number, 2),
          this.deviceService.monthbyDayCityFilter(year,number, 1)
        ]).subscribe(([list1, list2]) => {
          this.list1 = list1;
          this.list2 = list2;

        });
      }
      else if(this.selectedOption != 0 && this.selectedDate != undefined){
        let year = this.selectedDate!.getFullYear();

        forkJoin([
          this.deviceService.monthbySettlementCityFilter(year, this.selectedOption,2),
          this.deviceService.monthbySettlementCityFilter(year, this.selectedOption,1)
        ]).subscribe(([list1, list2]) => {
          this.list1 = list1;
          this.list2 = list2;

        });
      }
      else{
        forkJoin([
          this.deviceService.yearByMonthSettlement(this.selectedOption, 2),
          this.deviceService.yearByMonthSettlement(this.selectedOption, 1)
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
  const options = {
    fieldSeparator: ',',
    filename: 'consumption/production-year',
    quoteStrings: '"',
    useBom : true,
    decimalSeparator: '.',
    showLabels: true,
    useTextFile: false,
    headers: ['Month', 'Year', 'Consumption [kWh]', 'Production [kWh]']
  };

  const csvExporter = new ExportToCsv(options);
  const csvData = csvExporter.generateCsv(this.mergedList);

  }
}
