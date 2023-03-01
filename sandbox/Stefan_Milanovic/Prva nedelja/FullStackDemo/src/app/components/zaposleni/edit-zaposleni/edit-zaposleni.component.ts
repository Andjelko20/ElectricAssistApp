import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Zaposleni } from 'src/app/models/zaposlen.model';
import { ZaposleniService } from 'src/app/services/zaposleni.service';

@Component({
  selector: 'app-edit-zaposleni',
  templateUrl: './edit-zaposleni.component.html',
  styleUrls: ['./edit-zaposleni.component.css']
})
export class EditZaposleniComponent implements OnInit {

  zaposlenDetails: Zaposleni = {
    id: '',
    ime: '',
    email: '',
    telefon: '',
    plata: 0,
    odeljenje: ''
  };

  constructor(private route: ActivatedRoute, private zaposleniService: ZaposleniService, private router: Router) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe({
      next: (params) => {
        const id = params.get('id');

        if(id)
        {
          this.zaposleniService.getZaposlenog(id)
          .subscribe({
            next: (response) => {
              this.zaposlenDetails = response;
            }
          });
        }
      }
    })
  }

  updateZaposlenog()
  {
    this.zaposleniService.updateZaposlenog(this.zaposlenDetails.id, this.zaposlenDetails)
    .subscribe({
      next: (response) => {
        this.router.navigate(['zaposleni']);
      }
    });
  }
}
