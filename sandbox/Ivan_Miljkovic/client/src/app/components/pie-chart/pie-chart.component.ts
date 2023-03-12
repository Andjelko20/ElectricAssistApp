import { Component, OnInit } from '@angular/core';
import { Chart,registerables } from 'node_modules/chart.js'
Chart.register(...registerables)

import ChartDataLabels from 'chartjs-plugin-datalabels';
Chart.register(ChartDataLabels);
@Component({
  selector: 'app-pie-chart',
  templateUrl: './pie-chart.component.html',
  styleUrls: ['./pie-chart.component.css']
})
export class PieChartComponent implements OnInit {

  constructor() {
    
  }
  ngOnInit(): void {
    this.PieChart();
  }
  PieChart()
  {

    const Piechart =new Chart("piechart", {
      
      type: 'pie',
      data: {
        labels: ['Aerodrom', 'Bagremar', 'Erdoglija', 'Bresnica', 'Stanovo', 'Belosevac'],
        datasets: [{
          label: 'Percentage of Consumption',
          data: [30, 20, 15, 15, 14, 6],
          borderWidth: 1
        }]

      },
      plugins: [ChartDataLabels],
      options:{
        plugins: {
          legend: {
            position: 'top',
          },
          title: {
            display: true,
            text: 'Pie Chart'
          },
          datalabels: {
            formatter: (value, ctx) => {
                let sum = 0;
                let dataArr = ctx.chart.data.datasets[0].data;
                dataArr.map(data => {
                    sum += data;
                });
                let percentage = (value*100 / sum).toFixed(2)+"%";
                return percentage;
            },
            color: '#fff',
        }
          
        }
        
      },
      
      
    });
    
  } 
// }
// var data = [{
//   data: [50, 55, 60, 33],
//   labels: ["India", "China", "US", "Canada"],
//   backgroundColor: [
//       "#4b77a9",
//       "#5f255f",
//       "#d21243",
//       "#B27200"
//   ],
//   borderColor: "#fff"
// }];

// var options = {
//   tooltips: {
//       enabled: false
//   },
//   plugins: {
//       datalabels: {
//           formatter: (value, ctx) => {
//               let sum = 0;
//               let dataArr = ctx.chart.data.datasets[0].data;
//               dataArr.map(data => {
//                   sum += data;
//               });
//               let percentage = (value*100 / sum).toFixed(2)+"%";
//               return percentage;
//           },
//           color: '#fff',
//       }
//   }
// };

// var ctx = document.getElementById("pie-chart").getContext('2d');
// var myChart = new Chart(ctx, {
//   type: 'pie',
//   data: {
//       datasets: data
//   },
//   options: options
// });