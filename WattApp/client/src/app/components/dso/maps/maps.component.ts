import { HttpClient } from '@angular/common/http';
import { Component, OnInit, ViewChild } from '@angular/core';
import { catchError, map, Observable, of } from 'rxjs';
import * as L from 'leaflet';

@Component({
  selector: 'app-maps',
  templateUrl: './maps.component.html',
  styleUrls: ['./maps.component.css']
})
export class MapsComponent implements OnInit {

  private map!: L.Map;
  private marker!: L.Marker;

  ngOnInit(): void {

    const icon = L.icon({
      iconUrl: 'assets/marker-icon.png',
      shadowUrl: 'assets/marker-shadow.png',
      iconSize: [25, 41],
      iconAnchor: [12, 41],
      popupAnchor: [1, -34],
      tooltipAnchor: [16, -28],
      shadowSize: [41, 41]
    });

    L.Marker.prototype.options.icon = icon;
  
    this.map = L.map('map').setView([44.01721187973962, 20.90732574462891], 13); // postavljanje mape i početni prikaz

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      attribution: 'Map data © <a href="https://openstreetmap.org">OpenStreetMap</a> contributors',
      maxZoom: 18,
    }).addTo(this.map); // dodavanje OpenStreetMap sloja

    this.marker = L.marker([44.01721187973962, 20.90732574462891], { draggable: true }).addTo(this.map); // postavljanje čiode
    const latLng = this.marker.getLatLng();
    this.marker.bindPopup('Latitude: ' + latLng.lat + ', Longitude: ' + latLng.lng)

    // postavljanje događaja na klik na mapu
    this.map.on('click', (event: L.LeafletMouseEvent) => {
      this.marker.setLatLng(event.latlng);
      this.onSubmit();
      
    });
  }

  onSubmit(): void {
    const latLng = this.marker.getLatLng();
    this.marker.bindPopup('Latitude: ' + latLng.lat + ', Longitude: ' + latLng.lng)
    // slanje podataka na server
  }
}