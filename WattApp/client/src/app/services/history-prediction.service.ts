import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { WeekByDay, YearsByMonth } from '../models/devices.model';

@Injectable({
  providedIn: 'root'
})
export class HistoryPredictionService {

  constructor(private http:HttpClient) { }

  getTotalConsumptionProductionDSO(id:number):Observable<any>
  {
    return this.http.get<any>(environment.serverUrl+'/api/Prosumer/'+id,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});

  }
  getTotalConsumptionProductionProsumer(name:string,id:number):Observable<number>
  {
    return this.http.get<number>(environment.serverUrl+'/api/Prosumer/'+name+'/'+id,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});

  }
  getCurrentConsumptionProductionCity(deviceCategoryId : number,cityId : number):Observable<number>
  {
    return this.http.get<number>(environment.serverUrl+'/api/Prosumer/city/'+deviceCategoryId+'/'+cityId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}})
  }
  getAverageConsumptionProductionCity(idCat: number,idCity:number):Observable<number>
  {
    return this.http.get<number>(environment.serverUrl+'/api/Prosumer/average/'+idCat+'/'+idCity,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}})
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
