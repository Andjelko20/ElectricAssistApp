import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
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

          this.deviceService.monthByDayUser(Number(this.route.snapshot.paramMap.get('id')),2).subscribe((data:WeekByDay[])=>{
            this.list1 = data;
            this.deviceService.monthByDayUser(Number(this.route.snapshot.paramMap.get('id')),1).subscribe((data:WeekByDay[])=>{
              this.list2 = data;
            })
          })
  }

}