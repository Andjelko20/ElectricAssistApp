import { Component, OnInit } from '@angular/core';

import { Chart,registerables, Title } from 'node_modules/chart.js'
Chart.register(...registerables)

import ChartDataLabels from 'chartjs-plugin-datalabels';
Chart.register(ChartDataLabels);


@Component({
  selector: 'app-pie-chart',
  templateUrl: './pie-chart.component.html',
  styleUrls: ['./pie-chart.component.css']
})
export class PieChartComponent implements OnInit {

  itemList: string[] = ['Aerodrom', 'Bagremar', 'Erdoglija', 'Bresnica', 'Stanovo', 'Belosevac'];
  constructor() {
    
  }
  ngOnInit(): void {
    this.PieChart();
  }

  PieChart(){
    var data= [{
      label: 'Percentage of Consumption in kWh',
      data: [30, 20, 15, 15, 14, 6],
      // backgroundColor: [
      //         "#4b77a9",
      //         "#5f255f",
      //         "#d21243",
      //         "#B27200",
      //         "#00000"
      //     ],
      borderWidth: 1,
      borderColor: "#00000"
    },{label: 'Percentage of Production in kWh',
    data: [30, 20, 15, 15, 14, 6],
    // backgroundColor: [
    //         "#4b77a9",
    //         "#5f255f",
    //         "#d21243",
    //         "#B27200",
    //         "#00000"
    //     ],
    borderWidth: 1,
    borderColor: "#00000"
  },]
    var ctx = "piechart";
    var myChart = new Chart(ctx, {
      
      type: 'pie',
      data: {
        labels: this.itemList,
          datasets: data
      },
      options: {
        plugins: {
            datalabels: {
                formatter: (value: number, ctx: { chart: { data: { datasets: { data: any; }[]; }; }; }) => {
                let sum = 0;
                let dataArr = ctx.chart.data.datasets[0].data;
                dataArr.map((data: number) => {
                  sum += data;
                });
                
                let percentage = (value * 100 / sum).toFixed(2) + "%";
                return percentage;
              },
                color: '#fff',
            },
            legend: {
              position: 'bottom',
              onHover: function (event, legendItem, legend) {
                document.body.style.cursor = 'pointer';
              },
              onLeave: function (event, legendItem, legend) {
                  document.body.style.cursor = 'default';
              },
              title:{
                display:true,
                text: "Communities that consumes and produces"
              }
            },
            title: {
              display: true,
              text: 'Pie Chart'
            }
            
        }}
      
    
    });
  }
}