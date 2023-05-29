import { Component,OnInit, ViewChild,Output,EventEmitter, AfterViewInit, OnDestroy } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import * as Leaflet from 'leaflet';
import { environment } from 'src/environments/environment';
import { NgModel } from '@angular/forms';
import { MapService } from 'src/app/services/map.service';
import { data, error } from 'jquery';
import  { Popover } from 'bootstrap';

var cyrillic = ["а", "б", "в", "г", "д", "ђ", "е", "ж", "з", "и", "ј", "к", "л", "љ", "м", "н", "њ", "о", "п", "р", "с", "т", "ћ", "у", "ф", "х", "ц", "ч", "џ", "ш"];
var latin = ["a", "b", "v", "g", "d", "đ", "e", "ž", "z", "i", "j", "k", "l", "lj", "m", "n", "nj", "o", "p", "r", "s", "t", "ć", "u", "f", "h", "c", "č", "dž", "š"];


function cyrillicToLatin(text:string):string {

	var result = "";

	for (var i = 0; i < text.length; i++) {
	  var char = text.charAt(i);
	  var index = cyrillic.indexOf(char.toLowerCase());

	  if (index !== -1) {
		var latinChar = latin[index];
		if (char === char.toUpperCase()) {
		  latinChar = latinChar.toUpperCase();
		}
		result += latinChar;
	  } else {
		result += char;
	  }
	}

	return result;
  }


@Component({
  selector: 'map-input',
  templateUrl: './map-input.component.html',
  styleUrls: ['./map-input.component.css']
})
export class MapInputComponent implements OnInit, AfterViewInit,OnDestroy {
	@Output() locationChanged:EventEmitter<any>=new EventEmitter<any>();
	@Output() settlementChanged:EventEmitter<any>=new EventEmitter<any>();
	public searchResultVisible:boolean=false;
	  private map!: Leaflet.Map;
	  private searchUrl!:URL;
	  public searchInput!:string;
	  public locations:any[]=[];
	  public marker!:Leaflet.Marker;
	  public cities:any;
	  public settlements:any;

	  public settlementId:number=0;
	  public address:string="";

	  popover: Popover | undefined;
	  public popovers: Popover[] = [];
	  public cityElement!:HTMLSelectElement;
	  public settlementElement!:HTMLSelectElement;
	  public countryElement!:HTMLSelectElement;
	  constructor(private mapService:MapService){
		document.onclick=(event:any)=>{
			let searchResult=document.getElementsByClassName("search-result")[0];
			if(searchResult && event.target !== searchResult && !searchResult.contains(event.target as Node)){
				this.searchResultVisible=false;
			}
		}
	  }

	  changeAddress(lat:number,lon:number){
		this.mapService.getAddressByCoordinates(lat,lon).subscribe({
			next:(res:any)=>{
				try{
          let result = "";
					let address = res.address;
					if(address?.road !== undefined)
            result=cyrillicToLatin(address.road);
          if(address?.house_number !== undefined)
            result+=" "+address?.house_number;
          if(result != "")
            this.address=result;
				}
				catch{}
			},
			error:(err:any)=>{}
		})
	  }

