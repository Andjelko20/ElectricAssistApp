import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ExportToCsv } from 'export-to-csv';
import { Chart,registerables } from 'node_modules/chart.js'
import { WeekByDay } from 'src/app/models/devices.model';
import { Settlement } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { DevicesService } from 'src/app/services/devices.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';

@Component({
  selector: 'app-prediction-tabular-prosumer',
  templateUrl: './prediction-tabular-prosumer.component.html',
  styleUrls: ['./prediction-tabular-prosumer.component.css']
})
export class PredictionTabularProsumerComponent {

  list1:WeekByDay[] = [];
  list2:WeekByDay[] = [];

  mergedList: { day: number, month: string, year: number, consumption: number, production: number }[] = [];

  constructor(private deviceService:HistoryPredictionService,private route:ActivatedRoute) {
    
  }

  ngOnInit(): void {
  
    this.deviceService.predictionUser(Number(this.route.snapshot.paramMap.get('id')),2).subscribe((data1: WeekByDay[]) =>{
      this.list1 = data1;
      this.deviceService.predictionUser(Number(this.route.snapshot.paramMap.get('id')),1).subscribe((data2: WeekByDay[]) =>{
        this.list2 = data2;
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
