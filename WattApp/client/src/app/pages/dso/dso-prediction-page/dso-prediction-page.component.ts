import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-dso-prediction-page',
  templateUrl: './dso-prediction-page.component.html',
  styleUrls: ['./dso-prediction-page.component.css']
})
export class DsoPredictionPageComponent implements OnInit{
  
  graph:boolean = true;
  tabelar:boolean = false;
  currentTime!: Date;
  
  ngOnInit(): void {
    this.updateTime();
    setInterval(() => {
      this.updateTime();
    }, 1000);
  }
  updateTime() {
    this.currentTime = new Date();
  }
  showGraph(){
    this.graph = true;
    this.tabelar = false;
  }
  showTable(){
    this.graph = false;
    this.tabelar = true;
  }

  
}
