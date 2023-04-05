import { HttpErrorResponse } from '@angular/common/http';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { Users } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { Roles } from 'src/app/utilities/role';
import { environment } from 'src/environments/environment';
import { LatLng } from 'leaflet';

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
    roleId: 1,
	settlementId:0,
	address:'',
	latitude:0,
	longitude:0
  }
  public emailErrorMessage:string="";
	public errorMessage:string="";
	public success:boolean=false;
  public passwordGen='';
  public emailUp='';
  public roles=Roles;

  public cities:any;
  public settlements:any;
  
  constructor(private usersService:AuthService,private router:Router) { }

  ngOnInit(): void {
	/** 
	fetch(environment.serverUrl+"/api/users/roles",{headers:{"Authorization":"Bearer "+localStorage.getItem("token")}})
	.then(res=>res.json())
	.then(res=>{
		this.roles=res;
		this.addUserRequest.roleId=this.roles[0]?.id;
  	});
	*/
	fetch(environment.serverUrl+"/cities?countryId=1",{headers:{"Authorization":"Bearer "+localStorage.getItem("token")}})
	.then(res=>res.json())
	.then(res=>{
		this.cities=res.map((r:any)=>({id:r.id,name:r.name}));
		this.onSelectedCity({target:{value:this.cities[0].id}});
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
  onSelectedCity(event:any){
	console.log(event)
	let id=event.target.value;
	fetch(environment.serverUrl+"/settlements?cityId="+id,{headers:{"Authorization":"Bearer "+localStorage.getItem("token")}})
	.then(res=>res.json())
	.then(res=>{
			this.settlements=res.map((r:any)=>({id:r.id,name:r.name}));
			this.addUserRequest.settlementId=this.settlements[0].id;
		})
  }
  generatePassword() {
    this.passwordGen=Array(10).
    fill("0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz~!@-#$").
    map(function(x) { return x[Math.floor(Math.random() * x.length)] }).join('');
  }
  addUsers()
  {
	this.generatePassword();
    this.addUserRequest.password=this.passwordGen;
    this.usersService.addUsers(this.addUserRequest)
    .subscribe({
      next:()=>{
         this.router.navigate(['dashboard']);
      
      }
    });
  }
  locationChanged(latLng:LatLng){
	this.addUserRequest.latitude=latLng.lat;
	this.addUserRequest.longitude=latLng.lng;
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
