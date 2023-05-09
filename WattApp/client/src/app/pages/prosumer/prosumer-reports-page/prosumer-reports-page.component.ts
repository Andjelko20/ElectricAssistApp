import { Component, ElementRef, ViewChildren, QueryList, ViewChild, OnInit} from '@angular/core';

@Component({
  selector: 'app-prosumer-reports-page',
  templateUrl: './prosumer-reports-page.component.html',
  styleUrls: ['./prosumer-reports-page.component.css']
})
export class ProsumerReportsPageComponent implements OnInit {
  
  isContentVisible = false;
  isContentVisible1 = false;
  isContentVisible2 = false;
  isContentVisible3 = false;
  isContentVisible4 = false;
  dashboard:boolean = true;
  devices:boolean = false;
  day:boolean = true;
  week:boolean = false;
  month:boolean = false;
  year:boolean = false;
  dayTable:boolean = true;
  weekTable:boolean = false;
  monthTable:boolean = false;
  yearTable:boolean = false;
  constructor(private elementRef: ElementRef) {}
  ngOnInit(): void {
    this.isContentVisible = true;
    this.isContentVisible1 = false;
    this.isContentVisible2 = false;
    this.isContentVisible3 = true;
    this.isContentVisible4 = false;

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
  toogleDay()
  {
    this.day = true;
    this.week = false;
    this.month = false;
    this.year = false;
  }
  toogleWeek()
  {
    this.day = false;
    this.week = true;
    this.month = false;
    this.year = false;
  }
  toogleMonth()
  {
    this.day = false;
    this.week = false;
    this.month = true;
    this.year = false;
  }
  toogleYear()
  {
    this.day = false;
    this.week = false;
    this.month = false;
    this.year = true;
  }
  toogledayTable()
  {
    this.dayTable = true;
    this.weekTable = false;
    this.monthTable = false;
    this.yearTable = false;

  }
  toogleweekTable()
  {
    this.dayTable=false;
    this.weekTable=true;
    this.monthTable = false;
    this.yearTable = false;
  }
  tooglemonthTable()
  {
    this.dayTable=false;
    this.weekTable=false;
    this.monthTable=true;
    this.yearTable = false;
  }
  toogleyearTable()
  {
    this.dayTable=false;
    this.weekTable=false;
    this.monthTable=false;
    this.yearTable = true;
  }
  onClick()
  {
   const contentDiv = document.querySelector(".content") as HTMLDivElement;
   this.isContentVisible = !this.isContentVisible;
  if (this.isContentVisible) {
   contentDiv.style.display = 'block';
   } else {
   contentDiv.style.display = 'none';
  }
 }
  onClickProd()
  {
    const contentDiv = document.querySelector(".content2") as HTMLDivElement;
 this.isContentVisible1 = !this.isContentVisible1;
   if (this.isContentVisible1) {
 contentDiv.style.display = 'block';
 } else {
 contentDiv.style.display = 'none';
 }
 }
  onClickTable()
  {
    const contentDiv = document.querySelector(".content1") as HTMLDivElement;
 this.isContentVisible2 = !this.isContentVisible2;
 if (this.isContentVisible2) {
 contentDiv.style.display = 'block';
  } else {
 contentDiv.style.display = 'none';
 }
   }
  onClickPredGraph()
  {
    const contentDiv = document.querySelector(".content3") as HTMLDivElement;
    this.isContentVisible3 = !this.isContentVisible3;
    if (this.isContentVisible3) {
    contentDiv.style.display = 'block';
  } else {
      contentDiv.style.display = 'none';
  }
 }
  onClickPredTable()
  {
  const contentDiv = document.querySelector(".content4") as HTMLDivElement;
  this.isContentVisible4 = !this.isContentVisible4;
 if (this.isContentVisible4) {
  contentDiv.style.display = 'block';
 } else {
 contentDiv.style.display = 'none';
 }
   }
}
