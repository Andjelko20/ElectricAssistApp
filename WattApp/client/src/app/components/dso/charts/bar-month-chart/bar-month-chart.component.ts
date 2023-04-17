import { Component } from '@angular/core';
import { Chart,registerables } from 'node_modules/chart.js'
import { WeekByDay } from 'src/app/models/devices.model';
import { DevicesService } from 'src/app/services/devices.service';
Chart.register(...registerables)



Chart.defaults.color = "#fff";
Chart.defaults.color = "#fff";


@Component({
  selector: 'app-bar-month-chart',
  templateUrl: './bar-month-chart.component.html',
  styleUrls: ['./bar-month-chart.component.css']
})
export class BarMonthChartComponent {

  
  list1:WeekByDay[]=[];
  list2:WeekByDay[]=[];

  itemList: string[] = ['1','2','3','4','5','6','7','8','9','10','11','12','13','14','15','16','17','18','19'
  ,'20','21','22','23','24','25','26','27','28','29','30'];
  constructor(private deviceService:DevicesService) {
    
  }

  ngOnInit(): void {

    this.deviceService.monthByDay(2,2).subscribe((data:WeekByDay[])=>{
      console.log("Data => ", data);
      this.list1 = data;
      this.deviceService.monthByDay(2,1).subscribe((data:WeekByDay[])=>{
        console.log("Data => ", data);
        this.list2 = data;
        this.BarPlot();
      })
    })

    
  }
  BarPlot(){
    
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
