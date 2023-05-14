import { Component, ElementRef, ViewChildren, QueryList, ViewChild, OnInit} from '@angular/core';
import { forkJoin } from 'rxjs';
import { WeekByDay } from 'src/app/models/devices.model';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { JwtToken } from 'src/app/utilities/jwt-token';

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
  todayC:number = 0;
  todayP:number = 0;
  monthC:number = 0;
  monthP:number = 0;
  idPro!: number;
  maxc!: number;
  maxp!: number;
  meter!:string;
  meter1!:string;
  meter2!:string;
  meter3!:string;
  meter4!:string;
  constructor(private elementRef: ElementRef,private historyService:HistoryPredictionService) {}
  ngOnInit(): void {
    let token=new JwtToken();
    const id = token.data.id as number;
    this.isContentVisible = true;
    this.isContentVisible1 = false;
    this.isContentVisible2 = false;
    this.isContentVisible3 = true;
    this.isContentVisible4 = false;
    forkJoin([
      this.historyService.todayConsumptionUser(id, 2),
      this.historyService.todayConsumptionUser(id, 1),
      this.historyService.monthConsumptionUser(id, 2),
      this.historyService.monthConsumptionUser(id, 1),
    ]).subscribe(([todayC, todayP, monthC, monthP]) => {
      this.todayC = todayC;
      this.meter1=" kWh"
      if(this.todayC>999.99)
      {
        this.todayC=parseFloat((this.todayC*0.001).toFixed(2));
        this.meter1=" MWh";
        if(this.todayC>999.99)
        {
          this.todayC=parseFloat((this.todayC*0.001).toFixed(2));
          this.meter1=" GWh";
        }
      }
      this.todayP = todayP;
      this.meter2=" kWh"
      if(this.todayP>999.99)
      {
        this.todayP=parseFloat((this.todayP*0.001).toFixed(2));
        this.meter2=" MWh";
        if(this.todayP>999.99)
        {
          this.todayP=parseFloat((this.todayP*0.001).toFixed(2));
          this.meter2=" GWh";
        }
      }
      this.monthC = monthC;
      this.meter3=" kWh"
      if(this.monthC>999.99)
      {
        this.monthC=parseFloat((this.monthC*0.001).toFixed(2));
        this.meter3=" MWh";
        if(this.monthC>999.99)
        {
          this.monthC=parseFloat((this.monthC*0.001).toFixed(2));
          this.meter3=" GWh";
        }
      }
      this.monthP = monthP;
      this.meter4=" kWh"
      if(this.monthP>999.99)
      {
        this.monthP=parseFloat((this.monthP*0.001).toFixed(2));
        this.meter4=" MWh";
        if(this.monthP>999.99)
        {
          this.monthP=parseFloat((this.monthP*0.001).toFixed(2));
          this.meter4=" GWh";
        }
      }
    });
    this.idPro=token.data.id as number;
    this.historyService.predictionUser(this.idPro,2).subscribe((data: WeekByDay[]) =>{
      this.maxc=parseFloat(data.reduce((total,row)=> total+row.energyUsageResult,0).toFixed(2));
      this.meter=" kWh"
      if(this.maxc>999.99)
      {
        this.maxc=parseFloat((this.maxc*0.001).toFixed(2));
        this.meter=" MWh"
        if(this.maxc>999.99)
        {
          this.maxc=parseFloat((this.maxc*0.001).toFixed(2));
          this.meter=" GWh"
        }
      }
      })
      this.historyService.predictionUser(this.idPro,1).subscribe((data: WeekByDay[]) =>{
        this.maxp=parseFloat(data.reduce((total,row)=> total+row.energyUsageResult,0).toFixed(2));
        this.meter=" kWh"
        if(this.maxp>999.99)
        {
          this.maxp=parseFloat((this.maxp*0.001).toFixed(2));
          this.meter=" MWh";
          if(this.maxp>999.99)
          {
            this.maxp=parseFloat((this.maxp*0.001).toFixed(2));
            this.meter=" GWh";
          }
        }
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
