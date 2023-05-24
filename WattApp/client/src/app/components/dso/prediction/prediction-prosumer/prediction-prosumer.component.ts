import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Chart,registerables } from 'node_modules/chart.js'
import { WeekByDay } from 'src/app/models/devices.model';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { JwtToken } from 'src/app/utilities/jwt-token';
Chart.register(...registerables)

@Component({
  selector: 'app-prediction-prosumer',
  templateUrl: './prediction-prosumer.component.html',
  styleUrls: ['./prediction-prosumer.component.css']
})
export class PredictionProsumerComponent {

  loader:boolean=false;
  list1:WeekByDay[] = [];
  list2:WeekByDay[] = [];
  dayNames: string[] = [];
  idProsumer!:number;
  constructor(private deviceService:HistoryPredictionService,private route:ActivatedRoute) {
    
  }

  ngOnInit(): void {
    this.loader=true;
    let token=new JwtToken();
    this.idProsumer=token.data.id as number;

          this.dayNames = []
          const currentDate = new Date();
          currentDate.setDate(currentDate.getDate()+1)
          const enddate = new Date()
          enddate.setDate(enddate.getDate()+7)
          while (currentDate <= enddate) {
            const dayName = currentDate.toLocaleDateString('en-US', { weekday: 'short' });
            this.dayNames.push(dayName);
            currentDate.setDate(currentDate.getDate() + 1 );
          }
    this.deviceService.predictionUser(this.idProsumer,2).subscribe((data: WeekByDay[]) =>{
      
      this.list1 = data;
      this.deviceService.predictionUser(this.idProsumer,1).subscribe((data: WeekByDay[]) =>{

        this.list2 = data;
        this.loader=false;
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
    let max=0;
    if(energyUsageResults2[0]===0 && energyUsageResults2[1]===0 )
    {
      max=1;
    }
    const Linechart = new Chart("linechart1", {
      type: 'line',
      data : {
        labels: this.dayNames,
        datasets:  [
          {
            label: 'production',
            data: energyUsageResults2,
            tension:0.1,
            backgroundColor: 'rgba(29, 145, 192, 0.2)',
            borderColor: 'rgba(29, 145, 192, 1)',
            borderWidth: 1,
            pointBackgroundColor: 'rgba(29, 145, 192, 1)',
            pointBorderColor: 'rgba(29, 145, 192, 1)',
            pointBorderWidth: 8,
            pointRadius: 1,
            pointHoverRadius: 6,
            fill:true,
            segment:{
              borderDash:[6,6]
            }
          } 
        ]
      }
      ,
      options: {
        onHover: (e, chartEle) => {
          if (e.native) {
            const target = e.native.target as HTMLElement;
            if (target instanceof HTMLElement) {
              target.style.cursor = chartEle.length > 0 && chartEle[0] ? 'pointer' : 'default';
            } else {
              console.error('Invalid target element:', target);
            }
          } else {
            console.error('Missing native event:', e);
          }
        },  
        maintainAspectRatio: false,
        responsive: true,
        scales:{
          y: {
            ticks:{
              color:'#000',
              font:{
                size:13
              }
            },suggestedMax:max,
            position: "left",
            title:{
              display:true,
              text: "Production (kWh)",
              color:'#000',
              font:{
                size:13
              }
            }
          }
          ,
          x:{
            ticks:{
              color:'#000',
              font:{
                size:13
              }
            },
            title:{
              display:true,
              text: "Days in a week",
              color:'#000',
              font:{
                size:13
              }
            }
          }
          ,
        },
        interaction: {
          intersect: false,
          mode: 'index',
        },
        plugins: {
          tooltip: {
            enabled: true,
            boxHeight:5,
            boxWidth:5,
            boxPadding:3
          },
          datalabels:{display: false},
          legend:{
            display:false
          },
          title: {
            display: true,
            text: 'Prediction in a week',
            color:'#000',
            font:{
              size:15
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
    let max=0;
    if(energyUsageResults1[0]===0 && energyUsageResults1[1]===0 )
    {
      max=1;
    }
    const Linechart = new Chart("linechart2", {
      type: 'line',
      data : {
        labels: this.dayNames,
        
        datasets:  [
          {
            label: 'consumption',
            data: energyUsageResults1,
            backgroundColor: 'rgba(127, 205, 187, 0.2)',
            borderColor: ' rgba(127, 205, 187, 1)',
            borderWidth: 2,
            pointBackgroundColor: 'rgba(127, 205, 187, 1)',
            pointBorderColor: 'rgba(127, 205, 187, 1)',
            pointBorderWidth: 8,
            pointRadius: 1,
            pointHoverRadius: 6,
            fill:true,
            segment:{
              borderDash:[6,6]
            }
          },
          
        ]
        
      }
      ,
      options: {
        onHover: (e, chartEle) => {
          if (e.native) {
            const target = e.native.target as HTMLElement;
            if (target instanceof HTMLElement) {
              target.style.cursor = chartEle.length > 0 && chartEle[0] ? 'pointer' : 'default';
            } else {
              console.error('Invalid target element:', target);
            }
          } else {
            console.error('Missing native event:', e);
          }
        },  
        maintainAspectRatio: false,
        responsive: true,
        scales:{
          y: {
            ticks:{
              color:'#000',
              font:{
                size:13
              }
            },suggestedMax:max,
            position: "left",
            title:{
              display:true,
              text: "Consumption (kWh)",
              color:'#000',
              font:{
                size:13
              }
            }
          }
          ,
          x:{
            ticks:{
              color:'#000',
              font:{
                size:13
              }
            },
            title:{
              display:true,
              text: "Days in a week",
              color:'#000',
              font:{
                size:13
              }
            }
          }
          ,
        },
        interaction: {
          intersect: false,
          mode: 'index',
        },
        plugins: {
          tooltip: {
            enabled: true,
            boxHeight:5,
            boxWidth:5,
            boxPadding:3
          },
          datalabels:{display: false},
          legend:{
            display:false
          },
          title: {
            display: true,
            text: ' Consumption in a week',
            color:'#000',
            font:{
              size:15
            }
          }
        }
      }
    });
  }
}
