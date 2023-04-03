import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { JwtToken } from '../utilities/jwt-token';
import { Roles } from '../utilities/role';
import { AuthService } from '../services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
	constructor(private router:Router,private userservice: AuthService){}
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
		let token=new JwtToken();
		try{
			if(token.expired)
			{
				localStorage.removeItem("token")
    			this.userservice.isLoginSubject.next(false)
				this.router.navigate(["login"]);
				return false;
			}
			let role=token.data.role as string;
			
			if( !(role==Roles.ADMIN_NAME || role==Roles.SUPERADMIN_NAME))
				throw new Error();
			return true;
		}
		catch(error){
			// localStorage.removeItem("token")
			// localStorage.clear();
    		//this.userservice.isLoginSubject.next(false)
			// this.router.navigate(["login"]);
			
			// const currentUrl = this.router.url;
    		// this.router.navigateByUrl('/', {skipLocationChange: true}).then(() => {
      		// this.router.navigate([currentUrl]);});
			return false;
		}
  }
  
}
