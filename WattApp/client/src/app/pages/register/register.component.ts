import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Register, Users} from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  users:Users= {
    id: 0,
    name: '',
    username: '',
    email: '',
    roleId: 0,
    block: false,
    settlement: '',
    city: '',
    country: '',
    address: '',
    password: ''
  }
  registerName='';
  registerUserName='';
  registerRassword='';
  confirmPassword='';
  errorMsg='';
  constructor(private router: Router, private registerService: AuthService) { }

  ngOnInit(): void {
    
  }
  register()
  {
    if(this.registerName.trim().length===0){
			this.errorMsg="First name is required";
      return;
		}
    if(this.registerUserName.trim().length===0){
      this.errorMsg="User name is required";
			return;
		}
    if(this.registerRassword.trim().length===0){
			this.errorMsg="Password is required";
			return;
		}
    if(this.confirmPassword.trim().length===0){
			this.errorMsg="Repeat password is required";
			return;
		}
  }
//ko dobijem da nije admin on ga redirektuje 
}
