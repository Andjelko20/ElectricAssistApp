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
import { AuthService } from 'src/app/services/auth.service';
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
  selector: 'app-device-week',
  templateUrl: './device-week.component.html',
  styleUrls: ['./device-week.component.css'],
  providers: [
    {
      provide: MAT_DATE_RANGE_SELECTION_STRATEGY,
      useClass: FiveDayRangeSelectionStrategy,
    },
  ],
})
export class DeviceWeekComponent {

  currentDate = new Date();
  maxDate = new Date(this.currentDate.getFullYear(), this.currentDate.getMonth(),this.currentDate.getDate()-7);
  consumptionGraph:boolean = false;
  productionGraph:boolean = false;
  list1:WeekByDay[] = [];
  list2:WeekByDay[] = [];
  constructor(private deviceService:HistoryPredictionService,private route:ActivatedRoute,private authService:AuthService) {
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
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.authService.getDevice(id).subscribe(data=>{
    if((this.sdate == null && this.send == null) || (this.sdate != null && this.send == null)){
      
        if(data.deviceCategory == "Electricity Consumer")
        {
          this.consumptionGraph = true;
          this.deviceService.weekByDayDevice(id).subscribe(consumption =>{
            this.list1 = consumption;
            this.LineChartConsumption()
          })
          
        }
        else{
          this.productionGraph = true;
          this.deviceService.weekByDayDevice(id).subscribe(production =>{
            this.list2 = production;
            this.LineChartProduction();
          })
        }
      
      
    }
    else{
      
      const day1 = this.sdate.getDate();
          const month1 = this.sdate.getMonth()+1;
          const year1 = this.sdate.getFullYear();
          const day2 = this.send.getDate();
          const month2 = this.send.getMonth()+1;
          const year2 = this.send.getFullYear();
          let string1 = year1+'-'+month1+'-'+day1;
          let string2 = year2+'-'+month2+'-'+day2;
          forkJoin([
            this.deviceService.weekByDayDeviceFilter(string1,string2,id, 2),
            this.deviceService.weekByDayDeviceFilter(string1,string2,id, 1)
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

    const chartId = 'linechart1';
    const chartExists = Chart.getChart(chartId);
    if (chartExists) {
        chartExists.destroy();
    }

    const energyUsageResults2 = this.list2.map(day => day.energyUsageResult);
    const month1 = this.list2.map(day => day.day);
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
        responsive: true,
        scales:{
          y: {
            ticks:{
              color:'#000',
              font:{
                size:15
              }
            },
            position: "left",
            title:{
              display:true,
              text: "kWh",
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
          legend: {
            position: 'bottom',
            onHover: function (event, legendItem, legend) {
              document.body.style.cursor = 'pointer';
            },
            onLeave: function (event, legendItem, legend) {
                document.body.style.cursor = 'default';
            },
            labels:{
              usePointStyle: true,
              color:'#000',
              font:{
                size:20
              } 
           
            }
            ,
            align: "center"
          },
          title: {
            display: true,
            text: 'Production in a week for ',
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
    const month2 = this.list1.map(day => day.day);
    console.log(month2)
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
        responsive: true,
        scales:{
          y: {
            ticks:{
              color:'#000',
              font:{
                size:15
              }
            },
            position: "left",
            title:{
              display:true,
              text: "kWh",
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
          legend: {
            position: 'bottom',
            onHover: function (event, legendItem, legend) {
              document.body.style.cursor = 'pointer';
            },
            onLeave: function (event, legendItem, legend) {
                document.body.style.cursor = 'default';
            },
            labels:{
              usePointStyle: true,
              color:'#000',
              font:{
                size:20
              } 
           
            }
            ,
            align: "center"
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
