import { Component, OnInit } from '@angular/core';
import { Route,Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

@Component({
	selector: 'app-login',
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

	username='';
	password='';
	errorMsg='';

	constructor(private authService: AuthService,private router:Router) { }

	ngOnInit(): void {
	}

	login(){
		if(this.username.trim().length===0){
			this.errorMsg="Korisničko ime je obavezno";
			return;
		}
		if(this.password.trim().length===0){
			this.errorMsg="Lozinka je obavezna";
			return;
		}
		this.authService.login(this.username,this.password)
		.subscribe(
			{
				next:response=>{
					if(response.status==401)
					{
						this.errorMsg = "Pogresno korisnicko ime/lozinka!";
						return;
					}
					let body = response.body as any;
					localStorage.setItem("token",body.token);
					this.router.navigate(["/home"]);
				},
				error:response=>{
					if(response.status==401)
					{
						this.errorMsg = "Pogresno korisnicko ime/lozinka!";
						return;
					}
					let body = response.body as any;
					localStorage.setItem("token",body.token);
					this.router.navigate(["/home"]);
				}
			}
		  );
		

	}
}
