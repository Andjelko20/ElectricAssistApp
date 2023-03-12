import { Component, HostListener, OnInit,ElementRef,ViewChild } from '@angular/core';
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
    },
    {
      id:"3",
      name:'fsdf',
      userName:'dsda',
      password:'54654',
      block:'NO',
      role:'dso'
    },
    {
      id:"4",
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
  removeObject={};
  delete(id:string,index:number)
  {
    if(confirm('Are you sere to delete? '+id))
    {
      if(id)
        {
          this.removeObject =this.users.splice(index,1);
        }  
    }
  }
 
 
  block(user:Users,i:number)
  {
    if(user.block=="YES")
    {
      if(confirm('Are you sure you want to unblock them? '+user.id))
      {     
        user.block = "NO";
        this.users[i] = user;
      }
    }
    else{
      if(confirm('Are you sure you want to block them? '+user.id))
      {
        user.block = "YES";
        this.users[i] = user;
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
