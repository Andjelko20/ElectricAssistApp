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
  styleUrls: ['./line-day-chart.component.css']
})
export class LineDayChartComponent {

  constructor(private authService:AuthService,private deviceService:HistoryPredictionService) {
    
  }
  maxDate = new Date();
  list1:DayByHour[] = [];
  list2:DayByHour[] = [];
  settlements:Settlement[] = [];
  selectedOption: number = 0;

  onOptionSelected() {
    this.ngOnInit();
  }

  selectedDate!: Date;

  onDateSelected(event: { value: Date; }) {
    this.selectedDate = event.value;
    this.ngOnInit();
  }

  ngOnInit(): void {
    this.authService.getlogInUser().subscribe(user => {
      this.authService.getCityId(user.city).subscribe(number => {
        this.authService.getSettlement(number).subscribe((settlement: Settlement[]) => {
          this.settlements = settlement;
        });
        if (this.selectedOption == 0 && this.selectedDate === undefined) {
          forkJoin([
            this.deviceService.dayByHour(number, 2),
            this.deviceService.dayByHour(number, 1)
          ]).subscribe(([list1, list2]) => {
            this.list1 = list1;
            this.list2 = list2;
            this.LineChart();
          });
        } 
        else if(this.selectedOption == 0 && this.selectedDate !== undefined){
          const day = this.selectedDate.getDate();
          const month = this.selectedDate.getMonth()+1;
          const year = this.selectedDate.getFullYear();
          let string1 = '';
          let string2 = '';
          if(day == 1 || month == 1){
            if(month == 1 && day == 1){
              
              string1 = year+'-'+12+'-'+31
              string2 = year+'-'+month+'-'+day
            }
            else if(month != 1 && day == 1){
              if(month % 2 == 0){
                string1 = year+'-'+(month-1)+'-'+31
                string2 = year+'-'+month+'-'+day
              }
              else{
                string1 = year+'-'+(month-1)+'-'+30
                string2 = year+'-'+month+'-'+day
              }
             
            }
          }
          else{
            string1 = year+'-'+month+'-'+(day-1)
            string2 = year+'-'+month+'-'+day
          }

          forkJoin([
            this.deviceService.dayByHourCityFilter(string1,string2,number, 2),
            this.deviceService.dayByHourCityFilter(string1,string2,number, 1)
          ]).subscribe(([list1, list2]) => {
            this.list1 = list1;
            this.list2 = list2;
            this.LineChart();
          });
        }
        else if(this.selectedOption != 0 && this.selectedDate !== undefined){
          const day = this.selectedDate.getDate();
          const month = this.selectedDate.getMonth()+1;
          const year = this.selectedDate.getFullYear();
          let string1 = '';
          let string2 = '';
          if(day == 1 || month == 1){
            if(month == 1 && day == 1){
              
              string1 = year+'-'+12+'-'+31
              string2 = year+'-'+month+'-'+day
            }
            else if(month != 1 && day == 1){
              if(month % 2 == 0){
                string1 = year+'-'+(month-1)+'-'+31
                string2 = year+'-'+month+'-'+day
              }
              else{
                string1 = year+'-'+(month-1)+'-'+30
                string2 = year+'-'+month+'-'+day
              }
             
            }
          }
          else{
            string1 = year+'-'+month+'-'+(day-1)
            string2 = year+'-'+month+'-'+day
          }

          forkJoin([
            this.deviceService.dayByHourSettlementFilter(string1,string2,number, this.selectedOption),
            this.deviceService.dayByHourSettlementFilter(string1,string2,number, this.selectedOption)
          ]).subscribe(([list1, list2]) => {
            this.list1 = list1;
            this.list2 = list2;
            this.LineChart();
          });
        }
        else {
          forkJoin([
            this.deviceService.dayByHourSettlement(this.selectedOption, 2),
            this.deviceService.dayByHourSettlement(this.selectedOption, 1)
          ]).subscribe(([list1, list2]) => {
            this.list1 = list1;
            this.list2 = list2;
            this.LineChart();
          });
        }
      });
    });
  }
  LineChart(){

    const chartId = 'linechart';
    const chartExists = Chart.getChart(chartId);
    if (chartExists) {
        chartExists.destroy();
    }
    const energyUsageResults1 = this.list1.map(day => day.energyUsageResult);
    const energyUsageResults2 = this.list2.map(day => day.energyUsageResult);
    const Linechart =new Chart("linechart", {
      type: 'line',
      data : {
        labels: ['0','4','8','12','16','20',''],
        
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
              text: " kWh",
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
            position: 'bottom',
            onHover: function (event, legendItem, legend) {
              document.body.style.cursor = 'pointer';
            },
            onLeave: function (event, legendItem, legend) {
                document.body.style.cursor = 'default';
            },
            labels:{
              usePointStyle: true,
              color:'#000',
              font:{
                size:15
              } 
           
            }
            ,
            align: "center"
          },
          title: {
            
            display: true,
            text: 'Consumption and production in one day',
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
