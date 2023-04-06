import { Injectable} from '@angular/core';
import {HttpClient, HttpErrorResponse, HttpHeaders, HttpResponse} from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { BehaviorSubject, catchError, Observable } from 'rxjs';
import { Users } from '../models/users.model';
import { JwtToken } from '../utilities/jwt-token';
@Injectable({
  providedIn: 'root'
})
export class AuthService {

  
  isLoginSubject = new BehaviorSubject<boolean>(this.hasToken());
  public hasToken() : boolean {
    try{
		let token=new JwtToken();
		console.log(token.data)

		console.log(token.expired)
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

  sendEmail(email:string):Observable<any>{
	return this.http.post(environment.serverUrl+'/api/authentication/generate_reset_token',{email:email});
  }
  resetPasswordWithResetCode(resetKey:string,newPassword:string):Observable<any>{
	return this.http.post(environment.serverUrl+'/api/authentication/reset_password',{resetKey:resetKey,newPassword:newPassword});
  }
  getAllUsers():Observable<any>
  {
    return this.http.get<any>(environment.serverUrl+'/api/users/page/1',{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  addUsers(addUserRequest:Users):Observable<Users>
  {
    // addUserRequest.id='';
    return this.http.post<Users>(environment.serverUrl + '/api/users' ,addUserRequest,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  getUser(id:number):Observable<any>
  {
    return this.http.get<any>(environment.serverUrl+"/api/users/"+id,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  upDate(id:number,updateRequest:Users):Observable<Users>
  {
    return this.http.put<Users>(environment.serverUrl+'/api/users/'+id,updateRequest,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }
  delete(id:number):Observable<Users>
  {
    return this.http.delete<Users>(environment.serverUrl+"/api/users/"+id,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}}); 
  }

  blockUser(id: number): Observable<any> {
    const url = `${environment.serverUrl}/api/users/set_blocked_status/${id}`;
    return this.http.put(url, { blocked: true },{headers:{"Authorization":"Bearer "+localStorage.getItem("token")}});
  }
  unblockUser(id: number): Observable<any> {
    const url = `${environment.serverUrl}/api/users/set_blocked_status/${id}`;
    return this.http.put(url, { blocked: false },{headers:{"Authorization":"Bearer "+localStorage.getItem("token")}});
  }

  changePassword(oldPassword: string, newPassword: string): Observable<any> {
    return this.http.post('/api/users/change_password', { oldPassword, newPassword },{headers:{"Authorization":"Bearer "+localStorage.getItem("token")}});
  }
  
}