import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'ElectricAssist';
  islogg!: Observable<boolean>;
  constructor(public authService: AuthService) {
    this.islogg=authService.isLoginSubject;
  }
   
  ngOnInit(){
    document.title=this.title;
	this.authService.isLoginSubject.next(this.authService.hasToken());
    // if(localStorage.getItem('token'))
    // {

    //     this.islogg=true;
        
    // }
    // else
    // {
    //   this.islogg=false;
        
    // }
  }
 

}
