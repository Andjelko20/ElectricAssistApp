import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Chart,registerables } from 'node_modules/chart.js'
import { forkJoin } from 'rxjs';
import { YearsByMonth } from 'src/app/models/devices.model';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import {MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS} from '@angular/material-moment-adapter';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';
import {MatDatepicker} from '@angular/material/datepicker';
import moment, { Moment } from 'moment';
import { FormControl } from '@angular/forms';
Chart.register(...registerables)
Chart.register(...registerables)


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
  selector: 'app-bar-year-prosumer',
  templateUrl: './bar-year-prosumer.component.html',
  styleUrls: ['./bar-year-prosumer.component.css'],
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
export class BarYearProsumerComponent {

  currentDate = new Date();
  maxYear = new Date(this.currentDate.getFullYear(), this.currentDate.getMonth()-1, 1);
  list1:YearsByMonth[]=[];
  list2:YearsByMonth[]=[];
  itemList: string[] = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Avg','Sep','Okt','Nov','Dec'];
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
    const id = Number(this.route.snapshot.paramMap.get('id'));
    if(this.selectedDate == undefined){
      forkJoin([
        this.deviceService.yearByMonthUser(id, 2),
        this.deviceService.yearByMonthUser(id, 1)
      ]).subscribe(([list1, list2]) => {
        this.list1 = list1;
        this.list2 = list2;
        this.BarPlotProduction();
        this.BarPlotConsumption();
      });
    }
    else{
      const year = this.selectedDate.getFullYear();
      forkJoin([
        this.deviceService.monthbyDayUserFilter(year,id, 2),
        this.deviceService.monthbyDayUserFilter(year,id, 1)
      ]).subscribe(([list1, list2]) => {
        this.list1 = list1;
        this.list2 = list2;
        this.BarPlotProduction();
        this.BarPlotConsumption();
      });
    }
    }
    
    BarPlotProduction(){

      const chartId = 'barplot1';
      const chartExists = Chart.getChart(chartId);
      if (chartExists) {
          chartExists.destroy();
      }
  
      const energyUsageResults2 = this.list2.map(day => day.energyUsageResult);
      const month = this.list2.map(day => day.month);
  
      const Linechart =new Chart("barplot1", {
          type: 'bar',
         
          data : {
            labels: month,
            
            datasets: [
  
              {
                label: 'Production',
                data: energyUsageResults2,
                borderColor: 'rgb(255, 165, 0)',
                backgroundColor: 'rgb(255, 165, 0)'
              },
             
              
            ]
            
          },
          options: 
          {responsive: true,
            scales:{
              y: {
                ticks:{
                  color:'#000',
                  font:{
                    size:15
                  }
                },
                position: "left",
                suggestedMin: 5,
                suggestedMax: 140,
                title:{
                  display:true,
                  text: "kWh",
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
                  text: "Months in a Year",
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
                    size:20
                  } 
                  // ,
                  // boxHeight:100,
                  // boxWidth:100
                }
              },
              title: {
                display: true,
                text: 'Production in a year',
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
      const month = this.list1.map(day => day.month);
  
      const Linechart =new Chart("barplot2", {
          type: 'bar',
         
          data : {
            labels: month,
            
            datasets: [
              {
                label: 'Consumption',
                data: energyUsageResults1,
                borderColor: 'rgb(128, 0, 128)',
                backgroundColor: 'rgb(128, 0, 128)',
                
              },
             
              
            ]
            
          },
          options: 
          {responsive: true,
            scales:{
              y: {
                ticks:{
                  color:'#000',
                  font:{
                    size:15
                  }
                },
                position: "left",
                suggestedMin: 5,
                suggestedMax: 140,
                title:{
                  display:true,
                  text: "kWh",
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
                  text: "Months in a Year",
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
                    size:20
                  } 
                  // ,
                  // boxHeight:100,
                  // boxWidth:100
                }
              },
              title: {
                display: true,
                text: 'Consumption in a year',
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
