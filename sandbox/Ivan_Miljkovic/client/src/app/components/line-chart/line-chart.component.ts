import { Component, OnInit } from '@angular/core';

import { Chart,registerables } from 'node_modules/chart.js'
Chart.register(...registerables)

import ChartDataLabels from 'chartjs-plugin-datalabels';
Chart.register(ChartDataLabels);

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
            data: [21,100,41,60,120,140,80,10,20,30,150,140],
            borderColor: 'rgb(54, 162, 235)',
            backgroundColor: 'rgb(54, 162, 235)',
          }
        ]
      },
      options: {
        responsive: true,
        plugins: {
          legend: {
            position: 'bottom',
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
