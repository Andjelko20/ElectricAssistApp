import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-number-of-users-meter',
  templateUrl: './number-of-users-meter.component.html',
  styleUrls: ['./number-of-users-meter.component.css']
})
export class NumberOfUsersMeterComponent implements OnInit {

  ngOnInit(): void {
	  fetch(environment.serverUrl+"/api/ProsumersDetails/count",{headers:{"Authorization":"Bearer "+localStorage.getItem("token")}})
	  .then(res=>res.json())
	  .then(res=>this.value=res);
  }
enableThresholds: boolean = false;
value!: number ;
thick: number = 15;
type: any = "full";
cap: any = "round";
label: string = "Prosumers";
prepend: any = '';
append: any = '';
min: number = 0;
max: number = 50;

foregroundColor: string = '#537FE7';
backgroundColor: string = '#85CDFD';

enableMarkers: boolean = false;


markerConfig = {
    "3000": { color: '#blue', size: 8, label: '30', type: 'line'},
    "7000": { color: '#yellow', size: 8, label: '60', type: 'line'},
    "10000": { color: '#red', size: 8, label: '100', type: 'line'},
}

onClick() {
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




}
