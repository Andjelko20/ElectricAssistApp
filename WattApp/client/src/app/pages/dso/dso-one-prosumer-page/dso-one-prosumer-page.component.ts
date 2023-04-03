import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-dso-one-prosumer-page',
  templateUrl: './dso-one-prosumer-page.component.html',
  styleUrls: ['./dso-one-prosumer-page.component.css']
})
export class DsoOneProsumerPageComponent implements OnInit{
  currentTime!: Date;

  constructor() { }

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
