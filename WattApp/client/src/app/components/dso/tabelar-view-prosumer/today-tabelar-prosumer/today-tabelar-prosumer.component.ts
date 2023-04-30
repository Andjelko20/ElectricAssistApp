import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { DayByHour } from 'src/app/models/devices.model';
import { Settlement } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';

@Component({
  selector: 'app-today-tabelar-prosumer',
  templateUrl: './today-tabelar-prosumer.component.html',
  styleUrls: ['./today-tabelar-prosumer.component.css']
})
export class TodayTabelarProsumerComponent implements OnInit{
  
  list1:DayByHour[] = [];
  list2:DayByHour[] = [];
  constructor(private route:ActivatedRoute,private deviceService:HistoryPredictionService) {}
  
  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    const padNumber = (num: number) => num.toString().padStart(2, '0');
  
    this.deviceService.dayByHourUser(id, 2).subscribe((data: DayByHour[]) => {
      this.list1 = data.map(value => ({
        ...value,
        day: padNumber(value.day),
        hour: padNumber(value.hour)
      }));
    });
  
    this.deviceService.dayByHourUser(id, 1).subscribe((data: DayByHour[]) => {
      this.list2 = data;
    });
  }
  }

