import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Prosumers } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { JwtToken } from 'src/app/utilities/jwt-token';
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
  idUser!: number;
  updateUserDetail:Prosumers={
    id: 0,
    name: '',
    username: '',
    email: '',
    role: '',
    blocked: false,
    settlement:'',
    city:'',
    country: '',
    address:''
  }
  constructor(private route:ActivatedRoute,private router:Router,private auth: AuthService){
	this.route.queryParams.subscribe(params=>{
		this.prosumersPage=params['tab']!="map";
	})
  }
  ngOnInit(): void {
	  fetch(environment.serverUrl+"/api/ProsumersDetails/count",{headers:{"Authorization":"Bearer "+localStorage.getItem("token")}})
	  .then(res=>res.json())
	  .then(res=>this.numberOfProsumers=res);

	  const { id, role } = new JwtToken().data;
    this.idUser = id as number;
     this.auth.getlogInUser().subscribe(response=>{
      this.updateUserDetail = {
        id: this.idUser,
        name: response.name,
        username: response.username,
        email: response.email,
        blocked: response.blocked,
        role: response.role,
        settlement: response.settlement,
        city: response.city,
        country: response.country,
        address: response.address
	 }
	 })
  }
  showComponent() {
	this.router.navigate(["prosumers"],{
		queryParams:{
			tab:"table"
		}
	});
	this.prosumersPage=true;
  }
  showComponent1() {
	  this.router.navigate(["prosumers"],{
		queryParams:{
			tab:"map"
		}
	});
	this.prosumersPage=false;
  }
}
