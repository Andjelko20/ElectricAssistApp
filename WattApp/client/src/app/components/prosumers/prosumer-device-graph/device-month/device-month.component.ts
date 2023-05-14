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
Chart.register(...registerables)

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
  selector: 'app-device-month',
  templateUrl: './device-month.component.html',
  styleUrls: ['./device-month.component.css'],
  providers: [
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS],
    },

    {provide: MAT_DATE_FORMATS, useValue: MY_FORMATS},
  ],
})
export class DeviceMonthComponent {

  currentDate = new Date();
  consumptionGraph:boolean = false;
  productionGraph:boolean = false;
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
  selectedDate : Date = new Date();
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
          let month = this.selectedDate!.getMonth()+1;
          let monthString = String(month).padStart(2, '0');
          let year = this.selectedDate!.getFullYear();
          let string1 = year+'-'+monthString+'-0'+1+' '+'00:00:00';
          monthString = String(month+1).padStart(2, '0');
          let string2 = year+'-'+monthString+'-0'+1+' '+'00:00:00';
          if(month == 12){
            string2 = (year+1)+'-0'+1+'-0'+1
          }
          forkJoin([
            this.deviceService.weekByDayDeviceFilter(string1,string2,deviceId, 2),
            this.deviceService.weekByDayDeviceFilter(string1,string2,deviceId, 1)
          ]).subscribe(([list1, list2]) => {
            if(data.deviceCategory == "Electricity Consumer"){
              this.list1 = list1;
              this.consumptionGraph = true;
              this.BarPlotConsumption();
            }
            else{
              this.list2 = list2;
              this.productionGraph = true;
              this.BarPlotProduction();
            }
          });
  })
  }
  BarPlotProduction(){
    
    const chartId = 'barplot1';
    const chartExists = Chart.getChart(chartId);
    if (chartExists) {
        chartExists.destroy();
    }


    const energyUsageResults2 = this.list2.map(day => day.energyUsageResult);
    const monthbyday = this.list2.map(day => day.day);

    const Linechart =new Chart("barplot1", {
        type: 'bar',
       
        data : {
          labels: monthbyday,
          
          datasets: [
            {
              label: 'Production',
              data: energyUsageResults2,
              borderColor: '#1d91c0',
              backgroundColor: '#1d91c0'
            },
           
            
          ]
          
        },
        options: 
        {
          maintainAspectRatio: false,
          responsive: true, // Enable responsiveness
          
          scales:{
            y: {
              ticks:{
                color:'#000',
                font:{
                  size:13
                }
              },
              position: "left",
              title:{
                display:true,
                text: "Production (kWh)",
                color: '#000',
                font:{
                  size:13
                }
              }
            }
            ,
            x:{
              ticks:{
                color:'#000',
                font:{
                  size:13
                }
                
              },
              title:{
                display:true,
                text: "Days in a month",
                color: '#000',
                font:{
                  size:13
                }
              }
            }
          },
         
          plugins: {
            datalabels: {
              display: false
            },
            legend:{
              display:false
            },
            
            title: {
              display: true,
              text: 'Production in a month',
              color: '#000',
              font:{
                size:15
              }
            }
          }
        }
      });
  }
  BarPlotConsumption(){
    
    const chartId = 'barplot2';
    const chartExists = Chart.getChart(chartId);
    if (chartExists) {
        chartExists.destroy();
    }

    const energyUsageResults1 = this.list1.map(day => day.energyUsageResult);
    const monthbyday = this.list1.map(day => day.day);

    const Linechart =new Chart("barplot2", {
        type: 'bar',
       
        data : {
          labels: monthbyday,
          
          datasets: [
            {
              label: 'Consumption',
              data: energyUsageResults1,
              borderColor: '#7fcdbb',
              backgroundColor: '#7fcdbb',
              
            },
            
          ]
          
        },
        options: 
        {
          maintainAspectRatio: false,
          responsive: true, // Enable responsiveness
          
          scales:{
            y: {
              ticks:{
                color:'#000',
                font:{
                  size:13
                }
              },
              position: "left",
              title:{
                display:true,
                text: "Consumption (kWh)",
                color: '#000',
                font:{
                  size:13
                }
              }
            }
            ,
            x:{
              ticks:{
                color:'#000',
                font:{
                  size:13
                }
                
              },
              title:{
                display:true,
                text: "Days in a month",
                color: '#000',
                font:{
                  size:13
                }
              }
            }
          },
         
          plugins: {
            datalabels: {
              display: false
            },
            legend:{
              display:false
            },
           
            title: {
              display: true,
              text: 'Consumption in a month',
              color: '#000',
              font:{
                size:15
              }
            }
          }
        }
      });
  }
}
