import { Component } from '@angular/core';

import { Chart,registerables } from 'node_modules/chart.js'
import { WeekByDay, YearsByMonth } from 'src/app/models/devices.model';
import { Settlement } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { DevicesService } from 'src/app/services/devices.service';
Chart.register(...registerables)



Chart.defaults.color = "#fff";
Chart.defaults.color = "#fff";
@Component({
  selector: 'app-bar-year-chart',
  templateUrl: './bar-year-chart.component.html',
  styleUrls: ['./bar-year-chart.component.css']
})
export class BarYearChartComponent {

  list1:YearsByMonth[]=[];
  list2:YearsByMonth[]=[];
  settlements:Settlement[] = [];
  itemList: string[] = ['Jan','Feb','Mar','Apr','May','Jun','Jul','Avg','Sep','Okt','Nov','Dec'];
  constructor(private deviceService:DevicesService,private authService:AuthService) {
    
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
              this.BarPlot();
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
              this.BarPlot();
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
    //     this.BarPlot();
    //   })
    // })
  }
  BarPlot(){

    const chartId = 'barplot';
    const chartExists = Chart.getChart(chartId);
    if (chartExists) {
        chartExists.destroy();
    }


    const energyUsageResults1 = this.list1.map(day => day.energyUsageResult);
    const energyUsageResults2 = this.list2.map(day => day.energyUsageResult);
    const Linechart =new Chart("barplot", {
        type: 'bar',
       
        data : {
          labels: this.itemList,
          
          datasets: [
            {
              label: 'Consumption',
              data: energyUsageResults1,
              borderColor: 'rgb(128, 0, 128)',
              backgroundColor: 'rgb(128, 0, 128)',
              
            },
            {
              label: 'Production',
              data: energyUsageResults2,
              borderColor: 'rgb(255, 165, 0)',
              backgroundColor: 'rgb(255, 165, 0)'
            },
           
            
          ]
          
        },
        options: 
        {
          scales:{
            y: {
              ticks:{
                color:'#000',
                font:{
                  size:20
                }
              },
              position: "left",
              suggestedMin: 5,
              suggestedMax: 140,
              title:{
                display:true,
                text: "kWh",
                color: '#000',
                font:{
                  size:20
                }
                
              }
            }
            ,
            x:{
              ticks:{
                color:'#000',
                font:{
                  size:20
                }
                
              },
              title:{
                display:true,
                text: "Months in a Year",
                color: '#000',
                font:{
                  size:20
                }
              }
            }
            
              
            
            
            
          },
          responsive: true,
          plugins: {
            
            legend: {
              onHover: function (event, legendItem, legend) {
                document.body.style.cursor = 'pointer';
              },
              onLeave: function (event, legendItem, legend) {
                  document.body.style.cursor = 'default';
              },
              
              position: 'bottom',
              labels: {
                usePointStyle: true,
                color: '#000',
                font:{
                  size:20
                } 
                // ,
                // boxHeight:100,
                // boxWidth:100
              }
            },
            title: {
              display: true,
              text: 'Consumption and production in a year',
              color: '#000',
              font:{
                size:20
              }
            }
          }
        }
      });
  }
}
