import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { WeekByDay, YearsByMonth, updateDevices } from '../models/devices.model';
import { NavigationStart, Router } from '@angular/router';
import { Location } from '@angular/common';
import e from 'cors';
@Injectable({
  providedIn: 'root'
})
export class DevicesService {

  previousUrl:string;
  constructor(private http:HttpClient,private router:Router,private location: Location) {
    this.previousUrl=''
    this.router.events.subscribe((event) => {
    if (event instanceof NavigationStart) {
      
      if(this.location.path()==='/dashboard')
      {
        this.previousUrl = '/dashboard';
      }
      else if(this.location.path()==='/devices')
      {
        this.previousUrl = '/devices';
      }
     
    }
  }); }
  getBack():string
  {
    return this.previousUrl;
  }
  getAllDevices(pageNumber:number, pageSize:number,filter:any):Observable<any>
  {
    let url = new URL(environment.serverUrl + "/api/device");
    url.searchParams.set("pageNumber",pageNumber.toString());
	  url.searchParams.set("pageSize",pageSize.toString());
    if(filter == null)
      return this.http.get<any>(url.toString(),{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
    else{
      if(filter?.categoryId > 0)
        url.searchParams.set("categoryId", filter.categoryId);
      if(!isNaN(filter.turnOn) && filter.turnOn>-1){
        if(filter.turnOn == 1){
          url.searchParams.set("turnOn", "true");
        }
        else{
          url.searchParams.set("turnOn", "false");
        }
      }
      if(!isNaN(filter.visibility) && filter.visibility > -1){
        if(filter.visibility == 1)
          url.searchParams.set("visibility", "true");
        else
          url.searchParams.set("visibility", "false");
      }
      if(!isNaN(filter.controlability) && filter.controlability > -1){
        if(filter.controlability == 1)
          url.searchParams.set("controlability", "true");
        else
          url.searchParams.set("controlability", "false");
      }
      if(filter.searchValue != "")
        url.searchParams.set("searchValue", filter.searchValue);
      if(!isNaN(filter.sortCriteria)){
        if(filter.sortCriteria == 1)
          url.searchParams.set("sortCriteria", "0");
        else
          url.searchParams.set("sortCriteria", "1");   
      }
      if(!isNaN(filter.byAscending)){
        if(filter.byAscending == 1)
          url.searchParams.set("byAscending", "true")
        else
        url.searchParams.set("byAscending", "false")
      }
        
    }
    return this.http.get<any>(url.toString(),{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  getNumberOfDevices(userId : number):Observable<any>{
    return this.http.get<any>(environment.serverUrl+"/api/Prosumer/numberOfDevices/" + userId);
  }
  getAllDevicesNoPaggination():Observable<any>
  {
    return this.http.get<any>(environment.serverUrl+'/api/device',{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }
  getDevice(id:number):Observable<any>
  {
    return this.http.get<any>(environment.serverUrl+"/api/device/"+id,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  getDeviceProsumer(id:number,pageNumber:number,pageSize:number,filters:any):Observable<any>
  {
    let url = new URL(environment.serverUrl + "/api/Device/devices" + id);
    url.searchParams.set("pageNumber", pageNumber.toString());
    url.searchParams.set("pageSize", pageSize.toString());

    if(filters == null)
      return this.http.get<any>(url.toString(),{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
    else{
      if(filters.categoryId > 0)
        url.searchParams.set("categoryId", filters.categoryId);
      if(filters.typeId > 0)
        url.searchParams.set("typeId", filters.typeId);
      if(filters.brandId > 0)
        url.searchParams.set("brandId", filters.brandId);
      if(filters.turnOn > -1){
        if(filters.turnOn == 1)
          url.searchParams.set("turnOn", "true")
        else
          url.searchParams.set("turnOn", "false");
      }
      if(filters.controlability > -1){
        if(filters.controlability == 1)
          url.searchParams.set("controlability", "true")
        else
          url.searchParams.set("controlability", "false");
      }
      if(filters.energyInKwhValue != 0){
        if(filters.greaterThan == 0){
          url.searchParams.set("greaterThan", "false");
        }
        else{
          url.searchParams.set("greaterThan", "true");
        }

        url.searchParams.set("energyByKwh", filters.energyInKwhValue);
      }
      if(filters.searchValue != "")
        url.searchParams.set("searchValue", filters.searchValue);
    }
    return this.http.get<any>(url.toString(),{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
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
  turnOn(id: number,datestart:any): Observable<any> {
      
    return this.http.put<any>(environment.serverUrl+"/api/prosumer/device?deviceId="+id+"&turnedOn="+datestart,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}}); 
  }
  turnOff(id: number,dateend:any): Observable<any> {
      
    return this.http.put<any>(environment.serverUrl+"/api/prosumer/device?deviceId="+id+"&turnedOff="+dateend,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}}); 
  }
  visibility(id: number): Observable<any> {
      
    return this.http.put(environment.serverUrl+"/api/device/visibility"+id,{visibility:true},{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}}); 
  }
  durationDateTime(id: number): Observable<any>
  {
    return this.http.put<any>(environment.serverUrl+"/api/prosumer/device?deviceId="+id,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}}); 

  }
  controlability(id: number): Observable<any> {
      
    return this.http.put(environment.serverUrl+"/api/device/controlability"+id,{controlability:true},{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}}); 
  }

  weekByDay(cityId:number,deviceCategoryId:number): Observable<WeekByDay[]>{
    return this.http.get<WeekByDay[]>(environment.serverUrl+"/api/History/WeekByDay/City/"+cityId+"/"+deviceCategoryId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  weekByDaySettlement(settlementId:number,deviceCategoryId:number): Observable<WeekByDay[]>{
    return this.http.get<WeekByDay[]>(environment.serverUrl+"/api/History/WeekByDay/Settlement/"+settlementId+"/"+deviceCategoryId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  monthByDay(cityId:number,deviceCategoryId:number): Observable<WeekByDay[]>{
    return this.http.get<WeekByDay[]>(environment.serverUrl+"/api/History/MonthByDay/City/"+cityId+"/"+deviceCategoryId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  monthByDaySettlement(settlementId:number,deviceCategoryId:number): Observable<WeekByDay[]>{
    return this.http.get<WeekByDay[]>(environment.serverUrl+"/api/History/MonthByDay/Settlement/"+settlementId+"/"+deviceCategoryId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  yearByMonth(cityId:number,deviceCategoryId:number): Observable<YearsByMonth[]>{
    return this.http.get<YearsByMonth[]>(environment.serverUrl+"/api/History/YearByMonth/City/"+cityId+"/"+deviceCategoryId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  yearByMonthSettlement(settlementId:number,deviceCategoryId:number): Observable<YearsByMonth[]>{
    return this.http.get<YearsByMonth[]>(environment.serverUrl+"/api/History/YearByMonth/Settlement/"+settlementId+"/"+deviceCategoryId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  
  
  
 
}
