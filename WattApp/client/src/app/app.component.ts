import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from './services/auth.service';
import { Router } from '@angular/router';
import { JwtToken } from './utilities/jwt-token';
import { Roles } from './utilities/role';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'ElectricAssist';
  islogg!: Observable<boolean>;

  role?:string;
  admin?:string;
  dso?:string;
  prosumer?:string;
  superadmin?:string;
  
  constructor(public authService: AuthService,private router:Router) {
    this.islogg=authService.isLoginSubject;
    this.admin=Roles.ADMIN_NAME;
    this.dso=Roles.DISPATCHER_NAME;
    this.prosumer=Roles.PROSUMER_NAME;
    this.superadmin=Roles.SUPERADMIN_NAME;
  }
   
  ngOnInit():void{
    document.title=this.title;
	  this.authService.isLoginSubject.next(this.authService.hasToken());
    if(this.authService.hasToken())
    {
      let token=new JwtToken();
      this.role=token.data.role as string;
    }


   
  }
  

}
