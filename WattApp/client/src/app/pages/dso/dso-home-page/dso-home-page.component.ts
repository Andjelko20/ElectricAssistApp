import { AfterViewInit, Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Popover, Tooltip } from 'bootstrap';
import { first } from 'rxjs';
import { Prosumers } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { JwtToken } from 'src/app/utilities/jwt-token';
import { environment } from 'src/environments/environment';

declare var $: any;
@Component({
  selector: 'app-dso-home-page',
  templateUrl: './dso-home-page.component.html',
  styleUrls: ['./dso-home-page.component.css','./dso-css.css'],

})
export class DsoHomePageComponent implements AfterViewInit, OnInit{
 
  popover: Popover | undefined;
  tooltip: Tooltip | undefined;
  numberOfProsumers=0;
  avgProduction?:number;
  idUser!:number;
  role!:string;
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
  constructor(private avgConsumption:HistoryPredictionService,private route:ActivatedRoute,private router:Router,private updateService:AuthService){

  }
  ngAfterViewInit() {
    const popoverTriggerList = document.querySelectorAll('[data-bs-toggle="popover"]');
    const popoverList = Array.from(popoverTriggerList).map(function (popoverTriggerEl) {
      return new Popover(popoverTriggerEl)
    });
    this.popover = popoverList[0];
    const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
    const tooltipList = Array.from(tooltipTriggerList).map(function (tooltipTriggerEl) {
      return new Tooltip(tooltipTriggerEl)

      
    });
    this.tooltip = tooltipList[0];
   
  }
  async ngOnInit(): Promise<void> {
    let token=new JwtToken();
    this.idUser=token.data.id as number;
    this.role=token.data.role as string;
    console.log(this.idUser);
    this.updateService.getlogInUser()
        .subscribe({
          next:(response)=>{
            this.updateUserDetail={
              id:this.idUser,
              name:response.name,
              username:response.username,
              email:response.email,
              blocked:response.blocked,
              role:this.role,
              settlement:response.settlement,
              city:response.city,
              country: response.country,
              address:response.address
              
              };
            
            },
			error:(response)=>{
				this.router.navigate(["prosumer-account-page"]);
			}
          });
    fetch(environment.serverUrl+"/api/ProsumersDetails/count",{headers:{"Authorization":"Bearer "+localStorage.getItem("token")}})
	  .then(res=>res.json())
	  .then(res=>this.numberOfProsumers=res);
    const result = await this.avgConsumption.getAverageConsumptionProductionCity(1,2).pipe(first()).toPromise();
  
    this.avgProduction = result!;
    console.log(this.avgProduction);
    
  }

  graph:boolean = true;
  tabelar:boolean = false;


  compGraph = true;
  compGraph1 = false;
  compGraph2 = false;
  compGraph3 = false;

  compTable = true;
  compTable1 = false;
  compTable2 = false;
  compTable3 = false;
  showComponentTable() {
    this.compTable = true;
    this.compTable1=false;
    this.compTable2=false;
    this.compTable3 = false;
  }
  showComponentTable1() {
      this.compTable = false;
      this.compTable1=true;
      this.compTable2=false;
      this.compTable3 = false;
  }
  showComponentTable2() {
      this.compTable = false;
      this.compTable1=false;
      this.compTable2=true;
      this.compTable3 = false;
  }
  showComponentTable3() {
    this.compTable=false;
    this.compTable1=false;
    this.compTable2 = false;
    this.compTable3 = true;
}

showComponentGraph() {
  this.compGraph = true;
  this.compGraph1=false;
  this.compGraph2=false;
  this.compGraph3 = false;
}
showComponentGraph1() {
    this.compGraph = false;
    this.compGraph1=true;
    this.compGraph2=false;
    this.compGraph3 = false;
}
showComponentGraph2() {
    this.compGraph = false;
    this.compGraph1=false;
    this.compGraph2=true;
    this.compGraph3 = false;
}
showComponentGraph3() {
  this.compGraph=false;
  this.compGraph1=false;
  this.compGraph2 = false;
  this.compGraph3 = true;
}

showGraph(){
  this.graph = true;
  this.tabelar = false;
}
showTable(){
  this.graph = false;
  this.tabelar = true;
}
}
