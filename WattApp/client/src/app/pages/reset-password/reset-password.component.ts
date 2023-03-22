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
	constructor(private route:ActivatedRoute,private router:Router,private authService:AuthService){
		this.resetKey=this.route.snapshot.paramMap.get('id') ?? "";
	}

	resetPassword(){
		// validate
		if(this.newPassword!=this.confirmedPassword)
			return;
		this.authService.resetPasswordWithResetCode(this.resetKey,this.newPassword).subscribe(
			{
				next:(response)=>{
					this.success=true;
				},
				error:(response:HttpErrorResponse)=>{
					this.success=false;
				}
			}
		);

	}
}
