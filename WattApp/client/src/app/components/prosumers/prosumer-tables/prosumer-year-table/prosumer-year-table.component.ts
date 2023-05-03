import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ExportToCsv } from 'export-to-csv';
import { switchMap } from 'rxjs';
import { YearsByMonth } from 'src/app/models/devices.model';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { JwtToken } from 'src/app/utilities/jwt-token';

@Component({
  selector: 'app-prosumer-year-table',
  templateUrl: './prosumer-year-table.component.html',
  styleUrls: ['./prosumer-year-table.component.css']
})
export class ProsumerYearTableComponent {

  list1:YearsByMonth[]=[];
  list2:YearsByMonth[]=[];
  mergedList: {month: string, year: number, consumption: number, production: number }[] = [];
  constructor(private deviceService:HistoryPredictionService,private route:ActivatedRoute) {
    
  }
  ngOnInit(): void {
    let token=new JwtToken();
    const userId = token.data.id as number;
  
    this.deviceService.yearByMonthUser(userId, 2).pipe(
      switchMap((data1: YearsByMonth[]) => {
        this.list1 = data1;
        return this.deviceService.yearByMonthUser(userId, 1);
      })
    ).subscribe((data2: YearsByMonth[]) => {
      console.log("Data => ", data2);
      this.list2 = data2;
    });
  }
  downloadCSV(): void {
    this.mergedList = [];
    for (let i = 0; i < this.list1.length; i++) {
      for (let j = 0; j < this.list2.length; j++) {
        if (this.list1[i].month === this.list2[j].month && this.list1[i].year === this.list2[j].year) {
          this.mergedList.push({
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
    filename: 'consumption/production-year.csv',
    quoteStrings: '"',
    useBom : true,
    decimalSeparator: '.',
    showLabels: true,
    useTextFile: false,
    headers: ['Month', 'Year', 'Consumption', 'Production']
  };

  const csvExporter = new ExportToCsv(options);
  const csvData = csvExporter.generateCsv(this.mergedList);

  }
}
