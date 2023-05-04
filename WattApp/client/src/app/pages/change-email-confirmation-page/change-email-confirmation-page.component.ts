import { HttpClient } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-change-email-confirmation-page',
  templateUrl: './change-email-confirmation-page.component.html',
  styleUrls: ['./change-email-confirmation-page.component.css']
})
export class ChangeEmailConfirmationPageComponent implements OnInit, OnDestroy{
  isConfirmed : boolean = false;
  message : string = "";
  private mySubscription : Subscription = new Subscription();
  constructor(private http : HttpClient, private route : ActivatedRoute, private router : Router){

  }

  goToLogin(){
    this.router.navigate(['login']);
  }

  ngOnInit(){
    this.mySubscription = this.route.queryParams.subscribe(params => {
      const key = encodeURIComponent(params['key']);
      console.log(key);
      this.http.post<ConfirmEmailResponseDTO>(`${environment.serverUrl}/api/Users/changeEmailConfirmation/${key}`, null).subscribe((response) => {
        console.log(response);
        if(response && response.isConfirmed){
          this.isConfirmed = true;
        }
        else{
          this.message = response.error;
        }
      });
    });
  }

  ngOnDestroy(): void {
    this.mySubscription.unsubscribe();
}
  
}

interface ConfirmEmailResponseDTO{
  isConfirmed : boolean;
  error : string;
}
