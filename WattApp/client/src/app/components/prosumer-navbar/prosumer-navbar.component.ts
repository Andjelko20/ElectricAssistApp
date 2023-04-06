import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { JwtToken } from 'src/app/utilities/jwt-token';
import { Roles } from 'src/app/utilities/role';

@Component({
  selector: 'app-prosumer-navbar',
  templateUrl: './prosumer-navbar.component.html',
  styleUrls: ['./prosumer-navbar.component.css']
})
export class ProsumerNavbarComponent implements OnInit {

  
  role?:string;
  admin?:string;
  dso?:string;
  prosumer?:string;
  superadmin?:string;
  constructor(private router:Router,private usersService:AuthService,
    private route:ActivatedRoute) { 
      this.admin=Roles.ADMIN_NAME;
      this.dso=Roles.DISPATCHER_NAME;
      this.prosumer=Roles.PROSUMER_NAME;
      this.superadmin=Roles.SUPERADMIN_NAME;
    }
  ngOnInit(): void {

  }
  logout()
  {
    localStorage.removeItem('token');
    this.usersService.isLoginSubject.next(false)
    this.router.navigate(['/login']);
  }
}
