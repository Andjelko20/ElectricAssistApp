import { Component,OnInit, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, map, Observable, of } from 'rxjs';
import * as Leaflet from 'leaflet';

@Component({
  selector: 'app-prosumers-map',
  templateUrl: './prosumers-map.component.html',
  styleUrls: ['./prosumers-map.component.css']
})
export class ProsumersMapComponent {
	
	  private map!: Leaflet.Map;
	  private marker!: Leaflet.Marker;
	  private prosumers=[
		{
			latitude:44.048325,
			longitude:20.954041,
			name:"Pera Peric",
			consumption:100
	  	},
		{
			latitude:44.01721187973950,
			longitude:20.90732574462900,
			name:"Mika Mikic",
			consumption:100
	  	}
	];
	
	  ngOnInit(): void {
	
		const icon = Leaflet.icon({
		  iconUrl: 'assets/marker-icon.png',
		  shadowUrl: 'assets/marker-shadow.png',
		  iconSize: [25, 41],
		  iconAnchor: [12, 41],
		  popupAnchor: [1, -34],
		  tooltipAnchor: [16, -28],
		  shadowSize: [41, 41]
		});
	
		Leaflet.Marker.prototype.options.icon = icon;
	  
		this.map = Leaflet.map('prosumers-map').setView([44.01721187973962, 20.90732574462891], 13); // postavljanje mape i početni prikaz
	
		Leaflet.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
		  attribution: 'Map data © <a href="https://openstreetmap.org">OpenStreetMap</a> contributors',
		  maxZoom: 18,
		}).addTo(this.map); // dodavanje OpenStreetMap sloja
	
		for(let prosumer of this.prosumers){
			console.log(prosumer)
			let marker=Leaflet.marker([prosumer.latitude,prosumer.longitude]).addTo(this.map);
			marker.bindPopup(`<b>${prosumer.name}</b> ${prosumer.consumption}`);
		}
	  }
	
}
