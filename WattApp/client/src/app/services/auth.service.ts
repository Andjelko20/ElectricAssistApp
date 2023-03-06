import { Injectable,Observable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor() { }

  login(username:string,password:string){
    return true;
  }
}
