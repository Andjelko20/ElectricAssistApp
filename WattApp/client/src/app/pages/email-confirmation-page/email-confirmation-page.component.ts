import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-email-confirmation-page',
  templateUrl: './email-confirmation-page.component.html',
  styleUrls: ['./email-confirmation-page.component.css']
})
export class EmailConfirmationPageComponent implements OnInit{
  isConfirmed : boolean = false;
  constructor(private http : HttpClient, private route : ActivatedRoute){

  }

  ngOnInit(){
    this.route.queryParams.subscribe(params => {
      const key = params['key'];
      this.http.post<ConfirmEmailResponseDTO>(`https://localhost:7146/api/Users/emailConfirmation/${key}`, undefined).subscribe((response) => {
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
