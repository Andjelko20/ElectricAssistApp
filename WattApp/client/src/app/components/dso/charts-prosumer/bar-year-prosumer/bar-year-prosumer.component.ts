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
import { ExportToCsv } from 'export-to-csv';
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
  list1:YearsByMonth[]=[];
  list2:YearsByMonth[]=[];
  mergedList: {month: string, year: number, consumption: number, production: number }[] = [];
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
                borderColor: 'rgba(29, 145, 192, 1)',
                backgroundColor: 'rgba(29, 145, 192, 0.2)',
                borderWidth: 2,
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
                position: "left",
                suggestedMin: 5,
                suggestedMax: 140,
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
                display:false
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
                position: "left",
                suggestedMin: 5,
                suggestedMax: 140,
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
                display:false
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
