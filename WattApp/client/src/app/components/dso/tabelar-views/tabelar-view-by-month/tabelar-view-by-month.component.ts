import { Component, OnInit } from '@angular/core';
import { WeekByDay } from 'src/app/models/devices.model';
import { Settlement } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { DevicesService } from 'src/app/services/devices.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { saveAs } from 'file-saver';
import { ExportToCsv } from 'export-to-csv';
import { forkJoin } from 'rxjs';
import { FormControl, FormGroup } from '@angular/forms';

@Component({
  selector: 'app-tabelar-view-by-month',
  templateUrl: './tabelar-view-by-month.component.html',
  styleUrls: ['./tabelar-view-by-month.component.css']
})
export class TabelarViewByMonthComponent implements OnInit{
  list1:WeekByDay[]=[];
  list2:WeekByDay[]=[];
  settlements:Settlement[] = [];
  mergedList: { day: number, month: string, year: number, consumption: number, production: number }[] = [];
  constructor(private deviceService:HistoryPredictionService,private authService:AuthService){}


  campaignOne: FormGroup = new FormGroup({
    start: new FormControl(),
    end: new FormControl()
  });
  maxDate = new Date();

  selectedOption: number = 0;

  onOptionSelected() {
    this.ngOnInit();
  }
  ngOnInit(): void {

    this.authService.getlogInUser().subscribe(user=>{
      this.authService.getCityId(user.city).subscribe(number=>{
        this.authService.getSettlement(number).subscribe((settlement:Settlement[])=>{
          this.settlements = settlement;
        })
        if(this.selectedOption == 0){
          forkJoin([
            this.deviceService.monthByDay(number, 2),
            this.deviceService.monthByDay(number, 1)
          ]).subscribe(([data1, data2]) => {
            this.list1 = data1;
            this.list2 = data2;
          });
        }
        else{
          forkJoin([
            this.deviceService.monthByDaySettlement(this.selectedOption, 2),
            this.deviceService.monthByDaySettlement(this.selectedOption, 1)
          ]).subscribe(([data1, data2]) => {
            this.list1 = data1;
            this.list2 = data2;
          });
        }
      })
    })
  }
  downloadCSV(): void {
    this.mergedList = [];
    for (let i = 0; i < this.list1.length; i++) {
      for (let j = 0; j < this.list2.length; j++) {
        if (this.list1[i].day === this.list2[j].day && this.list1[i].month === this.list2[j].month && this.list1[i].year === this.list2[j].year) {
          this.mergedList.push({
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
    filename: 'consumption/production-month.csv',
    quoteStrings: '"',
    useBom : true,
    decimalSeparator: '.',
    showLabels: true,
    useTextFile: false,
    headers: ['Day', 'Month', 'Year', 'Consumption', 'Production']
  };

  const csvExporter = new ExportToCsv(options);
  const csvData = csvExporter.generateCsv(this.mergedList);

  }
  

}
