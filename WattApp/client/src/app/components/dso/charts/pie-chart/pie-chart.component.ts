import { Component, OnInit } from '@angular/core';

import { Chart,registerables } from 'node_modules/chart.js'
Chart.register(...registerables)

import ChartDataLabels from 'chartjs-plugin-datalabels';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { AuthService } from 'src/app/services/auth.service';
import { Settlement } from 'src/app/models/users.model';
Chart.register(ChartDataLabels);


@Component({
  selector: 'app-pie-chart',
  templateUrl: './pie-chart.component.html',
  styleUrls: ['./pie-chart.component.css']
})
export class PieChartComponent implements OnInit {

  settlements:Settlement[] = [];
  settlementsValue:number[] = [];
  itemList: string[] = ['Aerodrom', 'Bagremar', 'Erdoglija', 'Bresnica', 'Stanovo', 'Belosevac'];
  constructor(private authService:AuthService,private historyService:HistoryPredictionService) {
  
  }
  ngOnInit(): void {

    this.authService.getlogInUser().subscribe(user=>{
      this.authService.getCityId(user.city).subscribe(number=>{
        this.authService.getSettlement(number).subscribe((settlement:Settlement[])=>{
          this.settlements = settlement;
          this.settlements.forEach(settlement =>{
            this.historyService.getCurrentConsumptionProductionSettlement(2,settlement.id).subscribe(value =>{
              this.settlementsValue.push(value);
              this.PieChart();
            })
            
          })
        })
      })
    })
  }

  PieChart(){


    const chartId = 'piechart';
    const chartExists = Chart.getChart(chartId);
    if (chartExists) {
        chartExists.destroy();
    }

    const name = this.settlements.map(name => name.name);
    var data= [{
      label: '',
      data: this.settlementsValue,
      backgroundColor: [
         //dugine
        "#F7DC6F", 
        "#AF7AC5",
        " #2E8B57 ", 
        "#F5B7B1", 
        "#D5F5E3", 
        "#483D8B ",
        "#87CEFA",  
        "#4B0082 ",
        "#FFFFF0", 
        "#BC8F8F",
        "#696969 ",
        "#483D8B ",
        "#4B0082 ",

       
       
        
        
          ],
      borderWidth: 1,
      borderColor: "#00000"
  //   },{label: 'Percentage of Production in kWh',
  //   data: [30, 20, 15, 15, 14, 6],
  //   // backgroundColor: [
  //   //         "#4b77a9",
  //   //         "#5f255f",
  //   //         "#d21243",
  //   //         "#B27200",
  //   //         "#00000"
  //   //     ],
  //   borderWidth: 1,
  //   borderColor: "#00000"
  },]
    var ctx = "piechart";
    var myChart = new Chart(ctx, {
      
      type: 'doughnut',
      data: {
        labels: name,
          datasets: data
      },
      options: {
        plugins: {
            datalabels: {
                formatter: (value: number, ctx: { chart: { data: { datasets: { data: any; }[]; }; }; }) => {
                let sum = 0;
                let dataArr = ctx.chart.data.datasets[0].data;
                dataArr.map((data: number) => {
                  sum += data;
                });
                
                let percentage = value + "kWh";
                return percentage;
              },
                color: '#fff',
                font:{
                  size:10
                }
            },
            legend: {
              labels:{
                color:'#000',
                font:{
                  size:15
                }
                
              },
              
              position: 'right',
              onHover: function (event, legendItem, legend) {
                document.body.style.cursor = 'pointer';
              },
              onLeave: function (event, legendItem, legend) {
                  document.body.style.cursor = 'default';
              },
              
              
            },
            // title: {
            //   display: true,
            //   text: 'Current production in settlements',
            //   color:'#727272',
            //   font:{
            //     size:15
            //   }
              
            // }
            
        }}
      
    
    });
  }

}