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

  consumptionGraph:boolean = false;
  productionGraph:boolean = false;

  constructor(private route:ActivatedRoute,private deviceService:HistoryPredictionService,private authService:AuthService) {
    this.maxDate = new Date();
  }
  list1:DayByHour[] = [];
  list2:DayByHour[] = [];
  
  selectedDate!: Date;

  onDateSelected(event: { value: Date; }) {
    this.selectedDate = event.value;
    this.ngOnInit();
  }

  ngOnInit(): void {
    const deviceId = Number(this.route.snapshot.paramMap.get('id'));
    this.authService.getDevice(deviceId).subscribe(data=>{
      if(this.selectedDate == undefined){
        if(data.deviceCategory == "Electricity Consumer")
        {
          this.deviceService.dayByHourDevice(deviceId).subscribe(consumption=>{
            this.list1 = consumption;
            this.consumptionGraph = true;
          })
          
        }
        else{
          this.deviceService.dayByHourDevice(deviceId).subscribe(production=>{
            this.list2 = production;
            this.productionGraph = true;
          })
        }
      }
      else if(this.selectedDate !== undefined){
        const day = this.selectedDate.getDate();
        const month = this.selectedDate.getMonth()+1;
        const year = this.selectedDate.getFullYear();
        let string1 = '';
        let string2 = '';
        if(month % 2 )
            {
              if(day == 30 || (month == 2 && day == 28)){
                string1 = year+'-'+month+'-'+day
                string2 = year+'-'+(month+1)+'-'+1
              }
              else{
                string1 = year+'-'+month+'-'+day
                string2 = year+'-'+month+'-'+(day+1)
              }
            }
            else if(month % 2 == 1){
              if(day == 31 || (month == 6 || month == 7) ){
                string1 = year+'-'+month+'-'+day
                string2 = year+'-'+(month+1)+'-'+1
              }
              else{
                string1 = year+'-'+month+'-'+day
                string2 = year+'-'+month+'-'+(day+1)
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
      }
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
    else{
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
