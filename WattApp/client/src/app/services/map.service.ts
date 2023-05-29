import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

@Injectable({providedIn:"root"})
export class MapService{
	constructor(private http:HttpClient){}

	getAddressByCoordinates(lat:number,lon:number){
		let searchParams = new URLSearchParams();
		searchParams.set("format","json");
		searchParams.set("addressdetails","addressdetails");
		searchParams.set("polygon_geojson","0");
		searchParams.set("lat",lat.toString());
		searchParams.set("lon",lon.toString());
		return this.http.get("https://nominatim.openstreetmap.org/reverse?"+searchParams.toString());
	}
}