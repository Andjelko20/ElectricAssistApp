import { HttpErrorResponse } from '@angular/common/http';
import { Component, TemplateRef, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageService } from 'primeng/api';
import { Prosumers } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { JwtToken } from 'src/app/utilities/jwt-token';
import { Roles } from 'src/app/utilities/role';

@Component({
  selector: 'app-prosumer-change-password',
  templateUrl: './prosumer-change-password.component.html',
  styleUrls: ['./prosumer-change-password.component.css']
})
export class ProsumerChangePasswordComponent {
  myForm = this.formBuilder.group({
    old: ['', Validators.required],
    new: ['', Validators.required],
    confirm: ['', Validators.required]
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
  admin?:string;
  dso?:string;
  prosumer?:string;
  public idUser!:number;
  public role!:string;
  public name!:string;
  public emailErrorMessage:string="";
	public errorMessage:string="";
	public success:boolean=false;
  public passwordGen='';
  public emailUp='';
  public oldpass!:string;
  oldPassword!:string;
  newPassword!:string;
  confirmPassword!:string;
  pass!:string;
  errorMsg='';
  isFormDirty: boolean = false;
  storePassword=localStorage.getItem("password");


  @ViewChild('modalContent') modalContent!: TemplateRef<any>;
  body: string = ''; 
  confirm:boolean=false;
  btnAction:string='' 
  constructor(private formBuilder: FormBuilder,private route:ActivatedRoute,private router:Router,private updateService:AuthService, private modalService: NgbModal,private messageService:MessageService) { 
    
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
            },
			error:(response)=>{
				this.router.navigate(["/prosumer-change-password"]);
			}
          });
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
}
