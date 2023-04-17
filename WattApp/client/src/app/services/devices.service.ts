import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { WeekByDay, YearsByMonth, updateDevices } from '../models/devices.model';

@Injectable({
  providedIn: 'root'
})
export class DevicesService {

  constructor(private http:HttpClient) { }

  getAllDevices(pageNumber:number, pageSize:number,categoryId:number):Observable<any>
  {
    return this.http.get<any>(environment.serverUrl+'/api/device?pageNumber='+pageNumber+'&pageSize=12&categoryId='+categoryId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }
  getDevice(id:number):Observable<any>
  {
    return this.http.get<any>(environment.serverUrl+"/api/device/"+id,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  getDeviceProsumer(id:number,pageNumber:number,pageSize:number,category:number):Observable<any>
  {
    return this.http.get<any>(environment.serverUrl+"/api/Device/devices"+id+"?pageNumber="+pageNumber+"&pageSize="+pageSize+"&categoryId="+category,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
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

  weekByDay(cityId:number,deviceCategoryId:number): Observable<WeekByDay[]>{
    return this.http.get<WeekByDay[]>(environment.serverUrl+"/api/History/WeekByDay/City/"+cityId+"/"+deviceCategoryId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  monthByDay(cityId:number,deviceCategoryId:number): Observable<WeekByDay[]>{
    return this.http.get<WeekByDay[]>(environment.serverUrl+"/api/History/MonthByDay/City/"+cityId+"/"+deviceCategoryId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  yearByMonth(cityId:number,deviceCategoryId:number): Observable<YearsByMonth[]>{
    return this.http.get<YearsByMonth[]>(environment.serverUrl+"/api/History/YearByMonth/City/"+cityId+"/"+deviceCategoryId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }
  
  
 
}
