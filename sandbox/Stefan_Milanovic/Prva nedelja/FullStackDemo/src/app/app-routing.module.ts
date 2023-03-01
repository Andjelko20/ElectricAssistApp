import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DodajZaposlenogComponent } from './components/zaposleni/dodaj-zaposlenog/dodaj-zaposlenog.component';
import { EditZaposleniComponent } from './components/zaposleni/edit-zaposleni/edit-zaposleni.component';
import { ZaposleniListaComponent } from './components/zaposleni/zaposleni-lista/zaposleni-lista.component';

const routes: Routes = [
  {
    path: '', // home putanja
    component: ZaposleniListaComponent // ucitavamo komponentu, tj. listu zaposlenih
  },
  {
    path: 'zaposleni', // zaposleni putanja
    component: ZaposleniListaComponent // ucitavamo komponentu, tj. listu zaposlenih
  },
  {
    path: 'zaposleni/dodaj', // zaposleni/dodaj putanja
    component: DodajZaposlenogComponent
  },
  {
    path: 'zaposleni/edit/:id', // zaposleni/edit/id putanja
    component: EditZaposleniComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
