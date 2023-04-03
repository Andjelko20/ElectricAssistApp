import { HttpErrorResponse } from '@angular/common/http';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Users } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-admin-dso-add',
  templateUrl: './admin-dso-add.component.html',
  styleUrls: ['./admin-dso-add.component.css']
})
export class AdminDsoAddComponent implements OnInit{

  addUserRequest={
    name:'',
    username:'',
    password:'',
    email:'',
    block:false,
    roleId: 0
  }
  public emailErrorMessage:string="";
	public errorMessage:string="";
	public success:boolean=false;
  public passwordGen='';
  public emailUp='';
  roles:Array<any>=[];
  
  constructor(private usersService:AuthService,private router:Router) { }

  ngOnInit(): void {
	fetch(environment.serverUrl+"/api/users/roles",{headers:{"Authorization":"Bearer "+localStorage.getItem("token")}})
	.then(res=>res.json())
	.then(res=>{
		this.roles=res;
		this.addUserRequest.roleId=this.roles[0]?.id;
  	});
  }
  @ViewChild('teams') teams!: ElementRef;
  onSelectedBlock():void
  {
    this.addUserRequest.block = this.teams.nativeElement.value;
  }
  onSelectedRole(event:any)
  {
    this.addUserRequest.roleId = event.target.value; 
   
  }
  generatePassword() {
    this.passwordGen=Array(10).
    fill("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz~!@-#$").
    map(function(x) { return x[Math.floor(Math.random() * x.length)] }).join('');
  }
  addUsers()
  {
    this.addUserRequest.password=this.passwordGen;
    this.usersService.addUsers(this.addUserRequest)
    .subscribe({
      next:()=>{
         this.router.navigate(['/admin']);
      
      }
    });
  }

  sendEmail(){
		this.emailUp=this.addUserRequest.email;
		this.usersService.adminChangePasswordEmail(this.emailUp).subscribe({
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
