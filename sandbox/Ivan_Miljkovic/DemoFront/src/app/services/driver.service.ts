import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Driver } from '../models/driver';

@Injectable({
  providedIn: 'root'
})
export class DriverService {

  private url="Driver";
  constructor( private http: HttpClient) { }

  public getDrivers() : Observable<Driver[]>{
  
    return this.http.get<Driver[]>(`${environment.apiUrl}/${this.url}`);
  }

  getDriver(id: string | null): Observable<Driver> {
    return this.http.get<Driver>(`${environment.apiUrl}/${this.url}/${id}`);
  }

  public updateDriver(driverUp: Driver) : Observable<Driver>{
  
    return this.http.put<Driver>(`${environment.apiUrl}/${this.url}/${driverUp.id}`,driverUp);
  }

  public createDriver(driverAdd: Driver) : Observable<Driver>{
  
    driverAdd.id='';
    return this.http.post<Driver>(`${environment.apiUrl}/${this.url}`,driverAdd);
  }

  public deleteDriver(id: string) : Observable<Driver>{
  
    return this.http.delete<Driver>(`${environment.apiUrl}/${this.url}/${id}`);
  }
}
