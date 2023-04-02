import { HttpErrorResponse } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Users } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-admin-dso-update',
  templateUrl: './admin-dso-update.component.html',
  styleUrls: ['./admin-dso-update.component.css']
})
export class AdminDsoUpdateComponent implements OnInit {
	roles!:any[];
  updateUserDetail:Users={
	id:0,
    name:'',
    username:'',
    password:'',
    email:'',
    block:false,
    roleId:0
  }
  public emailErrorMessage:string="";
	public errorMessage:string="";
	public success:boolean=false;
  public passwordGen='';
  public emailUp='';
  constructor(private route:ActivatedRoute,private router:Router,private updateService:AuthService) { }

  ngOnInit(): void {
		fetch(environment.serverUrl+"/api/users/roles",{headers:{"Authorization":"Bearer "+localStorage.getItem("token")}})
		.then(res=>res.json())
		.then(res=>{
		this.roles=res;
        this.updateService.getUser( Number(this.route.snapshot.paramMap.get('id')) )
        .subscribe({
          next:(response)=>{
            this.updateUserDetail={
              id:Number(this.route.snapshot.paramMap.get('id')),
              name:response.name,
              username:response.username,
              password:"",
              email:response.email,
              block:response.blocked,
              roleId:this.roles.find(r=>r.name==response.role)?.id
              };
            },
			error:(response)=>{
				this.router.navigate(["home"]);
			}
          });
		});
  }
  
  
  upDate()
  {
    // this.onSelectedBlock(this.updateUserDetail.block);
    // console.log("Azuriran objekat: ",this.updateUserDetail);
    this.updateService.upDate(this.updateUserDetail.id,this.updateUserDetail)
    .subscribe({
      next:()=>{
        this.router.navigate(['home']);
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
