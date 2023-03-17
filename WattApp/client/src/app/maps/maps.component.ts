import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MapInfoWindow, MapMarker } from '@angular/google-maps';
import { catchError, map, Observable, of } from 'rxjs';
import * as Leaflet from 'leaflet';
// import maplibregl from 'maplibre-gl';


@Component({
  selector: 'app-maps',
  templateUrl: './maps.component.html',
  styleUrls: ['./maps.component.css']
})
export class MapsComponent implements OnInit {
  ngOnInit(): void {
    throw new Error('Method not implemented.');
  }

  constructor(){}
  title = 'AngularOSM';

  options: Leaflet.MapOptions = {
    layers: getLayers(),
    zoom: 12,
    center: new Leaflet.LatLng(43.530147, 16.488932)
  };
}
// const key = '8OcvVK6xO305e42bAe02';
// const map = new maplibregl.Map({
//   container: 'map', // container's id or the HTML element in which MapLibre GL JS will render the map
//   style: `https://api.maptiler.com/maps/streets-v2/style.json?key=${key}`, // style URL
//   center: [16.62662018, 49.2125578], // starting position [lng, lat]
//   zoom: 14, // starting zoom
// });



//   apiLoaded: Observable<boolean>;
//   constructor(httpClient: HttpClient) {
//     this.apiLoaded = httpClient.jsonp('https://maps.googleapis.com/maps/api/js?key=YOUR_KEY_HERE', 'callback')
//         .pipe(
//           map(() => true),
//           catchError(() => of(false)),
//         );
//   }

//   message = ""
//   latitude = 0.0;
//   longitude = 0.0;
//   brojac = 1;
//   ngOnInit(): void {
//     navigator.geolocation.getCurrentPosition((position) =>{
//       this.center ={
//         lat: 44.01749823218476,
//         lng: 20.90721429411449,
//       }
//     })

//   }
  
//   @ViewChild(MapInfoWindow) infoWindow: MapInfoWindow | undefined;
//   display: any;
//     center: google.maps.LatLngLiteral = {
//         lat: 24,
//         lng: 12

//     };
//     zoom = 18 ;
//     markerOptions: google.maps.MarkerOptions = {
//       draggable: false
//   };
//   markerPositions: google.maps.LatLngLiteral[] = [{
//     lat:44.01585976728324,
//     lng:20.908619344913156
//   },{
//     lat:44.01720922445889,
//     lng:20.90731151378836
//   },{
//     lat:44.01604418158692,
//     lng:20.90606696880545
//   }];
//   addMarker(event: google.maps.MapMouseEvent) {
//     if (event.latLng != null) {
//       console.log(event)
      
//       this.markerPositions.push(event.latLng.toJSON())
//       this.latitude = event.latLng.lat();
//       this.longitude = event.latLng.lng();

//     };
//   }
//   moveMap(event: google.maps.MapMouseEvent) {
//     if (event.latLng != null) this.center = (event.latLng.toJSON());
//   }
//   move(event: google.maps.MapMouseEvent) {
//       if (event.latLng != null) this.display = event.latLng.toJSON();
//   }
//   openInfoWindow(marker: MapMarker) {
//     if (this.infoWindow != undefined) {
//       this.message = "Lokacija "+this.brojac+" koordinate su "+this.latitude+" i "+this.longitude;
//       this.brojac++;
//       this.infoWindow.open(marker);
//     }
// }
// }

export const getMarkers = (): Leaflet.Marker[] => {
  return [
    new Leaflet.Marker(new Leaflet.LatLng(43.5121264, 16.4700729), {
      icon: new Leaflet.Icon({
        iconSize: [50, 41],
        iconAnchor: [13, 41],
        iconUrl: 'assets/blue-marker.svg',
      }),
      title: 'Workspace'
    } as Leaflet.MarkerOptions),
    new Leaflet.Marker(new Leaflet.LatLng(43.5074826, 16.4390046), {
      icon: new Leaflet.Icon({
        iconSize: [50, 41],
        iconAnchor: [13, 41],
        iconUrl: 'assets/red-marker.svg',
      }),
      title: 'Riva'
    } as Leaflet.MarkerOptions),
  ] as Leaflet.Marker[];
};

export const getRoutes = (): Leaflet.Polyline[] => {
  return [
    new Leaflet.Polyline([
      new Leaflet.LatLng(43.5121264, 16.4700729),
      new Leaflet.LatLng(43.5074826, 16.4390046),
    ] as Leaflet.LatLng[], {
      color: '#0d9148'
    } as Leaflet.PolylineOptions)
  ] as Leaflet.Polyline[];
};

export const getLayers = (): Leaflet.Layer[] => {
  return [
    // Basic style
    new Leaflet.TileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '&copy; OpenStreetMap contributors'
    } as Leaflet.TileLayerOptions),
     //Pastel style, remove if you want basic style. Uncomment if you want pastel style.

     new Leaflet.TileLayer('https://api.maptiler.com/maps/hybrid/style.json?key=8OcvVK6xO305e42bAe02', {
       attribution: '<a href="https://www.maptiler.com/copyright/" target="_blank">© MapTiler</a> <a href="https://www.openstreetmap.org/copyright" target="_blank">© OpenStreetMap contributors</a>',
     } as Leaflet.TileLayerOptions),
    ...getMarkers(),
    ...getRoutes(),
    ...getPolygons()
  ] as Leaflet.Layer[];
};

export const getPolygons = (): Leaflet.Polygon[] => {
  return [
    new Leaflet.Polygon([
      new Leaflet.LatLng(43.5181349, 16.4537963),
      new Leaflet.LatLng(43.517890, 16.439939),
      new Leaflet.LatLng(43.515599, 16.446556),
      new Leaflet.LatLng(43.518025, 16.463492)
    ] as Leaflet.LatLng[],
      {
        fillColor: '#eb530d',
        color: '#eb780d'
      } as Leaflet.PolylineOptions)
  ] as Leaflet.Polygon[];
};

