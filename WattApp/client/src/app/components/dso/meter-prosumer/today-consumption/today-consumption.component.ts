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
  maxkwh: number = 6000;
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
    "750": { color: '#57A75B', size: 4, type: 'line'},
    "1500": { color: '#57A75B', size: 8, label: '1500', type: 'line'},
    "2250": { color: '#F9D435', size: 4, type: 'line'},
    "3000": { color: '#F9D435', size: 8, label: '3000', type: 'line'},
    "3750": { color: '#F69E0B', size: 4, type: 'line'},
    "4500": { color: '#F69E0B', size: 8, label: '4500', type: 'line'},
    "5250": { color: '#E0453A', size: 4, type: 'line'},
    "6000": { color: '#E0453A', size: 8, label: '6000', type: 'line'},
  }
  thresholdConfigKWh = {
    '0': { color: 'green', "bgOpacity": 0.2 },
    '2223': { color: 'blue', "bgOpacity": 0.2 },
    '4445': { color: 'red', "bgOpacity": 0.2 }
  };

  markerConfigMWh = {
    "0": { color: '#57A75B', size: 8, label: '0', type: 'line'},
    "0.75": { color: '#57A75B', size: 4, type: 'line'},
    "1.5": { color: '#57A75B', size: 8, label: '1.5', type: 'line'},
    "2.25": { color: '#F9D435', size: 4, type: 'line'},
    "3": { color: '#F9D435', size: 8, label: '3', type: 'line'},
    "3.75": { color: '#F69E0B', size: 4, type: 'line'},
    "4.5": { color: '#F69E0B', size: 8, label: '4.5', type: 'line'},
    "5.2": { color: '#E0453A', size: 4, type: 'line'},
    "6": { color: '#E0453A', size: 8, label: '6', type: 'line'},
  }
  thresholdConfigMWh = {
    '0': { color: 'green', "bgOpacity": 0.2 },
    '2.223': { color: 'blue', "bgOpacity": 0.2 },
    '4.445': { color: 'red', "bgOpacity": 0.2 }
  };
  markerConfigGWh = {
    "0": { color: '#57A75B', size: 8, label: '0', type: 'line'},
    "0.0008": { color: '#57A75B', size: 4, type: 'line'},
    "0.002": { color: '#57A75B', size: 8, label: '0.002', type: 'line'},
    "0.0023": { color: '#F9D435', size: 4, type: 'line'},
    "0.003": { color: '#F9D435', size: 8, label: '0.003', type: 'line'},
    "0.004": { color: '#F69E0B', size: 4, type: 'line'},
    "0.0045": { color: '#F69E0B', size: 8, label: '0.0045', type: 'line'},
    "0.0052": { color: '#E0453A', size: 4, type: 'line'},
    "0.006": { color: '#E0453A', size: 8, label: '0.006', type: 'line'},
  }
  thresholdConfigGWh = {
    '0': { color: 'green', "bgOpacity": 0.2 },
    '0.0022': { color: 'blue', "bgOpacity": 0.2 },
    '0.0045': { color: 'red', "bgOpacity": 0.2 }
  };
  
}
