import { Component, OnInit } from '@angular/core';
import { DayByHour } from 'src/app/models/devices.model';
import { Settlement } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { saveAs } from 'file-saver';
import { ExportToCsv } from 'export-to-csv';
import { forkJoin } from 'rxjs';
import { DatePipe } from '@angular/common';
@Component({
  selector: 'app-tabelar-view',
  templateUrl: './tabelar-view.component.html',
  styleUrls: ['./tabelar-view.component.css']
})
export class TabelarViewComponent implements OnInit{

  maxDate = new Date();
  currentDate = new Date();
  list1:DayByHour[] = [];
  list2:DayByHour[] = [];
  settlements:Settlement[] = [];
  selectedOption: number = 0;
  mergedList: { hour: number, day: number, month: string, year: number, consumption: number, production: number}[] = [];
  onOptionSelected() {
    this.ngOnInit();
  }
  constructor(private authService:AuthService,private deviceService:HistoryPredictionService,private datePipe: DatePipe) {}

  selectedDate: Date = new Date();
  dateTime:{day:string,hour:string}[] = [];
  onDateSelected(event: { value: Date; }) {
    this.selectedDate = event.value;
    this.ngOnInit();
  }

  ngOnInit(): void {
    this.authService.getlogInUser().subscribe(user=>{
      this.authService.getCityId(user.city).subscribe(number=>{
        this.authService.getSettlement(number).subscribe((settlement:Settlement[])=>{
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
        if(this.selectedOption == 0 && this.selectedDate !== undefined){
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
            this.deviceService.dayByHourCityFilter(string1,string2,number, 2),
            this.deviceService.dayByHourCityFilter(string1,string2,number, 1)
          ]).subscribe(([list1, list2]) => {
            this.list1 = list1;
            this.dateTime = [];
            for (let i = 0; i < this.list1.length; i++) {
              const pad = (num: number): string => (num < 10 ? '0' + num : String(num));
              const formattedHour = `${pad(this.list1[i].hour)}:00:00`;
              const formattedDay = `${pad(this.list1[i].day)}`;
              this.dateTime.push({
                hour : formattedHour,
                day : formattedDay
              })
            }
            this.list2 = list2;
          });
        }
        else if(this.selectedOption != 0 && this.selectedDate !== undefined){
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
            this.deviceService.dayByHourSettlementFilter(string1,string2,this.selectedOption,2),
            this.deviceService.dayByHourSettlementFilter(string1,string2,this.selectedOption,1)
          ]).subscribe(([list1, list2]) => {
            this.list1 = list1;
            this.dateTime = [];
            for (let i = 0; i < this.list1.length; i++) {
              const pad = (num: number): string => (num < 10 ? '0' + num : String(num));
              const formattedHour = `${pad(this.list1[i].hour)}:00:00`;
              const formattedDay = `${pad(this.list1[i].day)}`;
              this.dateTime.push({
                hour : formattedHour,
                day : formattedDay
              })
            }
            this.list2 = list2;
          });
        }
      })
    })
    
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
    headers: ['Hour', 'Day', 'Month', 'Year', 'Consumption [kWh]', 'Production [kWh]']
  };

  const csvExporter = new ExportToCsv(options);

  const csvData = csvExporter.generateCsv(this.mergedList);

  }
}
