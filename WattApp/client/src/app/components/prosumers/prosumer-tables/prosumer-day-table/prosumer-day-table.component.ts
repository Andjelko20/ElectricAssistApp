import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ExportToCsv } from 'export-to-csv';
import { combineLatest, forkJoin } from 'rxjs';
import { DayByHour } from 'src/app/models/devices.model';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { JwtToken } from 'src/app/utilities/jwt-token';

@Component({
  selector: 'app-prosumer-day-table',
  templateUrl: './prosumer-day-table.component.html',
  styleUrls: ['./prosumer-day-table.component.css']
})
export class ProsumerDayTableComponent {

  maxDate: Date;
  currentDate = new Date();
  list1:DayByHour[] = [];
  list2:DayByHour[] = [];
  mergedList: { hour: number, day: number, month: string, year: number, consumption: number, production: number }[] = [];
  constructor(private route:ActivatedRoute,private deviceService:HistoryPredictionService) {
    this.maxDate = new Date();
  }
  
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
        this.deviceService.dayByHourUserFilter(string1,string2,userId, 2),
        this.deviceService.dayByHourUserFilter(string1,string2,userId, 1)
      ]).subscribe(([list1, list2]) => {
        this.list1 = list1;
        this.list2 = list2;
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
      filename: 'consumption/production-day.csv',
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
