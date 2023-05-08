import { Component, ViewChild } from '@angular/core';
import { Chart,registerables } from 'node_modules/chart.js'
import { forkJoin, switchMap } from 'rxjs';
import { WeekByDay } from 'src/app/models/devices.model';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { MatDatepickerModule} from '@angular/material/datepicker';
import { FormControl, FormGroup } from '@angular/forms';
import {MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS} from '@angular/material-moment-adapter';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';
import {MatDatepicker} from '@angular/material/datepicker';
import * as _moment from 'moment';
import {default as _rollupMoment, Moment} from 'moment';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { ExportToCsv } from 'export-to-csv';

const moment = _rollupMoment || _moment;

export const MY_FORMATS = {
  parse: {
    dateInput: 'MM/YYYY',
  },
  display: {
    dateInput: 'MM/YYYY',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
};
@Component({
  selector: 'app-device-month-tabular',
  templateUrl: './device-month-tabular.component.html',
  styleUrls: ['./device-month-tabular.component.css'],
  providers: [
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS],
    },

    {provide: MAT_DATE_FORMATS, useValue: MY_FORMATS},
  ],
})
export class DeviceMonthTabularComponent {


  consumptionGraph:boolean = false;
  productionGraph:boolean = false;
  currentDate = new Date();
  maxYear = new Date(this.currentDate.getFullYear(), this.currentDate.getMonth()-1, 1);
  list1:WeekByDay[]=[];
  list2:WeekByDay[]=[];
  itemList: string[] = ['1','2','3','4','5','6','7','8','9','10','11','12','13','14','15','16','17','18','19'
  ,'20','21','22','23','24','25','26','27','28','29','30'];
  constructor(private deviceService:HistoryPredictionService,private route:ActivatedRoute,private authService:AuthService) {
    this.date.valueChanges.subscribe((selectedDate : any) => {
      const arr1: any[] = [];
    arr1.push(Object.values(selectedDate)[4]);
    this.selectedDate=arr1[0];
    this.ngOnInit();
    });
  }
  selectedDate : Date | undefined;
  date = new FormControl(moment());

  setMonthAndYear(normalizedMonthAndYear: Moment, datepicker: MatDatepicker<Moment>) {
    const ctrlValue = this.date.value!;
    ctrlValue.month(normalizedMonthAndYear.month());
    ctrlValue.year(normalizedMonthAndYear.year());
    this.date.setValue(ctrlValue);
    datepicker.close();

  }
  
  ngOnInit(): void {
    const deviceId = Number(this.route.snapshot.paramMap.get('id'));
    this.authService.getDevice(deviceId).subscribe(data=>{
    if(this.selectedDate == undefined){
      
        if(data.deviceCategory == "Electricity Consumer")
        {
          this.deviceService.monthbyDayDevice(deviceId).subscribe(consumption=>{
            this.list1 = consumption;
            this.consumptionGraph = true;

          })
          
        }
        else{
          this.deviceService.monthbyDayDevice(deviceId).subscribe(production=>{
            this.list2 = production;
            this.productionGraph = true;
          })
        }
      
    }
    else{
          const month = this.selectedDate!.getMonth()+1;
          const year = this.selectedDate!.getFullYear();
          let string1 = year+'-'+month+'-'+1;
          let string2 = year+'-'+(month+1)+'-'+1;
          if(month == 12){
            string2 = (year+1)+'-'+1+'-'+1
          }
          forkJoin([
            this.deviceService.weekByDayDeviceFilter(string1,string2,deviceId, 2),
            this.deviceService.weekByDayDeviceFilter(string1,string2,deviceId, 1)
          ]).subscribe(([list1, list2]) => {
            if(data.deviceCategory == "Electricity Consumer"){
              this.list1 = list1;
              this.consumptionGraph = true;
            }
            else{
              this.list2 = list2;
              this.productionGraph = true;

            }
          });
    }
  })
  }
  downloadCSV(): void {
    const deviceId = Number(this.route.snapshot.paramMap.get('id'));
    
    this.authService.getDevice(deviceId).subscribe(data=>{
      if(data.deviceCategory == "Electricity Consumer"){
          const options = {
          fieldSeparator: ',',
          filename: 'consumption-week',
          quoteStrings: '"',
          useBom : true,
          decimalSeparator: '.',
          showLabels: true,
          useTextFile: false,
          headers: ['Hour', 'Day', 'Month', 'Year', 'Consumption', 'Production']
        };
        const csvExporter = new ExportToCsv(options);
        const csvData = csvExporter.generateCsv(this.list1);
      }
      else{
          const options = {
          fieldSeparator: ',',
          filename: 'production-week',
          quoteStrings: '"',
          useBom : true,
          decimalSeparator: '.',
          showLabels: true,
          useTextFile: false,
          headers: ['Hour', 'Month', 'Year', 'Consumption', 'Production']
        };
        const csvExporter = new ExportToCsv(options);
        const csvData = csvExporter.generateCsv(this.list2);
      }
    })
    } 
}
