import { Component } from '@angular/core';
import { Chart,registerables } from 'node_modules/chart.js'
import { WeekByDay } from 'src/app/models/devices.model';
import { Settlement } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { DevicesService } from 'src/app/services/devices.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
Chart.register(...registerables)
@Component({
  selector: 'app-line-week-chart',
  templateUrl: './line-week-chart.component.html',
  styleUrls: ['./line-week-chart.component.css']
})
export class LineWeekChartComponent {


  list1:WeekByDay[] = [];
  list2:WeekByDay[] = [];
  settlements:Settlement[] = [];
  constructor(private deviceService:HistoryPredictionService,private authService:AuthService) {
    
  }

  selectedOption: number = 0;

  onOptionSelected() {
    this.ngOnInit();
  }

  ngOnInit(): void {
    console.log("Selektovano je "+this.selectedOption); // Do whatever you want with the selected option here]
    this.authService.getlogInUser().subscribe(user=>{
      this.authService.getCityId(user.city).subscribe(number=>{
        this.authService.getSettlement(number).subscribe((settlement:Settlement[])=>{
          this.settlements = settlement;
        })
        if(this.selectedOption == 0){
          this.deviceService.weekByDay(number,2).subscribe((data: WeekByDay[]) =>{
            console.log("Data => ", data);
            this.list1 = data;
            this.deviceService.weekByDay(number,1).subscribe((data: WeekByDay[]) =>{
              console.log("Data => ", data);
              this.list2 = data;
              this.LineChart();
            })
      
          })
        }
        else{
          this.deviceService.weekByDaySettlement(this.selectedOption,2).subscribe((data: WeekByDay[]) =>{
            console.log("Data => ", data);
            this.list1 = data;
            this.deviceService.weekByDaySettlement(this.selectedOption,1).subscribe((data: WeekByDay[]) =>{
              console.log("Data => ", data);
              this.list2 = data;
              this.LineChart();
            })
      
          })
        }
        
      })
    })
    
    // this.deviceService.weekByDay(1,2).subscribe((data: WeekByDay[]) =>{
    //   console.log("Data => ", data);
    //   this.list1 = data;
    //   this.deviceService.weekByDay(1,1).subscribe((data: WeekByDay[]) =>{
    //     console.log("Data => ", data);
    //     this.list2 = data;
    //     this.LineChart();
    //   })

    // })
    
    // console.log(this.list);
    
    
  }
  LineChart(){

    const chartId = 'linechart';
    const chartExists = Chart.getChart(chartId);
    if (chartExists) {
        chartExists.destroy();
    }

    const energyUsageResults1 = this.list1.map(day => day.energyUsageResult);
    const energyUsageResults2 = this.list2.map(day => day.energyUsageResult);

    const Linechart = new Chart("linechart", {
      type: 'line',
      data : {
        labels: ['Mon','Tue','Wed','Thu','Fri','Sat','Sun'],
        
        datasets:  [
          {
            label: 'consumption',
            data: energyUsageResults1,
            backgroundColor: [
              'rgba(255, 99, 132, 0.2)',
              'rgba(54, 162, 235, 0.2)',
              'rgba(255, 206, 86, 0.2)',
              'rgba(75, 192, 192, 0.2)',
              'rgba(153, 102, 255, 0.2)',
              'rgba(255, 159, 64, 0.2)'
          ],
          borderColor: [
              'rgba(255,99,132,1)',
              'rgba(54, 162, 235, 1)',
              'rgba(255, 206, 86, 1)',
              'rgba(75, 192, 192, 1)',
              'rgba(153, 102, 255, 1)',
              'rgba(255, 159, 64, 1)'
          ],
          pointBorderColor: 'rgba(255,99,132,1)',
          pointBorderWidth: 7,
            pointRadius: 5,
          borderWidth: 2,
          fill: true
          },
          {
            label: 'production',
            data: energyUsageResults2,
            tension:0.5,
            backgroundColor: 'rgba(0, 255, 0, 0.2)',
            borderColor: 'rgba(0, 255, 0, 1)',
            borderWidth: 2,
            pointBackgroundColor: 'rgba(0, 255, 0, 1)',
            pointBorderColor: 'rgba(0, 255, 0, 1)',
            pointBorderWidth: 7,
            pointRadius: 5,
            pointHoverRadius: 6,
            fill:true
          }
          
        ]
        
      }
      ,
      options: {
        scales:{
          y: {
            ticks:{
              color:'#000',
              font:{
                size:20
              }
            },
            position: "left",
            title:{
              display:true,
              text: "kWh",
              color:'#000',
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
              text: "Days in a week",
              color:'#000',
              font:{
                size:20
              }
            }
          }
          ,
        },
        responsive: true,
        plugins: {
          datalabels:{display: false},
          legend: {
            position: 'bottom',
            onHover: function (event, legendItem, legend) {
              document.body.style.cursor = 'pointer';
            },
            onLeave: function (event, legendItem, legend) {
                document.body.style.cursor = 'default';
            },
            labels:{
              usePointStyle: true,
              color:'#000',
              font:{
                size:20
              } 
           
            }
            ,
            align: "center"
          },
          title: {
            display: true,
            text: ' Consumption and production in a week',
            color:'#000',
            font:{
              size:20
            }
          }
        }
      }
    });

  }
}
