import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { JwtToken } from 'src/app/utilities/jwt-token';
import { Prosumers } from 'src/app/models/users.model'
import { ActivatedRoute } from '@angular/router';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { first, forkJoin, switchMap } from 'rxjs';

@Component({
  selector: 'app-one-prosumer',
  templateUrl: './one-prosumer.component.html',
  styleUrls: ['./one-prosumer.component.css']
})
export class OneProsumerComponent implements OnInit{

  prosumer!: Prosumers;
  cC!: number;
  cP!:number;
  constructor(private authService:AuthService,private route:ActivatedRoute,private historyService:HistoryPredictionService) {

  }
  ngOnInit() {
    const prosumerId = Number(this.route.snapshot.paramMap.get('id'));
  
    this.authService.getProsumer(prosumerId).pipe(
      switchMap(user => {
        this.prosumer = user;
        const productionObs = this.historyService.currentUserProductionConsumption(prosumerId, 1);
        const consumptionObs = this.historyService.currentUserProductionConsumption(prosumerId, 2);
        return forkJoin([productionObs, consumptionObs]);
      })
    ).subscribe(([productionData, consumptionData]) => {
      this.cC = consumptionData;
      this.cP = productionData;
    });
  }
}