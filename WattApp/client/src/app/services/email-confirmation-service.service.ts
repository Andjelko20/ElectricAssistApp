import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class EmailConfirmationServiceService {

  constructor(private http : HttpClient) { }

  confirmEmailAddress(key : string){
    return this.http.post<any>(environment.serverUrl + '/api/Users/emailConfirmation/' + key, null);
  }

  changeEmailAddressConfirmation(key : string){
    return this.http.post<any>(environment.serverUrl + "/api/Users/changeEmailConfirmation/" + key, null);
  }
}
