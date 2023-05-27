import { Component } from '@angular/core';
import { ExportToCsv } from 'export-to-csv';
import { Chart,registerables } from 'node_modules/chart.js'
import { forkJoin } from 'rxjs';
import { DayByHour } from 'src/app/models/devices.model';
import { Settlement } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
Chart.register(...registerables)

@Component({
  selector: 'app-line-day-chart',
  templateUrl: './line-day-chart.component.html',
  styleUrls: ['./line-day-chart.component.css'],
})
export class LineDayChartComponent {

  loader:boolean=false;
  selectedOption: number;
  maxDate = new Date();
  currentDate = new Date();
  list1:DayByHour[] = [];
  list1pred:number[] = [];
  list2:DayByHour[] = [];
  list2pred:number[] = [];
  settlements:Settlement[] = [];
  mergedList: { hour: number, day: number, month: string, year: number, consumption: number, production: number}[] = [];
  constructor(private authService:AuthService,private deviceService:HistoryPredictionService) {
    this.selectedOption = 0;
  }
  onOptionSelected() {
    this.ngOnInit()
  }
  selectedDate: Date = new Date();
  onDateSelected(event: { value: Date; }) {
    this.selectedDate = event.value;
    this.ngOnInit();
  }
  ngOnInit(): void {
    this.loader=true;
    this.authService.getlogInUser().subscribe(user => {
      this.authService.getCityId(user.city).subscribe(number => {
        this.authService.getSettlement(number).subscribe((settlement: Settlement[]) => {
          this.loader=false;
          this.settlements = settlement;
          const selectElement = document.getElementById('dropdown') as HTMLSelectElement
          const selectedOptionName = selectElement.options[selectElement.selectedIndex].text;

          if (selectedOptionName === 'Total') {
            this.selectedOption = 0;
          } else {
            const selectedItem = this.settlements.find(item => item.name === selectedOptionName);
            if (selectedItem) {
              this.selectedOption = selectedItem.id;
            }
          }
        });
        if(this.selectedOption == 0 && this.selectedDate != undefined){
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
            else if( month == 6){
              string1 = year+'-'+month+'-'+day+' '+'00:00:00'
              string2 = year+'-'+(month+1)+'-'+1+' '+'00:00:00'
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
            
            this.deviceService.dayByHourCityFilter(string1,string2,number, 2),
            this.deviceService.dayByHourCityFilter(string1,string2,number, 1)
          ]).subscribe(([list1, list2]) => {
            this.list1 = list1;
            this.list1pred = [];
            for (const obj of this.list1) {
              const increasedEnergy = obj.energyUsageResult * (1 + Math.random() * (0.20) - 0.01);
              const roundedEnergy = increasedEnergy.toFixed(2);
              this.list1pred.push(Number(roundedEnergy));
            }
            this.list2 = list2;
            this.list2pred = [];
            for (const obj of this.list2) {
              const increasedEnergy = (obj.energyUsageResult * (1 + Math.random() * (0.20) - 0.01));
              const roundedEnergy = increasedEnergy.toFixed(2);
              this.list2pred.push(Number(roundedEnergy));
            }
            this.LineChartProduction();
            this.LineChartConsumption();
          });
        }
        else if(this.selectedOption != 0 && this.selectedDate != undefined){
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
            this.deviceService.dayByHourSettlementFilter(string1,string2,this.selectedOption,2),
            this.deviceService.dayByHourSettlementFilter(string1,string2,this.selectedOption,1)
          ]).subscribe(([list1, list2]) => {
            this.list1 = list1;
            this.list1pred = [];
            for (const obj of this.list1) {
              const increasedEnergy = obj.energyUsageResult * (1 + Math.random() * (0.20) - 0.01);
              const roundedEnergy = increasedEnergy.toFixed(2);
              this.list1pred.push(Number(roundedEnergy));
            }
            this.list2 = list2;
            this.list2pred = [];
            for (const obj of this.list2) {
              const increasedEnergy = obj.energyUsageResult * (1 + Math.random() * (0.20) - 0.01);
              const roundedEnergy = increasedEnergy.toFixed(2);
              this.list2pred.push(Number(roundedEnergy));
            }
            this.LineChartProduction();
            this.LineChartConsumption();
          });
        }
      });
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
        maintainAspectRatio:false,
        responsive: true,
        scales:{
          y: {
            ticks:{
              color:'gray',
              font:{
                size:15
              }
            },suggestedMax:max,
            position: "left",
            title:{
              display:true,
              text: "Production [kWh]",
              color:'gray',
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
              text: "Hours in a day",
              color:'gray',
              font:{
                size:15
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
            text: 'Production in one day',
            color: 'gray',
            font:{
              size:20
            }
          },
          
         
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
    const energyUsageResults1 = this.list1.map(day => day.energyUsageResult)
    
    const hours = this.list1.map(day => day.hour);
    let max=0;
    if(energyUsageResults1[0]===0 )
    {
      max=1;
    }
    const Linechart =new Chart("linechart2", {
      type: 'line',
      data : {
        labels: hours,
        
        datasets: [
          {
            label: ' Consumption',
            data: energyUsageResults1,
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
          {
            label: ' Prediction',
            data: this.list1pred,
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
        ],
        
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
        maintainAspectRatio:false,
        responsive: true,
        scales:{
          y: {
            ticks:{
              color:'gray',
              font:{
                size:15
              }
            },
            suggestedMax:max,
            position: "left",
            title:{
              display:true,
              text: "Consumption [kWh]",
              color:'gray',
              font:{
                size:15
              }
            }
          }
          ,
          x:{
            ticks:{
              color:'gray',
              font:{
                size:15
              }
            },
            title:{
              display:true,
              text: "Hours in a day",
              color:'gray',
              font:{
                size:15
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
            text: 'Consumption in one day',
            color: 'gray',
            font:{
              size:20
            }
          }
        }
      }
    });

  }
  downloadCSV(): void {
    this.mergedList = [];
    for (let i = 0; i < this.list1.length; i++) {
      for (let j = 0; j < this.list2.length; j++) {
        if (this.list1[i].hour === this.list2[j].hour && this.list1[i].day === this.list2[j].day && this.list1[i].month === this.list2[j].month && this.list1[i].year === this.list2[j].year) {
          this.mergedList.push({
            hour: this.list1[i].hour,
            day: this.list1[i].day,
            month: this.list1[i].month,
            year: this.list1[i].year,
            consumption: this.list1[i].energyUsageResult,
            production: this.list2[j].energyUsageResult
          });
          break;
        }
      }
  }
  const options = {
    fieldSeparator: ',',
    filename: 'consumption/production-day',
    quoteStrings: '"',
    useBom : true,
    decimalSeparator: '.',
    showLabels: true,
    useTextFile: false,
    headers: ['Hour', 'Day', 'Month', 'Year', 'Consumption [kWh]', 'Production [kWh]']
  };

  const csvExporter = new ExportToCsv(options);

  const csvData = csvExporter.generateCsv(this.mergedList);

  }
   
}
