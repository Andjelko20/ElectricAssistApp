import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';

@Component({
  selector: 'app-month-consumption',
  templateUrl: './month-consumption.component.html',
  styleUrls: ['./month-consumption.component.css']
})
export class MonthConsumptionComponent implements OnInit{


  value!:any;
  valuekWh!: any;
  valueMWh!: any;
  valueGWh!: any;
  min: number = 0;
  maxkwh: number = 5000;
  maxmwh: number = this.maxkwh*0.001;
  maxgwh: number = this.maxmwh*0.001;
  constructor(private historyService:HistoryPredictionService,private route:ActivatedRoute) {
    
  }
  ngOnInit(): void {
    this.historyService.historyMonthUser(Number(this.route.snapshot.paramMap.get('id')),2).subscribe(number=>{
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
    '2223': { color: 'blue', "bgOpacity": 0.2 },
    '3944': { color: 'red', "bgOpacity": 0.2 }
  };
  
  markerConfigMWh = {
    "0": { color: '#57A75B', size: 8, label: '0', type: 'line'},
    "0.63": { color: '#57A75B', size: 4, type: 'line'},
    "1.3": { color: '#57A75B', size: 8, label: '1.3', type: 'line'},
    "1.9": { color: '#F9D435', size: 4, type: 'line'},
    "2.5": { color: '#F9D435', size: 8, label: '2.77', type: 'line'},
    "3.13": { color: '#F69E0B', size: 4, type: 'line'},
    "3.8": { color: '#F69E0B', size: 8, label: '3.8', type: 'line'},
    "4.38": { color: '#E0453A', size: 4, type: 'line'},
    "5": { color: '#E0453A', size: 8, label: '5', type: 'line'},
  }
  thresholdConfigMWh = {
    '0': { color: 'green', "bgOpacity": 0.2 },
    '2.223': { color: 'blue', "bgOpacity": 0.2 },
    '4.444': { color: 'red', "bgOpacity": 0.2 }
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
}
