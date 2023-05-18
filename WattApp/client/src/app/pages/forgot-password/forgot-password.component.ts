import { HttpErrorResponse } from '@angular/common/http';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordPageComponent {
	public email:string="";
	public emailErrorMessage:string="";
	public errorMessage:string="";
	public success:boolean=false;

	@ViewChild("emailElement") emailDiv!:ElementRef<HTMLInputElement>;
	public backgroundImage = 'assets/images/background.jpg';
	constructor(public authService:AuthService) {
		
	}
	ngOnInit()
	{
		this.backgroundImage = 'assets/img/smart.jpg';
	}
	validateEmail(){
		let regex=/^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/;
		if(regex.test(this.email)){
			this.emailDiv.nativeElement.classList.remove("invalid");
			this.emailErrorMessage="";
			return true;
		}
		this.emailDiv.nativeElement.classList.add("invalid");
		this.emailErrorMessage="Not email";
		return false;
	}

	sendEmail(){
		if(!this.validateEmail())
			return;
		this.authService.sendEmail(this.email).subscribe({
			next:(response)=>{
				this.success=true;
			},
			error:(response:HttpErrorResponse)=>{
				this.success=false;
				this.errorMessage="This email doesn't exist in database";
			}
		})
	}
}
