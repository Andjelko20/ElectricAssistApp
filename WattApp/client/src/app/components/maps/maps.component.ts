import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MapInfoWindow, MapMarker } from '@angular/google-maps';
import { catchError, map, Observable, of } from 'rxjs';
import * as Leaflet from 'leaflet';

@Component({
  selector: 'app-maps',
  templateUrl: './maps.component.html',
  styleUrls: ['./maps.component.css']
})
export class MapsComponent {
  constructor(){}
  title = 'AngularOSM';

  options: Leaflet.MapOptions = {
    layers: getLayers(),
    zoom: 12,
    center: new Leaflet.LatLng(44.017319, 20.907224)
  };
}

export const getMarkers = (): Leaflet.Marker[] => {
  return [
    new Leaflet.Marker(new Leaflet.LatLng(44.017319, 20.907224), {
      icon: new Leaflet.Icon({
        iconSize: [50, 41],
        iconAnchor: [13, 41],
        iconUrl: 'assets/blue-marker.svg',
      }),
      title: 'Workspace'
    } as Leaflet.MarkerOptions),
    new Leaflet.Marker(new Leaflet.LatLng(44.01722378323754, 20.90793433434322), {
      icon: new Leaflet.Icon({
        iconSize: [50, 41],
        iconAnchor: [13, 41],
        iconUrl: 'assets/red-marker.svg',
      }),
      title: 'Riva'
    } as Leaflet.MarkerOptions),
  ] as Leaflet.Marker[];
};


export const getLayers = (): Leaflet.Layer[] => {
  return [
    // Basic style
    new Leaflet.TileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: '&copy; OpenStreetMap contributors'
    } as Leaflet.TileLayerOptions),
     //Pastel style, remove if you want basic style. Uncomment if you want pastel style.

    //  new Leaflet.TileLayer('https://api.maptiler.com/maps/hybrid/style.json?key=8OcvVK6xO305e42bAe02', {
    //    attribution: '<a href="https://www.maptiler.com/copyright/" target="_blank">© MapTiler</a> <a href="https://www.openstreetmap.org/copyright" target="_blank">© OpenStreetMap contributors</a>',
    //  } as Leaflet.TileLayerOptions),
    ...getMarkers(),
    // ...getRoutes(),
    // ...getPolygons()
  ] as Leaflet.Layer[];
};

// export const getPolygons = (): Leaflet.Polygon[] => {
//   return [
//     new Leaflet.Polygon([
//       new Leaflet.LatLng(44.017319, 20.907224),
//       new Leaflet.LatLng(44.01722378323754, 20.90793433434322),
      
//     ] as Leaflet.LatLng[],
//       {
//         fillColor: '#eb530d',
//         color: '#eb780d'
//       } as Leaflet.PolylineOptions)
//   ] as Leaflet.Polygon[];
// };
