import { AfterViewInit, Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Popover, Tooltip } from 'bootstrap';
import { AuthService } from 'src/app/services/auth.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { JwtToken } from 'src/app/utilities/jwt-token';
import { environment } from 'src/environments/environment';

declare var $: any;
@Component({
  selector: 'app-dso-home-page',
  templateUrl: './dso-home-page.component.html',
  styleUrls: ['./dso-home-page.component.css'],

})
export class DsoHomePageComponent implements AfterViewInit, OnInit,OnDestroy{
  loader:boolean=false;
  tooltip: Tooltip | undefined;
  numberOfProsumers=0;
  avgProduction?:number;
  totalProduction?:number;
  idUser!:number;
  role!:string;
  city! : string;
  private tooltips: Tooltip[] = [];
  constructor(private avgConsumption:HistoryPredictionService,private route:ActivatedRoute,private router:Router,private updateService:AuthService){

  }
  ngAfterViewInit() {
    const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
    const tooltipList = Array.from(tooltipTriggerList).map((tooltipTriggerEl) => {
      return new Tooltip(tooltipTriggerEl);
    });
    this.tooltips = tooltipList;
    
  }
  ngOnDestroy(): void {
    this.tooltips.forEach((tooltip) => {
      tooltip.dispose();
    });
    this.tooltips = [];
    this.tooltip = undefined;
  }
  async ngOnInit(): Promise<void> {
    this.loader=true;
    const { id, role } = new JwtToken().data;
    this.idUser = id as number;
    this.role = role as string;
     this.updateService.getlogInUser().subscribe(user=>{
      this.updateService.getCityId(user.city).subscribe(city=>{
        this.city = user.city;
        this.avgConsumption.getAverageConsumptionProductionCity(1,city).subscribe(result=>{
          this.loader=false;
          this.avgProduction = result;
        })
        this.avgConsumption.getCurrentConsumptionProductionCity(1, city).subscribe(result=>{
          this.loader=false;
            this.totalProduction = result;
          })
      })
     })
    const res = await fetch(environment.serverUrl + "/api/ProsumersDetails/count", {
      headers: { Authorization: "Bearer " + localStorage.getItem("token") }
    }).then(res => res.json());
    this.numberOfProsumers = res;
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
