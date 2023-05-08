import { Component, ElementRef, HostListener, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Users } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { JwtToken } from 'src/app/utilities/jwt-token';
import { Roles } from 'src/app/utilities/role';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  showDropdown = false;
  role?:string;
  admin?:string;
  dso?:string;
  prosumer?:string;
  superadmin?:string;
  name!:string;
  id?:number;
  user!:Users;
  constructor(private router:Router,private usersService:AuthService,private route:ActivatedRoute,private elementRef: ElementRef) {
      this.admin=Roles.ADMIN_NAME;
      this.dso=Roles.DISPATCHER_NAME;
      this.prosumer=Roles.PROSUMER_NAME;
      this.superadmin=Roles.SUPERADMIN_NAME;
     }
  ngOnInit(): void {
    let token=new JwtToken();
    this.id=token.data.id as number;
    this.role=token.data.role as string;
    this.usersService.getlogInUser().subscribe(user=>{
      this.user=user
      this.name=user.name
    });
  }
  logout()
  {
    localStorage.removeItem('token');
    this.usersService.isLoginSubject.next(false)
    this.router.navigate(['/login']);
  }
  toggleDropdown() {
    this.showDropdown = !this.showDropdown;
  }
  
}
