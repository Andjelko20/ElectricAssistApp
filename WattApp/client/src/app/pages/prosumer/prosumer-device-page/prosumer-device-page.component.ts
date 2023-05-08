import {  Component, ElementRef, ViewChildren, QueryList, ViewChild } from '@angular/core';

@Component({
  selector: 'app-prosumer-device-page',
  templateUrl: './prosumer-device-page.component.html',
  styleUrls: ['./prosumer-device-page.component.css']
})
export class ProsumerDevicePageComponent {
  @ViewChildren('collapsibleButton') collapsibleButtons!: QueryList<ElementRef>;

  constructor(private elementRef: ElementRef) {}

 
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

}
