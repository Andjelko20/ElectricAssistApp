import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Prosumers } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { HistoryPredictionService } from 'src/app/services/history-prediction.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-all-prosumers',
  templateUrl: './all-prosumers.component.html',
  styleUrls: ['./all-prosumers.component.css']
})
export class AllProsumersComponent implements OnInit {
  url = `${environment.serverUrl}/api/ProsumersDetails/page`;
  currentPage = 1;
  itemsPerPage = 10;
  totalItems = 20;
  data: Prosumers[] = [];
  prosumerValues: any[] = [];
i: any;

  constructor(
    private authService: AuthService,
    private historyService: HistoryPredictionService
  ) {}

  ngOnInit(): void {
    this.getProsumerValues();
  }

  getProsumerValues(): void {
    this.prosumerValues = [];
    this.data.forEach((prosumer) => {
      this.historyService.currentUserProductionConsumption(prosumer.id, 2).subscribe((vr1) => {
        if (typeof vr1 !== 'number') {
          vr1 = 0;
        }
        this.historyService.currentUserProductionConsumption(prosumer.id, 1).subscribe((vr2) => {
          if (typeof vr2 !== 'number') {
            vr2 = 0;
          }
          const prosumerData = {
            id: prosumer.id,
            consumption: vr1,
            production: vr2
          };
          this.prosumerValues.push(prosumerData);
        });
      });
    });
  }

  pageChanged(pageNumber: number): void {
    const url = new URL(this.url);
    url.searchParams.set('pageNumber', pageNumber.toString());
    url.searchParams.set('pageSize', this.itemsPerPage.toString());
    const controller = new AbortController();
    setTimeout(() => {
      controller.abort();
    }, 1000);
    fetch(url.toString(), { headers: { Authorization: `Bearer ${localStorage.getItem('token')}` }, signal: controller.signal })
      .then((res) => {
        if (res.status === 401 || res.status === 403) {
          return Promise.reject('Unauthorized');
        }
        return res.text();
      })
      .then((ress) => {
        try {
          const res = JSON.parse(ress);
          if (res?.data === undefined) {
            return;
          }
          this.data = res?.data.map((u: any) => ({
            id: u.id,
            name: u.name,
            settlement: u.settlement,
            city: u.city,
            country: u.country
          } as Prosumers));
          this.getProsumerValues();
          this.currentPage = pageNumber;
          this.totalItems = res.numberOfPages * this.itemsPerPage;
        } catch (err: any) {
          console.log(err.message);
        }
      });
  }
}