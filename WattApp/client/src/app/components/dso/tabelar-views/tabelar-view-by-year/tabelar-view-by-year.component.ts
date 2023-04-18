import { Component, OnInit } from '@angular/core';
import { YearsByMonth } from 'src/app/models/devices.model';
import { DevicesService } from 'src/app/services/devices.service';

@Component({
  selector: 'app-tabelar-view-by-year',
  templateUrl: './tabelar-view-by-year.component.html',
  styleUrls: ['./tabelar-view-by-year.component.css']
})
export class TabelarViewByYearComponent implements OnInit{
  list1:YearsByMonth[]=[];
  list2:YearsByMonth[]=[];
  itemList: string[] = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Avg','Sep','Okt','Nov','Dec'];
  constructor(private deviceService:DevicesService) {
    
  }

  ngOnInit(): void {
    this.deviceService.yearByMonth(2,2).subscribe((data:YearsByMonth[])=>{
      console.log("Data => ", data);
      this.list1 = data;
      this.deviceService.yearByMonth(2,1).subscribe((data:YearsByMonth[])=>{
        console.log("Data => ", data);
        this.list2 = data;
      })
    })
  }
}
