import { Component } from '@angular/core';

@Component({
  selector: 'app-consumption-today-wattmeter',
  templateUrl: './consumption-today-wattmeter.component.html',
  styleUrls: ['./consumption-today-wattmeter.component.css']
})
export class ConsumptionTodayWattmeterComponent {

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
}
