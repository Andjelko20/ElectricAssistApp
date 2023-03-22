import { HttpErrorResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordPageComponent {
	resetKey:string="";
	success:boolean=false;
	errorMessage:string="";

	newPassword:string="";
	confirmedPassword:string="";

	public errorNewPassword:string="";
	public errorConfirmedPassword:string="";

	constructor(private route:ActivatedRoute,private router:Router,private authService:AuthService){
		this.resetKey=this.route.snapshot.paramMap.get('id') ?? "";
	}
	validatePassword():boolean{
		let input=document.getElementsByName("new_password")[0] as HTMLInputElement;
		if(this.newPassword!=""){
			input.classList.remove("invalid");
			this.errorNewPassword="";
			return true;
		}
		input.classList.add("invalid");
		this.errorNewPassword="Password is required";
		return false;
	}

	validateConfirmedPassword():boolean{
		let input=document.getElementsByName("confirm_password")[0] as HTMLInputElement;
		if(this.confirmedPassword!="" && this.confirmedPassword==this.newPassword){
			input.classList.remove("invalid");
			this.errorConfirmedPassword="";
			return true;
		}
		input.classList.add("invalid");
		this.errorConfirmedPassword="Confirmed must be that same as required and not empty";
		return false;
	}
	resetPassword(){
		// validate
		if(!this.validatePassword() || !this.validateConfirmedPassword())
			return;
		this.authService.resetPasswordWithResetCode(this.resetKey,this.newPassword).subscribe(
			{
				next:(response)=>{
					this.success=true;
				},
				error:(response:HttpErrorResponse)=>{
					this.success=false;
					this.errorMessage="Reset key is expired or invalid";
				}
			}
		);

	}
}
