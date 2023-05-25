import { Component, OnInit } from '@angular/core';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { SessionService } from 'src/app/services/session.service';
import { JwtToken } from 'src/app/utilities/jwt-token';

@Component({
  selector: 'app-prosumer-home-page',
  templateUrl: './prosumer-home-page.component.html',
  styleUrls: ['./prosumer-home-page.component.css']
})
export class ProsumerHomePageComponent implements OnInit {
  
  constructor(private historyService:HistoryPredictionService, private filterSession : SessionService){}
  meter!:string;
  meter1!:string;
  meter2!:string;
  meter3!:string;

  id?:number
  currentConsumption! : number;
  todayConsumption!: number;
  monthConsumption!: number;
  todayProduction!:number;
  ngOnInit(): void {
    let token=new JwtToken();
    this.filterSession.setSession("filter", null);
    console.log(this.filterSession.getSession("filter"));
    this.id=token.data.id as number;
    this.historyService.currentUserProductionConsumption(this.id,2).subscribe(data =>{
      this.currentConsumption = data;
      this.meter=" kWh";
      if(this.currentConsumption>999.99)
      {
      this.currentConsumption=parseFloat((this.currentConsumption*0.001).toFixed(2));
        this.meter=" MWh";
        if(this.currentConsumption>999.99)
        {
          this.currentConsumption=parseFloat((this.currentConsumption*0.001).toFixed(2));
          this.meter=" GWh";
        }
      }
    })
    this.historyService.todayConsumptionUser(this.id,2).subscribe(data=>{
      this.todayConsumption = data;
      this.meter1=" kWh";
      if(this.todayConsumption>999.99)
      {
      this.todayConsumption=parseFloat((this.todayConsumption*0.001).toFixed(2));
        this.meter1=" MWh";
        if(this.todayConsumption>999.99)
        {
          this.todayConsumption=parseFloat((this.todayConsumption*0.001).toFixed(2));
          this.meter1=" GWh";
        }
      }
    })
    this.historyService.todayConsumptionUser(this.id,1).subscribe(data=>{
      this.todayProduction = data;
      this.meter2=" kWh";
      if(this.todayProduction>999.99)
      {
      this.todayProduction=parseFloat((this.todayProduction*0.001).toFixed(2));
        this.meter2=" MWh";
        if(this.todayProduction>999.99)
        {
          this.todayProduction=parseFloat((this.todayProduction*0.001).toFixed(2));
          this.meter2=" GWh";
        }
      }
    })
    this.historyService.monthConsumptionUser(this.id,2).subscribe(data=>{
      this.monthConsumption = data;
      this.meter3="kWh";
      if(this.monthConsumption>999.99)
      {
      this.monthConsumption=parseFloat((this.monthConsumption*0.001).toFixed(2));
        this.meter3=" MWh";
        if(this.monthConsumption>999.99)
        {
          this.monthConsumption=parseFloat((this.monthConsumption*0.001).toFixed(2));
          this.meter3=" GWh";
        }
      }
    })

  }

}
