import { Component, OnInit, ViewChild } from '@angular/core';
import { MapInfoWindow, MapMarker } from '@angular/google-maps';

@Component({
  selector: 'app-maps',
  templateUrl: './maps.component.html',
  styleUrls: ['./maps.component.css']
})
export class MapsComponent implements OnInit {

  constructor() { }

  message = ""
  latitude = 0.0;
  longitude = 0.0;
  brojac = 1;
  ngOnInit(): void {}
  
  @ViewChild(MapInfoWindow) infoWindow: MapInfoWindow | undefined;
  display: any;
    center: google.maps.LatLngLiteral = {
        lat: 24,
        lng: 12
    };
    zoom = 4;
    markerOptions: google.maps.MarkerOptions = {
      draggable: false
  };
  markerPositions: google.maps.LatLngLiteral[] = [];
  addMarker(event: google.maps.MapMouseEvent) {
    if (event.latLng != null) {
      this.markerPositions.push(event.latLng.toJSON())
      this.latitude = event.latLng.lat();
      this.longitude = event.latLng.lng();

    };
  }
  moveMap(event: google.maps.MapMouseEvent) {
    if (event.latLng != null) this.center = (event.latLng.toJSON());
  }
  move(event: google.maps.MapMouseEvent) {
      if (event.latLng != null) this.display = event.latLng.toJSON();
  }
  openInfoWindow(marker: MapMarker) {
    if (this.infoWindow != undefined) {
      this.message = "Lokacija "+this.brojac+" koordinate su "+this.latitude+" i "+this.longitude;
      this.brojac++;
      this.infoWindow.open(marker);
    }
}
}
