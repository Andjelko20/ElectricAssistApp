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
  min: number = 0;
  max: number = 1000;
  markerConfig = {
    "0": { color: '#57A75B', size: 8, label: '0', type: 'line'},
    "150": { color: '#57A75B', size: 4, type: 'line'},
    "300": { color: '#57A75B', size: 8, label: '300', type: 'line'},
    "400": { color: '#F9D435', size: 4, type: 'line'},
    "500": { color: '#F9D435', size: 8, label: '500', type: 'line'},
    "600": { color: '#F69E0B', size: 4, type: 'line'},
    "700": { color: '#F69E0B', size: 8, label: '700', type: 'line'},
    "850": { color: '#E0453A', size: 4, type: 'line'},
    "1000": { color: '#E0453A', size: 8, label: '1000', type: 'line'},
  }
  thresholdConfig = {
    '0': { color: 'green', "bgOpacity": 0.2 },
    '400': { color: 'orange', "bgOpacity": 0.2 },
    '750': { color: 'red', "bgOpacity": 0.2 }
  };
}
