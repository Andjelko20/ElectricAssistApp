import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';

@Component({
  selector: 'app-month-production',
  templateUrl: './month-production.component.html',
  styleUrls: ['./month-production.component.css']
})
export class MonthProductionComponent implements OnInit{

  value!:any;
  valuekWh!: any;
  valueMWh!: any;
  valueGWh!: any;
  constructor(private historyService:HistoryPredictionService,private route:ActivatedRoute) {
    
  }
  ngOnInit(): void {
    this.historyService.monthConsumptionUser(Number(this.route.snapshot.paramMap.get('id')),1).subscribe(number=>{
      if(number != null){
        this.value = number;  
        this.valuekWh = this.value.toFixed(2);
        this.valueMWh= (this.valuekWh*0.001).toFixed(2);
        this.valueGWh= (this.valueMWh*0.001).toFixed(2);
      }
      else{
        this.value = 0;
      }
    })
  }
  min: number = 0;
  maxkwh: number = 5000;
  maxmwh: number = this.maxkwh*0.001;
  maxgwh: number = this.maxmwh*0.001;
  markerConfigKWh = {
    "0": { color: '#57A75B', size: 8, label: '0', type: 'line'},
    "625": { color: '#57A75B', size: 4, type: 'line'},
    "1250": { color: '#57A75B', size: 8, label: '1,250', type: 'line'},
    "1875": { color: '#F9D435', size: 4, type: 'line'},
    "2500": { color: '#F9D435', size: 8, label: '2,500', type: 'line'},
    "3125": { color: '#F69E0B', size: 4, type: 'line'},
    "3750": { color: '#F69E0B', size: 8, label: '3,750', type: 'line'},
    "4375": { color: '#E0453A', size: 4, type: 'line'},
    "5000": { color: '#E0453A', size: 8, label: '5,000', type: 'line'},
  }
  thresholdConfigKWh = {
    '0': { color: 'green', "bgOpacity": 0.2 },
    '1601': { color: 'blue', "bgOpacity": 0.2 },
    '3900': { color: 'red', "bgOpacity": 0.2 }
  };
  markerConfigMWh = {
    "0": { color: '#57A75B', size: 8, label: '0', type: 'line'},
    "0.63": { color: '#57A75B', size: 4, type: 'line'},
    "1.25": { color: '#57A75B', size: 8, label: '1.25', type: 'line'},
    "1.88": { color: '#F9D435', size: 4, type: 'line'},
    "2.5": { color: '#F9D435', size: 8, label: '2.5', type: 'line'},
    "3.13": { color: '#F69E0B', size: 4, type: 'line'},
    "3.75": { color: '#F69E0B', size: 8, label: '3.75', type: 'line'},
    "4.38": { color: '#E0453A', size: 4, type: 'line'},
    "5": { color: '#E0453A', size: 8, label: '5', type: 'line'},
  }
  thresholdConfigMWh = {
    '0': { color: 'green', "bgOpacity": 0.2 },
    '1.6': { color: 'blue', "bgOpacity": 0.2 },
    '3.9': { color: 'red', "bgOpacity": 0.2 }
  };
  markerConfigGWh = {
    "0": { color: '#57A75B', size: 8, label: '0', type: 'line'},
    "0.00063": { color: '#57A75B', size: 4, type: 'line'},
    "0.0013": { color: '#57A75B', size: 8, label: '0.0013', type: 'line'},
    "0.0019": { color: '#F9D435', size: 4, type: 'line'},
    "0.0025": { color: '#F9D435', size: 8, label: '0.0025', type: 'line'},
    "0.0031": { color: '#F69E0B', size: 4, type: 'line'},
    "0.0038": { color: '#F69E0B', size: 8, label: '0.0038', type: 'line'},
    "0.0044": { color: '#E0453A', size: 4, type: 'line'},
    "0.005": { color: '#E0453A', size: 8, label: '0.005', type: 'line'},
  }
  thresholdConfigGWh = {
    '0': { color: 'green', "bgOpacity": 0.2 },
    '0.0016': { color: 'blue', "bgOpacity": 0.2 },
    '0.0039': { color: 'red', "bgOpacity": 0.2 }
  };
}
