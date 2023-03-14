import { Component, HostListener, OnInit,ElementRef,ViewChild } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { Users } from '../../models/users.model';
import { AuthService } from '../../services/auth.service';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  
  users:Users[] = [{
    id: 1,
    name: 'na',
    userName: 'string',
    password:'ddd',
    block: false,
    role:'dso'
  }];
 
  constructor(private router:Router,private usersService:AuthService,
    private route:ActivatedRoute) { }

  ngOnInit(): void {
    this.usersService.getAllUsers().subscribe(users => {
     this.getUsers();
    });
    }
  blockUser(id: number) {
      this.usersService.blockUser(id).subscribe(() => {
        const userIndex = this.users.findIndex(user => user.id === id);
        this.users[userIndex].block = true;
      });
  }
  unblockUser(id: number) {
      this.usersService.unblockUser(id).subscribe(() => {
        const userIndex = this.users.findIndex(user => user.id === id);
        this.users[userIndex].block = false;
      });
  }
  getUsers()
  {
    return this.users;
  }

  delete(id:number)
  {
    if(confirm('Are you sere to delete? '+id))
    {
      this.usersService.delete(id)
      .subscribe({
        next:(response)=>{
          this.router.navigate(['home']);
        }
      });
    }
  }
 
 
  block(user:Users,i:number)
  {
    if(user.block==true)
    {
      if(confirm('Are you sure you want to unblock them? '+user.id))
      {     
        user.block = false;
        this.users[i] = user;
      }
    }
    else{
      if(confirm('Are you sure you want to block them? '+user.id))
      {
        user.block = true;
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
