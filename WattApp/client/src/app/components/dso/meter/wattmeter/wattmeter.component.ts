import { Component, OnInit } from '@angular/core';
import { NgxGaugeType } from 'ngx-gauge/gauge/gauge';



@Component({
  selector: 'app-wattmeter',
  templateUrl: './wattmeter.component.html',
  styleUrls: ['./wattmeter.component.css']
})
export class WattmeterComponent implements OnInit {
  



enableThresholds: boolean = false;
value: number = 28.3;
thick: number = 20;
size: number = 300;
type: any = "semi";
cap: any = "round";
label: string = "Speed";
prepend: any = '';
append: any = 'km/hr';
min: number = 0;
max: number = 100;
foregroundColor: string = '#009688';
backgroundColor: string = '#ebebeb';

enableMarkers: boolean = false;

thresholdConfig = {
  '0': { color: 'green', bgOpacity: .2 },
  '40': { color: 'orange', bgOpacity: .2 },
  '75.5': { color: 'red', bgOpacity: .2 }
};

markerConfig = {
    "30": { color: '#555', size: 8, label: '30', type: 'line'},
    "70": { color: '#555', size: 8, label: '60', type: 'line'},
    "100": { color: '#555', size: 8, label: '100', type: 'line'},
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



  ngOnInit(): void {
    this.MeterChart();
  }
  MeterChart(){
    
  }
}
