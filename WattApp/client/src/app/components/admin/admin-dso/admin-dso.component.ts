import { Component, TemplateRef, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ShowUsers, Users } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-admin-dso',
  templateUrl: './admin-dso.component.html',
  styleUrls: ['./admin-dso.component.css']
})
export class AdminDsoComponent {
  @ViewChild('modalContent') modalContent!: TemplateRef<any>;
  body: string = ''; 
  btnAction:string='';

	currentPage:number=1;
	itemsPerPage:number=10;
	totalItems:number=10;

  confirmBlock?:boolean=false;
  showUsers:ShowUsers[]=[];

  onBlockClick!: (this: HTMLElement, ev: MouseEvent) => any;
  onUnblockClick!: (this: HTMLElement, ev: MouseEvent) => any;
  
  constructor(private router:Router,private usersService:AuthService,
    private route:ActivatedRoute,private modalService: NgbModal) { }

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
    
    this.modalService.open(this.modalContent);
    this.confirmBlock=false;
    const block = document.getElementById('popup');

    if (block != null) {
      this.body="Do you want to block this user?"
      this.btnAction="Block";

      block.removeEventListener('click', this.onBlockClick); // remove previous event listener
      this.onBlockClick = () => { // create new event listener function
        this.usersService.blockUser(id).subscribe(() => {
            const userIndex = this.showUsers.findIndex(user => user.id === id);
            this.showUsers[userIndex].block = true;
            this.confirmBlock=true;
        });
        block.removeEventListener('click', this.onBlockClick); // remove event listener after execution
      };
      block.addEventListener('click', this.onBlockClick); // add new event listener
    }
  }
  
  unblockUser(id: number) {

    this.modalService.open(this.modalContent);
    this.confirmBlock=false;
    const unblock = document.getElementById('popup');

    if (unblock != null) {
      this.body="Do you want to unblock this user?"
      this.btnAction="Unblock";

      unblock.removeEventListener('click', this.onUnblockClick); // remove previous event listener
      this.onUnblockClick = () => { // create new event listener function
        this.usersService.unblockUser(id).subscribe(() => {
          const userIndex = this.showUsers.findIndex(user => user.id === id);
          this.showUsers[userIndex].block = false;
          this.confirmBlock=true;
        });
        unblock.removeEventListener('click', this.onUnblockClick); // remove event listener after execution
      };
      unblock.addEventListener('click', this.onUnblockClick); // add new event listener
    }
  }
  
  delete(id:number)
  {
      this.modalService.open(this.modalContent);
      const deletePopup= document.getElementById('popup');
      
      if(deletePopup!=null)
      { 
        
        this.body = 'Do you want to delete this user?';
        this.btnAction="Delete";
        
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
 
  logout()
  {
    localStorage.removeItem('token');
    this.usersService.isLoginSubject.next(false)
    this.router.navigate(['/login']);
  }

}
