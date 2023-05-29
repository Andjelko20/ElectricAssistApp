import { Component, OnInit } from '@angular/core';

import { Chart,registerables } from 'node_modules/chart.js'
Chart.register(...registerables)

import ChartDataLabels from 'chartjs-plugin-datalabels';
import { AuthService } from 'src/app/services/auth.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { Settlement } from 'src/app/models/users.model';
import { nodeName } from 'jquery';
Chart.register(ChartDataLabels);


@Component({
  selector: 'app-pie-chart',
  templateUrl: './pie-chart.component.html',
  styleUrls: ['./pie-chart.component.css']
})
export class PieChartComponent implements OnInit {
  loader:boolean=false;
  settlements:Settlement[] = [];
  settlements1:Settlement[] = [];
  settlementsValue:number[] = [];
  noData:number[]=[];
  constructor(private authService:AuthService,private historyService:HistoryPredictionService) {
  
  }
  ngOnInit(): void {
    this.loader=true;
    this.authService.getlogInUser().subscribe(user=>{
      this.authService.getCityId(user.city).subscribe(number=>{
        this.authService.getSettlement(number).subscribe((settlement:Settlement[])=>{
          this.settlements = settlement;
          for (let i = 0; i < this.settlements.length; i++) {
            this.historyService.getCurrentConsumptionProductionSettlement(1,settlement[i].id).subscribe(value =>{
              this.loader=false;
              if(value!==0)
              {
                this.noData.push(1);
                this.settlementsValue.push(value);
                this.settlements1.push(settlement[i])
              } 
              if(this.noData[0]!=1)
              { 
                  this.loader=true;
              }
              if(i == this.settlements.length-1){
                this.PieChart()
              }
            })
            
          }
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

    const name = this.settlements1.map(name => name.name);
    var data= [{
      label: '',
      data: this.settlementsValue,
      backgroundColor: [
        '#7fcdbb',
        '#c7e9b4',  
        '#1d91c0', 
        '#225ea8', 
        '#253494', 
        '#081d58',
        '#52988f',
        '#86b59d',
        '#207b97',
        '#2d6b8b',
        '#2f317f',
        '#0d1848',
        '#3c6c68',
        '#aec9c5',
        '#749cab',
        '#8b99a8',
        '#141e70',
        '#193d36',
        '#8fc4c1',
        '#5a908b',
        '#15667c',
        '#1e5386',
        '#1f255e',
        '#0a0e2d',
        '#5d8581',
        '#6f7a91'
        ],
      borderWidth: 1,
      borderColor: "gray",
  },]
    const myChart = new Chart("piechart", {
      
      type: 'doughnut',
      data: {
        labels: name,
          datasets: data
      },
      options: {
        onHover: (e, chartEle) => {
          if (e.native) {
            const target = e.native.target as HTMLElement;
            if (target instanceof HTMLElement) {
              target.style.cursor = chartEle.length > 0 && chartEle[0] ? 'pointer' : 'default';
            } else {
              console.error('Invalid target element:', target);
            }
          } else {
            console.error('Missing native event:', e);
          }
        },  
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
                color:'gray',
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