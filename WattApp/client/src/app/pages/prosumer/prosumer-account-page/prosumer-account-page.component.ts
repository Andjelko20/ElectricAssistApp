import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { DeviceFilterModel } from 'src/app/components/prosumers/devices/all-devices/all-devices.component';
import { Prosumers } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { SessionService } from 'src/app/services/session.service';
import { JwtToken } from 'src/app/utilities/jwt-token';
import { Roles } from 'src/app/utilities/role';

@Component({
  selector: 'app-prosumer-account-page',
  templateUrl: './prosumer-account-page.component.html',
  styleUrls: ['./prosumer-account-page.component.css']
})
export class ProsumerAccountPageComponent {
  roles!:any[];
  updateUserDetail:Prosumers={
    id: 0,
    name: '',
    username: '',
    email: '',
    role: '',
    blocked: false,
    settlement:'',
    city:'',
    country: '',
    address:''
  }
  numOfDevices?:number;
  admin?:string;
  dso?:string;
  prosumer?:string;
  public idUser!:number;
  public role!:string;
  public name!:string;
  public username!:string;
  public email!:string;
  public emailErrorMessage:string="";
	public errorMessage:string="";
	public success:boolean=false;
  public passwordGen='';
  public emailUp='';
  public adres!:string;
  constructor(private route:ActivatedRoute,private router:Router,private updateService:AuthService, private sessionService : SessionService) { 
    this.admin=Roles.ADMIN_NAME;
    this.dso=Roles.DISPATCHER_NAME;
    this.prosumer=Roles.PROSUMER_NAME;
  }

  ngOnInit(): void {
    let token=new JwtToken();
    this.sessionService.setSession("filter", new DeviceFilterModel(
      0, 
      0, 
      0, 
      -1, 
      -1, 
      -1,
      1, 
      1, 
      0, 
      0, 
      ""
    ));
    this.idUser=token.data.id as number;
    this.updateService.getNumberOfDevices(this.idUser).subscribe({
      next:(response)=>{
        this.numOfDevices=response;
      }
    })
    this.role=token.data.role as string;
        this.updateService.getlogInUser()
        .subscribe({
          next:(response)=>{
            this.updateUserDetail={
              id:this.idUser,
              name:response.name,
              username:response.username,
              email:response.email,
              blocked:response.blocked,
              role:this.role,
              settlement:response.settlement,
              city:response.city,
              country: response.country,
              address:response.address
              
              };
              this.name=response.name;
              this.username = response.username;
              this.email=response.email;
              this.adres=response.address;
            },
			error:(response)=>{
				this.router.navigate(["prosumer-account-page"]);
			}
          });
  }
  

  logout()
  {
    localStorage.removeItem('token');
    localStorage.removeItem('filter');
    sessionStorage.removeItem('filter');
    this.updateService.isLoginSubject.next(false)
    this.router.navigate(['/login']);
  }
}
