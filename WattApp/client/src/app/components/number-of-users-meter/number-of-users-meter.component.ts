import { Component } from '@angular/core';

@Component({
  selector: 'app-number-of-users-meter',
  templateUrl: './number-of-users-meter.component.html',
  styleUrls: ['./number-of-users-meter.component.css']
})
export class NumberOfUsersMeterComponent {


enableThresholds: boolean = false;
value: number = 2222;
thick: number = 18;
size: number = 300;
type: any = "semi";
cap: any = "round";
label: string = "Active users";
prepend: any = '';
append: any = 'kWh';
min: number = 0;
max: number = 10000;
foregroundColor: string = '#009688';
backgroundColor: string = '#ebebeb';

enableMarkers: boolean = false;

thresholdConfig = {
  '0': { color: 'green', "bgOpacity": 0.2 },
  '4000': { color: 'orange', "bgOpacity": 0.2 },
  '7500': { color: 'red', "bgOpacity": 0.2 }
};

markerConfig = {
    "3000": { color: '#555', size: 8, label: '30', type: 'line'},
    "7000": { color: '#555', size: 8, label: '60', type: 'line'},
    "10000": { color: '#555', size: 8, label: '100', type: 'line'},
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
