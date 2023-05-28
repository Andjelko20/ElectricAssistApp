import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { SessionService } from 'src/app/services/session.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-all-prosumers',
  templateUrl: './all-prosumers.component.html',
  styleUrls: ['./all-prosumers.component.css']
})
export class AllProsumersComponent implements OnInit {
  url = `${environment.serverUrl}/api/ProsumersDetails/page/filters`;
  currentPage = 1;
  itemsPerPage = 10;
  totalItems = 20;
  data: any[] = [];
  prosumerValues: any[] = [];
  loader:boolean=false;

  filters : ProsumerFilterModel = new ProsumerFilterModel(
	0, 
	0, 
	1, 
	0, 
	""
  )

  settlements = [];

  constructor(private sessionService : SessionService) {}

  ngOnInit(): void {
    this.pageChanged(1);
  }

  pageChanged(pageNumber:number){
	this.loader=true;
	let url=new URL(this.url);
		url.searchParams.set("pageNumber",pageNumber.toString());
		url.searchParams.set("pageSize",this.itemsPerPage.toString());
		url.searchParams.set("cityId","-1");
		let controller=new AbortController();
		setTimeout(()=>{
			controller.abort();
		},3000);
		fetch(url.toString(),{headers:{"Authorization":"Bearer "+localStorage.getItem("token")},signal:controller.signal})
		.then(res=>{
			this.loader=false;
			if(res.status==401 || res.status==403){
				return Promise.reject("aaa");
			}
			return res.json();
		})
		.then(res=>{
			this.loader=false;
				if(res==undefined)
					return;
				this.data=res?.data;
				this.currentPage=pageNumber;
				this.totalItems=res.numberOfPages*this.itemsPerPage;
        		this.prosumerValues = res;
		})
  }


  onSelectedSearchValue(event : any){
	this.filters.searchValue = event.target.value;

	this.loader=true;
	let url=new URL(this.url);
		url.searchParams.set("pageNumber","1");
		url.searchParams.set("pageSize",this.itemsPerPage.toString());
		url.searchParams.set("cityId","-1");
		let controller=new AbortController();
		setTimeout(()=>{
			controller.abort();
		},3000);
		fetch(url.toString(),{headers:{"Authorization":"Bearer "+localStorage.getItem("token")},signal:controller.signal})
		.then(res=>{
			this.loader=false;
			if(res.status==401 || res.status==403){
				return Promise.reject("aaa");
			}
			return res.json();
		})
		.then(res=>{
			this.loader=false;
				if(res==undefined)
					return;
				this.data=res?.data;
				this.currentPage=1;
				this.totalItems=res.numberOfPages*this.itemsPerPage;
        		this.prosumerValues = res;
		})
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