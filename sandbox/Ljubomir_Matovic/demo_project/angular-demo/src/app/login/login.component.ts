import { Component, OnInit } from '@angular/core';

@Component({
	selector: 'app-login',
	templateUrl: './login.component.html',
	styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

	username='';
	password='';
	errorMsg='';

	constructor() { }

	ngOnInit(): void {
	}

	login(){
		if(this.username.trim().length===0){
			this.errorMsg="Korisničko ime je obavezno";
			return;
		}else if(this.password.trim().length===0){
			this.errorMsg="Lozinka je obavezna";
			return;
		}
	}
}
