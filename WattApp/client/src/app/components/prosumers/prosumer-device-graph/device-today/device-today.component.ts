import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Chart,registerables } from 'node_modules/chart.js'
import { combineLatest, forkJoin } from 'rxjs';
import { DayByHour } from 'src/app/models/devices.model';
import { AuthService } from 'src/app/services/auth.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
Chart.register(...registerables)

@Component({
  selector: 'app-device-today',
  templateUrl: './device-today.component.html',
  styleUrls: ['./device-today.component.css']
})
export class DeviceTodayComponent {

  currentDate = new Date();
  maxDate = new Date();
  consumptionGraph:boolean = false;
  productionGraph:boolean = false;  
  constructor(private route:ActivatedRoute,private deviceService:HistoryPredictionService,private authService:AuthService) {
  }
  list1:DayByHour[] = [];
  list2:DayByHour[] = [];
  
  selectedDate!: Date;

  onDateSelected(event: { value: Date; }) {
    this.selectedDate = event.value;
    this.ngOnInit();
  }

  ngOnInit(): void {
    const deviceId = Number(this.route.snapshot.paramMap.get('id'));
    this.authService.getDevice(deviceId).subscribe(data=>{
      if(this.selectedDate == undefined){
        if(data.deviceCategory == "Electricity Consumer")
        {
          this.consumptionGraph = true;
          this.deviceService.dayByHourDevice(deviceId).subscribe(consumption=>{
            this.list1 = consumption;
            this.LineChartConsumption();
          })
          
        }
        else{
          this.productionGraph = true;
          this.deviceService.dayByHourDevice(deviceId).subscribe(production=>{
            this.list2 = production;
            this.LineChartProduction();
          })
        }
      }
      else if(this.selectedDate !== undefined){
        const day = this.selectedDate.getDate();
        let dayString = String(day).padStart(2, '0');
        const month = this.selectedDate.getMonth()+1;
        let monthString = String(month).padStart(2, '0');
        const year = this.selectedDate.getFullYear();
        let string1 = '';
        let string2 = '';
        if(month % 2 == 0)
        {
          if(day == 30 || (month == 2 && day == 28)){
            string1 = year+'-'+monthString+'-'+dayString+' '+'00:00:00'
            monthString = String(month+1).padStart(2, '0');
            string2 = year+'-'+monthString+'-0'+1+' '+'00:00:00'
          }
          else if( month == 12){
            string1 = year+'-'+monthString+'-'+dayString+' '+'00:00:00'
            string2 = (year+1)+'-0'+1+'-0'+1+' '+'00:00:00'
          }
          else{
            string1 = year+'-'+monthString+'-'+dayString+' '+'00:00:00'
            dayString = String(day+1).padStart(2, '0');
            string2 = year+'-'+monthString+'-'+dayString+' '+'00:00:00'
          }
        }
        else{
          if(day == 31){
            string1 = year+'-'+monthString+'-'+dayString+' '+'00:00:00'
            monthString = String(month+1).padStart(2, '0');
            string2 = year+'-'+monthString+'-0'+1+' '+'00:00:00'
          }
          else{
            string1 = year+'-'+monthString+'-'+dayString+' '+'00:00:00'
            dayString = String(day+1).padStart(2, '0');
            string2 = year+'-'+monthString+'-'+dayString+' '+'00:00:00'
          }
        }
        forkJoin([
          this.deviceService.dayByHourDeviceFilter(string1,string2,deviceId, 2),
          this.deviceService.dayByHourDeviceFilter(string1,string2,deviceId, 1)
        ]).subscribe(([list1, list2]) => {
          if(data.deviceCategory == "Electricity Consumer"){
            this.list1 = list1;
            this.consumptionGraph = true;
            this.LineChartConsumption();
          }
          else{
            this.list2 = list2;
            this.productionGraph = true;
            this.LineChartProduction();
          }
        });
      }
    })
    
    
  }
  LineChartProduction(){

    const chartId = 'linechart11';
    const chartExists = Chart.getChart(chartId);
    if (chartExists) {
        chartExists.destroy();
    }
    const energyUsageResults2 = this.list2.map(day => day.energyUsageResult);
    const hours = this.list2.map(day => day.hour);
    let max=0;
    if(energyUsageResults2[0]===0 && energyUsageResults2[1]===0 )
    {
      max=1;
    }
    const Linechart =new Chart("linechart11", {
      type: 'line',
      data : {
        labels: hours,
        
        datasets: [
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
            },
            
            suggestedMax:max ,
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
              text: "Hours in a day",
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
            text: 'Production in one day',
            color: '#000',
            font:{
              size:20
            }
          }
        }
      }
    });

  }
  LineChartConsumption(){

    const chartId = 'linechart22';
    const chartExists = Chart.getChart(chartId);
    if (chartExists) {
        chartExists.destroy();
    }
    const energyUsageResults1 = this.list1.map(day => day.energyUsageResult);
    const hours = this.list1.map(day => day.hour);
    let max=0;
    if(energyUsageResults1[0]===0 && energyUsageResults1[1]===0 )
    {
      max=1;
      
    }
    const Linechart =new Chart("linechart22", {
      type: 'line',
      data : {
        labels: hours,
        
        datasets: [
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
            },
            
            suggestedMax:max ,
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
              text: "Hours in a day",
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
          legend:{
            display:false
          },
         
          title: {
            
            display: true,
            text: 'Consumption in one day',
            color: '#000',
            font:{
              size:15
            }
          }
        }
      }
    });

  }
}
