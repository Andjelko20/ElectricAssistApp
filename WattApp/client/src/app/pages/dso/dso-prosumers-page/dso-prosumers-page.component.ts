import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
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
  public prosumersPage=true;
  constructor(private route:ActivatedRoute,private router:Router){
	this.route.queryParams.subscribe(params=>{
		this.prosumersPage=params['tab']!="map";
	})
  }
  ngOnInit(): void {
	  fetch(environment.serverUrl+"/api/ProsumersDetails/count",{headers:{"Authorization":"Bearer "+localStorage.getItem("token")}})
	  .then(res=>res.json())
	  .then(res=>this.numberOfProsumers=res);
  }
  showComponent() {
    //this.componentShown = true;
    //this.componentShown1=false;
	this.router.navigate(["prosumers"],{
		queryParams:{
			tab:"table"
		}
	});
	this.prosumersPage=true;
  }
  showComponent1() {
      //this.componentShown1 = true;
      //this.componentShown=false;
	  this.router.navigate(["prosumers"],{
		queryParams:{
			tab:"map"
		}
	});
	this.prosumersPage=false;
  }
}
