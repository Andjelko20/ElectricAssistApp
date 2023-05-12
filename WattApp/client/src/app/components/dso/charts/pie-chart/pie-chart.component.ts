import { Component, OnInit } from '@angular/core';

import { Chart,registerables } from 'node_modules/chart.js'
Chart.register(...registerables)

import ChartDataLabels from 'chartjs-plugin-datalabels';
import { AuthService } from 'src/app/services/auth.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { Settlement } from 'src/app/models/users.model';
Chart.register(ChartDataLabels);


@Component({
  selector: 'app-pie-chart',
  templateUrl: './pie-chart.component.html',
  styleUrls: ['./pie-chart.component.css']
})
export class PieChartComponent implements OnInit {
  loader:boolean=false;
  settlements:Settlement[] = [];
  settlementsValue:number[] = [];
  constructor(private authService:AuthService,private historyService:HistoryPredictionService) {
  
  }
  ngOnInit(): void {
    this.loader=true;
    this.authService.getlogInUser().subscribe(user=>{
      this.authService.getCityId(user.city).subscribe(number=>{
        this.authService.getSettlement(number).subscribe((settlement:Settlement[])=>{
          this.settlements = settlement;
          this.settlements.forEach(settlement =>{
            this.historyService.getCurrentConsumptionProductionSettlement(1,settlement.id).subscribe(value =>{
              this.loader=false;
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
        '#7fcdbb',
          '#c7e9b4',  '#1d91c0', '#225ea8', '#253494', '#081d58','#081d58',
        // "#2B70A7",
        // "#BF1E2E",
        // "#E2B37D",
        // "#EF4136",
        // "#F15A2B",
        // "#006838",
        // "#B5D4EF",
        // "#77B3E1",
        // "#28AAE1",
        // "#2A3890",
        // "#F9ED32",
        // "#D7E022",
        // "#8DC73F",
        // "#39B54A",
        // "#009445",
        // "#F5F194",
        // "#F2F5CD",
        // "#7B5231",
        // "#68499E",
        // "#662D91",
        // "#AF7AC5",
        // " #2E8B57 ", 
        // "#87CEFA", 
        // "#D7E022",
          ],
      borderWidth: 1,
      borderColor: "#00000",
  },]
    var ctx = "piechart";
    var myChart = new Chart(ctx, {
      
      type: 'doughnut',
      data: {
        labels: name,
          datasets: data
      },
      options: {
        maintainAspectRatio:false,
        responsive:true,
        plugins: {
            datalabels: {
                formatter: (value: number, ctx: { chart: { data: { datasets: { data: any; }[]; }; }; }) => {
                let sum = 0;
                let dataArr = ctx.chart.data.datasets[0].data;
                dataArr.map((data: number) => {
                  sum += data;
                });
                
                let percentage = value;
                return percentage;
              },
                color: '#fff',
                font:{
                  size:11
                }
            },
            legend: {
              labels:{
                color:'#000',
                font:{
                  size:15
                }
                
              },
              
              position: 'bottom',
              onHover: function (event, legendItem, legend) {
                document.body.style.cursor = 'pointer';
              },
              onLeave: function (event, legendItem, legend) {
                  document.body.style.cursor = 'default';
              },
              
              
            },
           
            
        }}
      
    
    });
  }

}