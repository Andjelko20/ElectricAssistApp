import { Component, OnInit } from '@angular/core';
import { Route,Router,NavigationEnd } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { AuthService } from '../../services/auth.service';
import { Location } from '@angular/common';
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
	

	constructor(private authService: AuthService,private router:Router) { }

	ngOnInit(): void {
		
		const storedUsername = localStorage.getItem("username");
		const storedPassword = localStorage.getItem("password");
		if (storedUsername && storedPassword) {
			this.username = storedUsername;
			this.password = storedPassword;
		}

		// Reload the page when the user navigates to the login page
		// this.router.events.subscribe(event => {
		// 	if (event instanceof NavigationEnd) {
		// 		window.location.reload();
		// 	}
		// });
		document.onkeydown=(event)=>{
			let keycode = event.keyCode || event.which;
			if(keycode==13)
			{
				this.login();
			}
		};
	}
	
	login(){
		
		if (this.username.trim().length === 0) {
			this.errorMsg = "User name is required";
			return;
		}
		if (this.password.trim().length === 0) {
			this.errorMsg = "Password is required";
			return;
		}
	
		// Check if the "Remember me" checkbox is checked
		const rememberMe = (document.querySelector("#remember") as HTMLInputElement).checked;
	
		this.authService.login(this.username, this.password).subscribe({
			next: response => {
				if (response.status == 401) {
					this.errorMsg = "Pogresno korisnicko ime/lozinka!";
					return;
				}
	
				let body = response.body as any;
				localStorage.setItem("token", body.token);
	
				// If "Remember me" is checked, store the username and password in local storage
				if (rememberMe) {
					localStorage.setItem("username", this.username);
					localStorage.setItem("password", this.password);
				} else {
					// If "Remember me" is not checked, remove the stored credentials from local storage (if any)
					localStorage.removeItem("username");
					localStorage.removeItem("password");
				}
	
				this.authService.isLoginSubject.next(true);
				this.router.navigate([""]);
			},
			error: response => {
				if (response.status == 401) {
					this.errorMsg = "Pogresno korisnicko ime/lozinka!";
					return;
				}
			}
		});
	}
	// checkToken() {
	// 	const token = localStorage.getItem('token');
	// 	if (token) {
	// 	const decodeToken = jwt_decode(token) as Token;
	// 	const currentTime = new Date().getTime() / 1000;

	// 	if (currentTime > decodeToken.exp) {
	// 		alert('Token je istekao!');
	// 		// preusmeri korisnika na login stranicu
	// 		this.router.navigate(['/login']);
	// 	} else {
	// 		// token je validan
	// 		// dohvati podatke o korisniku
	// 		const userId = decodeToken.sub;
	// 		// ...
	// 	}
	// 	} else {
	// 	// token ne postoji u local storage-u
	// 	// preusmeri korisnika na login stranicu
	// 	alert("Token ne postoji u local storage-u");
	// 	this.router.navigate(['/login']);
	// 	}
	// }
}
