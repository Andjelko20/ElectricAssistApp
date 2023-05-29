import { Component,Input,OnInit,Output,EventEmitter } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-page-control',
  templateUrl: './page-control.component.html',
  styleUrls: ['./page-control.component.css']
})
export class PageControlComponent implements OnInit{

	@Output() pageChange:EventEmitter<number>=new EventEmitter<number>();

	constructor(private router:Router){
	}
	ngOnInit(){
		this.pageChanged(1);
	}
	pageChanged(pageNumber:number){
		this.pageChange.emit(pageNumber);
	}
}
