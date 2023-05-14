import { Component } from '@angular/core';
import { Chart,registerables } from 'node_modules/chart.js'
import { forkJoin } from 'rxjs';
import { DayByHour } from 'src/app/models/devices.model';
import { Settlement } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
Chart.register(...registerables)

@Component({
  selector: 'app-line-day-chart',
  templateUrl: './line-day-chart.component.html',
  styleUrls: ['./line-day-chart.component.css'],
})
export class LineDayChartComponent {

  loader:boolean=false;
  selectedOption: number;
  maxDate = new Date();
  currentDate = new Date();
  list1:DayByHour[] = [];
  list2:DayByHour[] = [];
  settlements:Settlement[] = [];
  
  constructor(private authService:AuthService,private deviceService:HistoryPredictionService) {
    this.selectedOption = 0;
  }
  onOptionSelected(event: any) {
    this.selectedOption = event.target.value;
    this.ngOnInit()
  }

  selectedDate!: Date;

  onDateSelected(event: { value: Date; }) {
    this.selectedDate = event.value;
    this.ngOnInit();
  }
  

  ngOnInit(): void {
    this.loader=true;
    this.authService.getlogInUser().subscribe(user => {
      this.authService.getCityId(user.city).subscribe(number => {
        this.authService.getSettlement(number).subscribe((settlement: Settlement[]) => {
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
            
        });
        if (this.selectedOption == 0 && this.selectedDate == undefined) {
          forkJoin([
            this.deviceService.dayByHour(number, 2),
            this.deviceService.dayByHour(number, 1)
          ]).subscribe(([list1, list2]) => {
            this.list1 = list1;
            this.list2 = list2;
            this.LineChartProduction();
            this.LineChartConsumption();
          });
        } 
        else if(this.selectedOption == 0 && this.selectedDate != undefined){
          const day = this.selectedDate.getDate();
          let dayString = String(day).padStart(2, '0');
          const month = this.selectedDate.getMonth()+1;
          let monthString = String(month).padStart(2, '0');
          const year = this.selectedDate.getFullYear();
          let string1 = '';
          let string2 = '';
          if(month % 2 == 0)
          {
            if(day == 30 || (month == 2 && day == 28)){
              string1 = year+'-'+monthString+'-'+dayString+' '+'00:00:00'
              monthString = String(month+1).padStart(2, '0');
              string2 = year+'-'+monthString+'-0'+1+' '+'00:00:00'
            }
            else if( month == 12){
              string1 = year+'-'+monthString+'-'+dayString+' '+'00:00:00'
              string2 = (year+1)+'-0'+1+'-0'+1+' '+'00:00:00'
            }
            else if( month == 6){
              string1 = year+'-'+month+'-'+day
              string2 = year+'-'+(month+1)+'-'+1
            }
            else{
              string1 = year+'-'+monthString+'-'+dayString+' '+'00:00:00'
              dayString = String(day+1).padStart(2, '0');
              string2 = year+'-'+monthString+'-'+dayString+' '+'00:00:00'
            }
          }
          else{
            if(day == 31){
              string1 = year+'-'+monthString+'-'+dayString+' '+'00:00:00'
              monthString = String(month+1).padStart(2, '0');
              string2 = year+'-'+monthString+'-0'+1+' '+'00:00:00'
            }
            else{
              string1 = year+'-'+monthString+'-'+dayString+' '+'00:00:00'
              dayString = String(day+1).padStart(2, '0');
              string2 = year+'-'+monthString+'-'+dayString+' '+'00:00:00'
            }
          }
          console.log(string1+'-'+string2);
          

          forkJoin([
            
            this.deviceService.dayByHourCityFilter(string1,string2,number, 2),
            this.deviceService.dayByHourCityFilter(string1,string2,number, 1)
          ]).subscribe(([list1, list2]) => {
            this.list1 = list1;
            this.list2 = list2;
            this.LineChartProduction();
            this.LineChartConsumption();
          });
        }
        else if(this.selectedOption != 0 && this.selectedDate != undefined){
          const day = this.selectedDate.getDate();
          let dayString = String(day).padStart(2, '0');
          const month = this.selectedDate.getMonth()+1;
          let monthString = String(month).padStart(2, '0');
          const year = this.selectedDate.getFullYear();
          let string1 = '';
          let string2 = '';
          if(month % 2 == 0)
          {
            if(day == 30 || (month == 2 && day == 28)){
              string1 = year+'-'+monthString+'-'+dayString+' '+'00:00:00'
              monthString = String(month+1).padStart(2, '0');
              string2 = year+'-'+monthString+'-0'+1+' '+'00:00:00'
            }
            else if( month == 12){
              string1 = year+'-'+monthString+'-'+dayString+' '+'00:00:00'
              string2 = (year+1)+'-0'+1+'-0'+1+' '+'00:00:00'
            }
            else{
              string1 = year+'-'+monthString+'-'+dayString+' '+'00:00:00'
              dayString = String(day+1).padStart(2, '0');
              string2 = year+'-'+monthString+'-'+dayString+' '+'00:00:00'
            }
          }
          else{
            if(day == 31){
              string1 = year+'-'+monthString+'-'+dayString+' '+'00:00:00'
              monthString = String(month+1).padStart(2, '0');
              string2 = year+'-'+monthString+'-0'+1+' '+'00:00:00'
            }
            else{
              string1 = year+'-'+monthString+'-'+dayString+' '+'00:00:00'
              dayString = String(day+1).padStart(2, '0');
              string2 = year+'-'+monthString+'-'+dayString+' '+'00:00:00'
            }
          }
          forkJoin([
            this.deviceService.dayByHourSettlementFilter(string1,string2,this.selectedOption,2),
            this.deviceService.dayByHourSettlementFilter(string1,string2,this.selectedOption,1)
          ]).subscribe(([list1, list2]) => {
            this.list1 = list1;
            this.list2 = list2;
            this.LineChartProduction();
            this.LineChartConsumption();
          });
        }
        else {
          forkJoin([
            this.deviceService.dayByHourSettlement(this.selectedOption, 2),
            this.deviceService.dayByHourSettlement(this.selectedOption, 1)
          ]).subscribe(([list1, list2]) => {
            this.list1 = list1;
            this.list2 = list2;
            this.LineChartProduction();
            this.LineChartConsumption();
          });
        }
      });
    });
  }
  LineChartProduction(){

    const chartId = 'linechart1';
    const chartExists = Chart.getChart(chartId);
    if (chartExists) {
        chartExists.destroy();
    }
    const energyUsageResults2 = this.list2.map(day => day.energyUsageResult);
    const hours = this.list2.map(day => day.hour);
    let max=0;
    if(energyUsageResults2[0]===0 && energyUsageResults2[1]===0 )
    {
      max=1;
    }
    const Linechart =new Chart("linechart1", {
      type: 'line',
      data : {
        labels: hours,
        
        datasets: [
          {
            label: 'production',
            data: energyUsageResults2,
            tension:0.5,
            backgroundColor: 'rgba(0, 255, 0, 0.2)',
            borderColor: 'rgba(0, 255, 0, 1)',
            borderWidth: 2,
            pointBackgroundColor: 'rgba(0, 255, 0, 1)',
            pointBorderColor: 'rgba(0, 255, 0, 1)',
            pointBorderWidth: 7,
            pointRadius: 5,
            pointHoverRadius: 6,
            fill:true
          }
        ]
      }
      ,
      options: {
        maintainAspectRatio:false,
        responsive: true,
        scales:{
          y: {
            ticks:{
              color:'#000',
              font:{
                size:15
              }
            },suggestedMax:max,
            position: "left",
            title:{
              display:true,
              text: "Production (kWh)",
              color:'#000',
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
              text: "Hours in a day",
              color:'#000',
              font:{
                size:15
              }
            }
          }
          ,
        },
        
        plugins: {
          datalabels:{display: false},
          legend: {display: false
          },
          title: {
            
            display: true,
            text: 'Production in one day',
            color: '#000',
            font:{
              size:20
            }
          }
        }
      }
    });

  }
  LineChartConsumption(){

    const chartId = 'linechart2';
    const chartExists = Chart.getChart(chartId);
    if (chartExists) {
        chartExists.destroy();
    }
    const energyUsageResults1 = this.list1.map(day => day.energyUsageResult);
    const hours = this.list1.map(day => day.hour);
    let max=0;
    if(energyUsageResults1[0]===0 && energyUsageResults1[1]===0 )
    {
      max=1;
    }
    const Linechart =new Chart("linechart2", {
      type: 'line',
      data : {
        labels: hours,
        
        datasets: [
          {
            label: 'consumption',
            data: energyUsageResults1,
            backgroundColor: [
              'rgba(255, 99, 132, 0.2)',
              'rgba(54, 162, 235, 0.2)',
              'rgba(255, 206, 86, 0.2)',
              'rgba(75, 192, 192, 0.2)',
              'rgba(153, 102, 255, 0.2)',
              'rgba(255, 159, 64, 0.2)'
          ],
          borderColor: [
              'rgba(255,99,132,1)',
              'rgba(54, 162, 235, 1)',
              'rgba(255, 206, 86, 1)',
              'rgba(75, 192, 192, 1)',
              'rgba(153, 102, 255, 1)',
              'rgba(255, 159, 64, 1)'
          ],
          pointBorderColor: 'rgba(255,99,132,1)',
          pointBorderWidth: 7,
            pointRadius: 5,
          borderWidth: 2,
          fill: true
          },
        ]
      }
      ,
      options: {
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
              text: "Consumption (kWh)",
              color:'#000',
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
              text: "Hours in a day",
              color:'#000',
              font:{
                size:15
              }
            }
          }
          ,
        },
        
        plugins: {
          datalabels:{display: false},
          legend: { 
            display: false
          },
          title: {
            display: true,
            text: 'Consumption in one day',
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
