import { HttpErrorResponse } from '@angular/common/http';
import { Component, ElementRef, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Prosumers, Users } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { JwtToken } from 'src/app/utilities/jwt-token';
import { Roles } from 'src/app/utilities/role';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-prosumer-account-settings-page',
  templateUrl: './prosumer-account-settings-page.component.html',
  styleUrls: ['./prosumer-account-settings-page.component.css']
})
export class ProsumerAccountSettingsPageComponent {
  myForm = this.formBuilder.group({
    name: ['', Validators.required],
    email:['',[Validators.required,Validators.email]],
    username: ['', Validators.required]
    
  })
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
  isFormDirty:boolean=false;
  admin?:string;
  dso?:string;
  prosumer?:string;
  public idUser!:number;
  public role!:string;
  public emailErrorMessage:string="";
	public errorMessage:string="";
	public success:boolean=false;
  public passwordGen='';
  public oldpass!:string;
  public emailUp='';
  public name='';
  public username='';
  pass!:string;
  errorMsg='';

  storePassword=localStorage.getItem("password");

  @ViewChild('modalContent') modalContent!: TemplateRef<any>;
  body: string = ''; 
  confirm:boolean=false;
  btnAction:string='' 
  constructor(private formBuilder: FormBuilder,private modalService: NgbModal,private router:Router,private updateService:AuthService) { 
  
    this.admin=Roles.ADMIN_NAME;
    this.dso=Roles.DISPATCHER_NAME;
    this.prosumer=Roles.PROSUMER_NAME;
  }

  ngOnInit(): void {

    let token=new JwtToken();
    this.idUser=token.data.id as number;

    this.role=token.data.role as string;
    this.pass = token.data.password as string;

        this.updateService.getlogInUser()
        .subscribe({
          next:(response)=>{
            this.updateUserDetail={
              id:this.idUser,
              name:response.name,
              username:response.username,
              email:response.email,
              blocked:response.blocked,
              role:response.role,
              settlement:response.settlement,
              city:response.city,
              country: response.country,
              address:response.address
              };
              this.name=response.name;
              this.emailUp=response.email;
              this.username=response.username;
            }
          });
  }
  
  upDateProsumer()
  {
    if(this.updateUserDetail.name!=this.name && this.updateUserDetail.username!=this.username && this.updateUserDetail.email!=this.emailUp)
    {
      this.body="Your name, username and email have been successfully changed. You need to confirm your email." 
    }
    else if(this.updateUserDetail.name!=this.name && this.updateUserDetail.username!=this.username )
    {
        this.body="Your name and username have been successfully changed." 
    }
    else if(this.updateUserDetail.name!==this.name)
    {
          this.body="Your name has been successfully changed." 
    }
    else if(this.updateUserDetail.email!==this.emailUp)
    {
          this.body="You need to confirm your email."   
    }
    else if(this.updateUserDetail.username!==this.username)
    {
      this.body="Your username has been successfully changed." 
    }
    else if(this.updateUserDetail.name===this.name && this.updateUserDetail.username===this.username && this.updateUserDetail.email===this.emailUp)
    {
      this.body="You didnt make any changes.";
    }
    this.updateService.upDateLogedIn(this.updateUserDetail)
    .subscribe({
      next:()=>{
        this.modalService.open(this.modalContent);
        this.router.navigate(['/profile-edit']);
      }
    });
    this.isFormDirty = false;
  }
  onFormChange(){
    if(this.updateUserDetail.name===this.name && this.updateUserDetail.username===this.username && this.updateUserDetail.email===this.emailUp)
    {
      this.isFormDirty= false;
      
    }
    else
    {
      this.isFormDirty = true;
    }
  }
}
