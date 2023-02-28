import { Component, EventEmitter, Input,Output } from '@angular/core';
import { Film } from 'src/app/models/film';
import { FilmServiceService } from 'src/app/services/film-service.service';

@Component({
  selector: 'app-edit-film',
  templateUrl: './edit-film.component.html',
  styleUrls: ['./edit-film.component.css']
})
export class EditFilmComponent {

  @Input() film?: Film;
  @Output() filmUpdated=new EventEmitter<Film[]>();
  constructor(private filmService:FilmServiceService){

  }
  ngOnInit(): void{

  }
  updateFilm(film: Film)
  {
    this.filmService
    .updateFilm(film)
    .subscribe((films: Film[])=>this.filmUpdated.emit(films));
  }
  deleteFilm(film: Film)
  {
    this.filmService
    .deleteFilm(film)
    .subscribe((films: Film[])=>this.filmUpdated.emit(films));
  }
  createFilm(film: Film)
  {
    this.filmService
    .createFilm(film)
    .subscribe((films: Film[])=>this.filmUpdated.emit(films));
  }
  
}
