import { Component,Input,OnInit,Output,EventEmitter } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-page-control',
  templateUrl: './page-control.component.html',
  styleUrls: ['./page-control.component.css']
})
export class PageControlComponent implements OnInit{
	//@Input() url!:string;

	//@Input() currentPage:number=1;
	//@Output() currentPageChange:EventEmitter<number>=new EventEmitter<number>();

	@Output() pageChange:EventEmitter<number>=new EventEmitter<number>();

	//@Input() itemsPerPage:number=10;
	//@Output() itemsPerPageChange:EventEmitter<number>=new EventEmitter<number>();

	//@Input() totalItems:number=20;
	//@Output() totalItemsChange:EventEmitter<number>=new EventEmitter<number>();

	//@Input() items:any[]=[];
	//@Output() itemsChange:EventEmitter<any[]>=new EventEmitter<any[]>();
	constructor(private router:Router){
	}
	ngOnInit(){
		this.pageChanged(1);
	}
	pageChanged(pageNumber:number){
		this.pageChange.emit(pageNumber);
		//this.currentPageChange.emit(pageNumber);
		/*
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
				//this.itemsChange.emit(res?.data);
				this.currentPageChange.emit(pageNumber);
				this.totalItemsChange.emit(res.numberOfPages*this.itemsPerPage);
			}
			catch(err:any){
				console.log(err.message);
			}
			
		})
		.catch(err=>{
			console.log(err?.name);
			localStorage.removeItem("token");
			this.router.navigate(["login"]);
		});
		*/
	}
}
