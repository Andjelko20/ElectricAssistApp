import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs';
import { AuthService } from 'src/app/services/auth.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { JwtToken } from 'src/app/utilities/jwt-token';

@Component({
  selector: 'app-consumption-year-wattmeter',
  templateUrl: './consumption-year-wattmeter.component.html',
  styleUrls: ['./consumption-year-wattmeter.component.css']
})
export class ConsumptionYearWattmeterComponent implements OnInit {
  value!:any;
  valuekWh!: any;
  valueMWh!: any;
  valueGWh!: any;
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
  constructor(private historyService:HistoryPredictionService,private authService:AuthService){

  }
    async ngOnInit(): Promise<void> {
    let token=new JwtToken();
    
    this.authService.getlogInUser().subscribe(user=>{
      this.authService.getCityId(user.city).subscribe(number=>{
        this.historyService.getYearTotalConsumption(number,2).subscribe(data=>{
          this.value= data;
          this.valuekWh = this.value.toFixed(2);
          this.valueMWh= (this.valuekWh*0.001).toFixed(2);
          this.valueGWh= (this.valueMWh*0.001).toFixed(2);;
        })
      })
    })

   
  }
}
