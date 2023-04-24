import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Chart,registerables } from 'node_modules/chart.js'
import { WeekByDay } from 'src/app/models/devices.model';
import { Settlement } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { DevicesService } from 'src/app/services/devices.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
Chart.register(...registerables)

@Component({
  selector: 'app-bar-month-prosumer',
  templateUrl: './bar-month-prosumer.component.html',
  styleUrls: ['./bar-month-prosumer.component.css']
})
export class BarMonthProsumerComponent {

  
  list1:WeekByDay[]=[];
  list2:WeekByDay[]=[];
  itemList: string[] = ['1','2','3','4','5','6','7','8','9','10','11','12','13','14','15','16','17','18','19'
  ,'20','21','22','23','24','25','26','27','28','29','30'];
  constructor(private deviceService:HistoryPredictionService,private route:ActivatedRoute) {
    
  }
  
  ngOnInit(): void {


    this.deviceService.monthByDayUser(Number(this.route.snapshot.paramMap.get('id')),2).subscribe((data:WeekByDay[])=>{
      this.list1 = data;
      this.deviceService.monthByDayUser(Number(this.route.snapshot.paramMap.get('id')),1).subscribe((data:WeekByDay[])=>{
        this.list2 = data;
        this.BarPlot();
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
                text: "Days in a month",
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
              text: 'Consumption and production in a month',
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
