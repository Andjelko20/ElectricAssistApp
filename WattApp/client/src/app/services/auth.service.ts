import { Injectable} from '@angular/core';
import {HttpClient, HttpErrorResponse, HttpResponse} from '@angular/common/http';
import { environment } from 'src/environments/environment';
import { catchError } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http:HttpClient) { }

  login(username:string,password:string){
      return this.http.post(environment.serverUrl + "/api/authentication", {username,password},{observe:"response"});
  }

}
