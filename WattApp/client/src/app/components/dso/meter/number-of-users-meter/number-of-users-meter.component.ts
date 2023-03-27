import { Component } from '@angular/core';

@Component({
  selector: 'app-number-of-users-meter',
  templateUrl: './number-of-users-meter.component.html',
  styleUrls: ['./number-of-users-meter.component.css']
})
export class NumberOfUsersMeterComponent {


enableThresholds: boolean = false;
value: number = 6222;
thick: number = 25;
size: number = 300;
type: any = "full";
cap: any = "round";
label: string = "of 10 000 users";
prepend: any = '';
append: any = '';
min: number = 0;
max: number = 10000;
foregroundColor: string = '#537FE7';
backgroundColor: string = '#85CDFD';

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



  ngOnInit(): void {
    this.MeterChart();
  }
  MeterChart(){
    
  }
}
