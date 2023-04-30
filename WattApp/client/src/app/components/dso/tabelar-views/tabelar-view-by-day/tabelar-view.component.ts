import { Component, OnInit } from '@angular/core';
import { DayByHour } from 'src/app/models/devices.model';
import { Settlement } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { saveAs } from 'file-saver';
import { ExportToCsv } from 'export-to-csv';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-tabelar-view',
  templateUrl: './tabelar-view.component.html',
  styleUrls: ['./tabelar-view.component.css']
})
export class TabelarViewComponent implements OnInit{

  list1:DayByHour[] = [];
  list2:DayByHour[] = [];
  settlements:Settlement[] = [];
  selectedOption: number = 0;
  mergedList: { hour: number, day: number, month: string, year: number, consumption: number, production: number }[] = [];
  onOptionSelected() {
    this.ngOnInit();
  }
  constructor(private authService:AuthService,private deviceService:HistoryPredictionService) {}
  ngOnInit(): void {
    this.authService.getlogInUser().subscribe(user=>{
      this.authService.getCityId(user.city).subscribe(number=>{
        this.authService.getSettlement(number).subscribe((settlement:Settlement[])=>{
          this.settlements = settlement;
        })
        if(this.selectedOption == 0){
          forkJoin([
            this.deviceService.dayByHour(number, 2),
            this.deviceService.dayByHour(number, 1)
          ]).subscribe(([list1, list2]) => {
            this.list1 = list1;
            this.list2 = list2;
          });
        }
        else{
          forkJoin([
            this.deviceService.dayByHourSettlement(this.selectedOption, 2),
            this.deviceService.dayByHourSettlement(this.selectedOption, 1)
          ]).subscribe(([list1, list2]) => {
            this.list1 = list1;
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
    filename: 'consumption/production-day.csv',
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
