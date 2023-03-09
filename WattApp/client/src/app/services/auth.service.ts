import { Injectable} from '@angular/core';
import {HttpClient, HttpErrorResponse, HttpResponse} from '@angular/common/http';
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
  
  delete(id:string):Observable<Users>
  {
    return this.http.delete<Users>(environment.serverUrl+'/api/Users/'+id);
 
  }
}
