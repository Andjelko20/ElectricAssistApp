import { Component, OnInit } from '@angular/core';
import { JwtToken } from 'src/app/utilities/jwt-token';
import { Roles } from '../../utilities/role'
import { Route, Router } from '@angular/router';
@Component({
  selector: 'app-home-page',
  templateUrl: './home-page.component.html',
  styleUrls: ['./home-page.component.css']
})
export class HomePageComponent implements OnInit{
  
  role?:string;
  admin?:string;
  dso?:string;
  prosumer?:string;
  constructor(private router:Router)
  {
    this.admin=Roles.ADMIN_NAME;
    this.dso=Roles.DISPATCHER_NAME;
    this.prosumer=Roles.PROSUMER_NAME;
  }
  ngOnInit(): void {
     
    
    let token=new JwtToken();
    this.role=token.data.role as string;
    if(this.role==this.admin)
    {
      this.router.navigate(['home','admin']);
    }
    else if(this.role==this.dso)
    {
      this.router.navigate(['home','dso']);
    }
    else if(this.role==this.prosumer)
    {
      this.router.navigate(['home','prosumer']);
    }
  }

}
