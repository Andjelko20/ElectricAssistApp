import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Prosumers } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { JwtToken } from 'src/app/utilities/jwt-token';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
@Component({
  selector: 'app-account-page',
  templateUrl: './account-page.component.html',
  styleUrls: ['./account-page.component.css']
})
export class AccountPageComponent implements OnInit {

  roles!:any[];
  updateUserDetail:Prosumers={
    id: 0,
    name: '',
    username: '',
    email: '',
    role: '',
    blocked: false,
    settlement:'',
    city:'',
    country: '',
    address:''
  }
  public idUser!:number;
  public role!:string;
  public name!:string;

  constructor(private formBuilder: FormBuilder,private route:ActivatedRoute,private router:Router,private updateService:AuthService) { }

  ngOnInit(): void {
    let token=new JwtToken();
    this.idUser=token.data.id as number;
    this.role=token.data.role as string;

    this.updateService.getlogInUser().subscribe({
          next:(response)=>{
            this.updateUserDetail={
              id:this.idUser,
              name:response.name,
              username:response.username,
              email:response.email,
              blocked:response.blocked,
              role:this.role,
              settlement:response.settlement,
              city:response.city,
              country: response.country,
              address:response.address
              
              };
            },
			
          });
  }
  upDateUser()
  {
    this.updateService.upDateLogedIn(this.updateUserDetail)
    .subscribe({
      next:()=>{
        if(this.role==='admin')
            this.router.navigate(['/profile-admin']);
      }
    });
  }
}
