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
  selector: 'app-prosumer-day-production',
  templateUrl: './prosumer-day-production.component.html',
  styleUrls: ['./prosumer-day-production.component.css']
})
export class ProsumerDayProductionComponent {
 
  maxDate = new Date();
  currentDate = new Date();
  mergedList: { hour: number, day: number, month: string, year: number, consumption: number, production: number }[] = [];
  constructor(private route:ActivatedRoute,private deviceService:HistoryPredictionService) {
    
  }
  list2:DayByHour[] = [];
  list2pred: number[] = [];
  selectedDate: Date = new Date();

  onDateSelected(event: { value: Date; }) {
    this.selectedDate = event.value;
    this.ngOnInit();
  }

  ngOnInit(): void {
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

        this.deviceService.dayByHourUserFilter(string1,string2,userId, 1)
      ]).subscribe(([list2]) => {
        this.list2 = list2;
            this.list2pred = [];
            for (const obj of this.list2) {
              const increasedEnergy = obj.energyUsageResult * (1 + Math.random() * (0.20) - 0.01); // Increase energy property by random percentage
              this.list2pred.push(increasedEnergy);
            }
        this.LineChartProduction();

      });
    }

  LineChartProduction(){

    const chartId = 'linechart1';
    const chartExists = Chart.getChart(chartId);
    if (chartExists) {
        chartExists.destroy();
    }
    const energyUsageResults2 = this.list2.map(day => day.energyUsageResult);
    const hours = this.list2.map(day => day.hour);
    let max=0;
    if(energyUsageResults2[0]===0 )
    {
      max=1;
      
    }
    const Linechart =new Chart("linechart1", {
      type: 'line',
      data : {
        labels: hours,
        
        datasets: [
         {
            label: ' Production',
            data: energyUsageResults2,
            backgroundColor: 'rgba(29, 145, 192, 0.2)',
            borderColor: 'rgba(29, 145, 192, 1)',
            borderWidth: 1,
            pointBackgroundColor: 'rgba(29, 145, 192, 1)',
            pointBorderColor: 'rgba(29, 145, 192, 1)',
            pointBorderWidth: 8,
            pointRadius: 1,
            pointHoverRadius: 6,
            fill:true,
            
          },
          {
            label: ' Prediction',
            data: this.list2pred,
            borderColor: 'rgba(252, 129, 155, 1)',
            borderWidth: 2,
            pointBackgroundColor: 'rgba(252, 129, 155, 1)',
            pointBorderColor: 'rgba(252, 129, 155, 1)',
            pointBorderWidth: 8,
            pointRadius: 1,
            pointHoverRadius: 6,
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
            },suggestedMax:max ,
            position: "left",
            title:{
              display:true,
              text: "Production [kWh]",
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
            color:'#000',
           
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
            text: 'Production in one day',
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
    filename: 'production-day',
    quoteStrings: '"',
    useBom : true,
    decimalSeparator: '.',
    showLabels: true,
    useTextFile: false,
    headers: ['Hour', 'Day', 'Month', 'Year', 'Production [kWh]']
  };

  const csvExporter = new ExportToCsv(options);
  const csvData = csvExporter.generateCsv(this.list2);

  }
}


