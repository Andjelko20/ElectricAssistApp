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
  maxkwh: number = 100000;
  maxmwh: number = this.maxkwh*0.001;
  maxgwh: number = this.maxmwh*0.001;
  constructor(private historyService:HistoryPredictionService,private route:ActivatedRoute) {
    
  }
  ngOnInit(): void {
    this.historyService.historyMonthUser(Number(this.route.snapshot.paramMap.get('id')),2).subscribe(number=>{
      
        this.value = number;  
        this.valuekWh = this.value.toFixed(2);
        this.valueMWh= (this.valuekWh*0.001).toFixed(2);
        this.valueGWh= (this.valueMWh*0.001).toFixed(2);
      
    })
  }
  markerConfigKWh = {
    "0": { color: '#57A75B', size: 8, label: '0', type: 'line'},
    "12500": { color: '#57A75B', size: 4, type: 'line'},
    "25000": { color: '#57A75B', size: 8, label: '25,000', type: 'line'},
    "37500": { color: '#F9D435', size: 4, type: 'line'},
    "50000": { color: '#F9D435', size: 8, label: '50,000', type: 'line'},
    "62500": { color: '#F69E0B', size: 4, type: 'line'},
    "75000": { color: '#F69E0B', size: 8, label: '75,000', type: 'line'},
    "87500": { color: '#E0453A', size: 4, type: 'line'},
    "100000": { color: '#E0453A', size: 8, label: '100,000', type: 'line'},
  }
  thresholdConfigKWh = {
    '0': { color: 'green', "bgOpacity": 0.2 },
    '43230': { color: 'blue', "bgOpacity": 0.2 },
    '76444': { color: 'red', "bgOpacity": 0.2 }
  };
  
  markerConfigMWh = {
    "0": { color: '#57A75B', size: 8, label: '0', type: 'line'},
    "12.5": { color: '#57A75B', size: 4, type: 'line'},
    "25": { color: '#57A75B', size: 8, label: '25', type: 'line'},
    "37.5": { color: '#F9D435', size: 4, type: 'line'},
    "50": { color: '#F9D435', size: 8, label: '50', type: 'line'},
    "62.5": { color: '#F69E0B', size: 4, type: 'line'},
    "75": { color: '#F69E0B', size: 8, label: '75', type: 'line'},
    "87.5": { color: '#E0453A', size: 4, type: 'line'},
    "100": { color: '#E0453A', size: 8, label: '100', type: 'line'},
  }
  thresholdConfigMWh = {
    '0': { color: 'green', "bgOpacity": 0.2 },
    '43.23': { color: 'blue', "bgOpacity": 0.2 },
    '76.44': { color: 'red', "bgOpacity": 0.2 }
  };

  markerConfigGWh = {
    "0": { color: '#57A75B', size: 8, label: '0', type: 'line'},
    "0.013": { color: '#57A75B', size: 4, type: 'line'},
    "0.025": { color: '#57A75B', size: 8, label: '0.03', type: 'line'},
    "0.038": { color: '#F9D435', size: 4, type: 'line'},
    "0.05": { color: '#F9D435', size: 8, label: '0.05', type: 'line'},
    "0.062": { color: '#F69E0B', size: 4, type: 'line'},
    "0.075": { color: '#F69E0B', size: 8, label: '0.08', type: 'line'},
    "0.088": { color: '#E0453A', size: 4, type: 'line'},
    "0.10": { color: '#E0453A', size: 8, label: '0.10', type: 'line'}
  }
  thresholdConfigGWh = {
    '0': { color: 'green', "bgOpacity": 0.2 },
    '0.043': { color: 'blue', "bgOpacity": 0.2 },
    '0.076': { color: 'red', "bgOpacity": 0.2 }
  };
}
