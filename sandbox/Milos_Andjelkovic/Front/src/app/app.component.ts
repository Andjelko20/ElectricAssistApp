import { Component } from '@angular/core';
import { Film } from './models/film';
import { FilmServiceService } from './services/film-service.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Front';
  films: Film[]=[];
  filmToEdit?: Film; 
  constructor(private filmservice: FilmServiceService) {
  }

  ngOnInit() :  void{
    this.filmservice.getFilm().subscribe((result: Film[])=>(
      this.films=result
      ))

  }
  updateFilmList(films: Film[]){
    this.films=films;
  }
  initNewFilm()
  {
    this.filmToEdit=new Film();
  }

  editFilm(film: Film)
  {
    this.filmToEdit=film;
  }
}
