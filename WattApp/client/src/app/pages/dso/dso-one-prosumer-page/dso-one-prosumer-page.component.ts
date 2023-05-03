import { AfterViewInit, Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { JwtToken } from 'src/app/utilities/jwt-token';
import { Prosumers } from 'src/app/models/users.model'
import { ActivatedRoute } from '@angular/router';
import { ShowDevices } from 'src/app/models/devices.model';
import { DevicesService } from 'src/app/services/devices.service';
import { Popover, Tooltip } from 'bootstrap';


@Component({
  selector: 'app-dso-one-prosumer-page',
  templateUrl: './dso-one-prosumer-page.component.html',
  styleUrls: ['./dso-one-prosumer-page.component.css']
})
export class DsoOneProsumerPageComponent implements AfterViewInit,OnInit{

  name!:string;
  popover: Popover | undefined;
  tooltip: Tooltip | undefined;
  currentTime!: Date;
  dashboard:boolean = true;
  devices:boolean = false;
  devicesList:ShowDevices[] = []
  deviceCategoryId!: number;
  constructor(private authService:AuthService,private deviceService:DevicesService,private route:ActivatedRoute) {  }

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
  ngOnInit(): void {
    this.updateTime();
    setInterval(() => {
      this.updateTime();
    }, 1000);

    this.authService.getProsumer(Number(this.route.snapshot.paramMap.get('id'))).subscribe(user=>{
        this.name=user.name;
    })

  }

  toggleD()
  {
    this.dashboard = false;
    this.devices = true;
  }
  toggleDa(){
    this.dashboard = true;
    this.devices = false;
  }

  updateTime() {
    this.currentTime = new Date();
  }
  onSelectedCategory(event:any){

this.deviceCategoryId = event.target.value;
this.deviceService.getDeviceProsumer(Number(this.route.snapshot.paramMap.get('id')),1,12,this.deviceCategoryId).subscribe(devices => {
  this.devicesList=devices.data.map((u:any)=>({
    id:u.id,
    userId: u.userId,
    deviceCategory:u.deviceCategory,
    deviceType: u.deviceType ,
    deviceBrand: u.deviceBrand ,
    deviceModel: u.deviceModel ,
    name: u.name ,
    energyInKwh: u.energyInKwh,
    standByKwh: u.standByKwh,
    visibility: u.visibility,
    controlability: u.controlability,
    turnOn: u.turnOn,

})as ShowDevices)

}, (error: { status: number; }) => {

if (error.status === 404) {

  this.devicesList=[]

  console.log('Devices not found in database');

}}

);
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
