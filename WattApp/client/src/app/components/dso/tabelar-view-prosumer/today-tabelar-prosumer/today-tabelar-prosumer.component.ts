import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ExportToCsv } from 'export-to-csv';
import { combineLatest, forkJoin } from 'rxjs';
import { DayByHour } from 'src/app/models/devices.model';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';

@Component({
  selector: 'app-today-tabelar-prosumer',
  templateUrl: './today-tabelar-prosumer.component.html',
  styleUrls: ['./today-tabelar-prosumer.component.css']
})
export class TodayTabelarProsumerComponent implements OnInit{

  maxDate: Date;
  list1:DayByHour[] = [];
  list2:DayByHour[] = [];
  mergedList: { hour: number, day: number, month: string, year: number, consumption: number, production: number }[] = [];
  constructor(private route:ActivatedRoute,private deviceService:HistoryPredictionService) {
    this.maxDate = new Date();
  }
  
  selectedDate!: Date;

  onDateSelected(event: { value: Date; }) {
    this.selectedDate = event.value;
    this.ngOnInit();
  }

  ngOnInit(): void {
    const userId = Number(this.route.snapshot.paramMap.get('id'));
  
    if(this.selectedDate == undefined){
      combineLatest([
        this.deviceService.dayByHourUser(userId, 2),
        this.deviceService.dayByHourUser(userId, 1)
      ]).subscribe(([list1, list2]) => {
        this.list1 = list1;
        this.list2 = list2;
      });
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
        this.deviceService.dayByHourUserFilter(string1,string2,userId, 2),
        this.deviceService.dayByHourUserFilter(string1,string2,userId, 1)
      ]).subscribe(([list1, list2]) => {
        this.list1 = list1;
        this.list2 = list2;
      });
    }
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
      headers: ['Hour', 'Day', 'Month', 'Year', 'Consumption', 'Production']
    };

    const csvExporter = new ExportToCsv(options);
    const csvData = csvExporter.generateCsv(this.mergedList);

    }
  }

