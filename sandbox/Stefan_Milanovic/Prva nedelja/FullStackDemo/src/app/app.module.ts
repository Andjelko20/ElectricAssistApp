import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { ZaposleniListaComponent } from './components/zaposleni/zaposleni-lista/zaposleni-lista.component';
import { DodajZaposlenogComponent } from './components/zaposleni/dodaj-zaposlenog/dodaj-zaposlenog.component';
import { FormsModule } from '@angular/forms';
import { EditZaposleniComponent } from './components/zaposleni/edit-zaposleni/edit-zaposleni.component';

@NgModule({
  declarations: [
    AppComponent,
    ZaposleniListaComponent,
    DodajZaposlenogComponent,
    EditZaposleniComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
