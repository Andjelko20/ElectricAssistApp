import {  Component, ElementRef, ViewChildren, QueryList, ViewChild, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

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
  constructor(private elementRef: ElementRef, private authService:AuthService,private route:ActivatedRoute) {}
  ngOnInit(): void {
    this.isContentVisible1=true;
    this.isContentVisible2=false;
    this.isContentVisible3 = false;
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

  graph:boolean = true;
  tabelar:boolean = false;

  graph1:boolean = true;
  tabelar1:boolean = false;

  compGraph = true;
  compGraph1 = false;
  compGraph2 = false;
  compGraph3 = false;

  compGraphh = true;
  compGraph11 = false;
  compGraph22 = false;
  compGraph33 = false;

  compTable = true;
  compTable1 = false;
  compTable2 = false;
  compTable3 = false;

  compTablee = true;
  compTable11 = false;
  compTable22 = false;
  compTable33 = false;

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

  showProducingGrafTablee(){
    this.compTablee = true;
    this.compTable11=false;
    this.compTable22=false;
    this.compTable33 = false;
  }
  showProducingGrafTablee1(){
    this.compTablee = false;
    this.compTable11=true;
    this.compTable22=false;
    this.compTable33 = false;
  }
  showProducingGrafTablee2(){
    this.compTablee = false;
    this.compTable11=false;
    this.compTable22=true;
    this.compTable33 = false;
  }
  showProducingGrafTablee3(){
    this.compTablee = false;
    this.compTable11=false;
    this.compTable22=false;
    this.compTable33 = true;
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

showComponentGraphh() {
  this.compGraphh = true;
  this.compGraph11=false;
  this.compGraph22=false;
  this.compGraph33 = false;
}
showComponentGraph11() {
    this.compGraphh = false;
    this.compGraph11=true;
    this.compGraph22=false;
    this.compGraph33 = false;
}
showComponentGraph22() {
    this.compGraphh = false;
    this.compGraph11=false;
    this.compGraph22=true;
    this.compGraph33 = false;
}
showComponentGraph33() {
  this.compGraphh=false;
  this.compGraph11=false;
  this.compGraph22 = false;
  this.compGraph33 = true;
}

showGraph(){
  this.graph = true;
  this.tabelar = false;
}
showGraph1(){
  this.graph1 = true;
  this.tabelar1 = false;
}
showTable(){
  this.graph = false;
  this.tabelar = true;
}
showTable1(){
  this.graph1 = false;
  this.tabelar1 = true;
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
