import { Component, Injectable, ViewChild } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { Chart,registerables } from 'node_modules/chart.js'
import { forkJoin } from 'rxjs';
import { WeekByDay } from 'src/app/models/devices.model';
import { Settlement } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';

import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import {DateAdapter} from '@angular/material/core';
import {
  MatDateRangeSelectionStrategy,
  DateRange,
  MAT_DATE_RANGE_SELECTION_STRATEGY,
  MatDatepickerInputEvent,
} from '@angular/material/datepicker';
import { ExportToCsv } from 'export-to-csv';


@Injectable()
export class FiveDayRangeSelectionStrategy<D> implements MatDateRangeSelectionStrategy<D> {
  constructor(private _dateAdapter: DateAdapter<D>) {}

  selectionFinished(date: D | null): DateRange<D> {
    return this._createFiveDayRange(date);
  }

  createPreview(activeDate: D | null): DateRange<D> {
    return this._createFiveDayRange(activeDate);
  }

  private _createFiveDayRange(date: D | null): DateRange<D> {
    if (date) {
      const start = this._dateAdapter.addCalendarDays(date, 0);
      const end = this._dateAdapter.addCalendarDays(date, 7);
      return new DateRange<D>(start, end);
    }

    return new DateRange<D>(null, null);
  }
}
Chart.register(...registerables)
@Component({
  selector: 'app-line-week-chart',
  templateUrl: './line-week-chart.component.html',
  styleUrls: ['./line-week-chart.component.css'],
  providers: [
    {
      provide: MAT_DATE_RANGE_SELECTION_STRATEGY,
      useClass: FiveDayRangeSelectionStrategy,
    },
  ],
})
export class LineWeekChartComponent {

  loader:boolean=false;
  currentDate = new Date();
  maxDate = new Date(this.currentDate.getFullYear(), this.currentDate.getMonth(),this.currentDate.getDate()-7);
  firstdate = new Date(this.currentDate.getFullYear(), this.currentDate.getMonth(),this.currentDate.getDate()-7);
  list1:WeekByDay[] = [];
  list2:WeekByDay[] = [];
  dayNames: string[] = [];
  settlements:Settlement[] = [];
  mergedList: { day: number, month: string, year: number, consumption: number, production: number }[] = [];
  list1pred: number[] = [];
  list2pred: number[] = [];
  constructor(private deviceService:HistoryPredictionService,private authService:AuthService) {
    this.campaignOne.valueChanges.subscribe((value) => {
      this.sdate = value.start;
      if(value.end == null){
        this.send = this.currentDate;
      }
      else{
        this.send = value.end
      }
      this.ngOnInit();
    });
  }

  selectedOption: number = 0;

  onOptionSelected() {
    this.ngOnInit();
  }

  campaignOne: FormGroup = new FormGroup({
    start: new FormControl(),
    end: new FormControl()
  });

  sdate = this.firstdate;
  send = this.currentDate;

