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
      return this.http.post(environment.serverUrl + "/api/authentication", {username,password},{observe:"response"});
  }

  getAllUsers():Observable<Users[]>
  {
    return this.http.get<Users[]>(environment.serverUrl+'/api/home');
  }

  addUsers(addUserRequest:Users):Observable<Users>
  {
    // addUserRequest.id='';
    return this.http.post<Users>(environment.serverUrl + "/api/home", addUserRequest);
  }

  getUsers(id:string):Observable<Users>
  {
    return this.http.get<Users>(environment.serverUrl+"/api/home"+id);
  }

  upDate(id:string,updateRequest:Users):Observable<Users>
  {
    return this.http.put<Users>('/api/home/'+id,updateRequest);
  }

  delete(id:string):Observable<Users>
  {
    return this.http.delete<Users>(environment.serverUrl+"/"+id); 
  }
  block(id:string,blockRequest:Users):Observable<Users>
  {
    return this.http.put<Users>(environment.serverUrl+"/"+id,blockRequest); 
  }
}
