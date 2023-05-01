import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { forkJoin } from 'rxjs';
import { WeekByDay } from 'src/app/models/devices.model';
import { AuthService } from 'src/app/services/auth.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { JwtToken } from 'src/app/utilities/jwt-token';

@Component({
  selector: 'app-prosumer-week-table',
  templateUrl: './prosumer-week-table.component.html',
  styleUrls: ['./prosumer-week-table.component.css']
})
export class ProsumerWeekTableComponent {
  list1:WeekByDay[] = [];
  list2:WeekByDay[] = [];
  constructor(private deviceService:HistoryPredictionService,private authService:AuthService,private route:ActivatedRoute) {}
  ngOnInit(): void {
    let token=new JwtToken();
    const id = token.data.id as number;
  
    forkJoin([
      this.deviceService.weekByDayUser(id, 2),
      this.deviceService.weekByDayUser(id, 1)
    ]).subscribe(([list1, list2]) => {
      console.log('List 1 =>', list1);
      console.log('List 2 =>', list2);
      this.list1 = list1;
      this.list2 = list2;
    });
  }
}
