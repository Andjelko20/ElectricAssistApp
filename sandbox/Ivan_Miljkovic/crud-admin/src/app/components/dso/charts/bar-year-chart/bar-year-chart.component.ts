import { Component } from '@angular/core';

import { Chart,registerables } from 'node_modules/chart.js'
Chart.register(...registerables)



Chart.defaults.color = "#fff";
Chart.defaults.color = "#fff";
@Component({
  selector: 'app-bar-year-chart',
  templateUrl: './bar-year-chart.component.html',
  styleUrls: ['./bar-year-chart.component.css']
})
export class BarYearChartComponent {

  
  itemList: string[] = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Avg','Sep','Okt','Nov','Dec'];
  constructor() {
    
  }

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
              data: [111,10,23,120,70,90,80,33,55,76,123,99],
              borderColor: 'rgb(128, 0, 128)',
              backgroundColor: 'rgb(128, 0, 128)',
              
            },
            {
              label: 'Production',
              data: [21,100,41,60,110,110,80,34,54,111,89,130],
              borderColor: 'rgb(255, 165, 0)',
              backgroundColor: 'rgb(255, 165, 0)'
            },
           
            
          ]
          
        },
        options: 
        {
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
                color: '#000',
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
                text: "Months in a Year",
                color: '#000',
                font:{
                  size:20
                }
              }
            }
            
              
            
            
            
          },
          responsive: true,
          plugins: {
            
            legend: {
              onHover: function (event, legendItem, legend) {
                document.body.style.cursor = 'pointer';
              },
              onLeave: function (event, legendItem, legend) {
                  document.body.style.cursor = 'default';
              },
              
              position: 'bottom',
              labels: {
                usePointStyle: true,
                color: '#000',
                font:{
                  size:20
                } 
                // ,
                // boxHeight:100,
                // boxWidth:100
              }
            },
            title: {
              display: true,
              text: 'Consumption and production in a year',
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
