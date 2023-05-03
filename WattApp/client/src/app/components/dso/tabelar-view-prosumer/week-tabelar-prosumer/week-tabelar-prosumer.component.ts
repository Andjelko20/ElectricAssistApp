import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
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

            this.deviceService.weekByDayUser(Number(this.route.snapshot.paramMap.get('id')),2).subscribe((data: WeekByDay[]) =>{
              //console.log("Data => ", data);
              this.list1 = data;
              this.deviceService.weekByDayUser(Number(this.route.snapshot.paramMap.get('id')),1).subscribe((data: WeekByDay[]) =>{
                //console.log("Data => ", data);
                this.list2 = data;
              })
            })

      
      

      
  }
}
