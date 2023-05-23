import { Component, Injectable } from '@angular/core';
import { Chart,registerables } from 'node_modules/chart.js'
import { forkJoin } from 'rxjs';
import { WeekByDay } from 'src/app/models/devices.model';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import {DateAdapter} from '@angular/material/core';
import {
  MatDateRangeSelectionStrategy,
  DateRange,
  MAT_DATE_RANGE_SELECTION_STRATEGY,
} from '@angular/material/datepicker';
import { ActivatedRoute } from '@angular/router';
import { FormControl, FormGroup } from '@angular/forms';
import { AuthService } from 'src/app/services/auth.service';
import { ExportToCsv } from 'export-to-csv';
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
  firstdate = new Date(this.currentDate.getFullYear(), this.currentDate.getMonth(),this.currentDate.getDate()-7);
  consumptionGraph:boolean = false;
  productionGraph:boolean = false;
  list1:WeekByDay[] = [];
  list2:WeekByDay[] = [];
  dayNames: string[] = [];
  mergedList: { day: number, month: string, year: number, consumption: number, production: number }[] = [];
  constructor(private deviceService:HistoryPredictionService,private route:ActivatedRoute,private authService:AuthService) {
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
          this.dayNames = []
          const currentDate = new Date(this.sdate);
          const enddate = new Date(this.send)
          enddate.setDate(enddate.getDate()-1)
          while (currentDate <= enddate) {
            const dayName = currentDate.toLocaleDateString('en-US', { weekday: 'long' });
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
    let max=0;
    if(energyUsageResults2[0]===0 && energyUsageResults2[1]===0 )
    {
      max=1;
      
    }
    const Linechart = new Chart("linechart1", {
      type: 'bar',
      data : {
        labels: this.dayNames,
        
        datasets:  [
          
          {
            label: 'Production',
            data: energyUsageResults2,
            borderColor: 'rgba(29, 145, 192, 1)',
            backgroundColor: 'rgba(29, 145, 192, 0.2)',
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
              text:"Prediction (kWh)",
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
            text: 'Production in a week for ',
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
    let max=0;
    if(energyUsageResults1[0]===0 && energyUsageResults1[1]===0 )
    {
      max=1;
      
    }
    const Linechart = new Chart("linechart2", {
      type: 'bar',
      data : {
        labels: this.dayNames,
        
        datasets:  [
          {
            label: 'Consumption ',
            data: energyUsageResults1,
            borderColor:  'rgba(127, 205, 187, 1)',
            backgroundColor:  'rgba(127, 205, 187, 0.3)',
            borderWidth: 2.5,
            
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
              text:"Consumption (kWh)",
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
            text: ' Consumption in a week',
            color:'#000',
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
