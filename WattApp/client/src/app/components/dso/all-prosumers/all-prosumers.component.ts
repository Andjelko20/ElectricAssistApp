import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Prosumers, Users } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-all-prosumers',
  templateUrl: './all-prosumers.component.html',
  styleUrls: ['./all-prosumers.component.css']
})
export class AllProsumersComponent implements OnInit{
	url=environment.serverUrl+"/api/ProsumersDetails/page";
	currentPage:number=1;
	itemsPerPage:number=10;
	totalItems:number=20;
	data:any[]=[];
  prosumers:Prosumers[] = [];
  filteredProsumers: Prosumers[] = [];
  _listFilter = '';
  constructor(private userService:AuthService)
  {
    
  }
  
  get listFilter(): string {
    return this._listFilter;
  }
  performFilter(filterBy: string): Prosumers[] {
    filterBy = filterBy.toLocaleLowerCase();
    return this.prosumers.filter((prosumers: Prosumers) =>
    prosumers.name.toLocaleLowerCase().indexOf(filterBy) !== -1);
  }
  set listFilter(value: string) {
    this._listFilter = value;
    this.filteredProsumers = this.listFilter ? this.performFilter(this.listFilter) : this.prosumers;
  }
  ngOnInit(): void {
	console.log(this.itemsPerPage)
    this.filteredProsumers = this.data?.map((u:any)=>({
		id: u.id,
		name: u.name,
		username: u.username,
		email: u.email,
		role: u.role,
		blocked: u.blocked,
		settlement:u.settlement,
		city:u.city,
		country: u.country
	}as Prosumers));
  }
  getUsers(): void {
    this.userService.getAllProsumers().subscribe(prosumers => {
      this.prosumers=prosumers.data.map((u:any)=>({
        id: u.id,
        name: u.name,
        username: u.username,
        email: u.email,
        role: u.role,
        blocked: u.blocked,
        settlement:u.settlement,
        city:u.city,
        country: u.country
    }as Prosumers));
    this.filteredProsumers=prosumers.data.map((u:any)=>({
      id: u.id,
      name: u.name,
      username: u.username,
      email: u.email,
      role: u.role,
      blocked: u.blocked,
      settlement:u.settlement,
      city:u.city,
      country: u.country
     }as Prosumers));
    
     });
  }
  pageChanged(pageNumber:number){
	let url=new URL(this.url);
		url.searchParams.set("pageNumber",pageNumber.toString());
		url.searchParams.set("pageSize",this.itemsPerPage.toString());
		let controller=new AbortController();
		setTimeout(()=>{
			controller.abort();
		},1000);
		fetch(url.toString(),{headers:{"Authorization":"Bearer "+localStorage.getItem("token")},signal:controller.signal})
		.then(res=>{
			if(res.status==401 || res.status==403){
				//alert('aaa');
				return Promise.reject("aaa");
			}
			return res.text();
		})
		.then(ress=>{
			try{
				let res=JSON.parse(ress);
				if(res?.data==undefined)
					return;
				console.log(res);
				this.data=res?.data;
				this.currentPage=pageNumber;
				this.totalItems=res.numberOfPages*this.itemsPerPage;
			}
			catch(err:any){
				console.log(err.message);
			}
			
		})
		.catch(err=>{
			console.log(err?.name);
			localStorage.removeItem("token");
			//this.router.navigate(["login"]);
		});
  }
}
