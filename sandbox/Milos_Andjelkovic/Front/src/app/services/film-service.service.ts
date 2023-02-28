import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Film } from '../models/film';

@Injectable({
  providedIn: 'root'
})
export class FilmServiceService {

  private url="Film";
  constructor( private http: HttpClient) { }

  public getFilm() : Observable<Film[]>{
  
    return this.http.get<Film[]>(`${environment.apiUrl}/${this.url}`);
  }
  public updateFilm(filmUp: Film) : Observable<Film[]>{
  
    return this.http.put<Film[]>(`${environment.apiUrl}/${this.url}`,filmUp);
  }
  public createFilm(filmAdd: Film) : Observable<Film[]>{
  
    return this.http.post<Film[]>(`${environment.apiUrl}/${this.url}`,filmAdd);
  }
  public deleteFilm(filmDel: Film) : Observable<Film[]>{
  
    return this.http.delete<Film[]>(`${environment.apiUrl}/${this.url}/${filmDel.id}`);
  }
}
