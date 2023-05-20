import { Component, OnInit } from '@angular/core';
import { Route,Router,NavigationEnd } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { AuthService } from '../../services/auth.service';
import { Location } from '@angular/common';
import { MessageService } from 'primeng/api';
// import jwt_decode from "jwt-decode";
// import { Token } from '../../models/users.model';

@Component({
	selector: 'app-login',
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

	username='';
	password='';
	errorMsg='';
	show=false;
	public backgroundImage = 'assets/images/background.jpg';
	constructor(private authService: AuthService,private router:Router,private messageService:MessageService) { }

	ngOnInit(): void {
		this.backgroundImage = 'assets/img/smart.jpg';
		const storedUsername = localStorage.getItem("username");
		const storedPassword = localStorage.getItem("password");
		if (storedUsername && storedPassword) {
			this.username = storedUsername;
			this.password = storedPassword;
		}
	}
	toPassword(event:any){
		let keycode = event.keyCode || event.which;
		if(keycode==13)
		{
			document.getElementById("password")?.focus();
		}
	}
	toLogin(event:any){
		let keycode = event.keyCode || event.which;
		if(keycode==13)
		{
			this.login();
		}
	}
	login(){
		this.show=false;
		if (this.username.trim().length === 0) {
			this.errorMsg = " User name is required!";
			this.show=true;
			return;
		}
		if (this.password.trim().length === 0) {
			this.errorMsg = " Password is required!";
			this.show=true;
			return;
		}
		const rememberMe = (document.querySelector("#remember") as HTMLInputElement).checked;
	
		this.authService.login(this.username, this.password).subscribe({
			next: response => {
				if (response.status == 401) {
					this.errorMsg = " Wrong username/password!";
					this.messageService.add({severity:"error",summary:"Error",detail:this.errorMsg})
					//this.show=true;
					return;
				}
	
				let body = response.body as any;
				localStorage.setItem("token", body.token);
				if (rememberMe) {
					localStorage.setItem("username", this.username);
					localStorage.setItem("password", this.password);
				} else {
					localStorage.removeItem("username");
					localStorage.removeItem("password");
				}
	
				this.authService.isLoginSubject.next(true);
				this.router.navigate([""]);
			},
			error: response => {
				if (response.status == 401) {
					this.errorMsg = " Wrong username/password!";
					//this.show=true;
					this.messageService.add({severity:"error",summary:"Error",detail:this.errorMsg})
					return;
				}
			}
		});
	}
}
