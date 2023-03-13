import { Component, OnInit } from '@angular/core';

import { Chart,registerables } from 'node_modules/chart.js'
Chart.register(...registerables)

import ChartDataLabels from 'chartjs-plugin-datalabels';
Chart.register(ChartDataLabels);

// const SAMPLE_BARCHART_DATA: any[] = [
//   { data: [65, 59, 80, 81, 56, 54, 30], label: 'Consumption'},
//   { data: [25, 39, 60, 91, 36, 54, 50], label: 'Production'}
// ];

// const SAMPLE_BARCHART_LABELS: string[] = ['Aerodrom', 'Bagremar', 'Erdoglija', 'Bresnica', 'Stanovo', 'Belosevac','Pivara'];
@Component({
  selector: 'app-bar-chart',
  templateUrl: './bar-chart.component.html',
  styleUrls: ['./bar-chart.component.css']
})
export class BarChartComponent implements OnInit{

  itemList: string[] = ['Aerodrom', 'Bagremar', 'Erdoglija', 'Bresnica', 'Stanovo', 'Belosevac','Pivara'];
  constructor() {
    
  }
  // public barChartData: any[]=SAMPLE_BARCHART_DATA;
  // public barChartLabels: string[]=SAMPLE_BARCHART_LABELS;
  // public barChartType = 'bar';
  // public barChartLegend = true;
  // public barChartOptions: any = {
  //   scaleShowVerticalLines: false,
  //   responsive: true
  // };

  ngOnInit(): void {
    this.BarPlot();
  }
  BarPlot(){
    
    const Linechart =new Chart("barplot", {
        type: 'bar',
        data : {
          labels: this.itemList,
          datasets: [
            {
              label: 'Consumption',
              data: [130,10,23,120,70,90,80,79,45,34,76,89],
              borderColor: 'rgb(255, 99, 132)',
              backgroundColor: 'rgb(255, 99, 132)'
            },
            {
              label: 'Production',
              data: [21,100,41,60,120,140,80,10,20,30,150,140],
              borderColor: 'rgb(54, 162, 235)',
              backgroundColor: 'rgb(54, 162, 235)'
            }
          ]
        },
        options: 
        {
          responsive: true,
          plugins: {
            legend: {
              position: 'top',
            },
            title: {
              display: true,
              text: 'Line Chart'
            }
          }
        }
      });
  }

}
