import { Component, ElementRef, HostListener, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Users } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { JwtToken } from 'src/app/utilities/jwt-token';
import { Roles } from 'src/app/utilities/role';
import { Location } from '@angular/common';
import { UnsavedChangesGuardGuard } from 'src/app/guards/unsaved-changes-guard.guard';

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
  constructor(private router:Router,private usersService:AuthService,private unsavedChangesGuard: UnsavedChangesGuardGuard,private elementRef: ElementRef,private location: Location) {
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
    this.router.navigateByUrl("/logout");
  }
  toggleDropdown() {
    this.showDropdown = !this.showDropdown;
  }
  @HostListener('document:click', ['$event'])
  onClick(event: MouseEvent) {
    const clickedElement = event.target as HTMLElement;
    const dropdownElement = this.elementRef.nativeElement;
    const navbarElement = dropdownElement.querySelector('.dropbtn') as HTMLElement;
    if (!dropdownElement.contains(clickedElement) || !navbarElement.contains(clickedElement)) {
      this.showDropdown = false;
    }
  }
  refresh()
  {
    if(this.location.path()==='/dashboard')
    {
      location.reload();
    }
  }
}
