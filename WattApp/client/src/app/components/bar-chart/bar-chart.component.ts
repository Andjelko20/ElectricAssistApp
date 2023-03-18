import { Component, OnInit } from '@angular/core';

import { Chart,registerables } from 'node_modules/chart.js'
Chart.register(...registerables)



Chart.defaults.color = "#fff";
Chart.defaults.color = "#fff";
@Component({
  selector: 'app-bar-chart',
  templateUrl: './bar-chart.component.html',
  styleUrls: ['./bar-chart.component.css']
})
export class BarChartComponent implements OnInit{

  itemList: string[] = ['Aerodrom', 'Bagremar', 'Erdoglija', 'Bresnica', 'Stanovo', 'Belosevac','Pivara'];
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
              data: [111,10,23,120,70,90,80],
              borderColor: 'rgb(128, 0, 128)',
              backgroundColor: 'rgb(128, 0, 128)'
              
            },
            {
              label: 'Production',
              data: [21,100,41,60,110,110,80],
              borderColor: 'rgb(255, 165, 0)',
              backgroundColor: 'rgb(255, 165, 0)'
            },
            {
              label: 'Number of Users',
              data: [111,10,23,150,70,90,80],
              borderColor: 'rgb(0, 128, 0)',
              backgroundColor: 'rgb(0, 128, 0)',
              yAxisID: 'Users',
              
            },
          ]
          
        },
        options: 
        {
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
                text: "communities"
              }
            }
            ,
            Users:{
              position:"right",
              title:{
                display:true,
                text: "number of users"
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
                
                // font:{
                //   size:12
                // } 
                // ,
                // boxHeight:10,
                // boxWidth:10
              }
            },
            title: {
              display: true,
              text: 'Bar plot'
            }
          }
        }
      });
  }

}
