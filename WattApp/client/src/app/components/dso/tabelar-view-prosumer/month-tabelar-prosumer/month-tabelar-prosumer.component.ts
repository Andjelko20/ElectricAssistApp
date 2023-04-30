import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { forkJoin } from 'rxjs';
import { WeekByDay } from 'src/app/models/devices.model';
import { AuthService } from 'src/app/services/auth.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';

@Component({
  selector: 'app-month-tabelar-prosumer',
  templateUrl: './month-tabelar-prosumer.component.html',
  styleUrls: ['./month-tabelar-prosumer.component.css']
})
export class MonthTabelarProsumerComponent implements OnInit{

  list1:WeekByDay[]=[];
  list2:WeekByDay[]=[];
  constructor(private deviceService:HistoryPredictionService,private authService:AuthService,private route:ActivatedRoute){}
  ngOnInit(): void {
    const userId = Number(this.route.snapshot.paramMap.get('id'));
  
    forkJoin({
      list1: this.deviceService.monthByDayUser(userId, 2),
      list2: this.deviceService.monthByDayUser(userId, 1)
    }).subscribe(({ list1, list2 }) => {
      this.list1 = list1;
      this.list2 = list2;
    });
  }

}