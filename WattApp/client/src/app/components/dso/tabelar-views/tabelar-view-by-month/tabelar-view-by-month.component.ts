import { Component, OnInit } from '@angular/core';
import { WeekByDay } from 'src/app/models/devices.model';
import { Settlement } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { DevicesService } from 'src/app/services/devices.service';

@Component({
  selector: 'app-tabelar-view-by-month',
  templateUrl: './tabelar-view-by-month.component.html',
  styleUrls: ['./tabelar-view-by-month.component.css']
})
export class TabelarViewByMonthComponent implements OnInit{
  list1:WeekByDay[]=[];
  list2:WeekByDay[]=[];
  settlements:Settlement[] = [];
  constructor(private deviceService:DevicesService,private authService:AuthService){}


  selectedOption: number = 0;

  onOptionSelected() {
    this.ngOnInit();
  }
  ngOnInit(): void {

    this.authService.getlogInUser().subscribe(user=>{
      this.authService.getCityId(user.city).subscribe(number=>{
        this.authService.getSettlement(number).subscribe((settlement:Settlement[])=>{
          this.settlements = settlement;
        })
        if(this.selectedOption == 0){
          this.deviceService.monthByDay(number,2).subscribe((data:WeekByDay[])=>{
            console.log("Data => ", data);
            this.list1 = data;
            this.deviceService.monthByDay(number,1).subscribe((data:WeekByDay[])=>{
              console.log("Data => ", data);
              this.list2 = data;
            })
          })
        }
        else{
          this.deviceService.monthByDaySettlement(this.selectedOption,2).subscribe((data:WeekByDay[])=>{
            console.log("Data => ", data);
            this.list1 = data;
            this.deviceService.monthByDaySettlement(this.selectedOption,1).subscribe((data:WeekByDay[])=>{
              console.log("Data => ", data);
              this.list2 = data;
            })
          })
        }
        
      })
    })
  }

  

}
