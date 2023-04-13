import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

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
  getTotalConsumptionProductionCity(name:string,city:string):Observable<number>
  {
    return this.http.get<number>(environment.serverUrl+'/api/Prosumer/'+name+'/city/'+city,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}})
  }
  getAverageConsumptionProductionCity(name:string,city:string):Observable<number>
  {
    return this.http.get<number>(environment.serverUrl+'/api/Prosumer/'+name+'/average/'+city,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}})
  }
  


}
