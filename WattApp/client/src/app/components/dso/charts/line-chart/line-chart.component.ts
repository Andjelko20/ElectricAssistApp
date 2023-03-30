import { Component, OnInit } from '@angular/core';

import { Chart,registerables } from 'node_modules/chart.js'
Chart.register(...registerables)


@Component({
  selector: 'app-line-chart',
  templateUrl: './line-chart.component.html',
  styleUrls: ['./line-chart.component.css']
})
export class LineChartComponent implements OnInit {

  constructor() {
    
  }
  ngOnInit(): void {
    this.LineChart();
  }
  LineChart(){
    
    const Linechart =new Chart("linechart", {
      type: 'line',
      data : {
        labels: ['Jan','Feb','Mar','Apr','May','Jun','Jul','Avg','Sep','Okt','Nov','Dec'],
        
        datasets: [
          {
            label: 'Consumption',
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
          borderWidth: 1,
          fill: true
          
            
          },
          {
            label: 'Production',
            data: [91,12,41,45,3,133,106,50,70,80,150,123],
            tension:0.5,
            // borderColor: 'rgb(54, 162, 235)',
            // backgroundColor: 'rgb(54, 162, 235)',
            backgroundColor: 'rgba(0, 255, 0, 0.2)',
            borderColor: 'rgba(0, 255, 0, 1)',
            borderWidth: 2,
            pointBackgroundColor: 'rgba(0, 255, 0, 1)',
            pointBorderColor: 'rgba(0, 255, 0, 1)',
            pointBorderWidth: 1,
            pointRadius: 3,
            pointHoverRadius: 6,
            fill:true
          }
          // ,
          // {
          //   label: 'Consumption Pivara',
          //   data: [120,100,54,80,120,140,80,10,20,30,150,140],
          //   borderDash: [5, 5],
          //   borderColor: '#3E5483',
          //   backgroundColor: '#3E5483',
            
          // }
          // ,
          // {
          //   label: 'Consumption Bagremar',
          //   data: [41,111,41,61,141,10,77,54,17,138,150,140],
            
          //   showLine: false,
          //   borderColor: '#000',
          //   backgroundColor: '#000',
          // }
          // ,
          
        ]
        
      }
      ,
      options: {
        scales:{
          y: {
            ticks:{
              color:'#000'
            },
            position: "left",
            suggestedMin: 5,
            suggestedMax: 140,
            title:{
              display:true,
              text: "consumption in kWh",
              color:'#000'
            }
          }
          ,
          x:{
            ticks:{
              color:'#000'
            },
            title:{
              display:true,
              text: "Past year consumption per months",
              color:'#000'
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
                size:30
              } 
           
            }
            ,
            align: "center"
          },
          title: {
            display: true,
            text: ' Line Chart'
          }
        }
      }
    });

  }
   
    
    
}
