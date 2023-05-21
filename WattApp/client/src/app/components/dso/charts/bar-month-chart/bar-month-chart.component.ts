import { Component, ViewChild } from '@angular/core';
import { Chart,registerables } from 'node_modules/chart.js'
import { forkJoin } from 'rxjs';
import { WeekByDay } from 'src/app/models/devices.model';
import { Settlement } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { MatDatepickerModule} from '@angular/material/datepicker';
import { FormControl, FormGroup } from '@angular/forms';
import {MomentDateAdapter, MAT_MOMENT_DATE_ADAPTER_OPTIONS} from '@angular/material-moment-adapter';
import {DateAdapter, MAT_DATE_FORMATS, MAT_DATE_LOCALE} from '@angular/material/core';
import {MatDatepicker} from '@angular/material/datepicker';
Chart.register(...registerables)

Chart.defaults.color = "#fff";
Chart.defaults.color = "#fff";


import * as _moment from 'moment';
import {default as _rollupMoment, Moment} from 'moment';

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
  selector: 'app-bar-month-chart',
  templateUrl: './bar-month-chart.component.html',
  styleUrls: ['./bar-month-chart.component.css'],
  providers: [
    {
      provide: DateAdapter,
      useClass: MomentDateAdapter,
      deps: [MAT_DATE_LOCALE, MAT_MOMENT_DATE_ADAPTER_OPTIONS],
    },

    {provide: MAT_DATE_FORMATS, useValue: MY_FORMATS},
  ],
})
export class BarMonthChartComponent {
  
  loader:boolean=false;
  currentDate = new Date();
  list1:WeekByDay[]=[];
  list2:WeekByDay[]=[];
  settlements:Settlement[] = [];
  constructor(private deviceService:HistoryPredictionService,private authService:AuthService) {
      this.date.valueChanges.subscribe((selectedDate : any) => {
        const arr1: any[] = [];
      arr1.push(Object.values(selectedDate)[4]);
      this.selectedDate=arr1[0];
      this.ngOnInit();
      });
  }

  selectedOption: number = 0;
  selectedDate : Date = new Date();
  onOptionSelected() {
    this.ngOnInit();
  }
  

  date = new FormControl(moment());

  setMonthAndYear(normalizedMonthAndYear: Moment, datepicker: MatDatepicker<Moment>) {
    const ctrlValue = this.date.value!;
    ctrlValue.month(normalizedMonthAndYear.month());
    ctrlValue.year(normalizedMonthAndYear.year());
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
        if(this.selectedOption == 0 && this.selectedDate != undefined){
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
            this.deviceService.weekByDayCityFilter(string1,string2,number, 2),
            this.deviceService.weekByDayCityFilter(string1,string2,number, 1)
          ]).subscribe(([list1, list2]) => {
            this.list1 = list1;
            this.list2 = list2;
            this.BarPlotConsumption();
            this.BarPlotProduction();
          });
        }
        else if(this.selectedOption != 0 && this.selectedDate != undefined){
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
            this.deviceService.weekByDaySettlementFilter(string1,string2, this.selectedOption,2),
            this.deviceService.weekByDaySettlementFilter(string1,string2, this.selectedOption,1)
          ]).subscribe(([list1, list2]) => {
            this.list1 = list1;
            this.list2 = list2;
            this.BarPlotConsumption();
            this.BarPlotProduction();
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
    const monthbyday = this.list2.map(day => day.day);

    const Linechart =new Chart("barplot1", {
        type: 'bar',
        data : {
          labels: monthbyday,
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
          maintainAspectRatio: false,
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
            datalabels:{display: false},
          legend:{display:false},
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
            
            datalabels:{display: false},
            legend:{display:false},
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
