import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Chart,registerables } from 'node_modules/chart.js'
import { WeekByDay } from 'src/app/models/devices.model';
import { Settlement } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { DevicesService } from 'src/app/services/devices.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { JwtToken } from 'src/app/utilities/jwt-token';
Chart.register(...registerables)

@Component({
  selector: 'app-prediction-prosumer',
  templateUrl: './prediction-prosumer.component.html',
  styleUrls: ['./prediction-prosumer.component.css']
})
export class PredictionProsumerComponent {


  list1:WeekByDay[] = [];
  list2:WeekByDay[] = [];
  idProsumer!:number;
  constructor(private deviceService:HistoryPredictionService,private route:ActivatedRoute) {
    
  }

  ngOnInit(): void {

    let token=new JwtToken();
    this.idProsumer=token.data.id as number;

    this.deviceService.predictionUser(this.idProsumer,2).subscribe((data: WeekByDay[]) =>{
      this.list1 = data;
      this.deviceService.predictionUser(this.idProsumer,1).subscribe((data: WeekByDay[]) =>{
        this.list2 = data;
        this.LineChartProduction();
        this.LineChartConsumption();
      })
    })
    
  }
  LineChartProduction(){

    const chartId = 'linechart1';
    const chartExists = Chart.getChart(chartId);
    if (chartExists) {
        chartExists.destroy();
    }

    const energyUsageResults2 = this.list2.map(day => day.energyUsageResult);
    const month1 = this.list2.map(day => day.day);
    const Linechart = new Chart("linechart1", {
      type: 'line',
      data : {
        labels: month1,
        
        datasets:  [
          
          {
            label: 'production',
            data: energyUsageResults2,
            tension:0.5,
            backgroundColor: 'rgba(0, 255, 0, 0.2)',
            borderColor: 'rgba(0, 255, 0, 1)',
            borderWidth: 2,
            pointBackgroundColor: 'rgba(0, 255, 0, 1)',
            pointBorderColor: 'rgba(0, 255, 0, 1)',
            pointBorderWidth: 7,
            pointRadius: 5,
            pointHoverRadius: 6,
            fill:true
          }
          
        ]
        
      }
      ,
      options: {
        responsive: true,
        scales:{
          y: {
            ticks:{
              color:'#000',
              font:{
                size:15
              }
            },
            position: "left",
            title:{
              display:true,
              text: "kWh",
              color:'#000',
              font:{
                size:15
              }
            }
          }
          ,
          x:{
            ticks:{
              color:'#000',
              font:{
                size:15
              }
            },
            title:{
              display:true,
              text: "Days in a week",
              color:'#000',
              font:{
                size:15
              }
            }
          }
          ,
        },
        
        plugins: {
          datalabels:{display: false},
          legend: {
            position: 'bottom',
            onHover: function (event, legendItem, legend) {
              document.body.style.cursor = 'pointer';
            },
            onLeave: function (event, legendItem, legend) {
                document.body.style.cursor = 'default';
            },
            labels:{
              usePointStyle: true,
              color:'#000',
              font:{
                size:20
              } 
           
            }
            ,
            align: "center"
          },
          title: {
            display: true,
            text: 'Production in a week',
            color:'#000',
            font:{
              size:20
            }
          }
        }
      }
    });

  }
  LineChartConsumption(){

    const chartId = 'linechart2';
    const chartExists = Chart.getChart(chartId);
    if (chartExists) {
        chartExists.destroy();
    }

    const energyUsageResults1 = this.list1.map(day => day.energyUsageResult);
    const month2 = this.list1.map(day => day.day);
    console.log(month2)
    const Linechart = new Chart("linechart2", {
      type: 'line',
      data : {
        labels: month2,
        
        datasets:  [
          {
            label: 'consumption',
            data: energyUsageResults1,
            backgroundColor: [
              'rgba(255, 99, 132, 0.2)',
              'rgba(54, 162, 235, 0.2)',
              'rgba(255, 206, 86, 0.2)',
              'rgba(75, 192, 192, 0.2)',
              'rgba(153, 102, 255, 0.2)',
              'rgba(255, 159, 64, 0.2)'
          ],
          borderColor: [
              'rgba(255,99,132,1)',
              'rgba(54, 162, 235, 1)',
              'rgba(255, 206, 86, 1)',
              'rgba(75, 192, 192, 1)',
              'rgba(153, 102, 255, 1)',
              'rgba(255, 159, 64, 1)'
          ],
          pointBorderColor: 'rgba(255,99,132,1)',
          pointBorderWidth: 7,
            pointRadius: 5,
          borderWidth: 2,
          fill: true
          },
          
        ]
        
      }
      ,
      options: {
        responsive: true,
        scales:{
          y: {
            ticks:{
              color:'#000',
              font:{
                size:15
              }
            },
            position: "left",
            title:{
              display:true,
              text: "kWh",
              color:'#000',
              font:{
                size:15
              }
            }
          }
          ,
          x:{
            ticks:{
              color:'#000',
              font:{
                size:15
              }
            },
            title:{
              display:true,
              text: "Days in a week",
              color:'#000',
              font:{
                size:15
              }
            }
          }
          ,
        },
        
        plugins: {
          datalabels:{display: false},
          legend: {
            position: 'bottom',
            onHover: function (event, legendItem, legend) {
              document.body.style.cursor = 'pointer';
            },
            onLeave: function (event, legendItem, legend) {
                document.body.style.cursor = 'default';
            },
            labels:{
              usePointStyle: true,
              color:'#000',
              font:{
                size:20
              } 
           
            }
            ,
            align: "center"
          },
          title: {
            display: true,
            text: ' Consumption in a week',
            color:'#000',
            font:{
              size:20
            }
          }
        }
      }
    });
  }
}
