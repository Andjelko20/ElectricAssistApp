import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { switchMap } from 'rxjs';
import { YearsByMonth } from 'src/app/models/devices.model';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { JwtToken } from 'src/app/utilities/jwt-token';

@Component({
  selector: 'app-prosumer-year-table',
  templateUrl: './prosumer-year-table.component.html',
  styleUrls: ['./prosumer-year-table.component.css']
})
export class ProsumerYearTableComponent {

  list1:YearsByMonth[]=[];
  list2:YearsByMonth[]=[];
  constructor(private deviceService:HistoryPredictionService,private route:ActivatedRoute) {
    
  }
  ngOnInit(): void {
    let token=new JwtToken();
    const userId = token.data.id as number;
  
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
