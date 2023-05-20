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
  maxkwh: number = 80000;
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
    "10000": { color: '#57A75B', size: 4, type: 'line'},
    "20000": { color: '#57A75B', size: 8, label: '20,000', type: 'line'},
    "30000": { color: '#F9D435', size: 4, type: 'line'},
    "40000": { color: '#F9D435', size: 8, label: '40,000', type: 'line'},
    "50000": { color: '#F69E0B', size: 4, type: 'line'},
    "60000": { color: '#F69E0B', size: 8, label: '60,000', type: 'line'},
    "70000": { color: '#E0453A', size: 4, type: 'line'},
    "80000": { color: '#E0453A', size: 8, label: '80,000', type: 'line'},
  }
  thresholdConfigKWh = {
    '0': { color: 'green', "bgOpacity": 0.2 },
    '33230': { color: 'blue', "bgOpacity": 0.2 },
    '66444': { color: 'red', "bgOpacity": 0.2 }
  };
  
  markerConfigMWh = {
    "0": { color: '#57A75B', size: 8, label: '0', type: 'line'},
    "10": { color: '#57A75B', size: 4, type: 'line'},
    "20": { color: '#57A75B', size: 8, label: '20', type: 'line'},
    "30": { color: '#F9D435', size: 4, type: 'line'},
    "40": { color: '#F9D435', size: 8, label: '40', type: 'line'},
    "50": { color: '#F69E0B', size: 4, type: 'line'},
    "60": { color: '#F69E0B', size: 8, label: '60', type: 'line'},
    "70": { color: '#E0453A', size: 4, type: 'line'},
    "80": { color: '#E0453A', size: 8, label: '80', type: 'line'},
  }
  thresholdConfigMWh = {
    '0': { color: 'green', "bgOpacity": 0.2 },
    '33.23': { color: 'blue', "bgOpacity": 0.2 },
    '66.44': { color: 'red', "bgOpacity": 0.2 }
  };

  markerConfigGWh = {
    "0": { color: '#57A75B', size: 8, label: '0', type: 'line'},
    "0.01": { color: '#57A75B', size: 4, type: 'line'},
    "0.02": { color: '#57A75B', size: 8, label: '0,02', type: 'line'},
    "0.03": { color: '#F9D435', size: 4, type: 'line'},
    "0.04": { color: '#F9D435', size: 8, label: '0,04', type: 'line'},
    "0.05": { color: '#F69E0B', size: 4, type: 'line'},
    "0.06": { color: '#F69E0B', size: 8, label: '0,06', type: 'line'},
    "0.07": { color: '#E0453A', size: 4, type: 'line'},
    "0.08": { color: '#E0453A', size: 8, label: '0.08', type: 'line'}
  }
  thresholdConfigGWh = {
    '0': { color: 'green', "bgOpacity": 0.2 },
    '0.033': { color: 'blue', "bgOpacity": 0.2 },
    '0.066': { color: 'red', "bgOpacity": 0.2 }
  };
}
