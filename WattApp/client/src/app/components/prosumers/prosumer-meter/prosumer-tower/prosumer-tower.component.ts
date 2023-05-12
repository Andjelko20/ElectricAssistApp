import { ChangeDetectorRef, Component } from '@angular/core';
import { first, subscribeOn } from 'rxjs';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { JwtToken } from 'src/app/utilities/jwt-token';

@Component({
  selector: 'app-prosumer-tower',
  templateUrl: './prosumer-tower.component.html',
  styleUrls: ['./prosumer-tower.component.css']
})
export class ProsumerTowerComponent {

  
  idProsumer?: number;
enableThresholds: boolean = false;
value!: number ;
thick: number = 15;
size: number = 200;
type: any = "full";
cap: any = "round";
label: string = "power transmission";
prepend: any = '';
append: any = 'kWh';
min: number = 0;
max: number = 10000;
foregroundColor: string = '#80F86C';
backgroundColor: string = '#80F86C';

enableMarkers: boolean = false;


markerConfig = {
    "3000": { color: '#blue', size: 8, label: '30', type: 'line'},
    "7000": { color: '#yellow', size: 8, label: '60', type: 'line'},
    "10000": { color: '#red', size: 8, label: '100', type: 'line'},
}
 

onClick() {
  console.log(this.foregroundColor, this.backgroundColor);
  this.foregroundColor = 'red';
}



showNewGauge = false;

// markerConfig = {
//   "0": { color: '#555', size: 8, label: '0', type: 'line'},
//   "15": { color: '#555', size: 4, type: 'line'},
//   "30": { color: '#555', size: 8, label: '30', type: 'line'},
//   "40": { color: '#555', size: 4, type: 'line'},
//   "50": { color: '#555', size: 8, label: '50', type: 'line'},
//   "60": { color: '#555', size: 4, type: 'line'},
//   "70": { color: '#555', size: 8, label: '70', type: 'line'},
//   "85": { color: '#555', size: 4, type: 'line'},
//   "100": { color: '#555', size: 8, label: '100', type: 'line'},
// }



constructor(private todayConsumption:HistoryPredictionService){

}

    async ngOnInit() {
      let token=new JwtToken();
      this.idProsumer=token.data.id as number;
      const result = await this.todayConsumption.getTotalConsumptionProductionProsumer(2,this.idProsumer).subscribe(result=>{
        this.value = result;
      })
    
      
    }
}
