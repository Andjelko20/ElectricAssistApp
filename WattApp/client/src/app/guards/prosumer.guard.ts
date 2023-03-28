import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { JwtToken } from '../utilities/jwt-token';
import { Roles } from '../utilities/role';

@Injectable({
  providedIn: 'root'
})
export class ProsumerGuard implements CanActivate {
	constructor(private router:Router){}
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
		let token=new JwtToken();
		try{
			let role=token.data.role as string;
			if(token.expired || !(role==Roles.PROSUMER_NAME || role==Roles.GUEST_NAME))
				throw new Error();
			return true;
		}
		catch(error){
			localStorage.removeItem("token")
			this.router.navigate(["login"]);
			return false;
		}
  }
  
}