  ngOnInit(): void {
    this.loader=true;
    this.authService.getlogInUser().subscribe(user=>{
      this.authService.getCityId(user.city).subscribe(number=>{
        this.authService.getSettlement(number).subscribe((settlement:Settlement[])=>{
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
        })
        if(this.selectedOption == 0 && (this.sdate != null && this.send != null)){
          this.dayNames = []
          const currentDate = new Date(this.sdate);
          const enddate = new Date(this.send)
          enddate.setDate(enddate.getDate()-1)
          while (currentDate <= enddate) {
            const dayName = currentDate.toLocaleDateString('en-US', { weekday: 'short' });
            this.dayNames.push(dayName);
            currentDate.setDate(currentDate.getDate() + 1 );
          }

          const day1 = this.sdate.getDate();
          const month1 = this.sdate.getMonth()+1;
          let dayString1 = String(day1).padStart(2, '0');
          let monthString1 = String(month1).padStart(2, '0');
          const year1 = this.sdate.getFullYear();
          const day2 = this.send.getDate();
          const month2 = this.send.getMonth()+1;
          let dayString2 = String(day2).padStart(2, '0');
          let monthString2 = String(month2).padStart(2, '0');
          const year2 = this.send.getFullYear();
          let string1 = year1+'-'+monthString1+'-'+dayString1+' '+'00:00:00';
          let string2 = year2+'-'+monthString2+'-'+dayString2+' '+'00:00:00';

          forkJoin([
            this.deviceService.weekByDayCityFilter(string1,string2,number, 2),
            this.deviceService.weekByDayCityFilter(string1,string2,number, 1)
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
        else if(this.selectedOption != 0 && (this.sdate != null && this.send != null)){
          this.dayNames = []
          const currentDate = new Date(this.sdate);
          const enddate = new Date(this.send)
          enddate.setDate(enddate.getDate()-1)
          while (currentDate <= enddate) {
            const dayName = currentDate.toLocaleDateString('en-US', { weekday: 'short' });
            this.dayNames.push(dayName);
            currentDate.setDate(currentDate.getDate() + 1 );
          }
          const day1 = this.sdate.getDate();
          const month1 = this.sdate.getMonth()+1;
          let dayString1 = String(day1).padStart(2, '0');
          let monthString1 = String(month1).padStart(2, '0');
          const year1 = this.sdate.getFullYear();
          const day2 = this.send.getDate();
          const month2 = this.send.getMonth()+1;
          let dayString2 = String(day2).padStart(2, '0');
          let monthString2 = String(month2).padStart(2, '0');
          const year2 = this.send.getFullYear();
          let string1 = year1+'-'+monthString1+'-'+dayString1+' '+'00:00:00';
          let string2 = year2+'-'+monthString2+'-'+dayString2+' '+'00:00:00';

          forkJoin([
            this.deviceService.weekByDaySettlementFilter(string1,string2,number, this.selectedOption),
            this.deviceService.weekByDaySettlementFilter(string1,string2,number, this.selectedOption)
          ]).subscribe(([list1, list2]) => {
            this.list1 = list1;
            this.list1pred = [];
            for (const obj of this.list1) {
              const increasedEnergy = obj.energyUsageResult * (1 + Math.random() * (0.20) - 0.01); // Increase energy property by random percentage
              const roundedEnergy = increasedEnergy.toFixed(2);
              this.list1pred.push(Number(roundedEnergy));
            }
            this.list2 = list2;
            this.list2pred = [];
            for (const obj of this.list2) {
              const increasedEnergy = obj.energyUsageResult * (1 + Math.random() * (0.20) - 0.01); // Increase energy property by random percentage
              const roundedEnergy = increasedEnergy.toFixed(2);
              this.list2pred.push(Number(roundedEnergy));
            }
            this.LineChartProduction();
            this.LineChartConsumption();
          });
        }
        
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
    if(energyUsageResults2[0]===0)
    {
      max=1;
    }
    const Linechart = new Chart("linechart1", {
      type: 'bar',
      data : {
        labels: this.dayNames,
        datasets:  [
          {
            label: ' Production',
            data: energyUsageResults2,
            borderColor: 'rgba(29, 145, 192, 1)',
            backgroundColor: 'rgba(29, 145, 192, 0.2)',
            borderWidth: 2,
          },
          {
            label: ' Prediction',
            data: this.list2pred,
            borderColor: 'rgba(252, 129, 155, 1)',
            backgroundColor: 'rgba(252, 129, 155, 0.2)',
            borderWidth: 2,
            
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
        maintainAspectRatio:false,
        responsive: true,
        scales:{
          y: {
            ticks:{
              color:'#000',
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
              color:'gray',
              font:{
                size:15
              }
            },
            title:{
              display:true,
              text: "Days in a week",
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
            text: ' Production in a week',
            color:'gray',
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
    let max=0;
    if(energyUsageResults1[0]===0 )
    {
      max=1;
    }
    const Linechart = new Chart("linechart2", {
      type: 'bar',
      data : {
        labels: this.dayNames,
        
        datasets:  [
          {
            label: ' Consumption',
            data: energyUsageResults1,
            borderColor:  'rgba(127, 205, 187, 1)',
            backgroundColor:  'rgba(127, 205, 187, 0.3)',
            borderWidth: 2.5,
          },
          {
            label: ' Prediction',
            data: this.list1pred,
            borderColor: 'rgba(252, 129, 155, 1)',
            backgroundColor: 'rgba(252, 129, 155, 0.2)',
            borderWidth: 2,
            
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
              text: "Days in a week",
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
            text: ' Consumption in a week',
            color:'gray',
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
        if (this.list1[i].day === this.list2[j].day && this.list1[i].month === this.list2[j].month && this.list1[i].year === this.list2[j].year) {
          this.mergedList.push({
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
    filename: 'consumption/production-week',
    quoteStrings: '"',
    useBom : true,
    decimalSeparator: '.',
    showLabels: true,
    useTextFile: false,
    headers: ['Day', 'Month', 'Year', 'Consumption [kWh]', 'Production [kWh]']
  };

  const csvExporter = new ExportToCsv(options);
  const csvData = csvExporter.generateCsv(this.mergedList);

  }
}
