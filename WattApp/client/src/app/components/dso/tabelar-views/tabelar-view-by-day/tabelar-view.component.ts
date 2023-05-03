import { Component, OnInit } from '@angular/core';
import { DayByHour } from 'src/app/models/devices.model';
import { Settlement } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';


@Component({
  selector: 'app-tabelar-view',
  templateUrl: './tabelar-view.component.html',
  styleUrls: ['./tabelar-view.component.css']
})
export class TabelarViewComponent implements OnInit{

  list1:DayByHour[] = [];
  list2:DayByHour[] = [];
  settlements:Settlement[] = [];
  selectedOption: number = 0;

  onOptionSelected() {
    //console.log("List1 ="+this.list1);
    //console.log("List2 ="+this.list2);

    this.ngOnInit();
  }
  constructor(private authService:AuthService,private deviceService:HistoryPredictionService) {}
  ngOnInit(): void {
    this.authService.getlogInUser().subscribe(user=>{
      this.authService.getCityId(user.city).subscribe(number=>{
        this.authService.getSettlement(number).subscribe((settlement:Settlement[])=>{
          this.settlements = settlement;
        })
        if(this.selectedOption == 0){
          this.deviceService.dayByHour(number,2).subscribe((data: DayByHour[]) =>{
            this.list1 = data;
            this.deviceService.dayByHour(number,1).subscribe((data: DayByHour[]) =>{
              this.list2 = data;
            })
      
          })
        }
        else{
          this.deviceService.dayByHourSettlement(this.selectedOption,2).subscribe((data: DayByHour[]) =>{
            this.list1 = data;
            this.deviceService.dayByHourSettlement(this.selectedOption,1).subscribe((data: DayByHour[]) =>{
              this.list2 = data;
            })
      
          })
        }
        
      })
    })
  }
}
