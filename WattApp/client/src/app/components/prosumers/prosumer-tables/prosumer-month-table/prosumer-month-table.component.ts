import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { forkJoin } from 'rxjs';
import { WeekByDay } from 'src/app/models/devices.model';
import { AuthService } from 'src/app/services/auth.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { JwtToken } from 'src/app/utilities/jwt-token';

@Component({
  selector: 'app-prosumer-month-table',
  templateUrl: './prosumer-month-table.component.html',
  styleUrls: ['./prosumer-month-table.component.css']
})
export class ProsumerMonthTableComponent {

  list1:WeekByDay[]=[];
  list2:WeekByDay[]=[];
  constructor(private deviceService:HistoryPredictionService,private authService:AuthService,private route:ActivatedRoute){}
  ngOnInit(): void {
    let token=new JwtToken();
    const userId = token.data.id as number;
  
    forkJoin({
      list1: this.deviceService.monthByDayUser(userId, 2),
      list2: this.deviceService.monthByDayUser(userId, 1)
    }).subscribe(({ list1, list2 }) => {
      this.list1 = list1;
      this.list2 = list2;
    });
  }
}
