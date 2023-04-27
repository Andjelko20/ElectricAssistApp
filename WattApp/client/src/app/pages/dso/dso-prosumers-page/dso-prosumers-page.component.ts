import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-dso-prosumers-page',
  templateUrl: './dso-prosumers-page.component.html',
  styleUrls: ['./dso-prosumers-page.component.css']
})
export class DsoProsumersPageComponent implements OnInit {


  componentShown = true;
  componentShown1 = false;
  numberOfProsumers=0;
  pageUrl=environment.serverUrl+"/api/ProsumersDetails/page";
  currentPage=1;
  itemsPerPage=10;
  totalItems=20;
  items:any[]=[];
  ngOnInit(): void {
	  fetch(environment.serverUrl+"/api/ProsumersDetails/count",{headers:{"Authorization":"Bearer "+localStorage.getItem("token")}})
	  .then(res=>res.json())
	  .then(res=>this.numberOfProsumers=res);
  }
  showComponent() {
    this.componentShown = true;
  
    
    this.componentShown1=false;
  }
  showComponent1() {
      this.componentShown1 = true;

      this.componentShown=false;
  }
}
