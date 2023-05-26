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
import { AuthService } from 'src/app/services/auth.service';
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
  selector: 'app-device-year',
  templateUrl: './device-year.component.html',
  styleUrls: ['./device-year.component.css'],
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
export class DeviceYearComponent {

  maxYear = new Date();
  list1:YearsByMonth[]=[];
  list2:YearsByMonth[]=[];
  list1pred: number[] = [];
  list2pred: number[] = [];
  mergedList: {month: string, year: number, consumption: number, production: number }[] = [];
  constructor(private deviceService:HistoryPredictionService,private route:ActivatedRoute,private authService:AuthService) {
    this.date.valueChanges.subscribe((selectedDate : any) => {
      const arr1: any[] = [];
    arr1.push(Object.values(selectedDate)[4]);
    this.selectedDate=arr1[0];
    this.ngOnInit();
    });
  }
  consumptionGraph:boolean = false;
  productionGraph:boolean = false;
  date = new FormControl(moment());
  selectedDate : Date = new Date();
  setYear(year: Moment, datepicker: MatDatepicker<Moment>) {
    const ctrlValue = this.date.value!;
    ctrlValue.year(year.year());
    this.date.setValue(ctrlValue);
    datepicker.close();
  }

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.authService.getDevice(id).subscribe(data=>{
      
        if(data.deviceCategory == "Electricity Consumer")
        {
          this.consumptionGraph = true;
          
        }
        else{
          this.productionGraph = true;
        }
      
    
      const year = this.selectedDate.getFullYear();
      forkJoin([
        this.deviceService.monthbyDayDeviceFilter(year,id, 2),
        this.deviceService.monthbyDayDeviceFilter(year,id, 1)
      ]).subscribe(([list1, list2]) => {
        if(data.deviceCategory == "Electricity Consumer"){
          this.list1 = list1;
            this.list1pred = [];
            for (const obj of this.list1) {
              const increasedEnergy = obj.energyUsageResult * (1 + Math.random() * (0.20) - 0.01); // Increase energy property by random percentage
              this.list1pred.push(increasedEnergy);
            }
          this.consumptionGraph = true;
          this.BarPlotConsumption();
        }
        else{
          this.list2 = list2;
            this.list2pred = [];
            for (const obj of this.list2) {
              const increasedEnergy = obj.energyUsageResult * (1 + Math.random() * (0.20) - 0.01); // Increase energy property by random percentage
              this.list2pred.push(increasedEnergy);
            }
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
      let max=0;
      if(energyUsageResults2[0]===0 )
      {
        max=1;
      }
      const Linechart =new Chart("barplot1", {
          type: 'bar',
         
          data : {
            labels: ["Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec"],
            
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
                suggestedMax:max,
                position: "left",
                title:{
                  display:true,
                  text: "Production [kWh]",
                  color: '#000',
                  font:{
                    size:13
                  }
                  
                }
              }
              ,
              x:{
                stacked:true,
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
                text: 'Production in a year',
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
      const month = this.list1.map(day => day.month);
      let max=0;
      if(energyUsageResults1[0]===0 )
      {
        max=1;
      }
      const Linechart =new Chart("barplot2", {
          type: 'bar',
         
          data : {
            labels: ["Jan","Feb","Mar","Apr","May","Jun","Jul","Aug","Sep","Oct","Nov","Dec"],
            
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
                suggestedMax:max,
                position: "left",
                title:{
                  display:true,
                  text: "Consumption [kWh]",
                  color: '#000',
                  font:{
                    size:13
                  }
                  
                }
              }
              ,
              x:{
                stacked:true,
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
    downloadCSV(): void {
      const deviceId = Number(this.route.snapshot.paramMap.get('id'));
      this.authService.getDevice(deviceId).subscribe(data=>{
        if(data.deviceCategory == "Electricity Consumer"){
            const options = {
            fieldSeparator: ',',
            filename: 'consumption-year',
            quoteStrings: '"',
            useBom : true,
            decimalSeparator: '.',
            showLabels: true,
            useTextFile: false,
            headers: ['Hour', 'Day', 'Month', 'Year', 'Consumption [kWh]']
          };
          const csvExporter = new ExportToCsv(options);
          const csvData = csvExporter.generateCsv(this.list1);
        }
        else if(data.deviceCategory == "Electricity Producer"){
            const options = {
            fieldSeparator: ',',
            filename: 'production-year',
            quoteStrings: '"',
            useBom : true,
            decimalSeparator: '.',
            showLabels: true,
            useTextFile: false,
            headers: ['Hour', 'Month', 'Year', 'Production [kWh]']
          };
          const csvExporter = new ExportToCsv(options);
          const csvData = csvExporter.generateCsv(this.list2);
        }
      })
      } 
}
