import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Prosumers } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
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
i: any;

  constructor(
    private authService: AuthService,
    private historyService: HistoryPredictionService
  ) {}

  ngOnInit(): void {
    this.pageChanged(1);
  }

  pageChanged(pageNumber:number){
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
			if(res.status==401 || res.status==403){
				return Promise.reject("aaa");
			}
			return res.json();
		})
		.then(res=>{
				if(res==undefined)
					return;
				this.data=res?.data;
				this.currentPage=pageNumber;
				this.totalItems=res.numberOfPages*this.itemsPerPage;
        		this.prosumerValues = res;	
		})
    
  }
}