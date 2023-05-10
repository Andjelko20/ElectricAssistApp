import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
@Component({
  selector: 'app-current-power-wattmeter',
  templateUrl: './current-power-wattmeter.component.html',
  styleUrls: ['./current-power-wattmeter.component.css']
})
export class CurrentPowerWattmeterComponent implements OnInit{
  
  value!:any;
  valuekWh!: any;
  valueMWh!: any;
  valueGWh!: any;
  loader:boolean=false;
  min: number = 0;
  max: number = 2400;
  markerConfig = {
    "0": { color: '#57A75B', size: 8, label: '0', type: 'line'},
    "300": { color: '#57A75B', size: 4, type: 'line'},
    "600": { color: '#57A75B', size: 8, label: '600', type: 'line'},
    "900": { color: '#F9D435', size: 4, type: 'line'},
    "1200": { color: '#F9D435', size: 8, label: '1200', type: 'line'},
    "1500": { color: '#F69E0B', size: 4, type: 'line'},
    "1800": { color: '#F69E0B', size: 8, label: '1800', type: 'line'},
    "2100": { color: '#E0453A', size: 4, type: 'line'},
    "2400": { color: '#E0453A', size: 8, label: '2400', type: 'line'},
}
thresholdConfig = {
  '0': { color: 'green', "bgOpacity": 0.2 },
  '351': { color: 'blue', "bgOpacity": 0.2 },
  '1601': { color: 'red', "bgOpacity": 0.2 }
};
constructor(private historyService:HistoryPredictionService,private authService:AuthService){
}
  ngOnInit(){
  this.loader=true;
  this.authService.getlogInUser().subscribe(user=>{
    this.authService.getCityId(user.city).subscribe(number=>{
      this.historyService.getCurrentConsumptionProductionCity(2,number).subscribe(data=>{
        this.loader=false;
        this.value=data;
        this.valuekWh = this.value.toFixed(2);
        this.valueMWh= (this.valuekWh*0.001).toFixed(2);
        this.valueGWh= (this.valueMWh*0.001).toFixed(2);
      })
    })
  })
}

}
