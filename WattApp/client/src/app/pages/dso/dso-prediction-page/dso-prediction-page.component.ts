import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-dso-prediction-page',
  templateUrl: './dso-prediction-page.component.html',
  styleUrls: ['./dso-prediction-page.component.css']
})
export class DsoPredictionPageComponent implements OnInit{
  
  
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

  
}
