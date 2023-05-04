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
  loader:boolean=false;
  valueGWh!: any;
  min: number = 0;
  maxkwh: number = 20000000;
  maxmwh: number = this.maxkwh*0.001;
  maxgwh: number = this.maxmwh*0.001;
  markerConfigKWh = {
    "0": { color: '#57A75B', size: 8, label: '0', type: 'line'},
    "2500000": { color: '#57A75B', size: 4, type: 'line'},
    "5000000": { color: '#57A75B', size: 8, label: '5000000', type: 'line'},
    "7500000": { color: '#F9D435', size: 4, type: 'line'},
    "10000000": { color: '#F9D435', size: 8, label: '10000000', type: 'line'},
    "12500000": { color: '#F69E0B', size: 4, type: 'line'},
    "15000000": { color: '#F69E0B', size: 8, label: '15000000', type: 'line'},
    "17500000": { color: '#E0453A', size: 4, type: 'line'},
    "20000000": { color: '#E0453A', size: 8, label: '20000000', type: 'line'},
  }
  thresholdConfigKWh = {
    '0': { color: 'green', "bgOpacity": 0.2 },
    '8000001': { color: 'blue', "bgOpacity": 0.2 },
    '16000001': { color: 'red', "bgOpacity": 0.2 }
  };
  markerConfigMWh = {
    "0": { color: '#57A75B', size: 8, label: '0', type: 'line'},
    "2500": { color: '#57A75B', size: 4, type: 'line'},
    "5000": { color: '#57A75B', size: 8, label: '5000', type: 'line'},
    "7500": { color: '#F9D435', size: 4, type: 'line'},
    "10000": { color: '#F9D435', size: 8, label: '10000', type: 'line'},
    "12500": { color: '#F69E0B', size: 4, type: 'line'},
    "15000": { color: '#F69E0B', size: 8, label: '15000', type: 'line'},
    "17500": { color: '#E0453A', size: 4, type: 'line'},
    "20000": { color: '#E0453A', size: 8, label: '20000', type: 'line'},
  }
  thresholdConfigMWh = {
    '0': { color: 'green', "bgOpacity": 0.2 },
    '8000': { color: 'blue', "bgOpacity": 0.2 },
    '16000': { color: 'red', "bgOpacity": 0.2 }
  };
  markerConfigGWh = {
    "0": { color: '#57A75B', size: 8, label: '0', type: 'line'},
    "25": { color: '#57A75B', size: 4, type: 'line'},
    "5": { color: '#57A75B', size: 8, label: '5', type: 'line'},
    "75": { color: '#F9D435', size: 4, type: 'line'},
    "10": { color: '#F9D435', size: 8, label: '10', type: 'line'},
    "12.5": { color: '#F69E0B', size: 4, type: 'line'},
    "15": { color: '#F69E0B', size: 8, label: '15', type: 'line'},
    "17.5": { color: '#E0453A', size: 4, type: 'line'},
    "20": { color: '#E0453A', size: 8, label: '20', type: 'line'},
  }
  thresholdConfigGWh = {
    '0': { color: 'green', "bgOpacity": 0.2 },
    '8': { color: 'blue', "bgOpacity": 0.2 },
    '1.6': { color: 'red', "bgOpacity": 0.2 }
  };
  constructor(private historyService:HistoryPredictionService,private authService:AuthService){

  }
    async ngOnInit(){
    let token=new JwtToken();
    this.loader=true;
    this.authService.getlogInUser().subscribe(user=>{
      this.authService.getCityId(user.city).subscribe(number=>{
        this.historyService.getYearTotalConsumption(number,2).subscribe(data=>{
          this.loader=false;
          this.value= data;
          this.valuekWh = this.value.toFixed(2);
          this.valueMWh= (this.valuekWh*0.001).toFixed(2);
          this.valueGWh= (this.valueMWh*0.001).toFixed(2);
        })
      })
    })

   
  }
}
