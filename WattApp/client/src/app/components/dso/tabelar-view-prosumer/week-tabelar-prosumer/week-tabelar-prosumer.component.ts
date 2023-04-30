import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { forkJoin } from 'rxjs';
import { WeekByDay } from 'src/app/models/devices.model';
import { AuthService } from 'src/app/services/auth.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';

@Component({
  selector: 'app-week-tabelar-prosumer',
  templateUrl: './week-tabelar-prosumer.component.html',
  styleUrls: ['./week-tabelar-prosumer.component.css']
})
export class WeekTabelarProsumerComponent implements OnInit{
  list1:WeekByDay[] = [];
  list2:WeekByDay[] = [];
  constructor(private deviceService:HistoryPredictionService,private authService:AuthService,private route:ActivatedRoute) {}
  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
  
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
