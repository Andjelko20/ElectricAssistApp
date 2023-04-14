import { Component } from '@angular/core';

@Component({
  selector: 'app-dso-home-page',
  templateUrl: './dso-home-page.component.html',
  styleUrls: ['./dso-home-page.component.css']
})
export class DsoHomePageComponent {

  graph:boolean = true;
  tabelar:boolean = false;


  componentShown = true;
  componentShown1 = false;
  componentShown2 = false;
  componentShown3 = false;
  showComponent() {
      this.componentShown = true;
  
    
    this.componentShown1=false;
    this.componentShown2=false;
    this.componentShown3 = false;
  }
  showComponent1() {
      this.componentShown1 = true;

      this.componentShown=false;
      this.componentShown2=false;
      this.componentShown3 = false;
  }
  showComponent2() {
      this.componentShown2 = true;

      this.componentShown=false;
      this.componentShown1=false;
      this.componentShown3 = false;
  }
  showComponent3() {
    this.componentShown3 = true;
    
    this.componentShown=false;
    this.componentShown1=false;
    this.componentShown2 = false;
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
