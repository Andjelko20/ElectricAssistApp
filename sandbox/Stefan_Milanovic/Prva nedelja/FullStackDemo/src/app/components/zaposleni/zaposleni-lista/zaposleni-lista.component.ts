import { Component, OnInit } from '@angular/core';
import { Zaposleni } from 'src/app/models/zaposlen.model';
import { ZaposleniService } from 'src/app/services/zaposleni.service';

@Component({
  selector: 'app-zaposleni-lista',
  templateUrl: './zaposleni-lista.component.html',
  styleUrls: ['./zaposleni-lista.component.css']
})
export class ZaposleniListaComponent implements OnInit {

  zaposleni: Zaposleni[] = [
    /*{
      id: 'id1',
      ime: 'Jovan Jovic',
      email: 'jovanjovic@gmail.com',
      telefon: '0619087654',
      plata: 80000,
      odeljenje: 'odeljenje1'
    },
    {
      id: 'id2',
      ime: 'Darko Daric',
      email: 'darkodaric@gmail.com',
      telefon: '0611234567',
      plata: 75000,
      odeljenje: 'odeljenje1'
    },
    {
      id: 'id3',
      ime: 'Isidor Isidorovic',
      email: 'isidorisidorovic@gmail.com',
      telefon: '0622214132',
      plata: 82000,
      odeljenje: 'odeljenje2'
    }*/
  ];
  constructor(private zaposleniService: ZaposleniService) {}

  ngOnInit(): void {

    //this.zaposleni.push()
    this.zaposleniService.getAllZaposleni()
    .subscribe({
      next: (zaposleni) => {
        //console.log(zaposleni);
        this.zaposleni = zaposleni; // u nas niz this.zaposleni koji smo kreirali iznad, smestamo niz zaposleni u kome su smesteni podaci iz baze preko .net
      },
      error: (response) => {
        console.log(response);
      }
    });
  }
}
