import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { JwtToken } from 'src/app/utilities/jwt-token';

@Component({
  selector: 'app-consumption-month-wattmeter',
  templateUrl: './consumption-month-wattmeter.component.html',
  styleUrls: ['./consumption-month-wattmeter.component.css']
})
export class ConsumptionMonthWattmeterComponent implements OnInit {
  valuekWh!: number;
  min: number = 0;
  max: number = 2400;
  markerConfig = {
    "0": { color: '#57A75B', size: 8, label: '0', type: 'line'},
    "300": { color: '#57A75B', size: 4, type: 'line'},
    "600": { color: '#57A75B', size: 8, label: '600', type: 'line'},
    "900": { color: '#F9D435', size: 4, type: 'line'},
    "1200": { color: '#F9D435', size: 8, label: '1200', type: 'line'},
    "1500": { color: '#F69E0B', size: 4, type: 'line'},
    "1800": { color: '#F69E0B', size: 8, label: '1800', type: 'line'},
    "2100": { color: '#E0453A', size: 4, type: 'line'},
    "2400": { color: '#E0453A', size: 8, label: '2400', type: 'line'},
  }
  thresholdConfig = {
    '0': { color: 'green', "bgOpacity": 0.2 },
    '351': { color: 'blue', "bgOpacity": 0.2 },
    '1601': { color: 'red', "bgOpacity": 0.2 }
  };
  constructor(private todayConsumption:HistoryPredictionService){

  }
    async ngOnInit(): Promise<void> {
    let token=new JwtToken();
    
    const result = await this.todayConsumption.getTotalConsumptionProductionCity("Electricity Producer","Kragujevac").pipe(first()).toPromise();
  
    this.valuekWh = result!;
  }
}
