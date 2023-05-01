import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Chart,registerables } from 'node_modules/chart.js'
import { forkJoin, switchMap } from 'rxjs';
import { WeekByDay } from 'src/app/models/devices.model';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { JwtToken } from 'src/app/utilities/jwt-token';
Chart.register(...registerables)

@Component({
  selector: 'app-prosumer-month-graph',
  templateUrl: './prosumer-month-graph.component.html',
  styleUrls: ['./prosumer-month-graph.component.css']
})
export class ProsumerMonthGraphComponent {

  
  list1:WeekByDay[]=[];
  list2:WeekByDay[]=[];
  itemList: string[] = ['1','2','3','4','5','6','7','8','9','10','11','12','13','14','15','16','17','18','19'
  ,'20','21','22','23','24','25','26','27','28','29','30'];
  constructor(private deviceService:HistoryPredictionService,private route:ActivatedRoute) {
    
  }
  
  ngOnInit(): void {
    let token=new JwtToken();
    const userId = token.data.id as number;
    const month1$ = this.deviceService.monthByDayUser(userId, 2);
    const month2$ = this.deviceService.monthByDayUser(userId, 1);
  
    forkJoin([month1$, month2$]).pipe(
      switchMap(([data1, data2]) => {
        this.list1 = data1;
        this.list2 = data2;
        this.BarPlot();
        return [];
      })
    ).subscribe();
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
            datalabels: {
              display: false
            },
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
