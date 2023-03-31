import { Component, OnInit } from '@angular/core';
import { JwtToken } from 'src/app/utilities/jwt-token';
import { Roles } from '../../utilities/role'
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
  constructor()
  {
    this.admin=Roles.ADMIN_NAME;
    this.dso=Roles.DISPATCHER_NAME;
    this.prosumer=Roles.PROSUMER_NAME;
  }
  ngOnInit(): void {
     
    
    let token=new JwtToken();
    this.role=token.data.role as string;
  }

}
