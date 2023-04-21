import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { Roles } from '../../utilities/role'
import { JwtToken } from 'src/app/utilities/jwt-token';
@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent implements OnInit{
  role?:string;
  admin?:string;
  superadmin?:string;
  dso?:string;
  prosumer?:string;
  currentUrl: string = '';
  constructor(public router:Router,private usersService:AuthService,
    public route:ActivatedRoute) { 

      this.admin=Roles.ADMIN_NAME;
      this.dso=Roles.DISPATCHER_NAME;
      this.prosumer=Roles.PROSUMER_NAME;
      this.superadmin=Roles.SUPERADMIN_NAME;
      
    }
  ngOnInit(): void {
    let token=new JwtToken();
    this.role=token.data.role as string;

    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.currentUrl = event.url;
        console.log(this.currentUrl);
        
      }
    });
    
  }
  showSidebarContent = false;

  toggleSidebarContent() {
    this.showSidebarContent = !this.showSidebarContent;
  }
  logout()
  {
    localStorage.removeItem('token');
    this.usersService.isLoginSubject.next(false)
    this.router.navigate(['/login']);
  }
  
}
