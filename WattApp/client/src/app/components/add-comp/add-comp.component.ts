import { Component, OnInit } from '@angular/core';
import { Route, Router } from '@angular/router';
import { Users } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-add-comp',
  templateUrl: './add-comp.component.html',
  styleUrls: ['./add-comp.component.css']
})
export class AddCompComponent implements OnInit {
  addUserRequest:Users={
    id:0,
    name:'',
    userName:'',
    password:'',
    block:false,
    role:''
  }
  constructor(private usersService:AuthService,private router:Router) { }

  ngOnInit(): void {
  }
  onSelectedBlock(value:boolean):void
  {
    this.addUserRequest.block = value;
  }
  onSelectedRole(value:string):void
  {
    this.addUserRequest.role = value;
  }
  
  addUsers()
  {
    this.usersService.addUsers(this.addUserRequest)
    .subscribe({
      next:(users)=>{
         this.router.navigate(['/home']);
      
      }
    });
  }
}
