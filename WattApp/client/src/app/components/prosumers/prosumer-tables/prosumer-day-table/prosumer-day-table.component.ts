import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { forkJoin } from 'rxjs';
import { DayByHour } from 'src/app/models/devices.model';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { JwtToken } from 'src/app/utilities/jwt-token';

@Component({
  selector: 'app-prosumer-day-table',
  templateUrl: './prosumer-day-table.component.html',
  styleUrls: ['./prosumer-day-table.component.css']
})
export class ProsumerDayTableComponent {
 
  list1:DayByHour[] = [];
  list2:DayByHour[] = [];
  constructor(private route:ActivatedRoute,private deviceService:HistoryPredictionService) {}
  
  ngOnInit(): void {
    let token=new JwtToken();
    const id = token.data.id as number;
  
    forkJoin([
      this.deviceService.dayByHourUser(id, 2),
      this.deviceService.dayByHourUser(id, 1)
    ]).subscribe(([list1, list2]) => {
      this.list1 = list1;
      this.list2 = list2;
    });
  }
}
