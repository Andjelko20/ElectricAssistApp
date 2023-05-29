import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { EmailConfirmationServiceService } from 'src/app/services/email-confirmation-service.service';

@Component({
  selector: 'app-change-email-confirmation-page',
  templateUrl: './change-email-confirmation-page.component.html',
  styleUrls: ['./change-email-confirmation-page.component.css']
})
export class ChangeEmailConfirmationPageComponent implements OnInit, OnDestroy{
  isConfirmed! : boolean;
  message! : string;
  loading : boolean = true;
  subscription : Subscription = new Subscription();

  constructor(
    private router : Router, 
    private route : ActivatedRoute, 
    private _service : EmailConfirmationServiceService
  ){}

  goToLoginPage(){
    this.router.navigate(['login']);
  }

  ngOnInit(): void {
    this.loading = true;
    this.route.queryParams.subscribe(params => {
      const key = params['key'];
	  
      this._service.changeEmailAddressConfirmation(key).subscribe((response : ConfirmEmailResponseDTO) => {
        if(response){
          if(response.isConfirmed){
            this.isConfirmed = true;
          }
          else{
            this.isConfirmed = false;
            this.message = response.error;
          }
        }
        this.loading = false;
      });
    });
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
    this.loading = true;
  }
  
}

interface ConfirmEmailResponseDTO{
  isConfirmed : boolean;
  error : string;
}
