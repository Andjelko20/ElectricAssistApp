import { Component, OnInit  } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { Roles } from '../../utilities/role'
import { JwtToken } from 'src/app/utilities/jwt-token';
import { Location } from '@angular/common';
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
  currentUrl?: string;
  tab?:string;
  showLink:boolean=false;
  showLink1:boolean=false;
  id?:string;
  constructor(public router:Router,private usersService:AuthService,
    public route:ActivatedRoute,private location: Location,) { 

      this.admin=Roles.ADMIN_NAME;
      this.dso=Roles.DISPATCHER_NAME;
      this.prosumer=Roles.PROSUMER_NAME;
      this.superadmin=Roles.SUPERADMIN_NAME;
      
       
    }
  ngOnInit(): void {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        const path = this.location.path(); 
        const segments = path.split('/'); 
        this.id = segments[segments.length - 1]; 
        this.showLink=this.showLink1=false
        if(event.url==='/prosumers?tab=table')
        {
          this.tab='table';
          this.showLink=false
        }
        else  if(event.url==='/prosumers?tab=map')
        {
          this.tab='map';
          this.showLink=false
        }
        else if(event.url==='/prosumer/'+this.id)
        {
          if(this.tab==='map')
          {
            this.tab='map';
          }
          else (this.tab==='table')
          {
            this.tab='table';
          }
          this.showLink=true
          
        }
        else if(event.url==='/device/'+this.id ||event.url==='/device-update/'+this.id || event.url==='/device-add')
        {
          this.showLink=true
        }
        else if(event.url==='/devices')
        {
          this.showLink=false
        }
        else if(event.url==='/profile')
        {
          this.showLink1=false
        }
        else if(event.url==='/profile-edit' || event.url==='/prosumer-change-password')
        {
          this.showLink1=true
        }
      }
    });
    let token=new JwtToken();
    this.role=token.data.role as string;

    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        this.currentUrl = event.url;
      }
    });
    const dashboard = document.getElementById('dashboard');
    const sidebar = document.getElementById('sidebar');
    
    if(sidebar && dashboard)
    {
      sidebar.style.height = dashboard.clientHeight + 'px';
    }
            window.addEventListener('resize', this.setSidebarHeight);
    
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
   setSidebarHeight() {
    const dashboard = document.getElementById('dashboard');
    const sidebar = document.getElementById('sidebar');
    
    if(sidebar && dashboard)
    {
      sidebar.style.height = dashboard.clientHeight + 'px';
    }
   }
}
