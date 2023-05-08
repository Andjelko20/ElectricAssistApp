import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Chart,registerables } from 'node_modules/chart.js'
import { combineLatest, forkJoin } from 'rxjs';
import { DayByHour } from 'src/app/models/devices.model';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
Chart.register(...registerables)

@Component({
  selector: 'app-line-day-prosumer',
  templateUrl: './line-day-prosumer.component.html',
  styleUrls: ['./line-day-prosumer.component.css']
})
export class LineDayProsumerComponent{
  maxDate: Date;

  constructor(private route:ActivatedRoute,private deviceService:HistoryPredictionService) {
    this.maxDate = new Date();
  }
  list1:DayByHour[] = [];
  list2:DayByHour[] = [];
  
  selectedDate!: Date;

  onDateSelected(event: { value: Date; }) {
    this.selectedDate = event.value;
    this.ngOnInit();
  }

  ngOnInit(): void {
    const userId = Number(this.route.snapshot.paramMap.get('id'));
  
    if(this.selectedDate == undefined){
      combineLatest([
        this.deviceService.dayByHourUser(userId, 2),
        this.deviceService.dayByHourUser(userId, 1)
      ]).subscribe(([list1, list2]) => {
        this.list1 = list1;
        this.list2 = list2;
        this.LineChartProduction();
        this.LineChartConsumption();
      });
    }
    else if(this.selectedDate !== undefined){
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
        this.deviceService.dayByHourUserFilter(string1,string2,userId, 2),
        this.deviceService.dayByHourUserFilter(string1,string2,userId, 1)
      ]).subscribe(([list1, list2]) => {
        this.list1 = list1;
        this.list2 = list2;
        this.LineChartProduction();
        this.LineChartConsumption();
      });
    }
    
  }
  LineChartProduction(){

    const chartId = 'linechart1';
    const chartExists = Chart.getChart(chartId);
    if (chartExists) {
        chartExists.destroy();
    }
    const energyUsageResults2 = this.list2.map(day => day.energyUsageResult);
    const hours = this.list2.map(day => day.hour);

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
