import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
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
  myForm: FormGroup;
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

  storePassword=localStorage.getItem("password");



  constructor(private formBuilder: FormBuilder,private route:ActivatedRoute,private router:Router,private updateService:AuthService) { 
    this.myForm = this.formBuilder.group({
      nameform1: ['', Validators.required],
      nameform2: ['', Validators.required],
      nameform3: ['', Validators.required]
    },{ validator: this.checkIfInputsAreEqual });
    this.admin=Roles.ADMIN_NAME;
    this.dso=Roles.DISPATCHER_NAME;
    this.prosumer=Roles.PROSUMER_NAME;
  }

  ngOnInit(): void {

    let token=new JwtToken();
    this.idUser=token.data.id as number;

    this.role=token.data.role as string;
    this.pass = token.data.password as string;
    console.log(this.pass);
    
    console.log(this.idUser);
    console.log(this.role);

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
				this.router.navigate(["prosumer-account-page"]);
			}
          });
  }
  
  
  upDateProsumer()
  {
   
    this.updateService.upDateLogedIn(this.updateUserDetail)
    .subscribe({
      next:()=>{
        this.router.navigate(['prosumer-account-page']);
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
    const oldpass = (document.querySelector('input[name="oldPassword"]') as HTMLInputElement).value;
    const newpass = (document.querySelector('input[name="newPassword"]') as HTMLInputElement).value;
    const confpass = (document.querySelector('input[name="confirmPassword"]') as HTMLInputElement).value;
    console.log(oldpass);
    console.log(newpass);
    console.log(confpass);

    if(newpass==confpass)
    {
      this.updateService.changePassword(oldpass,newpass).subscribe( 
       { next:() => {  
            this.router.navigate(['/prosumer-account-page']); 
            
     }} );
    }
    
   
    // localStorage.removeItem('token');
    // this.updateService.isLoginSubject.next(false)
    // this.router.navigate(['/login']);
    
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
