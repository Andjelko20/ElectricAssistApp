import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Observable } from 'rxjs';
import { Prosumers, Users } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
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
  prosumerValues: any[] = [];
  prosumers:Prosumers[] = [];
  filteredProsumers: Prosumers[] = [];
  _listFilter = '';
  constructor(private userService:AuthService,private historyService:HistoryPredictionService)
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
  //   this.prosumers = this.data?.map((u:any)=>({
	// 	id: u.id,
	// 	name: u.name,
	// 	username: u.username,
	// 	email: u.email,
	// 	role: u.role,
	// 	blocked: u.blocked,
	// 	settlement:u.settlement,
	// 	city:u.city,
	// 	country: u.country
	// }as Prosumers));


    // this.getUsers();
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
		url.searchParams.set("cityId","-1");
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
        this.prosumerValues = [];
        this.data.forEach(prosumer =>{
          this.historyService.currentUserProductionConsumption(prosumer.id,2).subscribe(vr1=>{
            if(typeof vr1 !== 'number'){
              vr1 = 0;
            }
            this.historyService.currentUserProductionConsumption(prosumer.id,1).subscribe(vr2=>{
              if(typeof vr2 !== 'number')
              {
                vr2 = 0;
              }
              const prosumerData = {
                id: prosumer.id,
                consumption: vr1,
                production: vr2
              };
              this.prosumerValues.push(prosumerData);
            })
          })
        })
			}
			catch(err:any){
				console.log(err.message);
			}
			
		})
		// .catch(err=>{
		// 	localStorage.removeItem("token");
		// 	//this.router.navigate(["login"]);
		// });
    
  }
}
