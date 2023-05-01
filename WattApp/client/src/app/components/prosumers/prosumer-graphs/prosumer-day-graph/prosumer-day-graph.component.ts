import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Chart,registerables } from 'node_modules/chart.js'
import { combineLatest } from 'rxjs';
import { DayByHour } from 'src/app/models/devices.model';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { JwtToken } from 'src/app/utilities/jwt-token';
Chart.register(...registerables)
@Component({
  selector: 'app-prosumer-day-graph',
  templateUrl: './prosumer-day-graph.component.html',
  styleUrls: ['./prosumer-day-graph.component.css']
})
export class ProsumerDayGraphComponent {
  

  constructor(private route:ActivatedRoute,private deviceService:HistoryPredictionService) {
    
  }
  list1:DayByHour[] = [];
  list2:DayByHour[] = [];
  ngOnInit(): void {
    let token=new JwtToken();
    const userId = token.data.id as number;
  
    combineLatest([
      this.deviceService.dayByHourUser(userId, 2),
      this.deviceService.dayByHourUser(userId, 1)
    ]).subscribe(([list1, list2]) => {
      this.list1 = list1;
      this.list2 = list2;
      this.LineChart();
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
        scales:{
          y: {
            ticks:{
              color:'#000',
              font:{
                size:20
              }
            },
            position: "left",
            title:{
              display:true,
              text: " kWh",
              color:'#000',
              font:{
                size:20
              }
            }
          }
          ,
          x:{
            ticks:{
              color:'#000',
              font:{
                size:20
              }
            },
            title:{
              display:true,
              text: "Hours in a day",
              color:'#000',
              font:{
                size:20
              }
            }
          }
          ,
        },
        responsive: true,
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
                size:20
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
