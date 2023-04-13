import { Component,OnInit, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as Leaflet from 'leaflet';
import { environment } from 'src/environments/environment';
import { NgModel } from '@angular/forms';

@Component({
  selector: 'app-prosumers-map',
  templateUrl: './prosumers-map.component.html',
  styleUrls: ['./prosumers-map.component.css']
})
export class ProsumersMapComponent {
	
	  private map!: Leaflet.Map;
	  private searchUrl!:URL;
	  public searchInput!:string;
	  public locations:any[]=[];
	  private prosumers!:any[];
	  /*[
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
	  	},
		{
			latitude:44.02995805102632,
			longitude:20.90509414672852,
			name:"Laza Lazic",
			consumption:100
	  	}
	];*/
	public legend=[
		{
			color:"green",
			description:"Green zone - Consumption less than 350 kWh"
		},
		{
			color:"blue",
			description:"Blue zone - Consumption between 351 and 1600 kWh"
		},
		{
			color:"red",
			description:"Red zone - Consumption more than 1601 kWh"
		}
	]
	private createMarker(url:string):Leaflet.Icon<Leaflet.IconOptions>{
		return Leaflet.icon({
			iconUrl: url,
			shadowUrl: 'assets/marker-shadow.png',
			iconSize: [30, 45],
			iconAnchor: [12, 41],
			popupAnchor: [1, -34],
			tooltipAnchor: [16, -28],
			shadowSize: [45, 45]
		});
	}
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

		const greenIcon = this.createMarker('assets/marker-green.png');

		const redIcon = this.createMarker('assets/marker-red.png');

		const blueIcon=this.createMarker('assets/blue-marker.png');
	
		Leaflet.Marker.prototype.options.icon = icon;
	  
		this.map = Leaflet.map('prosumers-map').setView([44.01721187973962, 20.90732574462891], 13); // postavljanje mape i početni prikaz
	
		Leaflet.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
		  attribution: 'Map data © <a href="https://openstreetmap.org">OpenStreetMap</a> contributors',
		  maxZoom: 19,
		}).addTo(this.map); // dodavanje OpenStreetMap sloja
		
		fetch(environment.serverUrl+"/api/ProsumersDetails",{headers:{"Authorization":"Bearer "+localStorage.getItem("token")}})
		.then(res=>res.json())
		.then(res=>{
			this.prosumers=res;
			for(let prosumer of this.prosumers){
				if(prosumer.consumption==undefined)
					prosumer.consumption=0;
				let icon;
				if(prosumer.consumption<=350)
					icon=greenIcon;
				else if(prosumer.consumption>=1600) icon=redIcon;
				else icon=blueIcon;
				let marker=Leaflet.marker([prosumer.latitude,prosumer.longitude],{icon}).addTo(this.map);
				marker.bindPopup(`<b>${prosumer.name}</b><br><a href="prosumer/${prosumer.id}">Details</a>`);
			}
		});
		
	  }

	  onSubmit(){
		if(this.searchInput=="" || this.searchInput==undefined)
			return;
		this.searchUrl.searchParams.set("q",this.searchInput);
		fetch(this.searchUrl.toString(),{headers:{"Accept-Language":"en-US"}})
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
	  }
}
