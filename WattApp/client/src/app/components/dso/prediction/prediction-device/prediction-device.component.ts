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
  selector: 'app-prediction-device',
  templateUrl: './prediction-device.component.html',
  styleUrls: ['./prediction-device.component.css']
})

export class PredictionDeviceComponent {

  
  list1:WeekByDay[] = [];
  list2:WeekByDay[] = [];
  constructor(private deviceService:HistoryPredictionService,private route:ActivatedRoute,private authService:AuthService) {
    
  }
  consumptionGraph:boolean = false;
  productionGraph:boolean = false;
  ngOnInit(): void {
  
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.authService.getDevice(id).subscribe(data=>{
      if(data.deviceCategory == "Electricity Consumer")
      {
        this.consumptionGraph = true;
        this.deviceService.predictionDevice(id).subscribe(consumption =>{
          this.list1 = consumption;
          
          this.LineChartConsumption();
        })
        
      }
      else{
        
        this.productionGraph=true;
        
        this.deviceService.predictionDevice(id).subscribe(production =>{
   
          const br: any = 0;
          if(production==br)
          {
            console.log("Nemamo dovoljno podataka");
          }
          else{
            this.list2=production;
            this.LineChartProduction();
          }
         
         
        })
      }
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
    let max=0;
    if(energyUsageResults2[0]===0 && energyUsageResults2[1]===0 )
    {
      max=1;
    }
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
              text: "Prediction (kWh)",
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
        
        plugins: {
          datalabels:{display: false},
          legend:{
            display: false
          },
         
          title: {
            display: true,
            text: 'Prediction production in a week',
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
    const month2 = this.list1.map(day => day.day);
    let max=0;
    if(energyUsageResults1[0]===0 && energyUsageResults1[1]===0 )
    {
      max=1;
    }
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
        
        plugins: {
          datalabels:{display: false},
          legend:{
            display:false
          },
         
          title: {
            display: true,
            text: 'Prediction consuming in a week',
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
