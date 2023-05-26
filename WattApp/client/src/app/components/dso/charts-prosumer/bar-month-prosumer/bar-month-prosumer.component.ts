import { Component } from '@angular/core';
import { Chart,registerables } from 'node_modules/chart.js'
import { forkJoin } from 'rxjs';
import { WeekByDay } from 'src/app/models/devices.model';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { FormControl } from '@angular/forms';
import {MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS} from '@angular/material-moment-adapter';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';
import {MatDatepicker} from '@angular/material/datepicker';
Chart.register(...registerables)

import * as _moment from 'moment';
import {default as _rollupMoment, Moment} from 'moment';
import { ActivatedRoute } from '@angular/router';
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
  list1pred: number[] = [];
  list2pred: number[] = [];
  mergedList: { day: number, month: string, year: number, consumption: number, production: number }[] = [];
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
            this.list1pred = [];
            for (const obj of this.list1) {
              const increasedEnergy = obj.energyUsageResult * (1 + Math.random() * (0.20) - 0.01);
              this.list1pred.push(increasedEnergy);
            }
            this.list2 = list2;
            this.list2pred = [];
            for (const obj of this.list2) {
              const increasedEnergy = obj.energyUsageResult * (1 + Math.random() * (0.20) - 0.01);
              this.list2pred.push(increasedEnergy);
            }
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
    let max=0;
    if(energyUsageResults2[0]===0)
    {
      max=1;
    }
    const Linechart =new Chart("barplot1", {
        type: 'bar',
       
        data : {
          labels: monthbyday,
          
          datasets: [
            {
              label: ' Production',
              data: energyUsageResults2,
              borderColor: 'rgba(29, 145, 192, 1)',
              backgroundColor: 'rgba(29, 145, 192, 0.2)',
              borderWidth: 2,
            },
            {
              label: ' Prediction',
              data: this.list2pred,
              borderColor: 'rgba(252, 129, 155, 1)',
              backgroundColor: 'rgba(252, 129, 155, 0.2)',
              borderWidth: 2,
              categoryPercentage:0.5
            }
            
          ]
          
        },
        options: 
        {
          onHover: (e, chartEle) => {
            if (e.native) {
              const target = e.native.target as HTMLElement;
              if (target instanceof HTMLElement) {
                target.style.cursor = chartEle.length > 0 && chartEle[0] ? 'pointer' : 'default';
              } else {
                console.error('Invalid target element:', target);
              }
            } else {
              console.error('Missing native event:', e);
            }
          },  
          maintainAspectRatio:false,
          responsive: true, 
          
          scales:{
            y: {
              ticks:{
                color:'#000',
                font:{
                  size:15
                }
              },
              suggestedMax:max,
              position: "left",
              title:{
                display:true,
                text: "Production [kWh]",
                color: '#000',
                font:{
                  size:15
                }
              }
            }
            ,
            x:{
              stacked:true,
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
          interaction: {
            intersect: false,
            mode: 'index',
          },
          plugins: {
            datalabels: {
              display: false
            },
            legend: {
              labels:{
              color:'#000',
             
              font:{
                size:16
              },
              boxWidth:15,
              boxHeight:15,
              useBorderRadius:true,
              borderRadius:7
            },
              
              position: 'bottom',
              onHover: function (event, legendItem, legend) {
                document.body.style.cursor = 'pointer';
              },
              onLeave: function (event, legendItem, legend) {
                  document.body.style.cursor = 'default';
              },
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
    let max=0;
    if(energyUsageResults1[0]===0)
    {
      max=1;
    }
    const Linechart =new Chart("barplot2", {
        type: 'bar',
       
        data : {
          labels: monthbyday,
          
          datasets: [
            {
              label: ' Consumption',
              data: energyUsageResults1,
              borderColor:  'rgba(127, 205, 187, 1)',
              backgroundColor:  'rgba(127, 205, 187, 0.3)',
              borderWidth: 2.5,
            },
            {
              label: ' Prediction',
              data: this.list1pred,
              borderColor: 'rgba(252, 129, 155, 1)',
              backgroundColor: 'rgba(252, 129, 155, 0.2)',
              borderWidth: 2,
              categoryPercentage:0.5
            },
            
          ]
          
        },
        options: 
        {
          onHover: (e, chartEle) => {
            if (e.native) {
              const target = e.native.target as HTMLElement;
              if (target instanceof HTMLElement) {
                target.style.cursor = chartEle.length > 0 && chartEle[0] ? 'pointer' : 'default';
              } else {
                console.error('Invalid target element:', target);
              }
            } else {
              console.error('Missing native event:', e);
            }
          },  
          maintainAspectRatio:false,
          responsive: true, 
          
          scales:{
            y: {
              ticks:{
                color:'#000',
                font:{
                  size:15
                }
              },
              suggestedMax:max,
              position: "left",
              title:{
                display:true,
                text: "Consumption [kWh]",
                color: '#000',
                font:{
                  size:15
                }
              }
            }
            ,
            x:{
              stacked:true,
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
          interaction: {
            intersect: false,
            mode: 'index',
          },
          plugins: {
            datalabels: {
              display: false
            },
            legend: {
              labels:{
              color:'#000',
             
              font:{
                size:16
              },
              boxWidth:15,
              boxHeight:15,
              useBorderRadius:true,
              borderRadius:7
            },
              
              position: 'bottom',
              onHover: function (event, legendItem, legend) {
                document.body.style.cursor = 'pointer';
              },
              onLeave: function (event, legendItem, legend) {
                  document.body.style.cursor = 'default';
              },
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
    filename: 'consumption/production-month',
    quoteStrings: '"',
    useBom : true,
    decimalSeparator: '.',
    showLabels: true,
    useTextFile: false,
    headers: ['Day', 'Month', 'Year', 'Consumption [kWh]', 'Production [kWh]']
  };

  const csvExporter = new ExportToCsv(options);
  const csvData = csvExporter.generateCsv(this.mergedList);

  }
}
