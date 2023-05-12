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

  currentDate = new Date();
  maxYear = new Date(this.currentDate.getFullYear(), this.currentDate.getMonth()-1, 1);
  list1:YearsByMonth[]=[];
  list2:YearsByMonth[]=[];
  itemList: string[] = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Avg','Sep','Okt','Nov','Dec'];
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
  selectedDate : Date | undefined;
  setYear(year: Moment, datepicker: MatDatepicker<Moment>) {
    const ctrlValue = this.date.value!;
    ctrlValue.year(year.year());
    this.date.setValue(ctrlValue);
    datepicker.close();
  }

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.authService.getDevice(id).subscribe(data=>{
    if(this.selectedDate == undefined){
      
        if(data.deviceCategory == "Electricity Consumer")
        {
          this.consumptionGraph = true;
          this.deviceService.yearByMonthDevice(id).subscribe(consumption =>{
            this.list1 = consumption
            this.BarPlotConsumption();
          })
          
        }
        else{
          this.productionGraph = true;
          this.deviceService.yearByMonthDevice(id).subscribe(production =>{
            this.list2 = production
            this.BarPlotProduction();
          })
          
        }
      
    }
    else{
      const year = this.selectedDate.getFullYear();
      forkJoin([
        this.deviceService.monthbyDayDeviceFilter(year,id, 2),
        this.deviceService.monthbyDayDeviceFilter(year,id, 1)
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
    }
     })
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
                borderColor: '#1d91c0',
                backgroundColor: '#1d91c0',
              },
             
              
            ]
            
          },
          options: 
          {
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
          {
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
                display: false
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
