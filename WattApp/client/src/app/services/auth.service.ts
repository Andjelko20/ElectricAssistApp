import { Injectable} from '@angular/core';
import {HttpClient, HttpErrorResponse, HttpHeaders, HttpResponse} from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { BehaviorSubject, catchError, map, Observable, of } from 'rxjs';
import { Prosumers, Settlement, Users } from '../models/users.model';
import { JwtToken } from '../utilities/jwt-token';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  
  isLoginSubject = new BehaviorSubject<boolean>(this.hasToken());
  public hasToken() : boolean {
    try{
		let token=new JwtToken();
		if(token.expired)
			throw new Error();
		return true;
	}
	catch(error){
		return false;
	}
  }
  
  constructor(private http:HttpClient) { }

  
  login(username:string,password:string){
    
      return this.http.post(environment.serverUrl + "/api/authentication/login", {username,password},{observe:"response"});
  }

  logout()
  {
    this.isLoginSubject.next(false);
    return this.http.get(environment.serverUrl );
  }

  getAllUsers(pageNumber:number,pageSize:number=10,filters?:any):Observable<any>
  {
	let url=new URL(environment.serverUrl+'/api/users/page');
	url.searchParams.set("pageNumber",pageNumber.toString());
	url.searchParams.set("pageSize",pageSize.toString());
	if(filters?.role>0)
		url.searchParams.set("RoleId",filters.role);
	if(filters?.city>0)
		url.searchParams.set("CityId",filters.city);
	if(filters?.settlement>0)
		url.searchParams.set("SettlmentId",filters.settlement);
	if(filters?.name!==undefined && filters.name.trim()!=='')
		url.searchParams.set("SearchValue",filters.name);
	let blocked=Number(filters?.blocked);
	if(!isNaN(blocked) && blocked>-1)
		url.searchParams.set("Blocked",blocked?"true":"false");
	url.searchParams.set("SortByNameAscending","true");
    return this.http.get<any>(url.toString(),{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }
  getAllProsumers():Observable<any>
  {
    return this.http.get<any>(environment.serverUrl+'/api/ProsumersDetails/page/?pageNumber=1&cityId=-1&pageSize=10',{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }
  addUsers(addUserRequest:any):Observable<any>
  {
    
    return this.http.post<any>(environment.serverUrl + '/api/users' ,addUserRequest,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  getUser(id:number):Observable<any>
  {
    return this.http.get<any>(environment.serverUrl+"/api/users/"+id,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  getProsumer(id:number):Observable<any>
  {
    return this.http.get<any>(environment.serverUrl+"/api/prosumersDetails/"+id,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  upDate(id:number,updateRequest:Users):Observable<Users>
  {
    return this.http.put<Users>(environment.serverUrl+'/api/users/'+id,updateRequest,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }
  getlogInUser():Observable<any>
  {
    return this.http.get<any>(environment.serverUrl+"/api/users/my_data",{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }
  upDateLogedIn(updateRequest:Prosumers):Observable<Prosumers>
  {
    return this.http.put<Prosumers>(environment.serverUrl+'/api/users',updateRequest,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }
  
  delete(id:number):Observable<Users>
  {
    return this.http.delete<Users>(environment.serverUrl+"/api/users/"+id,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}}); 
  }

  blockUser(id: number): Observable<any> {
    
    return this.http.put(environment.serverUrl+"/api/users/set_blocked_status/"+id,{Status: true },{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}}); 
  }
  unblockUser(id: number): Observable<any> {
    
    return this.http.put(environment.serverUrl+"/api/users/set_blocked_status/"+id,{Status: false },{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}}); 
  }
  changePassword(oldPassword: string, newPassword: string): Observable<any> {
    return this.http.put<any>(environment.serverUrl +'/api/Users/change_password', { oldPassword, newPassword },{headers:{"Authorization":"Bearer "+localStorage.getItem("token")}});
  }
  adminChangePasswordEmail(email:string): Observable<any> {
    return this.http.put<any>(environment.serverUrl +'/api/users/generate_reset_token_admin', {email:email},{headers:{"Authorization":"Bearer "+localStorage.getItem("token")}});
  }
  sendEmail(email:string):Observable<any>{
    return this.http.post(environment.serverUrl+'/api/authentication/generate_reset_token',{email:email});
  }
  resetPasswordWithResetCode(resetKey:string,newPassword:string):Observable<any>{   
    return this.http.post(environment.serverUrl+'/api/authentication/reset_password',{resetKey:resetKey,newPassword:newPassword});
  }

  getCityId(cityName:string):Observable<any>{
    return this.http.get<any>(environment.serverUrl+'/api/DSO/City?cityName='+cityName);

  }
  
  getSettlement(cityId:number):Observable<Settlement[]>{
    return this.http.get<Settlement[]>(environment.serverUrl+"/settlements?cityId="+cityId,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }
  getNumberOfDevices(id:number):Observable<any>
  {
    return this.http.get<any>(environment.serverUrl+'/api/Prosumer/numberOfDevices/'+id,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});

  }
  getMyLocation(): Observable<{ latitude: number, longitude: number }> {
    const url = `${environment.serverUrl}/api/Users/my_location`;
    const headers = { 'Authorization': 'Bearer ' + localStorage.getItem('token') };
  
    return this.http.get<{ lat: number, lon: number }>(url, { headers }).pipe(
      catchError((error: any) => {
        console.error('Error getting user location: ', error);
        const defaultLocation = { lat: 0, lon: 0 };
        return of(defaultLocation);
      }),
      map((response: { lat: number, lon: number }) => {
        return { latitude: response.lat, longitude: response.lon };
      })
    );
    }
}