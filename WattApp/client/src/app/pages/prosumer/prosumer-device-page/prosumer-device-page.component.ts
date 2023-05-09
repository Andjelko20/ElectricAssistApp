import {  Component, ElementRef, ViewChildren, QueryList, ViewChild, OnInit } from '@angular/core';

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
  constructor(private elementRef: ElementRef) {}
  ngOnInit(): void {
    this.isContentVisible1=true;
    this.isContentVisible2=false;
    this.isContentVisible3 = false;
  }

  ngAfterViewInit() {
    this.collapsibleButtons.forEach(button => {
      button.nativeElement.addEventListener('click', () => {
        button.nativeElement.classList.toggle('active');
        const content = button.nativeElement.nextElementSibling;
        if (content.style.display === 'block') {
          content.style.display = 'none';
        } else {
          content.style.display = 'block';
        }
      });
    });
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
