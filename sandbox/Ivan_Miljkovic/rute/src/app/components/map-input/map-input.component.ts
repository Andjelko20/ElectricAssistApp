import { Component,OnInit, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as Leaflet from 'leaflet';
import { environment } from 'src/environments/environment';
import { NgModel } from '@angular/forms';

@Component({
  selector: 'app-prosumers-map',
  templateUrl: './map-input.component.html',
  styleUrls: ['./map-input.component.css']
})
export class MapInputComponent {
	
	  private map!: Leaflet.Map;
	  private searchUrl!:URL;
	  public searchInput!:string;
	  public locations:any[]=[];
	  public marker!:Leaflet.Marker;
	
	  ngOnInit(): void {
		this.searchUrl=new URL(environment.mapSearchUrl);
		this.searchUrl.searchParams.set("format","json");
		this.searchUrl.searchParams.set("addressdetails","addressdetails");
		this.searchUrl.searchParams.set("polygon_geojson","0");

		
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
		this.marker = Leaflet.marker([44.01721187973962, 20.90732574462891], { draggable: true }).addTo(this.map); // postavljanje čiode
		let latLng = this.marker.getLatLng();
		this.marker.bindPopup('Latitude: ' + latLng.lat + ', Longitude: ' + latLng.lng)

    	// postavljanje događaja na klik na mapu
		this.map.on('click', (event: Leaflet.LeafletMouseEvent) => {
			this.marker.setLatLng(event.latlng);
			latLng = this.marker.getLatLng();
			this.marker.bindPopup('Latitude: ' + latLng.lat + ', Longitude: ' + latLng.lng);
		});
		this.marker.on("dragend",(event:L.DragEndEvent)=>{
			this.marker.setLatLng(event.target.getLatLng());
			latLng = this.marker.getLatLng();
			this.marker.bindPopup('Latitude: ' + latLng.lat + ', Longitude: ' + latLng.lng);
		});
	  }

	  onSubmit(){
		if(this.searchInput=="")
			return;
		this.searchUrl.searchParams.set("q",this.searchInput);
		fetch(this.searchUrl.toString())//,{headers:{"Accept-Language":"en-US"}})
		.then(res=>res.json())
		.then(res=>{
			this.locations=res;
		});
	  }
	  changeFocus(location:any){
		let options:Leaflet.ZoomPanOptions={
			animate:true
		};
		this.map.setView([location.lat,location.lon],this.map.getZoom(),options);
		this.marker.setLatLng([location.lat,location.lon]);
		let latLng=this.marker.getLatLng();
		this.marker.bindPopup('Latitude: ' + latLng.lat + ', Longitude: ' + latLng.lng);
	  }
}