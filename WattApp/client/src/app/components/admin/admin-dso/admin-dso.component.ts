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
	currentPage:number=1;
	itemsPerPage:number=10;
	totalItems:number=10;
  
  showUsers:ShowUsers[]=[];
  onBlockClick!: (this: HTMLElement, ev: MouseEvent) => any;
  onUnblockClick!: (this: HTMLElement, ev: MouseEvent) => any;
  
  constructor(private router:Router,private usersService:AuthService,
    private route:ActivatedRoute) { }

  ngOnInit(): void {
    this.usersService.getAllUsers(1).subscribe(users => {
	 this.totalItems=users.numberOfPages*this.itemsPerPage;
     this.showUsers=users.data.map((u:any)=>({
       id: u.id,
       name: u.name,
       username: u.username,
       block: u.blocked,
       email: u.email,
       role: u.role,
       settlement:u.settlement,
       city:u.city,
       address:u.address,
       country:u.country
     } as ShowUsers));
    });
    
    
    }
	pageChanged(pageNumber:number){
		this.currentPage=pageNumber;
		this.usersService.getAllUsers(pageNumber,this.itemsPerPage).subscribe(users => {
			this.totalItems=users.numberOfPages*this.itemsPerPage;
			this.showUsers=users.data.map((u:any)=>({
			  id: u.id,
       name: u.name,
       username: u.username,
       block: u.blocked,
       email: u.email,
       role: u.role,
       settlement:u.settlement,
       city:u.city,
       address:u.address,
       country:u.country
			} as ShowUsers));
		   });
	}

  blockUser(id: number) {
    const block = document.getElementById('block-popup');
    if (block != null) {
      block.removeEventListener('click', this.onBlockClick); // remove previous event listener
      this.onBlockClick = () => { // create new event listener function
        this.usersService.blockUser(id).subscribe(() => {
          
          const userIndex = this.showUsers.findIndex(user => user.id === id);
          this.showUsers[userIndex].block = true;
        });
        block.removeEventListener('click', this.onBlockClick); // remove event listener after execution
      };
      block.addEventListener('click', this.onBlockClick); // add new event listener
    }
  }
  
  unblockUser(id: number) {
    const unblock = document.getElementById('unblock-popup');
    if (unblock != null) {
      unblock.removeEventListener('click', this.onUnblockClick); // remove previous event listener
      this.onUnblockClick = () => { // create new event listener function
        this.usersService.unblockUser(id).subscribe(() => {
  
          const userIndex = this.showUsers.findIndex(user => user.id === id);
          this.showUsers[userIndex].block = false;
        });
        unblock.removeEventListener('click', this.onUnblockClick); // remove event listener after execution
      };
      unblock.addEventListener('click', this.onUnblockClick); // add new event listener
    }
  }
  

  getUsers()
  {
    return this.showUsers;
  }

  delete(id:number)
  {
   
    const deletePopup= document.getElementById('delete-admin-popup');
    
    if(deletePopup!=null)
    {
     deletePopup.addEventListener('click', () => {
      this.usersService.delete(id)
      .subscribe(()=>{
          this.router.navigate(['dashboard']);
          this.usersService.getAllUsers(1).subscribe(users => {
            this.totalItems=users.numberOfPages*this.itemsPerPage;
              this.showUsers=users.data.map((u:any)=>({
                id: u.id,
                name: u.name,
                username: u.username,
                block: u.blocked,
                email: u.email,
                role: u.role,
                settlement:u.settlement,
                city:u.city,
                address:u.address,
                country:u.country
              } as ShowUsers));
             });
      });
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
    this.usersService.isLoginSubject.next(false)
    this.router.navigate(['/login']);
  }

}
