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
Chart.register(...registerables)

import * as _moment from 'moment';
import {default as _rollupMoment, Moment} from 'moment';
import { ActivatedRoute } from '@angular/router';

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
  selector: 'app-bar-month-prosumer',
  templateUrl: './bar-month-prosumer.component.html',
  styleUrls: ['./bar-month-prosumer.component.css'],
  providers: [
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS],
    },

    {provide: MAT_DATE_FORMATS, useValue: MY_FORMATS},
  ],
})
export class BarMonthProsumerComponent {

  currentDate = new Date();
  list1:WeekByDay[]=[];
  list2:WeekByDay[]=[];
  constructor(private deviceService:HistoryPredictionService,private route:ActivatedRoute) {
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
    const userId = Number(this.route.snapshot.paramMap.get('id'));
          let month = this.selectedDate!.getMonth()+1;
          let monthString = String(month).padStart(2, '0');
          let year = this.selectedDate!.getFullYear();
          let string1 = year+'-'+monthString+'-0'+1+' '+'00:00:00';
          monthString = String(month+1).padStart(2, '0');
          let string2 = year+'-'+monthString+'-0'+1+' '+'00:00:00';
          if(month == 12){
            string2 = (year+1)+'-0'+1+'-0'+1+' '+'00:00:00'
          }
          forkJoin([
            this.deviceService.weekByDayUserFilter(string1,string2,userId, 2),
            this.deviceService.weekByDayUserFilter(string1,string2,userId, 1)
          ]).subscribe(([list1, list2]) => {
            this.list1 = list1;
            this.list2 = list2;
            this.BarPlotProduction();
            this.BarPlotConsumption();
          });
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
              borderColor:  '#1d91c0',
              backgroundColor:  '#1d91c0'
            },
           
            
          ]
          
        },
        options: 
        {

          responsive: true, // Enable responsiveness
          
          scales:{
            y: {
              ticks:{
                color:'#000',
                font:{
                  size:15
                }
              },
              position: "left",
              title:{
                display:true,
                text: "Production (kWh)",
                color: '#000',
                font:{
                  size:15
                }
              }
            }
            ,
            x:{
              ticks:{
                color:'#000',
                font:{
                  size:15
                }
                
              },
              title:{
                display:true,
                text: "Days in a month",
                color: '#000',
                font:{
                  size:15
                }
              }
            }
          },
         
          plugins: {
            datalabels: {
              display: false
            },
            legend: {
              onHover: function (event, legendItem, legend) {
                document.body.style.cursor = 'pointer';
              },
              onLeave: function (event, legendItem, legend) {
                  document.body.style.cursor = 'default';
              },
              
              position: 'bottom',
              labels: {
                usePointStyle: true,
                color: '#000',
                font:{
                  size:15
                } 
                // ,
                // boxHeight:100,
                // boxWidth:100
              }
            },
            title: {
              display: true,
              text: 'Production in a month',
              color: '#000',
              font:{
                size:20
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
              borderColor:  '#7fcdbb',
              backgroundColor:  '#7fcdbb',
              
            },
            
          ]
          
        },
        options: 
        {

          responsive: true, // Enable responsiveness
          
          scales:{
            y: {
              ticks:{
                color:'#000',
                font:{
                  size:15
                }
              },
              position: "left",
              title:{
                display:true,
                text: "Consumption (kWh)",
                color: '#000',
                font:{
                  size:15
                }
              }
            }
            ,
            x:{
              ticks:{
                color:'#000',
                font:{
                  size:15
                }
                
              },
              title:{
                display:true,
                text: "Days in a month",
                color: '#000',
                font:{
                  size:15
                }
              }
            }
          },
         
          plugins: {
            datalabels: {
              display: false
            },
            legend: {
              onHover: function (event, legendItem, legend) {
                document.body.style.cursor = 'pointer';
              },
              onLeave: function (event, legendItem, legend) {
                  document.body.style.cursor = 'default';
              },
              
              position: 'bottom',
              labels: {
                usePointStyle: true,
                color: '#000',
                font:{
                  size:15
                } 
              }
            },
            title: {
              display: true,
              text: 'Consumption in a month',
              color: '#000',
              font:{
                size:20
              }
            }
          }
        }
      });
  }
}
