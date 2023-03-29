import { Component, OnInit } from '@angular/core';
import { Route,Router,NavigationEnd } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { AuthService } from '../../services/auth.service';
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
		// this.router.events.subscribe(event => {
		// 	if (event instanceof NavigationEnd) {
		// 	  // pozovite funkciju za proveru tokena ovde
		// 	  this.checkToken();
		// 	}
		//   });
	}

	login(){
		
		if(this.username.trim().length===0){
			this.errorMsg="User name is required";
			return;
		}
		if(this.password.trim().length===0){
			this.errorMsg="Password is required";
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
					this.authService.isLoginSubject.next(true)
					this.router.navigate(["/admindso"]);
				},
				error:response=>{
					if(response.status==401)
					{
						this.errorMsg = "Pogresno korisnicko ime/lozinka!";
						return;
					}

					// let body = response.body as any;
					// localStorage.setItem("token",body.token);
					// this.authService.isLoginSubject.next(true)
					// this.router.navigate(["/admindso"]);
				}
			}
		  );
	
    		
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
