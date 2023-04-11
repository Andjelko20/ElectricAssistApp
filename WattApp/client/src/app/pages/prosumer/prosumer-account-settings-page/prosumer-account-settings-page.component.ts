import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Prosumers, Users } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { JwtToken } from 'src/app/utilities/jwt-token';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-prosumer-account-settings-page',
  templateUrl: './prosumer-account-settings-page.component.html',
  styleUrls: ['./prosumer-account-settings-page.component.css']
})
export class ProsumerAccountSettingsPageComponent {
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
  public idUser!:number;
  public role!:string;
  public emailErrorMessage:string="";
	public errorMessage:string="";
	public success:boolean=false;
  public passwordGen='';
  public emailUp='';
  constructor(private route:ActivatedRoute,private router:Router,private updateService:AuthService) { }

  ngOnInit(): void {
    let token=new JwtToken();
    this.idUser=token.data.id as number;

    this.role=token.data.role as string;
    console.log(this.idUser);
	
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
            },
			error:(response)=>{
				this.router.navigate(["prosumer-account-page"]);
			}
          });
  }
  
  
  upDateProsumer()
  {
    // this.onSelectedBlock(this.updateUserDetail.block);
    // console.log("Azuriran objekat: ",this.updateUserDetail);
    this.updateService.upDateProsumer(this.updateUserDetail)
    .subscribe({
      next:()=>{
        this.router.navigate(['prosumer-account-page']);
      }
    });
  }
  generatePassword() {
    this.passwordGen=Array(10).
    fill("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz~!@-#$").
    map(function(x) { return x[Math.floor(Math.random() * x.length)] }).join('');
  }
  sendEmail(){
		this.emailUp=this.updateUserDetail.email;
		this.updateService.adminChangePasswordEmail(this.emailUp).subscribe({
			next:()=>{
				this.success=true;
			},
			error:(response:HttpErrorResponse)=>{
				this.success=false;
				this.errorMessage=response.error.message;
			}
		})
	}
}
