import { Component, OnInit } from '@angular/core';
import { ForecastService } from 'src/app/services/forecast.service';

interface ForecastTimeline {
  time: string;
  temp: number;
}

@Component({
  selector: 'app-today',
  templateUrl: './today.component.html',
  styleUrls: ['./today.component.css']
})
export class TodayComponent implements OnInit {

  cels = true;
  timeline: ForecastTimeline[] = [];
  weatherNow: any;
  currentTime = new Date();
  location: any;

  constructor(private forecastService: ForecastService) {}

  ngOnInit(): void {
    this.forecastService.getWeatherForecast().subscribe(data => {
      this.getTodayForecast(data);
    });
  }

  dateRange() {
    const start = new Date();
    start.setHours(start.getHours() + (start.getTimezoneOffset() / 60));
    const to = new Date(start);
    to.setHours(to.getHours() + 2, to.getMinutes() + 59, to.getSeconds() + 59);
    return { start, to };
  }

  getTodayForecast(today: any) {
    this.location = today.city;
    this.timeline = today.list.slice(0, 8).map((forecast: { dt_txt: any; main: { temp: any; }; }) => ({
      time: forecast.dt_txt,
      temp: forecast.main.temp
    }));
    const weatherNow = today.list.find((forecast: { dt_txt: string | number | Date; }) => {
      const apiDate = new Date(forecast.dt_txt).getTime();
      return this.dateRange().start.getTime() <= apiDate && this.dateRange().to.getTime() >= apiDate;
    });
    if (weatherNow) {
      this.weatherNow = weatherNow;
    }
  }


}