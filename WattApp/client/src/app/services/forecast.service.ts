import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, from, throwError } from 'rxjs';
import { catchError, map, switchMap } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class ForecastService {

  constructor(private http: HttpClient,private authService:AuthService) { }

  getWeatherForecast(): Observable<any> {
    let defaultLocation = {
      lat: 51.5074,
      lon: -0.1278
    };
    
    return this.authService.getMyLocation().pipe(
      switchMap((location: { latitude: number, longitude: number }) => {
        const apiLocation = { lat: location.latitude, lon: location.longitude };
        return this.makeWeatherApiCall(apiLocation);
      }),
      catchError((error: any) => {
        console.error('Error getting weather forecast: ', error);
        return this.makeWeatherApiCall(defaultLocation);
      })
    );
  }

  private makeWeatherApiCall(location: any): Observable<any> {
    const params = new HttpParams()
      .set('lon', location.lon)
      .set('lat', location.lat)
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
