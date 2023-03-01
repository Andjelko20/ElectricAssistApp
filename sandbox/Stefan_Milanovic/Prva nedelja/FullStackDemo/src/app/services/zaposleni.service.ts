import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Zaposleni } from '../models/zaposlen.model';

@Injectable({
  providedIn: 'root'
})
export class ZaposleniService {

  baseApiUrl: string; // = environment.baseApiUrl;
  constructor(private http: HttpClient) { 
    this.baseApiUrl = environment.baseApiUrl;
  }

  getAllZaposleni(): Observable<Zaposleni[]> {
    return this.http.get<Zaposleni[]>(this.baseApiUrl + '/api/zaposleni') // primili smo podatke o zaposlenima iz baze, sad ih trebamo mapirati na front
  }

  addZaposlenog(addZaposlenogRequest: Zaposleni): Observable<Zaposleni> {
    addZaposlenogRequest.id = '00000000-0000-0000-0000-000000000000';
    return this.http.post<Zaposleni>(this.baseApiUrl + '/api/zaposleni', addZaposlenogRequest);
  }

  getZaposlenog(id: string): Observable<Zaposleni> {
    return this.http.get<Zaposleni>(this.baseApiUrl + '/api/zaposleni/' + id);  
  }

  updateZaposlenog(id: string, updateZaposlenogRequest: Zaposleni): Observable<Zaposleni>
  {
    return this.http.put<Zaposleni>(this.baseApiUrl + '/api/zaposleni/' + id, updateZaposlenogRequest);
  }
}