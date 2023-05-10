import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
@Component({
  selector: 'app-consumption-month-wattmeter',
  templateUrl: './consumption-month-wattmeter.component.html',
  styleUrls: ['./consumption-month-wattmeter.component.css']
})
export class ConsumptionMonthWattmeterComponent implements OnInit {
  value!:any;
  valuekWh!: any;
  valueMWh!: any;
  valueGWh!: any;
  loader:boolean=false;
  min: number = 0;
  maxkwh: number = 1500000;
  maxmwh: number = this.maxkwh*0.001;
  maxgwh: number = this.maxmwh*0.001;
  markerConfigKWh = {
    "0": { color: '#57A75B', size: 8, label: '0', type: 'line'},
    "187500": { color: '#57A75B', size: 4, type: 'line'},
    "375000": { color: '#57A75B', size: 8, label: '375000', type: 'line'},
    "562500": { color: '#F9D435', size: 4, type: 'line'},
    "750000": { color: '#F9D435', size: 8, label: '750000', type: 'line'},
    "937500": { color: '#F69E0B', size: 4, type: 'line'},
    "1125000": { color: '#F69E0B', size: 8, label: '1125000', type: 'line'},
    "1312500": { color: '#E0453A', size: 4, type: 'line'},
    "1500000": { color: '#E0453A', size: 8, label: '1500000', type: 'line'},
  }
  thresholdConfigKWh = {
    '0': { color: 'green', "bgOpacity": 0.2 },
    '666668': { color: 'blue', "bgOpacity": 0.2 },
    '1333334': { color: 'red', "bgOpacity": 0.2 }
  };
  
  markerConfigMWh = {
    "0": { color: '#57A75B', size: 8, label: '0', type: 'line'},
    "187.5": { color: '#57A75B', size: 4, type: 'line'},
    "375": { color: '#57A75B', size: 8, label: '375', type: 'line'},
    "562.5": { color: '#F9D435', size: 4, type: 'line'},
    "750": { color: '#F9D435', size: 8, label: '750', type: 'line'},
    "937.5": { color: '#F69E0B', size: 4, type: 'line'},
    "1125": { color: '#F69E0B', size: 8, label: '1125', type: 'line'},
    "1312.5": { color: '#E0453A', size: 4, type: 'line'},
    "1500": { color: '#E0453A', size: 8, label: '1500', type: 'line'},
  }
  thresholdConfigMWh = {
    '0': { color: 'green', "bgOpacity": 0.2 },
    '666.66': { color: 'blue', "bgOpacity": 0.2 },
    '1333.33': { color: 'red', "bgOpacity": 0.2 }
  };

  markerConfigGWh = {
    "0": { color: '#57A75B', size: 8, label: '0', type: 'line'},
    "0.1875": { color: '#57A75B', size: 4, type: 'line'},
    "0.375": { color: '#57A75B', size: 8, label: '0.375', type: 'line'},
    "0.5625": { color: '#F9D435', size: 4, type: 'line'},
    "0.750": { color: '#F9D435', size: 8, label: '0.750', type: 'line'},
    "0.9375": { color: '#F69E0B', size: 4, type: 'line'},
    "1.125": { color: '#F69E0B', size: 8, label: '1.125', type: 'line'},
    "1.3125": { color: '#E0453A', size: 4, type: 'line'},
    "1.5": { color: '#E0453A', size: 8, label: '1.5', type: 'line'},
  }
  thresholdConfigGWh = {
    '0': { color: 'green', "bgOpacity": 0.2 },
    '0.66666': { color: 'blue', "bgOpacity": 0.2 },
    '1.33333': { color: 'red', "bgOpacity": 0.2 }
  };
  constructor(private historyService:HistoryPredictionService,private authService:AuthService){

  } 
   ngOnInit(){
    this.loader=true;
    this.authService.getlogInUser().subscribe(user=>{
      this.authService.getCityId(user.city).subscribe(number=>{
        this.historyService.getMonthTotalConsumption(number,2).subscribe(data=>{
          this.loader=false;
          this.value = data;  
          this.valuekWh = this.value.toFixed(2);
          this.valueMWh= (this.valuekWh*0.001).toFixed(2);
          this.valueGWh= (this.valueMWh*0.001).toFixed(2);
        })
      })
    })
   
  }
}
