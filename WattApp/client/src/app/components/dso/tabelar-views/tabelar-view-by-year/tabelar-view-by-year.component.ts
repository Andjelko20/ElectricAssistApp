import { Component, OnInit } from '@angular/core';
import { YearsByMonth } from 'src/app/models/devices.model';
import { Settlement } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { DevicesService } from 'src/app/services/devices.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';

@Component({
  selector: 'app-tabelar-view-by-year',
  templateUrl: './tabelar-view-by-year.component.html',
  styleUrls: ['./tabelar-view-by-year.component.css']
})
export class TabelarViewByYearComponent implements OnInit{
  list1:YearsByMonth[]=[];
  list2:YearsByMonth[]=[];
  settlements:Settlement[] = [];
  itemList: string[] = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Avg','Sep','Okt','Nov','Dec'];
  constructor(private deviceService:HistoryPredictionService,private authService:AuthService) {
    
  }

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
          this.deviceService.yearByMonth(number,2).subscribe((data:YearsByMonth[])=>{
            console.log("Data => ", data);
            this.list1 = data;
            this.deviceService.yearByMonth(number,1).subscribe((data:YearsByMonth[])=>{
              console.log("Data => ", data);
              this.list2 = data;
            })
          })
        }
        else{
          this.deviceService.yearByMonthSettlement(this.selectedOption,2).subscribe((data:YearsByMonth[])=>{
            console.log("Data => ", data);
            this.list1 = data;
            this.deviceService.yearByMonthSettlement(this.selectedOption,1).subscribe((data:YearsByMonth[])=>{
              console.log("Data => ", data);
              this.list2 = data;
            })
          })
        }
        
      })
    })
    // this.deviceService.yearByMonth(2,2).subscribe((data:YearsByMonth[])=>{
    //   console.log("Data => ", data);
    //   this.list1 = data;
    //   this.deviceService.yearByMonth(2,1).subscribe((data:YearsByMonth[])=>{
    //     console.log("Data => ", data);
    //     this.list2 = data;
    //   })
    // })
  }
}
