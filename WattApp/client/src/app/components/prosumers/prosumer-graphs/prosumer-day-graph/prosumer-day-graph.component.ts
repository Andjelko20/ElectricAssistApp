import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ExportToCsv } from 'export-to-csv';
import { Chart,registerables } from 'node_modules/chart.js'
import { combineLatest, forkJoin } from 'rxjs';
import { DayByHour } from 'src/app/models/devices.model';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { JwtToken } from 'src/app/utilities/jwt-token';
Chart.register(...registerables)
@Component({
  selector: 'app-prosumer-day-graph',
  templateUrl: './prosumer-day-graph.component.html',
  styleUrls: ['./prosumer-day-graph.component.css']
})
export class ProsumerDayGraphComponent {

  loader:boolean=false;
  maxDate = new Date();
  currentDate = new Date();
  list1:DayByHour[] = [];
  list1pred: number[] = [];
  mergedList: { hour: number, day: number, month: string, year: number, consumption: number, production: number }[] = [];
  selectedDate: Date = new Date();

  constructor(private route:ActivatedRoute,private deviceService:HistoryPredictionService) {
    
  }
  
  onDateSelected(event: { value: Date; }) {
    this.selectedDate = event.value;
    this.ngOnInit();
  }

  ngOnInit(): void {
    this.loader=true;
    let token=new JwtToken();
    const userId = token.data.id as number;
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
        this.deviceService.dayByHourUserFilter(string1,string2,userId, 2),
        this.deviceService.dayByHourUserFilter(string1,string2,userId, 1)
      ]).subscribe(([list1]) => {
        this.loader=false;
        this.list1 = list1;
            this.list1pred = [];
            for (const obj of this.list1) {
              const increasedEnergy = obj.energyUsageResult * (1 + Math.random() * (0.20) - 0.01); // Increase energy property by random percentage
              this.list1pred.push(increasedEnergy);
            }
        this.LineChartConsumption();

      });
    }
  
  LineChartConsumption(){

    const chartId = 'linechart2';
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
    const Linechart =new Chart("linechart2", {
      type: 'line',
      data : {
        labels: hours,
        
        datasets: [
          {
            label: 'Consumption ',
            data: energyUsageResults1,
            tension:0.1,
            backgroundColor: 'rgba(127, 205, 187, 0.3)',
            borderColor: ' rgba(127, 205, 187, 1)',
            borderWidth: 1.5,
            pointBackgroundColor: 'rgba(127, 205, 187, 1)',
            pointBorderColor: 'rgba(127, 205, 187, 1)',
            pointBorderWidth: 8,
            pointRadius: 1,
            pointHoverRadius: 6,
            fill:true,
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
          legend:{display:false},
        
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
  downloadCSV(): void {
  const options = {
    fieldSeparator: ',',
    filename: 'consumption-day',
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
}
