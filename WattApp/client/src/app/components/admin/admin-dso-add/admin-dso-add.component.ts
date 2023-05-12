import { HttpErrorResponse } from '@angular/common/http';
import { Component, ElementRef, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { NavigationEnd, Router } from '@angular/router';
import { Users } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { Roles } from 'src/app/utilities/role';
import { environment } from 'src/environments/environment';
import { LatLng } from 'leaflet';
import { Location } from '@angular/common';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
@Component({
  selector: 'app-admin-dso-add',
  templateUrl: './admin-dso-add.component.html',
  styleUrls: ['./admin-dso-add.component.css']
})
export class AdminDsoAddComponent implements OnInit{
 
  addUserRequest={
    name:'',
    username:'',
    email:'',
    block:false,
    roleId: 1,
	settlementId:0,
	address:'',
	latitude:44.01721187973962,
	longitude:20.90732574462891
  }
  public emailErrorMessage:string="";
  public errorMessage:string=""; 
  public success:boolean=false;
  public passwordGen='';
  public emailUp='';
  public roles=Roles;

  public cities:any;
  public settlements:any;
  currentUrl: any;
  
  constructor(private usersService:AuthService,private router:Router) { }

  ngOnInit(): void {




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
	let id=event.target.value;
	fetch(environment.serverUrl+"/settlements?cityId="+id,{headers:{"Authorization":"Bearer "+localStorage.getItem("token")}})
	.then(res=>res.json())
	.then(res=>{
			this.settlements=res.map((r:any)=>({id:r.id,name:r.name}));
			this.addUserRequest.settlementId=this.settlements[0].id;
		})
  }
  addUsers()
  {
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
	settlementChanged(event:any){
		this.addUserRequest.address=event.address;
		this.addUserRequest.settlementId=event.settlement;
	}
  
}
