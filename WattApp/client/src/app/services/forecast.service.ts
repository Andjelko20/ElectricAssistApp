import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, from, throwError } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ForecastService {

  constructor(private http: HttpClient) { }

  getWeatherForecast(): Observable<any> {
    const defaultLocation = {
      latitude: 51.5074,
      longitude: -0.1278
    };

    if (navigator.geolocation) {
      return from(new Promise((resolve, reject) => {
        navigator.geolocation.getCurrentPosition(resolve, reject);
      })).pipe(
        map((position: any) => {
          return {
            latitude: position.coords.latitude,
            longitude: position.coords.longitude
          };
        }),
        switchMap((location: any) => {
          return this.makeWeatherApiCall(location);
        }),
        catchError((error: any) => {
          console.error('Error getting user location: ', error);
          return this.makeWeatherApiCall(defaultLocation);
        })
      );
    } else {
      console.error('Geolocation is not supported by this browser.');
      return this.makeWeatherApiCall(defaultLocation);
    }
  }

  private makeWeatherApiCall(location: any): Observable<any> {
    const params = new HttpParams()
      .set('lon', location.longitude)
      .set('lat', location.latitude)
      .set('units', "metric")
      .set('appid', environment.openWeatherMapApiKey);

    return this.http.get('https://api.openweathermap.org/data/2.5/forecast', { params })
      .pipe(
        catchError((error: any) => {
          console.error('Error retrieving weather data: ', error);
          return throwError(error);
        })
      );
  }
}