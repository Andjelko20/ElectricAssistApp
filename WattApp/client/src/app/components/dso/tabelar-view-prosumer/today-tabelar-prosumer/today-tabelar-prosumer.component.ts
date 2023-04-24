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
  settlements:Settlement[] = [];
  selectedOption: number = 0;

  onOptionSelected() {
    console.log("List1 ="+this.list1);
    console.log("List2 ="+this.list2);

    this.ngOnInit();
  }
  constructor(private route:ActivatedRoute,private deviceService:HistoryPredictionService) {}
  ngOnInit(): void {
      
          this.deviceService.dayByHourUser(Number(this.route.snapshot.paramMap.get('id')),2).subscribe((data: DayByHour[]) =>{
            this.list1 = data.map(value=>{
              value.day = value.day.toString().padStart(2,'0');
              value.hour = value.hour.toString().padStart(2,'0');

              return value;
            });
            this.deviceService.dayByHourUser(Number(this.route.snapshot.paramMap.get('id')),1).subscribe((data: DayByHour[]) =>{
              this.list2 = data;
            })
          })
        
       
    }
  }

