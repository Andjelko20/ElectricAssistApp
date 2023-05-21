import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Chart,registerables } from 'node_modules/chart.js'
import { forkJoin } from 'rxjs';
import { YearsByMonth } from 'src/app/models/devices.model';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { JwtToken } from 'src/app/utilities/jwt-token';
import {MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS} from '@angular/material-moment-adapter';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';
import { FormControl } from '@angular/forms';
import moment, { Moment } from 'moment';
import { MatDatepicker } from '@angular/material/datepicker';
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
  selector: 'app-prosumer-year-graph',
  templateUrl: './prosumer-year-graph.component.html',
  styleUrls: ['./prosumer-year-graph.component.css'],
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
export class ProsumerYearGraphComponent {

  maxYear = new Date();
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
    let token=new JwtToken();
    const id = token.data.id as number;
    if(this.selectedDate == undefined){
      forkJoin([
        this.deviceService.yearByMonthUser(id, 2),
      ]).subscribe(([list1]) => {
        this.list1 = list1;

        this.BarPlotConsumption();
      });
    }
    else{
      const year = this.selectedDate.getFullYear();
      forkJoin([
        this.deviceService.monthbyDayUserFilter(year,id, 2),
      ]).subscribe(([list1]) => {
        this.list1 = list1;

        this.BarPlotConsumption();
      });
    }
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
              borderColor:  'rgba(127, 205, 187, 1)',
              backgroundColor:  'rgba(127, 205, 187, 0.3)',
              borderWidth: 2.5,
              
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
          maintainAspectRatio: false,
          responsive: true,
          scales:{
            y: {
              ticks:{
                color:'#000',
                font:{
                  size:13
                }
              },
              position: "left",
              suggestedMin: 5,
              suggestedMax: 140,
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
                text: "Months in a Year",
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
              text: 'Consumption in a year',
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
