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

@Component({
  selector: 'app-device-week-tabular',
  templateUrl: './device-week-tabular.component.html',
  styleUrls: ['./device-week-tabular.component.css'],
  providers: [
    {
      provide: MAT_DATE_RANGE_SELECTION_STRATEGY,
      useClass: FiveDayRangeSelectionStrategy,
    },
  ],
})
export class DeviceWeekTabularComponent {


  consumptionGraph:boolean = false;
  productionGraph:boolean = false;
  currentDate = new Date();
  maxDate = new Date(this.currentDate.getFullYear(), this.currentDate.getMonth(),this.currentDate.getDate()-7);
  firstdate = new Date(this.currentDate.getFullYear(), this.currentDate.getMonth(),this.currentDate.getDate()-7);
  list1:WeekByDay[] = [];
  list2:WeekByDay[] = [];
  dateTime: any[] = [];
  datePipe: any;
  constructor(private deviceService:HistoryPredictionService,private route:ActivatedRoute,private authService:AuthService) {
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
  campaignOne: FormGroup = new FormGroup({
    start: new FormControl(),
    end: new FormControl()
  });

  sdate = this.firstdate;
  send = this.currentDate;

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.authService.getDevice(id).subscribe(data=>{
    if((this.sdate == null && this.send == null) || (this.sdate != null && this.send == null)){
      
        if(data.deviceCategory == "Electricity Consumer")
        {
          this.deviceService.weekByDayDevice(id).subscribe(consumption =>{
            this.list1 = consumption;
            this.consumptionGraph = true;
          })
          
        }
        else{
          this.deviceService.weekByDayDevice(id).subscribe(production =>{
            this.list2 = production;
            this.productionGraph = true;
          })
        }
      
      
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
            this.deviceService.weekByDayDeviceFilter(string1,string2,id, 2),
            this.deviceService.weekByDayDeviceFilter(string1,string2,id, 1)
          ]).subscribe(([list1, list2]) => {
            if(data.deviceCategory == "Electricity Consumer"){
              this.list1 = list1;
              this.dateTime = [];
              for (let i = 0; i < this.list1.length; i++) {
                const pad = (num: number): string => (num < 10 ? '0' + num : String(num));
                const formattedDay = `${pad(this.list1[i].day)}`;
                this.dateTime.push(formattedDay)
              }
              this.consumptionGraph = true;

            }
            else{
              this.list2 = list2;
              this.dateTime = [];
              for (let i = 0; i < this.list2.length; i++) {
                const pad = (num: number): string => (num < 10 ? '0' + num : String(num));
                const formattedDay = `${pad(this.list2[i].day)}`;
                this.dateTime.push(formattedDay)
              }
              this.productionGraph = true;
            }
          });
      }
    })
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
