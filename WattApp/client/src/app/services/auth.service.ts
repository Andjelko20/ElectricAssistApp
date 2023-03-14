import { Injectable} from '@angular/core';
import {HttpClient, HttpErrorResponse, HttpHeaders, HttpResponse} from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { catchError, Observable } from 'rxjs';
import { Users } from '../models/users.model';
@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http:HttpClient) { }

  login(username:string,password:string){
      return this.http.post(environment.serverUrl + "/api/authentication/login", {username,password},{observe:"response"});
  }
  logout()
  {
    return this.http.get(environment.serverUrl );
  }
  getAllUsers():Observable<Users[]>
  {
    return this.http.get<Users[]>(environment.serverUrl+'/api/users/page/1',{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  addUsers(addUserRequest:Users):Observable<Users>
  {
    // addUserRequest.id='';
    return this.http.post<Users>(environment.serverUrl + '/api/users' ,addUserRequest,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  getUsers(id:number):Observable<Users>
  {
    return this.http.get<Users>("/api/home"+id,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }

  upDate(id:number,updateRequest:Users):Observable<Users>
  {
    return this.http.put<Users>(environment.serverUrl+'/api/home/'+id,updateRequest,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}});
  }
  delete(id:number):Observable<Users>
  {
    return this.http.delete<Users>(environment.serverUrl+"/api/home/"+id,{headers:{"Authorization":"Bearer "+localStorage.getItem('token')}}); 
  }

  blockUser(id: number): Observable<any> {
    const url = `${environment.serverUrl}/users/${id}`;
    return this.http.put(url, { blocked: true });
  }
  unblockUser(id: number): Observable<any> {
    const url = `${environment.serverUrl}/users/${id}`;
    return this.http.put(url, { blocked: false });
  }

  changePassword(oldPassword: string, newPassword: string): Observable<any> {
    return this.http.post('/api/home', { oldPassword, newPassword });
  }
  
}
