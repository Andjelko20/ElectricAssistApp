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

  constructor(private authService:AuthService,private historyService:HistoryPredictionService,private route:ActivatedRoute){}

  valuekwh!:number;
  ngOnInit(): void {
    this.historyService.historyDayUser(Number(this.route.snapshot.paramMap.get('id')),1).subscribe(number=>{
      if(number != null){
        this.valuekwh = number;
      }
      else{
        this.valuekwh = 0;
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
