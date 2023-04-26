import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-change-email-confirmation-page',
  templateUrl: './change-email-confirmation-page.component.html',
  styleUrls: ['./change-email-confirmation-page.component.css']
})
export class ChangeEmailConfirmationPageComponent {
  isConfirmed : boolean = false;
  constructor(private http : HttpClient, private route : ActivatedRoute, private router : Router){

  }

  goToLogin(){
    this.router.navigate(['login']);
  }

  ngOnInit(){
    this.route.queryParams.subscribe(params => {
      const key = params['key'];
      console.log(key);
      this.http.post<ConfirmEmailResponseDTO>(`https://localhost:7146/api/Users/changeEmailConfirmation/${key}`, undefined).subscribe((response) => {
        console.log(response);
        if(response && response.isConfirmed){
          this.isConfirmed = true;
        }
      });
    });
  }
}

interface ConfirmEmailResponseDTO{
  isConfirmed : boolean;
  error : string;
}
