import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
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
  itemList: string[] = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Avg','Sep','Okt','Nov','Dec'];
  constructor(private deviceService:HistoryPredictionService,private route:ActivatedRoute) {
    
  }
  ngOnInit(): void {
          this.deviceService.yearByMonthUser(Number(this.route.snapshot.paramMap.get('id')),2).subscribe((data:YearsByMonth[])=>{
            this.list1 = data;
            this.deviceService.yearByMonthUser(Number(this.route.snapshot.paramMap.get('id')),1).subscribe((data:YearsByMonth[])=>{
              //console.log("Data => ", data);
              this.list2 = data;
            })
          })
  }
}
