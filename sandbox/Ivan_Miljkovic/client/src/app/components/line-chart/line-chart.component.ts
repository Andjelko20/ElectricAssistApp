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
    labels: [2,
    4,
    8,
    12],
    datasets: [
      {
        label: 'Dataset 1',
        data: [130,10,23,120,70,90,80,79,45,34,76,89],
        borderColor: 'rgb(255, 99, 132)',
        backgroundColor: 'rgb(255, 99, 132),0.5',
      },
      {
        label: 'Dataset 2',
        data: [21,100,41,60,120,140,80,10,20,30,150,140],
        borderColor: 'rgb(54, 162, 235)',
        backgroundColor: 'rgb(54, 162, 235),0.5',
      }
    ]
  },
  options: {
    responsive: true,
    plugins: {
      legend: {
        position: 'top',
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
