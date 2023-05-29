import { Component, ElementRef, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { Roles } from 'src/app/utilities/role';
import { environment } from 'src/environments/environment';
import { LatLng } from 'leaflet';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder,Validators } from '@angular/forms';
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
  userForm=this.fb.group({
    name:['',Validators.required],
    email:['',[Validators.required,Validators.email]],
    username:['',Validators.required],
    role:[1,Validators.required],
  })
  public roles=Roles;

  public cities:any;
  public settlements:any;
  currentUrl: any;
  isFormDirty: boolean = false;
  isFormDirty2: boolean = false;
  btnAction:string=''  
  confirm:boolean=false;

  @ViewChild('modalContent') modalContent!: TemplateRef<any>;
  body: string = ''; 

  constructor(private usersService:AuthService,private router:Router,private modalService: NgbModal,private fb:FormBuilder) { }

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
    this.isFormDirty2=true
    this.usersService.addUsers(this.addUserRequest)
    .subscribe({
      next:()=>{
        this.modalService.open(this.modalContent);
        this.body="Email confirmation has been successfully sent to the user's email.";
        this.router.navigate(['/add-user']);
        location.reload()
      
      }
    });
    this.isFormDirty = false;
  }
  locationChanged(latLng:LatLng){
	this.addUserRequest.latitude=latLng.lat;
	this.addUserRequest.longitude=latLng.lng;
  }
	settlementChanged(event:any){
		this.addUserRequest.address=event.address;
		this.addUserRequest.settlementId=event.settlement;
	}
  onFormChange() {

    if(this.addUserRequest.name==="" && this.addUserRequest.email==="" && this.addUserRequest.username==="" )
    {
      this.isFormDirty = false;
    }
    else
    {
      this.isFormDirty = true;
    }
    
  }
}
