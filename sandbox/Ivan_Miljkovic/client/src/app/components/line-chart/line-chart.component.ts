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
        labels: [1,2,3,4,5,6,7,8,9,10,11,12],
        
        datasets: [
          {
            label: 'Consumption Aerodrom',
            data: [130,10,23,120,70,90,80,79,45,34,76,89],
            borderColor: 'rgb(255, 99, 132)',
            backgroundColor: 'rgb(255, 99, 132)',
          },
          {
            label: 'Consumption Bresnica',
            data: [91,12,41,45,3,133,106,50,70,80,150,123],
            tension:0.5,
            borderColor: 'rgb(54, 162, 235)',
            backgroundColor: 'rgb(54, 162, 235)',
          }
          ,
          {
            label: 'Consumption Pivara',
            data: [120,100,54,80,120,140,80,10,20,30,150,140],
            borderDash: [5, 5],
            borderColor: '#fff',
            backgroundColor: '#fff',
          }
          ,
          {
            label: 'Consumption Bagremar',
            data: [41,111,41,61,141,10,77,54,17,138,150,140],
            
            showLine: false,
            borderColor: '#000',
            backgroundColor: '#000',
          }
          ,
          
        ]
        
      }
      ,
      options: {
        scales:{
          y: {
            position: "left",
            suggestedMin: 5,
            suggestedMax: 140,
            title:{
              display:true,
              text: "consumption in kWh"
            }
          }
          ,
          x:{
            title:{
              display:true,
              text: "Past year consumption per months"
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
