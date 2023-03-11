import { Component, HostListener, OnInit } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
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
      id:"1",
      name:'fsdf',
      userName:'dsda',
      password:'54646',
      block:'YES',
      role:'dso'
    },
    {
      id:"2",
      name:'fsdf',
      userName:'dsda',
      password:'54654',
      block:'NO',
      role:'dso'
    }
  ];
 
  constructor(private router:Router,private usersService:AuthService,
    private route:ActivatedRoute) { }

  ngOnInit(): void {
    
    }

  getUsers()
  {
    return this.users;
  }

  delete(id:string)
  {
    if(confirm('Are you sere to delete? '+id))
    {
      if(id)
        {
          this.usersService.delete(id)
          .subscribe(()=>this.getUsers);
        }  
    }
  }

  block(block:string,id:string)
  {
    if(block=="YES")
    {
      alert("You cant change, becouse is allready blocked! "+id);
    }
    else{
      if(confirm('Are you sure you want to block them? '+id))
      {
          console.log("Block them!");
      }
    }
    
  }
  logout()
  {
    localStorage.removeItem('token');
    localStorage.clear();
    this.router.navigate(['/login']);
  }

}
