import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { JwtToken } from '../utilities/jwt-token';

@Injectable({
  providedIn: 'root'
})
export class UnauthenticatedGuard implements CanActivate {
  constructor(private router:Router){}
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
	  let token=localStorage.getItem("token");
      if(token==null){
        return true;
      }
	  try{
		let tokenObj=new JwtToken(token);
		if(tokenObj.expired){
			localStorage.removeItem("token");
      
			return true;
		}
	  }
	  catch(error){
		return true;
	  }
      this.router.navigate([""]);
      
      return false;
  }
  
}
