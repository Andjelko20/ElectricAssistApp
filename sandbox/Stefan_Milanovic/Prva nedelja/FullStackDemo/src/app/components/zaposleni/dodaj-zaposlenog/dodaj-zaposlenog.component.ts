import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Zaposleni } from 'src/app/models/zaposlen.model';
import { ZaposleniService } from 'src/app/services/zaposleni.service';

@Component({
  selector: 'app-dodaj-zaposlenog',
  templateUrl: './dodaj-zaposlenog.component.html',
  styleUrls: ['./dodaj-zaposlenog.component.css']
})
export class DodajZaposlenogComponent implements OnInit {

  addZaposlenogRequest: Zaposleni = {
    id: '',
    ime: '',
    email: '',
    telefon: '',
    plata: 0,
    odeljenje: ''
  };

  constructor(private zaposleniService: ZaposleniService, private router: Router) { }

  ngOnInit(): void {}

  addZaposlenog() {
    //console.log(this.addZaposlenogRequest);
    this.zaposleniService.addZaposlenog(this.addZaposlenogRequest)
    .subscribe({
      next: (zaposleni) => {
        //console.log(zaposleni);
        this.router.navigate(['zaposleni']);
      }
    })
  }
}
