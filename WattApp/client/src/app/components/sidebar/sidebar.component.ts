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
  @ViewChild('modalContent') modalContent!: TemplateRef<any>;
  onClickLeave!: (this: HTMLElement, ev: MouseEvent) => any;
  role?:string;
  admin?:string;
  superadmin?:string;
  dso?:string;
  prosumer?:string;
  currentUrl: string = '';
  constructor(public router:Router,private usersService:AuthService,
    public route:ActivatedRoute,private modalService: NgbModal,private location: Location,
    private renderer: Renderer2,private elRef: ElementRef) { 

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
  clickSidebar(event: Event, url: string) {
    event.preventDefault();
  
    if(this.location.path() === '/add-user')
    {
      this.router.navigateByUrl('/add-user');
      if (url !== '/add-user') {
      
        this.modalService.open(this.modalContent);
        const controlabilityOnPopup = document.getElementById('popup');
        if (controlabilityOnPopup != null) {
          controlabilityOnPopup.removeEventListener('click', this.onClickLeave);
          this.onClickLeave = () => {
            this.modalService.dismissAll();
            this.router.navigateByUrl(url);
            controlabilityOnPopup.removeEventListener('click', this.onClickLeave);
          };
          controlabilityOnPopup.addEventListener('click', this.onClickLeave);
        }
      }
    }
    if(this.location.path() === '/profile-admin')
    {
      this.router.navigateByUrl('/profile-admin');
      if (url !== '/profile-admin') {
      
        this.modalService.open(this.modalContent);
        const controlabilityOnPopup = document.getElementById('popup');
        if (controlabilityOnPopup != null) {
          controlabilityOnPopup.removeEventListener('click', this.onClickLeave);
          this.onClickLeave = () => {
            this.modalService.dismissAll();
            this.router.navigateByUrl(url);
            controlabilityOnPopup.removeEventListener('click', this.onClickLeave);
          };
          controlabilityOnPopup.addEventListener('click', this.onClickLeave);
        }
      }
    }
    
    
  }
  
  
  
  

  // clickSidebar(event: Event)
  // {
   
  //   if(this.location.path()=="/add-user")
  //   {
      
  //     const routerLink = (event.target as Element).closest('a[routerLink]')?.getAttribute('routerLink');
  //     if (routerLink === '/dashboard') {
  //       this.modalService.open(this.modalContent);
  //       const controlabilityOnPopup= document.getElementById('popup');
  //       if(controlabilityOnPopup!=null)
  //       {
  //         controlabilityOnPopup.removeEventListener('click', this.onClickLeave);
          
  //           this.onClickLeave=()=> {

  //             controlabilityOnPopup.removeEventListener('click',this.onClickLeave);
  //         };
  //         controlabilityOnPopup.addEventListener('click',this.onClickLeave);
          
  //       }
  //     }else if (routerLink === '/profile-admin') {
  //       this.modalService.open(this.modalContent);
  //       const controlabilityOnPopup= document.getElementById('popup');
  //       if(controlabilityOnPopup!=null)
  //       {
  //         controlabilityOnPopup.removeEventListener('click', this.onClickLeave);
          
  //           this.onClickLeave=()=> {

  //             controlabilityOnPopup.removeEventListener('click',this.onClickLeave);
  //         };
  //         controlabilityOnPopup.addEventListener('click',this.onClickLeave);
          
  //       }
  //     }
      
  //   }
   
    

   
  // }
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
