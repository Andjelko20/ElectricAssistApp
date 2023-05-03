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
  maxkwh: number = 60000;
  maxmwh: number = this.maxkwh*0.001;
  maxgwh: number = this.maxmwh*0.001;
  constructor(private historyService:HistoryPredictionService,private route:ActivatedRoute){}
  ngOnInit(): void {
    this.historyService.historyDayUser(Number(this.route.snapshot.paramMap.get('id')),2).subscribe(number=>{
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
    "7500": { color: '#57A75B', size: 4, type: 'line'},
    "15000": { color: '#57A75B', size: 8, label: '15000', type: 'line'},
    "22500": { color: '#F9D435', size: 4, type: 'line'},
    "30000": { color: '#F9D435', size: 8, label: '30000', type: 'line'},
    "37500": { color: '#F69E0B', size: 4, type: 'line'},
    "45000": { color: '#F69E0B', size: 8, label: '45000', type: 'line'},
    "52500": { color: '#E0453A', size: 4, type: 'line'},
    "60000": { color: '#E0453A', size: 8, label: '60000', type: 'line'},
  }
  thresholdConfigKWh = {
    '0': { color: 'green', "bgOpacity": 0.2 },
    '22223': { color: 'blue', "bgOpacity": 0.2 },
    '44445': { color: 'red', "bgOpacity": 0.2 }
  };

  markerConfigMWh = {
    "0": { color: '#57A75B', size: 8, label: '0', type: 'line'},
    "7.5": { color: '#57A75B', size: 4, type: 'line'},
    "15": { color: '#57A75B', size: 8, label: '15', type: 'line'},
    "22.5": { color: '#F9D435', size: 4, type: 'line'},
    "30": { color: '#F9D435', size: 8, label: '30', type: 'line'},
    "37.5": { color: '#F69E0B', size: 4, type: 'line'},
    "45": { color: '#F69E0B', size: 8, label: '45', type: 'line'},
    "52.5": { color: '#E0453A', size: 4, type: 'line'},
    "60": { color: '#E0453A', size: 8, label: '60', type: 'line'},
  }
  thresholdConfigMWh = {
    '0': { color: 'green', "bgOpacity": 0.2 },
    '22.223': { color: 'blue', "bgOpacity": 0.2 },
    '44.445': { color: 'red', "bgOpacity": 0.2 }
  };
  markerConfigGWh = {
    "0": { color: '#57A75B', size: 8, label: '0', type: 'line'},
    "0.007": { color: '#57A75B', size: 4, type: 'line'},
    "0.015": { color: '#57A75B', size: 8, label: '0.015', type: 'line'},
    "0.023": { color: '#F9D435', size: 4, type: 'line'},
    "0.03": { color: '#F9D435', size: 8, label: '0.03', type: 'line'},
    "0.038": { color: '#F69E0B', size: 4, type: 'line'},
    "0.045": { color: '#F69E0B', size: 8, label: '0.045', type: 'line'},
    "0.053": { color: '#E0453A', size: 4, type: 'line'},
    "0.06": { color: '#E0453A', size: 8, label: '0.06', type: 'line'},
  }
  thresholdConfigGWh = {
    '0': { color: 'green', "bgOpacity": 0.2 },
    '0.02': { color: 'blue', "bgOpacity": 0.2 },
    '0.04': { color: 'red', "bgOpacity": 0.2 }
  };
}
