import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { updateDevices } from '../models/devices.model';

@Injectable({
  providedIn: 'root'
})
export class DevicesService {

  constructor(private http:HttpClient) { }

  getAllDevices(pageNumber:number, pageSize:number,categoryId:number):Observable<any>
  {
    return this.http.get<any>(environment.serverUrl+'/api/device?pageNumber='+pageNumber+'&pageSize='+pageSize+'&categoryId='+categoryId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }
  getDevice(id:number):Observable<any>
  {
    return this.http.get<any>(environment.serverUrl+"/api/device/"+id,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }
  addDevices(addDeviceRequest:any):Observable<any>
  {
    
    return this.http.post<any>(environment.serverUrl + '/api/device' ,addDeviceRequest,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }
  upDateDevice(updateRequest:updateDevices):Observable<updateDevices>
  {
    return this.http.put<updateDevices>(environment.serverUrl+'/api/device',updateRequest,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }
  delete(id:number):Observable<updateDevices>
  {
    return this.http.delete<updateDevices>(environment.serverUrl+"/api/device/"+id,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}}); 
  }
  //samo id saljem
  turnOnOff(id: number): Observable<any> {
      
    return this.http.put(environment.serverUrl+"/api/device/turnOn"+id,{turnOn:true},{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}}); 
  }
  visibility(id: number): Observable<any> {
      
    return this.http.put(environment.serverUrl+"/api/device/visibility"+id,{visibility:true},{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}}); 
  }
  
  controlability(id: number): Observable<any> {
      
    return this.http.put(environment.serverUrl+"/api/device/controlability"+id,{controlability:true},{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}}); 
  }
  

  
  
  
 
}
