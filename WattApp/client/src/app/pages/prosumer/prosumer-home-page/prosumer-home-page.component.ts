import { Component, OnInit } from '@angular/core';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { JwtToken } from 'src/app/utilities/jwt-token';

@Component({
  selector: 'app-prosumer-home-page',
  templateUrl: './prosumer-home-page.component.html',
  styleUrls: ['./prosumer-home-page.component.css']
})
export class ProsumerHomePageComponent implements OnInit {
  
  constructor(private historyService:HistoryPredictionService){}

  id?:number
  currentConsumption! : number;
  todayConsumption!: number;
  monthConsumption!: number;
  todayProduction!:number;
  ngOnInit(): void {
    let token=new JwtToken();
    this.id=token.data.id as number;
    console.log(this.id)
    this.historyService.currentUserProductionConsumption(this.id,2).subscribe(data =>{
      this.currentConsumption = data;
    })
    this.historyService.todayConsumptionUser(this.id,2).subscribe(data=>{
      this.todayConsumption = data;
    })
    this.historyService.todayConsumptionUser(this.id,1).subscribe(data=>{
      this.todayProduction = data;
    })
    this.historyService.monthConsumptionUser(this.id,2).subscribe(data=>{
      this.monthConsumption = data;
    })

  }

}
