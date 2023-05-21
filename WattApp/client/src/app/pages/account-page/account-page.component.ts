import { Component, HostListener, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LogedUser, Prosumers } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { JwtToken } from 'src/app/utilities/jwt-token';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService } from 'primeng/api';
import { HttpErrorResponse } from '@angular/common/http';
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

  storePassword=localStorage.getItem("password");
  constructor(private formBuilder: FormBuilder,private route:ActivatedRoute,
    private router:Router,private updateService:AuthService,private modalService: NgbModal,private messageService:MessageService) {
  
   }

  ngOnInit(): void {
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
    if(this.updateUserDetail.name!==this.logedDetail.name)
    {
          this.body="Your name has been changed." 
    }
    else if(this.updateUserDetail.email!==this.logedDetail.email)
    {
          this.body="You need to confirm your email."   
    }
    else if(this.updateUserDetail.username!==this.logedDetail.username)
    {
      this.body="Your username has been changed." 
    }
    else if(this.updateUserDetail.name===this.logedDetail.name && this.updateUserDetail.username===this.logedDetail.username && this.updateUserDetail.email===this.logedDetail.email)
    {
      this.body="You didnt make any changes.";
      
    }
    else if(this.updateUserDetail.name!=this.logedDetail.name && this.updateUserDetail.username!=this.logedDetail.username )
    {
        this.body="Your name and username has been changed." 
    }
    else if(this.updateUserDetail.name!=this.logedDetail.name && this.updateUserDetail.username!=this.logedDetail.username && this.updateUserDetail.email!=this.logedDetail.email)
    {
      this.body="Your name, username and email has been changed. You need to confirm your email" 
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
    const oldpass = (document.querySelector('input[name="oldPassword"]') as HTMLInputElement).value;
    const newpass = (document.querySelector('input[name="newPassword"]') as HTMLInputElement).value;
    const confpass = (document.querySelector('input[name="confirmPassword"]') as HTMLInputElement).value;
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
  @HostListener('window:beforeunload')
  canDeactivate(): boolean {
    if (this.isFormDirty) {
      return confirm('Are you sure you want to leave? Your unsaved changes will be lost.');
    }
    return true;
  }

  updatePasswordProsumer()
  {
    const oldpass = (document.querySelector('input[name="oldPassword"]') as HTMLInputElement).value;
    const newpass = (document.querySelector('input[name="newPassword"]') as HTMLInputElement).value;
    const confpass = (document.querySelector('input[name="confirmPassword"]') as HTMLInputElement).value;
    if(oldpass==="" && newpass==="" && confpass==="")
    {
      this.isFormDirty = false;
    }
    else if(newpass===confpass) {
      
      
        this.updateService.changePassword(oldpass,newpass).subscribe( 
        { next:(response:any) => {  
            
          //this.modalService.open(this.modalContent);
          //this.body="Your password has been changed.";
		  this.messageService.add({severity:"success",summary:"Success",detail:response.message});
		  this.oldPassword="";
		  this.newPassword="";
		  this.confirmPassword="";
          this.ngOnInit();
      }, error:(response:HttpErrorResponse)=>{
		this.messageService.add({severity:"error",summary:"Error",detail:response.error.message});
	  }} );
      
      this.isFormDirty = false;
    } else {
		this.messageService.add({severity:"error",summary:"Error",detail:"New password isn't confirmed!"});
	}
    
  }
  checkIfInputsAreEqual(group: FormGroup) {
    const input1 = group.controls['nameform2'];
    const input2 = group.controls['nameform3'];
    const input3 = group.controls['nameform1'];
 

    if (input1.value !== input2.value) {
      input2.setErrors({ notEqual: true });
      input1.setErrors({ notEqual: true });
    } else {
      input2.setErrors(null);
      input1.setErrors(null);
      input3.setErrors(null);
      
    }

    return null;
  }
}
