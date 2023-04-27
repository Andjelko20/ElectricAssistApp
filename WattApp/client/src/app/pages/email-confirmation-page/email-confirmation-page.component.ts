import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-email-confirmation-page',
  templateUrl: './email-confirmation-page.component.html',
  styleUrls: ['./email-confirmation-page.component.css']
})
export class EmailConfirmationPageComponent implements OnInit{
  isConfirmed : boolean | null = null;
  message : string | null = null;
  constructor(private http : HttpClient, private route : ActivatedRoute, private router : Router){

  }

  goToLogin(){
    this.router.navigate(['login']);
  }

  ngOnInit(){
    this.route.queryParams.subscribe(params => {
      const key = encodeURIComponent(params['key']);
      console.log(key);
      this.http.post<ConfirmEmailResponseDTO>(`${environment.serverUrl}/api/Users/emailConfirmation/${key}`, null).subscribe((response) => {
        console.log(response);
        console.log(response.isConfirmed);
        console.log(response.error);
        if(response.isConfirmed){
          this.isConfirmed = true;
        }
        else{
          this.isConfirmed = false;
          this.message = response.error;
        }
      });
    });
  }

}

interface ConfirmEmailResponseDTO{
  isConfirmed : boolean;
  error : string;
}
