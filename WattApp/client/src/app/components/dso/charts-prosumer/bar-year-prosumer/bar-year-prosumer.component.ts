import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { Chart,registerables } from 'node_modules/chart.js'
import { WeekByDay, YearsByMonth } from 'src/app/models/devices.model';
import { Settlement } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { DevicesService } from 'src/app/services/devices.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
Chart.register(...registerables)
@Component({
  selector: 'app-bar-year-prosumer',
  templateUrl: './bar-year-prosumer.component.html',
  styleUrls: ['./bar-year-prosumer.component.css']
})
export class BarYearProsumerComponent {

  list1:YearsByMonth[]=[];
  list2:YearsByMonth[]=[];
  itemList: string[] = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Avg','Sep','Okt','Nov','Dec'];
  constructor(private deviceService:HistoryPredictionService,private route:ActivatedRoute) {
    
  }
  ngOnInit(): void {

    this.deviceService.yearByMonth(Number(this.route.snapshot.paramMap.get('id')),2).subscribe((data:YearsByMonth[])=>{
      this.list1 = data;
      this.deviceService.yearByMonth(Number(this.route.snapshot.paramMap.get('id')),1).subscribe((data:YearsByMonth[])=>{
        this.list2 = data;
      })
    })
  }
  BarPlot(){

    const chartId = 'barplot';
    const chartExists = Chart.getChart(chartId);
    if (chartExists) {
        chartExists.destroy();
    }


    const energyUsageResults1 = this.list1.map(day => day.energyUsageResult);
    const energyUsageResults2 = this.list2.map(day => day.energyUsageResult);
    const Linechart =new Chart("barplot", {
        type: 'bar',
       
        data : {
          labels: this.itemList,
          
          datasets: [
            {
              label: 'Consumption',
              data: energyUsageResults1,
              borderColor: 'rgb(128, 0, 128)',
              backgroundColor: 'rgb(128, 0, 128)',
              
            },
            {
              label: 'Production',
              data: energyUsageResults2,
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
