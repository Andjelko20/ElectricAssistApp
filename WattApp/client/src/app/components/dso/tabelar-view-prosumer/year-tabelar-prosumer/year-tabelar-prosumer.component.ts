import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { switchMap } from 'rxjs';
import { YearsByMonth } from 'src/app/models/devices.model';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';

@Component({
  selector: 'app-year-tabelar-prosumer',
  templateUrl: './year-tabelar-prosumer.component.html',
  styleUrls: ['./year-tabelar-prosumer.component.css']
})
export class YearTabelarProsumerComponent {

  list1:YearsByMonth[]=[];
  list2:YearsByMonth[]=[];
  constructor(private deviceService:HistoryPredictionService,private route:ActivatedRoute) {
    
  }
  ngOnInit(): void {
    const userId = Number(this.route.snapshot.paramMap.get('id'));
  
    this.deviceService.yearByMonthUser(userId, 2).pipe(
      switchMap((data1: YearsByMonth[]) => {
        this.list1 = data1;
        return this.deviceService.yearByMonthUser(userId, 1);
      })
    ).subscribe((data2: YearsByMonth[]) => {
      console.log("Data => ", data2);
      this.list2 = data2;
    });
  }
}
