import { HttpErrorResponse } from '@angular/common/http';
import { AfterViewInit, Component, ElementRef, HostListener, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ShowUsers, Users } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { environment } from 'src/environments/environment';
import { FormBuilder, Validators } from '@angular/forms';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
@Component({
  selector: 'app-admin-dso',
  templateUrl: './admin-dso.component.html',
  styleUrls: ['./admin-dso.component.css']
})
export class AdminDsoComponent implements OnInit {
  @ViewChild('modalContent') modalContent!: TemplateRef<any>;
  @ViewChild('modalContent1') modalContent1!: TemplateRef<any>;
  @ViewChild('modalContent2') modalContent2!: TemplateRef<any>;
  body: string = ''; 
  btnAction:string='';

  userForm=this.fb.group({

    email:['',[Validators.required,Validators.email]],
    
  })

	currentPage:number=1;
	itemsPerPage:number=10;
	totalItems:number=10;

  confirmBlock?:boolean=false;
  showUsers:ShowUsers[]=[];
  cities:any[]=[];
  settlements:any[]=[];
  loading:boolean=true;
  public filters={
    blocked:-1,
    role:0,
    settlement:0,
    city:0,
    name:''
  };
  onBlockClick!: (this: HTMLElement, ev: MouseEvent) => any;
  onUnblockClick!: (this: HTMLElement, ev: MouseEvent) => any;
  oneUser?:string;
  roles!:any[];
  updateUserDetail:Users={

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
	public success:boolean=false;
  showDropdown = false;
  msgShow:boolean=false;
  constructor(private router:Router,private usersService:AuthService,
    private route:ActivatedRoute,private modalService: NgbModal,private elementRef: ElementRef,private fb:FormBuilder) { }

	getSettlements(){

		fetch(environment.serverUrl+"/settlements?cityId="+this.filters.city)
		.then(res=>res.json())
		.then(res=>{
	  		this.settlements=res;
		});
	}
	cityChanged(){
		this.getSettlements();
		this.filters.settlement=0;
		this.pageChanged(1);
	}
  ngOnInit(){
	this.pageChanged(1);
	fetch(environment.serverUrl+"/cities?countryId=1")
	.then(res=>res.json())
	.then(res=>{
		this.cities=res
  	});
  }
	pageChanged(pageNumber:number){
		this.currentPage=pageNumber;
		this.loading=true;
		this.usersService.getAllUsers(pageNumber,this.itemsPerPage,this.filters).subscribe({
			next:(users:any) => {
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
			setTimeout(()=>{
				this.loading=false;
			},0);
			
		   },
		   error:err=>{
			this.showUsers=[];
			this.totalItems=0;
			setTimeout(()=>{
				this.loading=false;
			},0);
			}
		});
		}

  blockUser(id: number) {
    
    this.modalService.open(this.modalContent);
    this.confirmBlock=false;
    const block = document.getElementById('popup');

    if (block != null) {
      this.body="Do you want to block this user?"
      this.btnAction="Block";

      block.removeEventListener('click', this.onBlockClick); 
      this.onBlockClick = () => { 
        this.usersService.blockUser(id).subscribe(() => {
            const userIndex = this.showUsers.findIndex(user => user.id === id);
            this.showUsers[userIndex].block = true;
            this.confirmBlock=true;
        });
        block.removeEventListener('click', this.onBlockClick); 
      };
      block.addEventListener('click', this.onBlockClick); 
    }
  }
  
  unblockUser(id: number) {

    this.modalService.open(this.modalContent);
    this.confirmBlock=false;
    const unblock = document.getElementById('popup');

    if (unblock != null) {
      this.body="Do you want to unblock this user?"
      this.btnAction="Unblock";

      unblock.removeEventListener('click', this.onUnblockClick); 
      this.onUnblockClick = () => { 
        this.usersService.unblockUser(id).subscribe(() => {
          const userIndex = this.showUsers.findIndex(user => user.id === id);
          this.showUsers[userIndex].block = false;
          this.confirmBlock=true;
        });
        unblock.removeEventListener('click', this.onUnblockClick); 
      };
      unblock.addEventListener('click', this.onUnblockClick); 
    }
  }
  
  deleteUser(id:number)
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
              this.usersService.getAllUsers(this.currentPage,this.itemsPerPage,this.filters).subscribe({
				next:users => {
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
              },
			  error:(_)=>{
				let page=this.currentPage-1;
				if(page<=0)
					page=1;
				this.usersService.getAllUsers(page,this.itemsPerPage,this.filters).subscribe({next:users => {
					this.totalItems=users.numberOfPages*this.itemsPerPage;
					this.currentPage=page;
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
			  },error:()=>{
				this.showUsers=[];
				this.currentPage=1;
				this.totalItems=0;
			  }});
          }
		});
      });
	});
	}
  }
  updatePage(id:number)
  {
    this.modalService.open(this.modalContent1);
    fetch(environment.serverUrl+"/api/users/roles",{headers:{"Authorization":"Bearer "+localStorage.getItem("token")}})
		.then(res=>res.json())
		.then(res=>{
		this.roles=res;
    
        this.usersService.getUser( id )
        .subscribe({
          next:(response)=>{
            this.oneUser=response.name;
            this.updateUserDetail={
              id:id,
              name:response.name,
              username:response.username,
              password:"",
              email:response.email,
              block:response.blocked,
              roleId:this.roles.find(r=>r.name==response.role)?.id,
              settlement: response.settlement,
              city: response.city,
              country: response.country,
              address: response.address
              };
            }
          });
		});

  }
  upDate()
  {
    
    this.usersService.upDate(this.updateUserDetail.id,this.updateUserDetail)
    .subscribe({
      next:()=>{
        this.modalService.open(this.modalContent2);
        this.body="Email confirmation has been successfully sent to the user's email."
        this.router.navigate(['dashboard']);
      },error:()=>{
        this.modalService.open(this.modalContent2);
        this.body="User with that email address already exists."
        this.router.navigate(['dashboard']);
        
      }
    });
  }

  toggleDropdown() {
    this.showDropdown = !this.showDropdown;
  }
  @HostListener('document:click', ['$event'])
  onClick(event: MouseEvent) {
    const clickedElement = event.target as HTMLElement;
    const dropdownElement = this.elementRef.nativeElement;
    const navbarElement = dropdownElement.querySelector('#dropbtn1') as HTMLElement;
    const dropdownContent = dropdownElement.querySelector('.dropdown-content') as HTMLElement;

    if (!dropdownElement.contains(clickedElement) || (!navbarElement.contains(clickedElement) && !dropdownContent.contains(clickedElement))) {
      this.showDropdown = false;
    }
  }
  countActiveFilters() {
    let count = 0;
    if (this.filters.blocked !== -1) {
      count++;
    }
    if (this.filters.role !== 0) {
      count++;
    }
    if (this.filters.settlement !== 0) {
      count++;
    }
    if (this.filters.city !== 0) {
      count++;
    }
    if (this.filters.name.trim() !== '') {
      count++;
    }
  
    return count;
  }
  clearFilters() {
    this.filters = {
      blocked:-1,
      role:0,
      settlement:0,
      city:0,
      name:''
    };
    this.pageChanged(1); 
  }
  logout()
  {
    localStorage.removeItem('token');
    this.usersService.isLoginSubject.next(false)
    this.router.navigate(['/login']);
  }
}