	  ngAfterViewInit() {
		const popoverTriggerList = document.querySelectorAll('[data-bs-toggle="popover"]');
		const popoverList = Array.from(popoverTriggerList).map((popoverTriggerEl) => {
		return new Popover(popoverTriggerEl);
		});
		this.popovers = popoverList;

	  }
	  ngOnDestroy(): void {
		this.popovers.forEach((popover) => {
			popover.dispose();
		});
		this.popovers = [];
		this.popover = undefined;
	  }
	  ngOnInit(): void {
		this.countryElement=document.getElementById("country") as HTMLSelectElement
		this.cityElement=document.getElementById("city") as HTMLSelectElement;
		this.settlementElement=document.getElementById("settlement") as HTMLSelectElement;

		fetch(environment.serverUrl+"/cities?countryId=1",{headers:{"Authorization":"Bearer "+localStorage.getItem("token")}})
	.then(res=>res.json())
	.then(res=>{
		this.cities=res.map((r:any)=>({id:r.id,name:r.name}));
		this.onSelectedCity(
			{
			target:{
				value:JSON.stringify({id:this.cities[0].id})
			}
		}
		);
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

		Leaflet.Marker.prototype.options.icon = icon;

		this.map = Leaflet.map('prosumers-map').setView([44.01721187973962, 20.90732574462891], 13);// postavljanje mape i početni prikaz
		Leaflet.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
		  attribution: 'Map data © <a href="https://openstreetmap.org">OpenStreetMap</a> contributors',
		  maxZoom: 18,
		}).addTo(this.map); // dodavanje OpenStreetMap sloja
		this.marker = Leaflet.marker([44.01721187973962, 20.90732574462891], { draggable: true }).addTo(this.map); // postavljanje čiode
		let latLng = this.marker.getLatLng();
		//this.marker.bindPopup('Latitude: ' + latLng.lat + ', Longitude: ' + latLng.lng)

    	// postavljanje događaja na klik na mapu
		this.map.on('click', (event: Leaflet.LeafletMouseEvent) => {
			this.marker.setLatLng(event.latlng);
			latLng = this.marker.getLatLng();
			//this.marker.bindPopup('Latitude: ' + latLng.lat + ', Longitude: ' + latLng.lng);
			this.locationChanged.emit(latLng);
			this.changeAddress(latLng.lat,latLng.lng);
		});
		this.marker.on("dragend",(event:L.DragEndEvent)=>{
			this.marker.setLatLng(event.target.getLatLng());
			latLng = this.marker.getLatLng();
			//this.marker.bindPopup('Latitude: ' + latLng.lat + ', Longitude: ' + latLng.lng);
			this.locationChanged.emit(latLng);
			this.changeAddress(latLng.lat,latLng.lng);
		});
	  }

	  changeLocations(event:KeyboardEvent){
		if(this.address==undefined){
			this.searchResultVisible=false;
			return;
		}

		let trimmedAddress=this.address.trim();
		if(trimmedAddress=="" && trimmedAddress.length<2){
			this.searchResultVisible=false;
			return;
		}
		if(event?.key!==undefined && !/[A-Za-z0-9\ \-]+/.test(event.key))
			return;
		/*
		this.searchUrl.searchParams.set("country",JSON.parse(this.countryElement.value).name);
		this.searchUrl.searchParams.set("city",JSON.parse(this.cityElement.value).name);
		this.searchUrl.searchParams.set("street",this.address);
		*/
		this.searchResultVisible=true;
		let countryName=JSON.parse(this.countryElement.value).name;
		let cityName=JSON.parse(this.cityElement.value).name;
		let address=countryName+","+cityName+","+this.address;
		this.searchUrl.searchParams.set("q",address.trim());
		fetch(this.searchUrl.toString(),{headers:{"Accept-Language":"en-US"}})
		.then(res=>res.json())
		.then(res=>{
			this.locations=res.map((place:any)=>{
				place.display_name=cyrillicToLatin(place.display_name);
				if(place.address.road!==undefined && place.address.road!==null){
					place.address.road=cyrillicToLatin(place.address.road);
				}
				return place;
			});
		});
		this.sendData();
	  }
	  changeFocus(location:any){
		const road=location.address?.road;
		if(road!==undefined){
			this.address=road;
		}
		let options:Leaflet.ZoomPanOptions={
			animate:true
		};
		this.map.setView([location.lat,location.lon],this.map.getZoom(),options);
		this.marker.setLatLng([location.lat,location.lon]);
		let latLng=this.marker.getLatLng();
		this.marker.bindPopup('Latitude: ' + latLng.lat + ', Longitude: ' + latLng.lng);
		this.searchResultVisible=false;
	  }
	  onSelectedCity(event:any){
		let city=JSON.parse(event.target.value);
		let id=city.id;
		this.changeLocations(({}) as KeyboardEvent);
		fetch(environment.serverUrl+"/settlements?cityId="+id,{headers:{"Authorization":"Bearer "+localStorage.getItem("token")}})
		.then(res=>res.json())
		.then(res=>{
				this.settlements=res.map((r:any)=>({id:r.id,name:r.name}));
				this.settlementId=this.settlements[0].id;
			})
	  }
	  stringify(obj:any){
		return JSON.stringify(obj);
	  }
	  sendData(){
		this.settlementChanged.emit({settlement:this.settlementId,address:this.address});
	  }
}
