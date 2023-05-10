import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { DayByHour, WeekByDay, YearsByMonth } from '../models/devices.model';

@Injectable({
  providedIn: 'root'
})
export class HistoryPredictionService {

  constructor(private http:HttpClient) { }

  getTotalConsumptionProductionDSO(id:number):Observable<any>
  {
    return this.http.get<any>(environment.serverUrl+'/api/Prosumer/'+id,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});

  }
  getTotalConsumptionProductionProsumer(id1:number,id:number):Observable<number>
  {
    return this.http.get<number>(environment.serverUrl+'/api/Prosumer/user/'+id1+'/'+id,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});

  }
  getCurrentConsumptionProductionCity(deviceCategoryId : number,cityId : number):Observable<number>
  {
    return this.http.get<number>(environment.serverUrl+'/api/Prosumer/city/'+deviceCategoryId+'/'+cityId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}})
  }
  getCurrentConsumptionProductionSettlement(deviceCategoryId : number,settlementId : number):Observable<number>
  {
    return this.http.get<number>(environment.serverUrl+'/api/Prosumer/'+deviceCategoryId+'/'+settlementId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}})
  }
  getAverageConsumptionProductionCity(idCat: number,idCity:number):Observable<number>
  {
    return this.http.get<number>(environment.serverUrl+'/api/Prosumer/average/'+idCat+'/'+idCity,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}})
  }
  
  weekByDay(cityId:number,deviceCategoryId:number): Observable<WeekByDay[]>{
    return this.http.get<WeekByDay[]>(environment.serverUrl+"/api/History/WeekByDay/City/"+cityId+"/"+deviceCategoryId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  weekByDayUser(userId:number,deviceCategoryId:number): Observable<WeekByDay[]>{
    return this.http.get<WeekByDay[]>(environment.serverUrl+"/api/History/WeekByDay/User/"+userId+"/"+deviceCategoryId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  weekByDaySettlement(settlementId:number,deviceCategoryId:number): Observable<WeekByDay[]>{
    return this.http.get<WeekByDay[]>(environment.serverUrl+"/api/History/WeekByDay/Settlement/"+settlementId+"/"+deviceCategoryId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  monthByDay(cityId:number,deviceCategoryId:number): Observable<WeekByDay[]>{
    return this.http.get<WeekByDay[]>(environment.serverUrl+"/api/History/MonthByDay/City/"+cityId+"/"+deviceCategoryId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  monthByDayUser(userId:number,deviceCategoryId:number): Observable<WeekByDay[]>{
    return this.http.get<WeekByDay[]>(environment.serverUrl+"/api/History/MonthByDay/User/"+userId+"/"+deviceCategoryId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  monthByDaySettlement(settlementId:number,deviceCategoryId:number): Observable<WeekByDay[]>{
    return this.http.get<WeekByDay[]>(environment.serverUrl+"/api/History/MonthByDay/Settlement/"+settlementId+"/"+deviceCategoryId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  yearByMonth(cityId:number,deviceCategoryId:number): Observable<YearsByMonth[]>{
    return this.http.get<YearsByMonth[]>(environment.serverUrl+"/api/History/YearByMonth/City/"+cityId+"/"+deviceCategoryId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  yearByMonthUser(userId:number,deviceCategoryId:number): Observable<YearsByMonth[]>{
    return this.http.get<YearsByMonth[]>(environment.serverUrl+"/api/History/YearByMonth/User/"+userId+"/"+deviceCategoryId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  yearByMonthDevice(deviceId:number): Observable<YearsByMonth[]>{
    return this.http.get<YearsByMonth[]>(environment.serverUrl+"/api/History/YearByMonth/Device/"+deviceId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  yearByMonthSettlement(settlementId:number,deviceCategoryId:number): Observable<YearsByMonth[]>{
    return this.http.get<YearsByMonth[]>(environment.serverUrl+"/api/History/YearByMonth/Settlement/"+settlementId+"/"+deviceCategoryId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  historyDayUser(userId:number,deviceCategoryId:number): Observable<number>{
    return this.http.get<number>(environment.serverUrl+"/api/History/Day/User/"+userId+"/"+deviceCategoryId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  currentUserProductionConsumption(userId:number,deviceCategoryId:number): Observable<number>{
    return this.http.get<number>(environment.serverUrl+"/api/Prosumer/user/"+deviceCategoryId+"/"+userId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  historyMonthUser(userId:number,deviceCategoryId:number): Observable<number>{
    return this.http.get<number>(environment.serverUrl+"/api/History/Month/User/"+userId+"/"+deviceCategoryId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }
  dayByHour(cityId:number,deviceCategoryId:number): Observable<DayByHour[]>{
    return this.http.get<DayByHour[]>(environment.serverUrl+"/api/DSO/City?deviceCategoryId="+deviceCategoryId+"&todayByHourCityId="+cityId+"&cityName=null",{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  dayByHourUser(userId:number,deviceCategoryId:number): Observable<DayByHour[]>{
    return this.http.get<DayByHour[]>(environment.serverUrl+"/api/History/DayByHour/User/"+userId+"/"+deviceCategoryId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  dayByHourSettlement(settlementId:number,deviceCategoryId:number): Observable<DayByHour[]>{
    return this.http.get<DayByHour[]>(environment.serverUrl+"/api/DSO/Settlement?settlementId="+settlementId+"&deviceCategoryId="+deviceCategoryId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  getTodayTotalConsumption(cityId:number,deviceCategoryId:number): Observable<number>{
    return this.http.get<number>(environment.serverUrl+"/api/DSO/City?deviceCategoryId="+deviceCategoryId+"&todayCityId="+cityId+"&cityName=null",{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  getMonthTotalConsumption(cityId:number,deviceCategoryId:number): Observable<number>{
    return this.http.get<number>(environment.serverUrl+"/api/DSO/City?deviceCategoryId="+deviceCategoryId+"&thisMonthCityId="+cityId+"&cityName=null",{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  getYearTotalConsumption(cityId:number,deviceCategoryId:number): Observable<number>{
    return this.http.get<number>(environment.serverUrl+"/api/DSO/City?deviceCategoryId="+deviceCategoryId+"&thisYearCityId="+cityId+"&cityName=null",{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }


  predictionUser(userId:number,deviceCategoryId:number): Observable<WeekByDay[]>{
    return this.http.get<WeekByDay[]>(environment.serverUrl+"/api/Prediction/WeekByDay/User/"+userId+"/"+deviceCategoryId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});

  }

  predictionDevice(deviceId:number): Observable<WeekByDay[]>{
    return this.http.get<WeekByDay[]>(environment.serverUrl+"/api/Prediction/WeekByDay/Device/"+deviceId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }
  predictionCity(cityId:number,deviceCategoryId:number): Observable<WeekByDay[]>{
    return this.http.get<WeekByDay[]>(environment.serverUrl+"/api/Prediction/WeekByDay?cityId="+cityId+"&deviceCategoryId="+deviceCategoryId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }
  predictionSettlement(settlementId:number,deviceCategoryId:number): Observable<WeekByDay[]>{
    return this.http.get<WeekByDay[]>(environment.serverUrl+"/api/Prediction/WeekByDay?settlementId="+settlementId+"&deviceCategoryId="+deviceCategoryId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  todayConsumptionUser(userId:number,deviceCategoryId:number): Observable<number>{
    return this.http.get<number>(environment.serverUrl+"/api/CurrentPeriodHistory/user?deviceCategoryId="+deviceCategoryId+"&doubleTodayUserId="+userId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }
  monthConsumptionUser(userId:number,deviceCategoryId:number): Observable<number>{
    return this.http.get<number>(environment.serverUrl+"/api/CurrentPeriodHistory/user?deviceCategoryId="+deviceCategoryId+"&doubleMonthUserId="+userId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  dayByHourCityFilter(fromDate:string,toDate:string,cityId:number,deviceCategoryId:number): Observable<DayByHour[]>{
    return this.http.get<DayByHour[]>(environment.serverUrl+"/api/HistoryFromTo?fromDate="+fromDate+"&toDate="+toDate+"&deviceCategoryId="+deviceCategoryId+"&byHourCityId="+cityId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }
  dayByHourDevice(deviceId:number): Observable<DayByHour[]>{
    return this.http.get<DayByHour[]>(environment.serverUrl+"/api/Prosumer/today?deviceId="+deviceId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }
  dayByHourDeviceFilter(fromDate:string,toDate:string,deviceId:number,deviceCategoryId:number): Observable<DayByHour[]>{
    return this.http.get<DayByHour[]>(environment.serverUrl+"/api/HistoryFromTo?fromDate="+fromDate+"&toDate="+toDate+"&deviceCategoryId="+deviceCategoryId+"&byHourDeviceId="+deviceId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }
  dayByHourSettlementFilter(fromDate:string,toDate:string,settlementId:number,deviceCategoryId:number): Observable<DayByHour[]>{
    return this.http.get<DayByHour[]>(environment.serverUrl+"/api/HistoryFromTo?fromDate="+fromDate+"&toDate="+toDate+"&deviceCategoryId="+deviceCategoryId+"&byHourSettlementId="+settlementId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  weekByDayCityFilter(fromDate:string,toDate:string,cityId:number,deviceCategoryId:number): Observable<WeekByDay[]>{
    return this.http.get<WeekByDay[]>(environment.serverUrl+"/api/HistoryFromTo?fromDate="+fromDate+"&toDate="+toDate+"&deviceCategoryId="+deviceCategoryId+"&byDayCityId="+cityId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }
  weekByDayDeviceFilter(fromDate:string,toDate:string,deviceId:number,deviceCategoryId:number): Observable<WeekByDay[]>{
    return this.http.get<WeekByDay[]>(environment.serverUrl+"/api/HistoryFromTo?fromDate="+fromDate+"&toDate="+toDate+"&deviceCategoryId="+deviceCategoryId+"&byDayDeviceId="+deviceId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }
  weekByDayDevice(deviceId:number): Observable<WeekByDay[]>{
    return this.http.get<WeekByDay[]>(environment.serverUrl+"/api/History/WeekByDay/Device/"+deviceId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }
  weekByDaySettlementFilter(fromDate:string,toDate:string,settlementId:number,deviceCategoryId:number): Observable<WeekByDay[]>{
    return this.http.get<WeekByDay[]>(environment.serverUrl+"/api/HistoryFromTo?fromDate="+fromDate+"&toDate="+toDate+"&deviceCategoryId="+deviceCategoryId+"&byDaySettlementId="+settlementId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  weekByDayUserFilter(fromDate:string,toDate:string,userId:number,deviceCategoryId:number): Observable<DayByHour[]>{
    return this.http.get<DayByHour[]>(environment.serverUrl+"/api/HistoryFromTo?fromDate="+fromDate+"&toDate="+toDate+"&deviceCategoryId="+deviceCategoryId+"&byDayUserId="+userId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }
  dayByHourUserFilter(fromDate:string,toDate:string,userId:number,deviceCategoryId:number): Observable<DayByHour[]>{
    return this.http.get<DayByHour[]>(environment.serverUrl+"/api/HistoryFromTo?fromDate="+fromDate+"&toDate="+toDate+"&deviceCategoryId="+deviceCategoryId+"&byHourUserId="+userId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }
  monthbyDayUserFilter(date:number,userId:number,deviceCategoryId:number): Observable<WeekByDay[]>{
    return this.http.get<WeekByDay[]>(environment.serverUrl+"/api/History/ThatYear/"+date+"?deviceCategoryId="+deviceCategoryId+"&userId="+userId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  monthbyDayDeviceFilter(date:number,deviceId:number,deviceCategoryId:number): Observable<WeekByDay[]>{
    return this.http.get<WeekByDay[]>(environment.serverUrl+"/api/History/ThatYear/"+date+"?deviceCategoryId="+deviceCategoryId+"&deviceId="+deviceId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  monthbyDayDevice(deviceId:number): Observable<WeekByDay[]>{
    return this.http.get<WeekByDay[]>(environment.serverUrl+"/api/History/MonthByDay/Device/"+deviceId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  monthbyDayCityFilter(date:number,cityId:number,deviceCategoryId:number): Observable<WeekByDay[]>{
    return this.http.get<WeekByDay[]>(environment.serverUrl+"/api/History/ThatYear/"+date+"?deviceCategoryId="+deviceCategoryId+"&cityId="+cityId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }
  monthbySettlementCityFilter(date:number,settlementId:number,deviceCategoryId:number): Observable<WeekByDay[]>{
    return this.http.get<WeekByDay[]>(environment.serverUrl+"/api/History/ThatYear/"+date+"?deviceCategoryId="+deviceCategoryId+"&settlementId="+settlementId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});

  }
  
}
