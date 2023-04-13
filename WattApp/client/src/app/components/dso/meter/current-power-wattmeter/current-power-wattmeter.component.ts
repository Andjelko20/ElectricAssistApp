import { Component, OnInit } from '@angular/core';
import { first } from 'rxjs';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { JwtToken } from 'src/app/utilities/jwt-token';

@Component({
  selector: 'app-current-power-wattmeter',
  templateUrl: './current-power-wattmeter.component.html',
  styleUrls: ['./current-power-wattmeter.component.css']
})
export class CurrentPowerWattmeterComponent implements OnInit{
  
  value!:number;
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
// gaugeValue = 50;

//   updateValue() {
//     this.gaugeValue = Math.floor(Math.random() * 101);
//   }
constructor(private todayConsumption:HistoryPredictionService){

}
  async ngOnInit(): Promise<void> {
  let token=new JwtToken();
  
  const result = await this.todayConsumption.getTotalConsumptionProductionCity("Electricity Consumer","Kragujevac").pipe(first()).toPromise();

  this.value = result!;
}

}
