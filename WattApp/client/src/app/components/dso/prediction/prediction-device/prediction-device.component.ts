import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ExportToCsv } from 'export-to-csv';
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

  loader:boolean=false;
  list1:WeekByDay[] = [];
  list2:WeekByDay[] = [];
  dayNames: string[] = [];
  constructor(private deviceService:HistoryPredictionService,private route:ActivatedRoute,private authService:AuthService) {
    
  }
  consumptionGraph:boolean = false;
  productionGraph:boolean = false;
  ngOnInit(): void {
    this.loader=true;
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.authService.getDevice(id).subscribe(data=>{
      
      if(data.deviceCategory == "Electricity Consumer")
      {
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
        this.consumptionGraph = true;
        this.loader=false;
        this.deviceService.predictionDevice(id).subscribe(consumption =>{
          
          this.list1 = consumption;
          
          this.LineChartConsumption();
          
        })
        
      }
      else{ 
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
        this.productionGraph=true;
        this.loader=false;
        this.deviceService.predictionDevice(id).subscribe(production =>{
          
        const br: any = 0;
          if(production==br)
          {
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

    const chartId = 'linechartP';
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
    const Linechart = new Chart("linechartP", {
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
              color:'gray',
              font:{
                size:13
              }
            },suggestedMax:max,
            position: "left",
            title:{
              display:true,
              text: "Prediction [kWh]",
              color:'gray',
              font:{
                size:13
              }
            }
          }
          ,
          x:{
            ticks:{
              color:'gray',
              font:{
                size:13
              }
            },
            title:{
              display:true,
              text: "Days in a week",
              color:'gray',
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
          legend: {
            labels:{
            color:'gray',
           
            font:{
              size:16
            },
            boxWidth:15,
            boxHeight:15,
            useBorderRadius:true,
            borderRadius:7
          },
            
            position: 'bottom',
            onHover: function (event, legendItem, legend) {
              document.body.style.cursor = 'pointer';
            },
            onLeave: function (event, legendItem, legend) {
                document.body.style.cursor = 'default';
            },
          },
         
          title: {
            display: true,
            text: 'Prediction production in a week',
            color:'gray',
            font:{
              size:15
            }
          }
        }
      }
    });

  }
  LineChartConsumption(){

    const chartId = 'linechartC';
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
    const Linechart = new Chart("linechartC", {
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
      },
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
              color:'gray',
              font:{
                size:13
              }
            },suggestedMax:max,
            position: "left",
            title:{
              display:true,
              text: "Consumption [kWh]",
              color:'gray',
              font:{
                size:13
              }
            }
          }
          ,
          x:{
            ticks:{
              color:'gray',
              font:{
                size:13
              }
            },
            title:{
              display:true,
              text: "Days in a week",
              color:'gray',
              font:{
                size:13
              }
            }
          },
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
          legend: {
            labels:{
            color:'gray',
           
            font:{
              size:16
            },
            boxWidth:15,
            boxHeight:15,
            useBorderRadius:true,
            borderRadius:7
          },
            
            position: 'bottom',
            onHover: function (event, legendItem, legend) {
              document.body.style.cursor = 'pointer';
            },
            onLeave: function (event, legendItem, legend) {
                document.body.style.cursor = 'default';
            },
          },
          title: {
            display: true,
            text: 'Prediction consuming in a week',
            color:'gray',
            font:{
              size:15
            }
          }
        }
      }
    });
  }
  downloadCSV(): void {
    const deviceId = Number(this.route.snapshot.paramMap.get('id'));
    this.authService.getDevice(deviceId).subscribe(data=>{
      if(data.deviceCategory == "Electricity Consumer"){
          const options = {
          fieldSeparator: ',',
          filename: 'consumption-week',
          quoteStrings: '"',
          useBom : true,
          decimalSeparator: '.',
          showLabels: true,
          useTextFile: false,
          headers: ['Hour', 'Day', 'Month', 'Year', 'Consumption [kWh]']
        };
        const csvExporter = new ExportToCsv(options);
        const csvData = csvExporter.generateCsv(this.list1);
      }
      else if(data.deviceCategory == "Electricity Producer"){
          const options = {
          fieldSeparator: ',',
          filename: 'production-week',
          quoteStrings: '"',
          useBom : true,
          decimalSeparator: '.',
          showLabels: true,
          useTextFile: false,
          headers: ['Hour', 'Month', 'Year', 'Production [kWh]']
        };
        const csvExporter = new ExportToCsv(options);
        const csvData = csvExporter.generateCsv(this.list2);
      }
    })
    } 
}
