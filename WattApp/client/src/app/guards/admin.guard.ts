import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { JwtToken } from '../utilities/jwt-token';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
	constructor(private router:Router){}
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
		let token=new JwtToken();
		try{
			let role=token.data.role as string;
			if(token.expired || !(role=="admin" || role=="superadmin"))
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
