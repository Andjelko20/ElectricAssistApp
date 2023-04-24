import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { JwtToken } from 'src/app/utilities/jwt-token';
import { Prosumers } from 'src/app/models/users.model'
import { ActivatedRoute } from '@angular/router';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { first } from 'rxjs';

@Component({
  selector: 'app-one-prosumer',
  templateUrl: './one-prosumer.component.html',
  styleUrls: ['./one-prosumer.component.css']
})
export class OneProsumerComponent implements OnInit{

  id?:number;
  dso!: Prosumers;
  avg!: number;
  constructor(private authService:AuthService,private route:ActivatedRoute,private historyService:HistoryPredictionService) {
    
  }
  async ngOnInit(){
    let token=new JwtToken();
    this.id=token.data.id as number;

    this.authService.getProsumer(Number(this.route.snapshot.paramMap.get('id'))).subscribe(user=>{
      this.dso = user;
      console.log(this.dso);
      this.historyService.avgDayUser(Number(this.route.snapshot.paramMap.get('id')),2).subscribe(data=>{
        this.avg = data;
      })
    })

    

  }
}
