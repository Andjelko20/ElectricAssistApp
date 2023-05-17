import { Component, Injectable, ViewChild } from '@angular/core';
import { Chart,registerables } from 'node_modules/chart.js'
import { forkJoin } from 'rxjs';
import { WeekByDay } from 'src/app/models/devices.model';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import {DateAdapter} from '@angular/material/core';
import {
  MatDateRangeSelectionStrategy,
  DateRange,
  MAT_DATE_RANGE_SELECTION_STRATEGY,
  MatDatepickerInputEvent,
} from '@angular/material/datepicker';
import { ActivatedRoute } from '@angular/router';
import { FormControl, FormGroup } from '@angular/forms';
Chart.register(...registerables)

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
      const end = this._dateAdapter.addCalendarDays(date, 8);
      return new DateRange<D>(start, end);
    }

    return new DateRange<D>(null, null);
  }
}

@Component({
  selector: 'app-line-week-prosumer',
  templateUrl: './line-week-prosumer.component.html',
  styleUrls: ['./line-week-prosumer.component.css'],
  providers: [
    {
      provide: MAT_DATE_RANGE_SELECTION_STRATEGY,
      useClass: FiveDayRangeSelectionStrategy,
    },
  ],
})
export class LineWeekProsumerComponent {

  currentDate = new Date();
  maxDate = new Date(this.currentDate.getFullYear(), this.currentDate.getMonth(),this.currentDate.getDate()-7);
  firstdate = new Date(this.currentDate.getFullYear(), this.currentDate.getMonth(),this.currentDate.getDate()-7);
  list1:WeekByDay[] = [];
  list2:WeekByDay[] = [];
  constructor(private deviceService:HistoryPredictionService,private route:ActivatedRoute) {
    this.campaignOne.valueChanges.subscribe((value) => {
      this.sdate = value.start;
      this.send = value.end;
      if(this.send > this.currentDate){
        this.sdate = null;
      }
      else{
        this.ngOnInit()
      }
    });
  }
  campaignOne: FormGroup = new FormGroup({
    start: new FormControl(),
    end: new FormControl()
  });

  sdate = this.campaignOne.value.start;
  send = this.campaignOne.value.end;

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));

    if((this.sdate == null && this.send == null) || (this.sdate != null && this.send == null)){
      forkJoin([
        this.deviceService.weekByDayUser(id, 2),
        this.deviceService.weekByDayUser(id, 1),
      ]).subscribe(([list1, list2]) => {
        this.list1 = list1;
        this.list2 = list2;
        this.LineChartProduction();
        this.LineChartConsumption();
    });
    }
    else{
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
            this.deviceService.weekByDayUserFilter(string1,string2,id, 2),
            this.deviceService.weekByDayUserFilter(string1,string2,id, 1)
          ]).subscribe(([list1, list2]) => {
            this.list1 = list1;
            this.list2 = list2;
            this.LineChartProduction();
            this.LineChartConsumption();
          });
    }
    
  }
  LineChartProduction(){

    const chartId = 'linechart1';
    const chartExists = Chart.getChart(chartId);
    if (chartExists) {
        chartExists.destroy();
    }

    const energyUsageResults2 = this.list2.map(day => day.energyUsageResult);
    const month = this.list2.map(day => day.day);

    let max=0;
    if(energyUsageResults2[0]===0 && energyUsageResults2[1]===0 )
    {
      max=1;
    }
    const Linechart = new Chart("linechart1", {
      type: 'line',
      data : {
        labels: month,
        
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
              text: "Production (kWh)",
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
          legend: {display:false
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
    const month = this.list1.map(day => day.day);
    let max=0;
    if(energyUsageResults1[0]===0 && energyUsageResults1[1]===0 )
    {
      max=1;
    }
    const Linechart = new Chart("linechart2", {
      type: 'line',
      data : {
        labels: month,
        
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
              text: "Consumption (kWh)",
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
          legend: {display:false
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
