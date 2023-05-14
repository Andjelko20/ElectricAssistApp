import { Component, HostListener, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LogedUser, Prosumers } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { JwtToken } from 'src/app/utilities/jwt-token';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
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
  public idUser!:number;
  public role!:string;
  public name!:string;
  public emailErrorMessage:string="";
	public errorMessage:string="";
	public success:boolean=false;
  public passwordGen='';
  public emailUp='';
  public oldpass!:string;
  myForm!: FormGroup;
  myForm1!: FormGroup;
  isFormDirty: boolean = false;
  isFormDirty1: boolean = false;
  oldPassword!:string;
  newPassword!:string;
  confirmPassword!:string;
  pass!:string;
  errorMsg='';

  storePassword=localStorage.getItem("password");
  constructor(private formBuilder: FormBuilder,private route:ActivatedRoute,private router:Router,private updateService:AuthService) {
  
   }

  ngOnInit(): void {
    let token=new JwtToken();
    this.idUser=token.data.id as number;
    this.role=token.data.role as string;

    this.updateService.getlogInUser().subscribe({
          next:(response)=>{
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
    this.updateService.upDateLogedIn(this.logedDetail)
    .subscribe({
      next:()=>{
        if(this.role==='admin')
        {
          this.router.navigate(['/profile-admin']);
        }
        else
        {
          this.router.navigate(['/profile-dso']);
        }
      }
    });
    this.isFormDirty1 = false;
  }
  onFormChange() {
    this.isFormDirty = true;
  }
  onFormChange1() {
    this.isFormDirty1 = true;
    
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

    if(newpass==confpass)
    {
      this.updateService.changePassword(oldpass,newpass).subscribe( 
       { next:() => {  
           
            
     }} );
    }
    this.isFormDirty = false;
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
