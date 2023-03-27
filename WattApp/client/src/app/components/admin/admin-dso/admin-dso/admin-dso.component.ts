import { Component } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { ShowUsers, Users } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-admin-dso',
  templateUrl: './admin-dso.component.html',
  styleUrls: ['./admin-dso.component.css']
})
export class AdminDsoComponent {

  
  showUsers:ShowUsers[]=[];
  constructor(private router:Router,private usersService:AuthService,
    private route:ActivatedRoute) { }

  ngOnInit(): void {
    this.usersService.getAllUsers().subscribe(users => {
     this.showUsers=users.data.map((u:any)=>({
       id: u.id,
       name: u.name,
       username: u.username,
       block: u.blocked,
       email: u.email,
       role: u.role
     } as ShowUsers));
    });
    }


  blockUser(id: number) {
      this.usersService.blockUser(id).subscribe(() => {
        const userIndex = this.showUsers.findIndex(user => user.id === id);
        this.showUsers[userIndex].block = true;
      });
  }
  unblockUser(id: number) {
      this.usersService.unblockUser(id).subscribe(() => {
        const userIndex = this.showUsers.findIndex(user => user.id === id);
        this.showUsers[userIndex].block = false;
      });
  }


  getUsers()
  {
    return this.showUsers;
  }

  delete(id:number)
  {
    if(confirm('Are you sere to delete? '+id))
    {
      this.usersService.delete(id)
      .subscribe({
        next:(response)=>{
          this.router.navigate(['/home']);
          location.reload();
        }
      });
    }
  }
 
 
  block(user:ShowUsers,i:number)
  {
    if(user.block==true)
    {
      if(confirm('Are you sure you want to unblock them? '+user.id))
      {     
        user.block = false;
        this.showUsers[i] = user;
      }
    }
    else{
      if(confirm('Are you sure you want to block them? '+user.id))
      {
        user.block = true;
        this.showUsers[i] = user;
      }
    }
    
  }
  logout()
  {
    localStorage.removeItem('token');
    localStorage.clear();
    this.usersService.isLoginSubject.next(false)
    this.router.navigate(['/login']);
  }

}
