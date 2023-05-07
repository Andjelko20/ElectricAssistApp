import { Component } from '@angular/core';
import { ExportToCsv } from 'export-to-csv';
import { forkJoin } from 'rxjs';
import { WeekByDay } from 'src/app/models/devices.model';
import { Settlement } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';

@Component({
  selector: 'app-prediction-tabelar-dso',
  templateUrl: './prediction-tabelar-dso.component.html',
  styleUrls: ['./prediction-tabelar-dso.component.css']
})
export class PredictionTabelarDsoComponent {
  list1:WeekByDay[] = [];
  list2:WeekByDay[] = [];
  settlements:Settlement[] = [];
  mergedList: { day: number, month: string, year: number, consumption: number, production: number }[] = [];
  constructor(private authService:AuthService,private deviceService:HistoryPredictionService){}

  selectedOption: number = 0;

  onOptionSelected() {
    this.ngOnInit();
  }
  ngOnInit() {
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
        
        if(this.selectedOption == 0){
          forkJoin([
            this.deviceService.weekByDay(number, 2),
            this.deviceService.weekByDay(number, 1)
          ]).subscribe(([list1, list2]) => {
            this.list1 = list1;
            this.list2 = list2;
          });
          
        }
        else{
          forkJoin([
            this.deviceService.weekByDaySettlement(this.selectedOption, 2),
            this.deviceService.weekByDaySettlement(this.selectedOption, 1)
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
    filename: 'consumption/production-week',
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
