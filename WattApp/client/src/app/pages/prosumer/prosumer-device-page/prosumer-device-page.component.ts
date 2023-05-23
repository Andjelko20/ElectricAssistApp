import {  Component, ElementRef, ViewChildren, QueryList, ViewChild, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, NavigationStart, Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { DevicesService } from 'src/app/services/devices.service';
import { Location } from '@angular/common';
@Component({
  selector: 'app-prosumer-device-page',
  templateUrl: './prosumer-device-page.component.html',
  styleUrls: ['./prosumer-device-page.component.css']
})
export class ProsumerDevicePageComponent implements OnInit{
  @ViewChildren('collapsibleButton') collapsibleButtons!: QueryList<ElementRef>;
  
  isContentVisible1 = false;
  isContentVisible2 = false;
  isContentVisible3 = false;

  consm:boolean=false;
  prodc:boolean=false;
  previousUrl:string="";
  constructor(private elementRef: ElementRef, private authService:AuthService,private route:ActivatedRoute,private router:Router,
    private device:DevicesService) {
    
    }
  ngOnInit(): void {
    this.isContentVisible1=true;
    this.isContentVisible2=true;
    this.isContentVisible3 = true;
    const deviceId = Number(this.route.snapshot.paramMap.get('id'));
    this.authService.getDevice(deviceId).subscribe(data=>{
      if(data.deviceCategory == "Electricity Consumer")
        {
          this.consm=true;

        }
        else{
          this.prodc=true;
        }
    })
    
  }
  goBack()
  {
    this.router.navigateByUrl(this.device.getBack())
  }
  graph:boolean = true;
  tabelar:boolean = false;

  graph1:boolean = true;
  tabelar1:boolean = false;

  compGraph = true;
  compGraph1 = false;
  compGraph2 = false;
  compGraph3 = false;

  compTable = true;
  compTable1 = false;

showComponentTable() {
  this.compTable = true;
  this.compTable1=false;
}
showComponentTable1() {
  this.compTable = false;
  this.compTable1=true;
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
onClick()
  {
    const contentDiv = document.querySelector(".content1") as HTMLDivElement;
    this.isContentVisible1 = !this.isContentVisible1;
    if (this.isContentVisible1) {
    contentDiv.style.display = 'block';
    } else {
    contentDiv.style.display = 'none';
    }
 }
 onClick1()
 {
    const contentDiv = document.querySelector(".content2") as HTMLDivElement;
    this.isContentVisible2 = !this.isContentVisible2;
    if (this.isContentVisible2) {
      contentDiv.style.display = 'block';
      } else {
      contentDiv.style.display = 'none';
    }
}

onClick2()
{
    const contentDiv = document.querySelector(".content3") as HTMLDivElement;
    this.isContentVisible3 = !this.isContentVisible3;
    if (this.isContentVisible3) {
      contentDiv.style.display = 'block';
    } else {
      contentDiv.style.display = 'none';
    }
}
}
