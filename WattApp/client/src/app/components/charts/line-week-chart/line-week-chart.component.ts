import { Component } from '@angular/core';
import { Chart,registerables } from 'node_modules/chart.js'
Chart.register(...registerables)
@Component({
  selector: 'app-line-week-chart',
  templateUrl: './line-week-chart.component.html',
  styleUrls: ['./line-week-chart.component.css']
})
export class LineWeekChartComponent {

  constructor() {
    
  }
  ngOnInit(): void {
    this.LineChart();
  }
  LineChart(){
    
    const Linechart =new Chart("linechart", {
      type: 'line',
      data : {
        labels: ['0','1','2','3','4','5','6',''],
        
        datasets:  [
          {
            label: 'consumption',
            data: [130,10,23,120,70,90,80,79,45,34,76,89],
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
            data: [91,12,41,45,3,133,106,50,70,80,150,123],
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
            suggestedMin: 5,
            suggestedMax: 140,
            title:{
              display:true,
              text: "kWh",
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
              text: "Days in a week",
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
            text: ' Consumption and production in a week',
            color:'#000',
            font:{
              size:20
            }
          }
        }
      }
    });

  }
}
