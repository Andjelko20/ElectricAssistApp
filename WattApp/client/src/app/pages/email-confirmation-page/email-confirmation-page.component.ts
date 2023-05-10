import { Component,  OnDestroy,  OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { EmailConfirmationServiceService } from 'src/app/services/email-confirmation-service.service';

@Component({
  selector: 'app-email-confirmation-page',
  templateUrl: './email-confirmation-page.component.html',
  styleUrls: ['./email-confirmation-page.component.css']
})
export class EmailConfirmationPageComponent implements OnInit, OnDestroy {
  isConfirmed! : boolean;
  message!: string;
  loading : boolean = true;
  subscription : Subscription = new Subscription();

  constructor(
    private route : ActivatedRoute,
    private router : Router,
    private emaileConfirmationService : EmailConfirmationServiceService)
    {}

  goToLoginPage(){
    this.router.navigate(['login']);
  }
  
  ngOnInit(): void {
    this.loading = true;
    this.route.queryParams.subscribe(params => {
      const key = params['key'] ;
      console.log(key);

      this.emaileConfirmationService.confirmEmailAddress(key).subscribe((response : ConfirmEmailResponseDTO) => {
        if(response != null){
          if(response.isConfirmed){
            this.isConfirmed = true;
            //console.log("Uspesno potvrdjen mail.");
          }
          else{
            this.isConfirmed = false;
            this.message = response.error;
            //console.log("Neuspesno potvrdjivanje");
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

interface ConfirmEmailResponseDTO {
  isConfirmed: boolean;
  error: string;
}
