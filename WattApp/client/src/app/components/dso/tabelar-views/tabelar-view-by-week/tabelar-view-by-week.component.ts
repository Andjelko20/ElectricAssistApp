import { Component, OnInit } from '@angular/core';
import { WeekByDay } from 'src/app/models/devices.model';
import { Settlement } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { DevicesService } from 'src/app/services/devices.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';

@Component({
  selector: 'app-tabelar-view-by-week',
  templateUrl: './tabelar-view-by-week.component.html',
  styleUrls: ['./tabelar-view-by-week.component.css']
})
export class TabelarViewByWeekComponent implements OnInit {
  list1:WeekByDay[] = [];
  list2:WeekByDay[] = [];
  settlements:Settlement[] = [];
  constructor(private deviceService:HistoryPredictionService,private authService:AuthService) {}
  selectedOption: number = 0;

  onOptionSelected() {
    this.ngOnInit();
  }
    ngOnInit(): void {

      console.log("Selektovano je "+this.selectedOption);
      this.authService.getlogInUser().subscribe(user=>{
        this.authService.getCityId(user.city).subscribe(number=>{
          this.authService.getSettlement(number).subscribe((settlement:Settlement[])=>{
            this.settlements = settlement;
          })
          if(this.selectedOption == 0){
            this.deviceService.weekByDay(number,2).subscribe((data: WeekByDay[]) =>{
              this.list1 = data;
              this.deviceService.weekByDay(number,1).subscribe((data: WeekByDay[]) =>{
                this.list2 = data;
              })
        
            })
          }
          else{
            this.deviceService.weekByDaySettlement(this.selectedOption,2).subscribe((data: WeekByDay[]) =>{
              this.list1 = data;
              this.deviceService.weekByDaySettlement(this.selectedOption,1).subscribe((data: WeekByDay[]) =>{
                this.list2 = data;
              })
            })
          }
          
        })
      })

      
      

      
  }
}