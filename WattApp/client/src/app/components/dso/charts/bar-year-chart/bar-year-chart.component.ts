import { Chart,registerables } from 'node_modules/chart.js'
import { forkJoin } from 'rxjs';
import { WeekByDay, YearsByMonth } from 'src/app/models/devices.model';
import { Settlement } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import {Component, ViewEncapsulation} from '@angular/core';
import {FormControl} from '@angular/forms';
import {MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS} from '@angular/material-moment-adapter';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';
import {MatDatepicker} from '@angular/material/datepicker';
import moment, { Moment } from 'moment';
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

Chart.defaults.color = "#fff";
Chart.defaults.color = "#fff";
@Component({
  selector: 'app-bar-year-chart',
  templateUrl: './bar-year-chart.component.html',
  styleUrls: ['./bar-year-chart.component.css'],
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


export class BarYearChartComponent {

  loader:boolean=false;
  currentDate = new Date();
  maxYear = new Date(this.currentDate.getFullYear(), this.currentDate.getMonth()-1, 1);
  list1:YearsByMonth[]=[];
  list2:YearsByMonth[]=[];
  settlements:Settlement[] = [];
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

  ngOnInit(): void {
    this.loader=true;
    this.authService.getlogInUser().subscribe(user=>{
      this.authService.getCityId(user.city).subscribe(number=>{
        this.authService.getSettlement(number).subscribe((settlement:Settlement[])=>{
          this.loader=false;
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
            this.BarPlotProduction();
            this.BarPlotConsumption();
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
            this.BarPlotProduction();
            this.BarPlotConsumption();
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
            this.BarPlotProduction();
            this.BarPlotConsumption();
          });
        }
        else{
          forkJoin([
            this.deviceService.yearByMonthSettlement(this.selectedOption, 2),
            this.deviceService.yearByMonthSettlement(this.selectedOption, 1)
          ]).subscribe(([list1, list2]) => {
            this.list1 = list1;
            this.list2 = list2;
            this.BarPlotProduction();
            this.BarPlotConsumption();
          });
        }
        
      })
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
              borderColor: 'rgb(255, 165, 0)',
              backgroundColor: 'rgb(255, 165, 0)'
            },
           
            
          ]
          
        },
        options: 
        {
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
            datalabels:{display: false},
            legend: { 
              display: false
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
        
        {
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
            datalabels:{display: false},
          legend: { 
            display: false
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
