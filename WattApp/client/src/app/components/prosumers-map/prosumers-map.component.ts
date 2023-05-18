import { AfterViewInit, Component,OnInit, ViewChild } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as Leaflet from 'leaflet';
import { environment } from 'src/environments/environment';
import { NgModel } from '@angular/forms';
import { Popover, Tooltip } from 'bootstrap';

function escape(regex:string) {
	return regex.replace(/[.*+?^${}()|[\]\\]/g, "\\$&"); // Koristi escape() funkciju da izbegne specijalne karaktere
}

@Component({
  selector: 'app-prosumers-map',
  templateUrl: './prosumers-map.component.html',
  styleUrls: ['./prosumers-map.component.css']
})
export class ProsumersMapComponent implements OnInit,AfterViewInit {
	popover: Popover | undefined;
  	tooltip: Tooltip | undefined;
	ngAfterViewInit(): void {
		const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
		const tooltipList = Array.from(tooltipTriggerList).map(function (tooltipTriggerEl) {
		  return new Tooltip(tooltipTriggerEl)
		});
		this.tooltip = tooltipList[0];
	}
	
	  private map!: Leaflet.Map;
	  private markers:Leaflet.Marker[]=[];
	  private greenIcon:any;
	  private blueIcon:any;
	  private redIcon:any;
	  private searchUrl!:URL;
	  public searchInput!:string;
	  public locations:any[]=[];
	  public prosumers:any[]=[];
	  public filteredUsers:any[]=[];
	  private mapLayer:Leaflet.LayerGroup<any>|null=null;
	  public zone:any="0";
	  public city:any="0";
	  public cities:any[]=[];
	  private prosumersUrl!:URL;
	  public name:string="";
	  public loading:boolean=true;
	  public searchResultVisible:boolean=false;
	public legend=[
		{
			color:"--green-square",
			description:"Green zone - Consumption less than 350 kWh"
		},
		{
			color:"--blue-square",
			description:"Blue zone - Consumption between 351 and 1600 kWh"
		},
		{
			color:"--red-square",
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
		document.addEventListener('click', (event) => {
			const searchResult = document.querySelector('.search-result');
			if (searchResult && event.target !== searchResult && !searchResult.contains(event.target as Node)) {
			  this.searchResultVisible = false;
			}
		  });

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

		this.greenIcon = this.createMarker('assets/marker-green.png');

		this.redIcon = this.createMarker('assets/marker-red.png');

		this.blueIcon=this.createMarker('assets/blue-marker.png');
	
		Leaflet.Marker.prototype.options.icon = icon;
		this.map = Leaflet.map('prosumers-map').setView([44.01721187973962, 20.90732574462891], 13); // postavljanje mape i početni prikaz
		fetch(environment.serverUrl+"/api/users/my_location",{headers:{"Authorization":"Bearer "+localStorage.getItem("token")}})
		.then(res=>{
			if(res.ok)
				return res.json();
			return Promise.reject();
		})
		.then(res=>{this.map.setView([res.lat, res.lon], 13);})
		.catch(_=>{this.map.setView([44.01721187973962, 20.90732574462891], 13);})
	
		Leaflet.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
		  attribution: 'Map data © <a href="https://openstreetmap.org">OpenStreetMap</a> contributors',
		  maxZoom: 19,
		}).addTo(this.map); // dodavanje OpenStreetMap sloja
		this.prosumersUrl=new URL(environment.serverUrl+"/api/ProsumersDetails");
		/*fetch(environment.serverUrl+"/cities?countryId=1")
		.then(res=>res.json())
		.then(res=>{
			this.cities=res;
		})*/
		this.fetchMarkers();
		
	  }
	  fetchMarkers(){
		if(this.mapLayer!=null)
			this.mapLayer.removeFrom(this.map);
		this.markers=[];
		this.loading=true;
		fetch(this.prosumersUrl.toString(),{headers:{"Authorization":"Bearer "+localStorage.getItem("token")}})
		.then(res=>res.json())
		.then(res=>{
			this.loading=false;
			this.prosumers=res.map((r:any,i:number)=>{
				r.index=i;
				return r;
			});
			let markers=[];
			this.mapLayer=new Leaflet.LayerGroup<any>();
			for(let prosumer of res){
				if(prosumer.consumption==undefined)
					prosumer.consumption=0;
				let icon;
				if(prosumer.consumption<=350)
					icon=this.greenIcon;
				else if(prosumer.consumption>1600) icon=this.redIcon;
				else icon=this.blueIcon;
				
				let marker=Leaflet.marker([prosumer.latitude,prosumer.longitude],{icon}).addTo(this.mapLayer);
				markers.push(marker);
				marker.bindPopup(`<b>${prosumer.name}</b><br><a href="prosumer/${prosumer.id}">Details</a>`);
			}
			this.mapLayer.addTo(this.map);
			this.markers=markers;
		});
	  }
	  changeFocus(location:any){
		let options:Leaflet.ZoomPanOptions={
			animate:true
		};
		this.map.flyTo([location.lat,location.lon],this.map.getZoom(),options);
		//this.map.setView([location.lat,location.lon],13,options);
	  }
	  changeZone(){
		this.prosumersUrl.searchParams.set("zone",this.zone);
		this.fetchMarkers();
	  }
	  changeCity(){
		let city=this.cities.find(city=>city.id==this.city);
		this.searchUrl.searchParams.set("q",city.name+",Serbia");
		fetch(this.searchUrl.toString(),{headers:{"Accept-Language":"en-US"}})
		.then(res=>res.json())
		.then(res=>{
			this.changeFocus(res[0]);
		});
	  }
	  changeName(){
		if(this.name=="" || this.name==undefined || this.name.length<2){
			this.filteredUsers=[];
			this.searchResultVisible=false;
			return;
		}
		const regexObject = new RegExp(escape(this.name), "gi"); // Kreiranje RegExp objekta sa ignorisanjem specijalnih karaktera
		this.filteredUsers=this.prosumers.filter(prosumer=>{
			if(!regexObject.test(prosumer.name) && !regexObject.test(prosumer.address))
				return false;
			return true;
		});
		this.searchResultVisible=true;
	  }
	  focusUser(user:any){
		this.name="";
		this.changeFocus({lat:user.latitude,lon:user.longitude});
		this.searchResultVisible=false;
		this.markers[user.index].openPopup();
	  }
}
