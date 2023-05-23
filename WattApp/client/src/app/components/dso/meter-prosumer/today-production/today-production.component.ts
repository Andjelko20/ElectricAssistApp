import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';

@Component({
  selector: 'app-today-production',
  templateUrl: './today-production.component.html',
  styleUrls: ['./today-production.component.css']
})
export class TodayProductionComponent implements OnInit{

  value!:any;
  valuekWh!: any;
  valueMWh!: any;
  valueGWh!: any;
  constructor(private authService:AuthService,private historyService:HistoryPredictionService,private route:ActivatedRoute){}

  
  ngOnInit(): void {
    this.historyService.historyDayUser(Number(this.route.snapshot.paramMap.get('id')),1).subscribe(number=>{
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
  maxkwh: number = 600;
  maxmwh: number = this.maxkwh*0.001;
  maxgwh: number = this.maxmwh*0.001;
  markerConfigKWh = {
    "0": { color: '#57A75B', size: 8, label: '0', type: 'line'},
    "75": { color: '#57A75B', size: 4, type: 'line'},
    "150": { color: '#57A75B', size: 8, label: '150', type: 'line'},
    "225": { color: '#F9D435', size: 4, type: 'line'},
    "300": { color: '#F9D435', size: 8, label: '300', type: 'line'},
    "375": { color: '#F69E0B', size: 4, type: 'line'},
    "450": { color: '#F69E0B', size: 8, label: '450', type: 'line'},
    "525": { color: '#E0453A', size: 4, type: 'line'},
    "600": { color: '#E0453A', size: 8, label: '500', type: 'line'},
  }
  thresholdConfigKWh = {
    '0': { color: 'green', "bgOpacity": 0.2 },
    '151': { color: 'blue', "bgOpacity": 0.2 },
    '300': { color: 'red', "bgOpacity": 0.2 }
  };
  markerConfigMWh = {
    "0": { color: '#57A75B', size: 8, label: '0', type: 'line'},
    "75": { color: '#57A75B', size: 4, type: 'line'},
    "150": { color: '#57A75B', size: 8, label: '150', type: 'line'},
    "225": { color: '#F9D435', size: 4, type: 'line'},
    "300": { color: '#F9D435', size: 8, label: '300', type: 'line'},
    "375": { color: '#F69E0B', size: 4, type: 'line'},
    "450": { color: '#F69E0B', size: 8, label: '450', type: 'line'},
    "525": { color: '#E0453A', size: 4, type: 'line'},
    "600": { color: '#E0453A', size: 8, label: '500', type: 'line'},
  }
  thresholdConfigMWh = {
    '0': { color: 'green', "bgOpacity": 0.2 },
    '151': { color: 'blue', "bgOpacity": 0.2 },
    '300': { color: 'red', "bgOpacity": 0.2 }
  };
  markerConfigGWh = {
    "0": { color: '#57A75B', size: 8, label: '0', type: 'line'},
    "75": { color: '#57A75B', size: 4, type: 'line'},
    "150": { color: '#57A75B', size: 8, label: '150', type: 'line'},
    "225": { color: '#F9D435', size: 4, type: 'line'},
    "300": { color: '#F9D435', size: 8, label: '300', type: 'line'},
    "375": { color: '#F69E0B', size: 4, type: 'line'},
    "450": { color: '#F69E0B', size: 8, label: '450', type: 'line'},
    "525": { color: '#E0453A', size: 4, type: 'line'},
    "600": { color: '#E0453A', size: 8, label: '500', type: 'line'},
  }
  thresholdConfigGWh = {
    '0': { color: 'green', "bgOpacity": 0.2 },
    '151': { color: 'blue', "bgOpacity": 0.2 },
    '300': { color: 'red', "bgOpacity": 0.2 }
  };
}
