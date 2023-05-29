import { HttpErrorResponse } from '@angular/common/http';
import { Component, ElementRef, ViewChild } from '@angular/core';
import { MessageService } from 'primeng/api';
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
	constructor(public authService:AuthService,private messageService:MessageService) {
		
	}
	ngOnInit()
	{
		this.backgroundImage = 'assets/img/smart.jpg';
	}
	validateEmail(){
		let regex=/^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/;
		if(regex.test(this.email)){
			
			this.emailErrorMessage="";
			this.messageService.add({severity:"error",summary:"Error",detail:this.emailErrorMessage});
			return true;
		}
		
		this.emailErrorMessage="Not email";
		this.messageService.add({severity:"error",summary:"Error",detail:this.emailErrorMessage});
		return false;
	}

	sendEmail(){
		if(!this.validateEmail())
			return;
		this.authService.sendEmail(this.email).subscribe({
			next:(response)=>{
				//this.success=true;
				this.messageService.add({severity:"success",summary:"Success",detail:"Email for password reset sent successfully."});
			},
			error:(response:HttpErrorResponse)=>{
				let message=response.error.message;
				
				this.messageService.add({severity:"error",summary:"Error",detail:message});
				//this.success=false;
				//this.errorMessage="This email doesn't exist in database";
			}
		})
	}
}
