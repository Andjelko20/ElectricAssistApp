import { Component, OnInit, TemplateRef, ViewChild,ElementRef  } from '@angular/core';
import { ActivatedRoute, NavigationCancel, NavigationEnd, NavigationStart, Router, RouterLinkWithHref } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { Roles } from '../../utilities/role'
import { JwtToken } from 'src/app/utilities/jwt-token';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Location } from '@angular/common';
import { Renderer2 } from '@angular/core';
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
  id?:string;
  constructor(public router:Router,private usersService:AuthService,
    public route:ActivatedRoute,private modalService: NgbModal,private location: Location,
    private renderer: Renderer2,private elRef: ElementRef) { 

      this.admin=Roles.ADMIN_NAME;
      this.dso=Roles.DISPATCHER_NAME;
      this.prosumer=Roles.PROSUMER_NAME;
      this.superadmin=Roles.SUPERADMIN_NAME;
      
       
    }
  ngOnInit(): void {
    this.router.events.subscribe(event => {
      if (event instanceof NavigationEnd) {
        const path = this.location.path(); // Get the current URL path
        const segments = path.split('/'); // Split the path into segments
        this.id = segments[segments.length - 1]; // Extract the last segment (assuming the ID is at the end)
        this.showLink=false
        if(event.url==='/prosumers?tab=table')
        {
          this.currentUrl="'/prosumers?tab=table'";
          this.tab='table';
          this.showLink=false
        }
        else  if(event.url==='/prosumers?tab=map')
        {
          this.currentUrl="'/prosumers?tab=map'";
          this.tab='map';
          this.showLink=false
        }
        else if(event.url==='/prosumer/'+this.id)
        {
          
          this.currentUrl="['/prosumer/"+this.id+"']";
          this.showLink=true
          
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
