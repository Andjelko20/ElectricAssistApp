import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Users } from '../models/users.model';
import { AuthService } from '../services/auth.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  
  users:Users[] = [
    {
      id:'1',
      name:"Marko",
      userName:"Markovic",
      block:"NO",
      role:"DSO"
    },
    {
      id:'2',
      name:"Sasa",
      userName:"Sasic",
      block:"YES",
      role:"Prosumer"
    }
  ]
 
  constructor(private router:Router,private deleteService:AuthService) { }

  ngOnInit(): void {
  }
  delete(name:string)
  {
    this.deleteService.delete(name)
    .subscribe({
      next:(response)=>{
        this.router.navigate(['user']);
      }
    });
  }
}
