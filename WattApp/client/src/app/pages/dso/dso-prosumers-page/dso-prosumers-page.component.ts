import { Component } from '@angular/core';

@Component({
  selector: 'app-dso-prosumers-page',
  templateUrl: './dso-prosumers-page.component.html',
  styleUrls: ['./dso-prosumers-page.component.css']
})
export class DsoProsumersPageComponent {

  componentShown = true;
  componentShown1 = false;

  showComponent() {
    this.componentShown = true;
  
    
    this.componentShown1=false;
  }
  showComponent1() {
      this.componentShown1 = true;

      this.componentShown=false;
  }
}
