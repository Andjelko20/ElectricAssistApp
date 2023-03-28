import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Devices } from '../models/device.model';
import { Users } from '../models/users.model';

@Injectable({
  providedIn: 'root'
})
export class DevicesService {

  constructor(private http:HttpClient) { }

  getAllDevices():Observable<any>
  {
    return this.http.get<any>(environment.serverUrl+'/api/device',{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }
  getDevice(id:number):Observable<any>
  {
    return this.http.get<any>(environment.serverUrl+"/api/device/"+id,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }
  addDevice(addDeviceRequest:Devices):Observable<Devices>
  {
    
    return this.http.post<Devices>(environment.serverUrl + '/api/device' ,addDeviceRequest,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }
  delete(id:number):Observable<Devices>
  {
    return this.http.delete<Devices>(environment.serverUrl+"/api/device/"+id,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}}); 
  }
  //samo id saljem
  // turnOn(id: number): Observable<any> {
    
  //   return this.http.put(environment.serverUrl+"/api/device/turnOn"+id,{turnOn: true },{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}}); 
  // }
  // turnOff(id: number): Observable<any> {
    
  //   return this.http.put(environment.serverUrl+"/api/device/turnOn"+id,{turnOn: false },{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}}); 
  // }
  


}
