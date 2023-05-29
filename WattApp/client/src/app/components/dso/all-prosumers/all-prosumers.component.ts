import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { SessionService } from 'src/app/services/session.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-all-prosumers',
  templateUrl: './all-prosumers.component.html',
  styleUrls: ['./all-p.css','./all-prosumers.component.css']
})
export class AllProsumersComponent implements OnInit {
  url = `${environment.serverUrl}/api/ProsumersDetails/page/filters`;
  currentPage = 1;
  itemsPerPage = 10;
  totalItems = 20;
  data: any[] = [];
  prosumerValues: any[] = [];
  loader:boolean=false;
  showFilters:boolean = !environment.production;
  settlements:any[]=[];

  constructor(private service : AuthService) {}
  loadSettlements(){
	fetch(`${environment.serverUrl}/my_settlements`,{headers:{Authorization:"Bearer "+localStorage.getItem("token")}})
	.then(res=>res.json())
	.then(res=>{
		this.settlements=res;
	});
  }
  ngOnInit(): void {
	this.loadSettlements();
    this.pageChanged(1);
  }

  pageChanged(pageNumber:number){
	this.loader=true;
	let url=new URL(this.url);
		url.searchParams.set("pageNumber",pageNumber.toString());
		url.searchParams.set("pageSize",this.itemsPerPage.toString());
		url.searchParams.set("cityId","-1");
		if(this.filters.name != "")
			url.searchParams.set("SearchValue",this.filters.name);
		if(this.filters.settlement!=0)
			url.searchParams.set("SettlmentId",this.filters.settlement.toString());
		let controller=new AbortController();
		setTimeout(()=>{
			controller.abort();
		},3000);
		fetch(url.toString(),{headers:{"Authorization":"Bearer "+localStorage.getItem("token")},signal:controller.signal})
		.then(res=>{
			setTimeout(()=>{
				this.loader=false;
			},10)
			
			if(res.status>200){
				return Promise.reject("aaa");
			}
			return res.json();
		})
		.then(res=>{
			setTimeout(()=>{
				this.loader=false;
			},10)
				if(res==undefined)
					return;
				this.data=res?.data;
				this.currentPage=pageNumber;
				this.totalItems=res.numberOfPages*this.itemsPerPage;
        		this.prosumerValues = res;
		})
		.catch(err=>{
			console.log('err')
			this.data=[];
				this.currentPage=pageNumber;
				this.totalItems=0;
        		this.prosumerValues = [];
		});
  }

  public filters={
    settlement:0,
    name:'',
	categoryId:1,
	greaterThan:"true",
	kWhValue:0
  };
  showDropdown = false;
  msgShow:boolean=false;
  toggleDropdown() {
    this.showDropdown = !this.showDropdown;
  }
  countActiveFilters() {
    let count = 0;
    if (this.filters.settlement !== 0) {
      count++;
    }
    if (this.filters.name.trim() !== '') {
      count++;
    }
  
    return count;
  }
  clearFilters() {
    this.filters = {
      settlement:0,
      name:'',
	  categoryId:1,
	  greaterThan:"true",
	  kWhValue:0
    };
    this.pageChanged(1); 
  }

}



export class ProsumerFilterModel{
	settlementId : number;

	categoryId : number;
	greaterThan : number;
	value : number;

	searchValue : string;

	constructor(settlementId : number,
				categoryId : number,
				greaterThan : number,
				value : number,
				searchValue : string)
	{
		this.settlementId = settlementId;
		this.categoryId = categoryId;
		this.greaterThan = greaterThan;
		this.value = value;
		this.searchValue = searchValue;
	}
}
