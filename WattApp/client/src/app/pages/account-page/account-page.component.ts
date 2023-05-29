import { Component, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { LogedUser, Prosumers } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { JwtToken } from 'src/app/utilities/jwt-token';
import { FormBuilder, FormGroup,Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService } from 'primeng/api';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
@Component({
  selector: 'app-account-page',
  templateUrl: './account-page.component.html',
  styleUrls: ['./account-page.component.css']
})
export class AccountPageComponent implements OnInit {

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
  logedDetail:LogedUser={
    id: 0,
    name: '',
    username: '',
    email: '',
    
  }
  userForm=this.fb.group({
    name:['',Validators.required],
    email:['',[Validators.required,Validators.email]],
    username:['',Validators.required],
    
  })
  passForm=this.fb.group({
    old:['',Validators.required],
    new:['',Validators.required],
    confirm:['',Validators.required],
    
  })
  @ViewChild('modalContent') modalContent!: TemplateRef<any>;
  body: string = ''; 
  public idUser!:number;
  public role!:string;
  public name!:string;
  public emailErrorMessage:string="";
	public errorMessage:string="";
	public success:boolean=false;
  public passwordGen='';
  public emailUp='';
  public oldpass!:string;
  isFormDirty: boolean = false;
  isFormDirty1: boolean = false;
  oldPassword!:string;
  newPassword!:string;
  confirmPassword!:string;
  pass!:string;
  errorMsg='';
  btnAction:string=''  
  confirm:boolean=false;
  storePassword=localStorage.getItem("password");
  constructor(private fb: FormBuilder,private route:ActivatedRoute,
    private router:Router,private updateService:AuthService,private modalService: NgbModal,private messageService:MessageService) {
  
   }
  ngOnInit(): void {
    this.confirm=false;
    let token=new JwtToken();
    this.idUser=token.data.id as number;
    this.role=token.data.role as string;

    this.updateService.getlogInUser().subscribe({
          next:(response:any)=>{
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
              this.logedDetail={
                id:this.idUser,
                name:response.name,
                username:response.username,
                email:response.email,
              }
            },
			
          });
  }
  upDateUser()
  {
    if(this.updateUserDetail.name!=this.logedDetail.name && this.updateUserDetail.username!=this.logedDetail.username && this.updateUserDetail.email!=this.logedDetail.email)
    {
      this.body="Your name, username and email have been successfully changed. You need to confirm your email." 
    }
    else if(this.updateUserDetail.name!=this.logedDetail.name && this.updateUserDetail.username!=this.logedDetail.username )
    {
        this.body="Your name and username have been successfully changed." 
    }
    else if(this.updateUserDetail.name!==this.logedDetail.name)
    {
          this.body="Your name has been successfully changed." 
    }
    else if(this.updateUserDetail.email!==this.logedDetail.email)
    {
          this.body="You need to confirm your email."   
    }
    else if(this.updateUserDetail.username!==this.logedDetail.username)
    {
      this.body="Your username has been successfully changed." 
    }
    else if(this.updateUserDetail.name===this.logedDetail.name && this.updateUserDetail.username===this.logedDetail.username && this.updateUserDetail.email===this.logedDetail.email)
    {
      this.body="You didnt make any changes.";
    }
    
    this.updateService.upDateLogedIn(this.logedDetail)
    .subscribe({
      next:()=>{
        this.modalService.open(this.modalContent);
        this.ngOnInit();
      }
    });
    this.isFormDirty1 = false;
  }
  onFormChange() {
    const oldpass = (document.querySelector('input[id="oldPassword"]') as HTMLInputElement).value;
    const newpass = (document.querySelector('input[id="newPassword"]') as HTMLInputElement).value;
    const confpass = (document.querySelector('input[id="confirmPassword"]') as HTMLInputElement).value;
    if(oldpass==="" && newpass==="" && confpass==="")
    {
      this.isFormDirty = false;
    }
    else
    {
      this.isFormDirty = true;
    }
    
  }
  onFormChange1() {
    if(this.updateUserDetail.name===this.logedDetail.name && this.updateUserDetail.username===this.logedDetail.username && this.updateUserDetail.email===this.logedDetail.email)
    {
      this.isFormDirty1 = false;
      
    }
    else
    {
      this.isFormDirty1 = true;
    }
  }

  updatePasswordProsumer()
  {
    const oldpass = (document.querySelector('input[id="oldPassword"]') as HTMLInputElement).value;
    const newpass = (document.querySelector('input[id="newPassword"]') as HTMLInputElement).value;
    const confpass = (document.querySelector('input[id="confirmPassword"]') as HTMLInputElement).value;
    if(oldpass==="" && newpass==="" && confpass==="")
    {
      this.isFormDirty = false;
    }
    else if(newpass===confpass) {
      
      
        this.updateService.changePassword(oldpass,newpass).subscribe( 
        { next:(response:any) => {  
            
          this.modalService.open(this.modalContent);
          this.body="Your password has been successfully changed.";
      },error:()=>{
        
        
        this.messageService.add({severity:"error",summary:"Error",detail:"Your old password is not valid."});
      }
    } );
      
      this.isFormDirty = false;
    } else {
		this.messageService.add({severity:"error",summary:"Error",detail:"New password isn't confirmed!"});
	}
    
  }
  
}
