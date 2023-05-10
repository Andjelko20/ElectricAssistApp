import { Component, Injectable } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Chart,registerables } from 'node_modules/chart.js'
import { forkJoin } from 'rxjs';
import { WeekByDay } from 'src/app/models/devices.model';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { JwtToken } from 'src/app/utilities/jwt-token';
import {DateAdapter} from '@angular/material/core';
import {
  MatDateRangeSelectionStrategy,
  DateRange,
  MAT_DATE_RANGE_SELECTION_STRATEGY,
  MatDatepickerInputEvent,
} from '@angular/material/datepicker';
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
      const end = this._dateAdapter.addCalendarDays(date, 7);
      return new DateRange<D>(start, end);
    }

    return new DateRange<D>(null, null);
  }
}
@Component({
  selector: 'app-prosumer-week-production',
  templateUrl: './prosumer-week-production.component.html',
  styleUrls: ['./prosumer-week-production.component.css'],
  providers: [
    {
      provide: MAT_DATE_RANGE_SELECTION_STRATEGY,
      useClass: FiveDayRangeSelectionStrategy,
    },
  ],
})
export class ProsumerWeekProductionComponent {

  currentDate = new Date();
  maxDate = new Date(this.currentDate.getFullYear(), this.currentDate.getMonth(),this.currentDate.getDate()-7);
  list1:WeekByDay[] = [];
  list2:WeekByDay[] = [];
  constructor(private deviceService:HistoryPredictionService,private route:ActivatedRoute) {
    this.campaignOne.valueChanges.subscribe((value) => {
      this.sdate = value.start;
      this.send = value.end;
      if(this.send > this.maxDate){
        this.send = null;
      }
      this.ngOnInit();
    });
  }
  campaignOne: FormGroup = new FormGroup({
    start: new FormControl(),
    end: new FormControl()
  });

  sdate = this.campaignOne.value.start;
  send = this.campaignOne.value.end;

  ngOnInit(): void {
    let token=new JwtToken();
    const id = token.data.id as number;

    if((this.sdate == null && this.send == null) || (this.sdate != null && this.send == null)){
      forkJoin([
        this.deviceService.weekByDayUser(id, 1),
      ]).subscribe(([list2]) => {
        this.list2 = list2;
        console.log(this.list2)
        this.LineChartProduction();
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
            this.deviceService.weekByDayUserFilter(string1,string2,id, 1)
          ]).subscribe(([list2]) => {
            this.list2 = list2;
            this.LineChartProduction();
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
            position: "left",
            title:{
              display:true,
              text: "kWh",
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
            text: 'Production in a week',
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

