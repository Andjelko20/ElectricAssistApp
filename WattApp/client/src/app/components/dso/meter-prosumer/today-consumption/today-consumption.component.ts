import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';

@Component({
  selector: 'app-today-consumption',
  templateUrl: './today-consumption.component.html',
  styleUrls: ['./today-consumption.component.css']
})
export class TodayConsumptionComponent implements OnInit{

  value!:any;
  valuekWh!: any;
  valueMWh!: any;
  valueGWh!: any;
  min: number = 0;
  maxkwh: number = 10000;
  maxmwh: number = this.maxkwh*0.001;
  maxgwh: number = this.maxmwh*0.001;
  constructor(private historyService:HistoryPredictionService,private route:ActivatedRoute){}
  ngOnInit(): void {
    this.historyService.historyDayUser(Number(this.route.snapshot.paramMap.get('id')),2).subscribe(number=>{
      
        this.value = number;  
        this.valuekWh = this.value.toFixed(2);
        this.valueMWh= (this.valuekWh*0.001).toFixed(2);
        this.valueGWh= (this.valueMWh*0.001).toFixed(2);
     
    })
  }


  markerConfigKWh = {
    "0": { color: '#57A75B', size: 8, label: '0', type: 'line'},
    "1250": { color: '#57A75B', size: 4, type: 'line'},
    "2500": { color: '#57A75B', size: 8, label: '2500', type: 'line'},
    "3750": { color: '#F9D435', size: 4, type: 'line'},
    "5000": { color: '#F9D435', size: 8, label: '5000', type: 'line'},
    "6250": { color: '#F69E0B', size: 4, type: 'line'},
    "7500": { color: '#F69E0B', size: 8, label: '7500', type: 'line'},
    "8750": { color: '#E0453A', size: 4, type: 'line'},
    "10000": { color: '#E0453A', size: 8, label: '10000', type: 'line'},
  }
  thresholdConfigKWh = {
    '0': { color: 'green', "bgOpacity": 0.2 },
    '4223': { color: 'blue', "bgOpacity": 0.2 },
    '7445': { color: 'red', "bgOpacity": 0.2 }
  };

  markerConfigMWh = {
    "0": { color: '#57A75B', size: 8, label: '0', type: 'line'},
    "1.25": { color: '#57A75B', size: 4, type: 'line'},
    "2.5": { color: '#57A75B', size: 8, label: '2.5', type: 'line'},
    "3.75": { color: '#F9D435', size: 4, type: 'line'},
    "5": { color: '#F9D435', size: 8, label: '5', type: 'line'},
    "6.25": { color: '#F69E0B', size: 4, type: 'line'},
    "7.5": { color: '#F69E0B', size: 8, label: '7.5', type: 'line'},
    "8.75": { color: '#E0453A', size: 4, type: 'line'},
    "10": { color: '#E0453A', size: 8, label: '10', type: 'line'},
  }
  thresholdConfigMWh = {
    '0': { color: 'green', "bgOpacity": 0.2 },
    '4.223': { color: 'blue', "bgOpacity": 0.2 },
    '7.445': { color: 'red', "bgOpacity": 0.2 }
  };
  markerConfigGWh = {
    "0": { color: '#57A75B', size: 8, label: '0', type: 'line'},
    "0.001": { color: '#57A75B', size: 4, type: 'line'},
    "0.002": { color: '#57A75B', size: 8, label: '0.002', type: 'line'},
    "0.004": { color: '#F9D435', size: 4, type: 'line'},
    "0.005": { color: '#F9D435', size: 8, label: '0.005', type: 'line'},
    "0.006": { color: '#F69E0B', size: 4, type: 'line'},
    "0.008": { color: '#F69E0B', size: 8, label: '0.008', type: 'line'},
    "0.009": { color: '#E0453A', size: 4, type: 'line'},
    "0.01": { color: '#E0453A', size: 8, label: '0.01', type: 'line'},
  }
  thresholdConfigGWh = {
    '0': { color: 'green', "bgOpacity": 0.2 },
    '0.0042': { color: 'blue', "bgOpacity": 0.2 },
    '0.0075': { color: 'red', "bgOpacity": 0.2 }
  };
  
}
