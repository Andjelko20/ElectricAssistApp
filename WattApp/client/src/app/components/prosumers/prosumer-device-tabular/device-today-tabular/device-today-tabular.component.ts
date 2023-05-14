import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ExportToCsv } from 'export-to-csv';
import { Chart,registerables } from 'node_modules/chart.js'
import { combineLatest, forkJoin } from 'rxjs';
import { DayByHour } from 'src/app/models/devices.model';
import { AuthService } from 'src/app/services/auth.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
@Component({
  selector: 'app-device-today-tabular',
  templateUrl: './device-today-tabular.component.html',
  styleUrls: ['./device-today-tabular.component.css']
})
export class DeviceTodayTabularComponent {
  maxDate: Date;
  currentDate = new Date();
  consumptionGraph:boolean = false;
  productionGraph:boolean = false;
  datePipe: any;

  constructor(private route:ActivatedRoute,private deviceService:HistoryPredictionService,private authService:AuthService) {
    this.maxDate = new Date();
  }
  list1:DayByHour[] = [];
  list2:DayByHour[] = [];
  
  selectedDate: Date = new Date();

  onDateSelected(event: { value: Date; }) {
    this.selectedDate = event.value;
    this.ngOnInit();
  }

  ngOnInit(): void {
    const deviceId = Number(this.route.snapshot.paramMap.get('id'));
    this.authService.getDevice(deviceId).subscribe(data=>{
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
          }
          else{
            this.list2 = list2;
            this.productionGraph = true;
          }
        });
    })
  }  
  downloadCSV(): void {
  const deviceId = Number(this.route.snapshot.paramMap.get('id'));
  this.authService.getDevice(deviceId).subscribe(data=>{
    if(data.deviceCategory == "Electricity Consumer"){
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
    else if(data.deviceCategory == "Electricity Producer"){
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
  })
  } 
}
