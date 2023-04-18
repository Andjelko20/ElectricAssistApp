import { Component, OnInit } from '@angular/core';
import { WeekByDay } from 'src/app/models/devices.model';
import { DevicesService } from 'src/app/services/devices.service';

@Component({
  selector: 'app-tabelar-view-by-month',
  templateUrl: './tabelar-view-by-month.component.html',
  styleUrls: ['./tabelar-view-by-month.component.css']
})
export class TabelarViewByMonthComponent implements OnInit{
  list1:WeekByDay[]=[];
  list2:WeekByDay[]=[];
  constructor(private deviceService:DevicesService){}

  ngOnInit(): void {
    this.deviceService.monthByDay(2,2).subscribe((data:WeekByDay[])=>{
      console.log("Data => ", data);
      this.list1 = data;
      this.deviceService.monthByDay(2,1).subscribe((data:WeekByDay[])=>{
        console.log("Data => ", data);
        this.list2 = data;
        
      })
    })
  }

  

}
