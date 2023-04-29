import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { from } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ForecastService {

  constructor(private http: HttpClient) { }

  getWeatherForecast() {
    return from(new Promise((resolve, reject) => {
      navigator.geolocation.getCurrentPosition(resolve, reject);
    })).pipe(
      map((position: any) => {
        return new HttpParams()
          .set('lon', position.coords.longitude)
          .set('lat', position.coords.latitude)
          .set('units', "metric")
          .set('appid', environment.openWeatherMapApiKey)
      }),
      switchMap((params: HttpParams) => {
        return this.http.get('https://api.openweathermap.org/data/2.5/forecast', { params })
      }),
      catchError((error: any) => {
        return from(Promise.reject(error));
      })
    );
  }

}