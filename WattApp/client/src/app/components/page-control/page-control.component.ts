import { Component } from '@angular/core';

@Component({
  selector: 'app-page-control',
  templateUrl: './page-control.component.html',
  styleUrls: ['./page-control.component.css']
})
export class PageControlComponent {
	public currentPage:number=1;
}
