import { AfterViewInit, Component, OnInit } from '@angular/core';
import { Popover, Tooltip } from 'bootstrap';
import { environment } from 'src/environments/environment';

declare var $: any;
@Component({
  selector: 'app-dso-home-page',
  templateUrl: './dso-home-page.component.html',
  styleUrls: ['./dso-home-page.component.css'],

})
export class DsoHomePageComponent implements AfterViewInit{
  popover: Popover | undefined;
  tooltip: Tooltip | undefined;
  
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
}
