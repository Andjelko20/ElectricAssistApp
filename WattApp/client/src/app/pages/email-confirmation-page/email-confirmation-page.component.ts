import { HttpClient } from '@angular/common/http';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-email-confirmation-page',
  templateUrl: './email-confirmation-page.component.html',
  styleUrls: ['./email-confirmation-page.component.css']
})
export class EmailConfirmationPageComponent implements OnInit, OnDestroy {
  isConfirmed: boolean | null = null;
  message: string | null = null;
  private routeSubscription: Subscription | null = null;

  constructor(private http: HttpClient, private route: ActivatedRoute, private router: Router) {}

  goToLogin() {
    this.router.navigate(['login']);
  }

  ngOnInit() {
    console.log("ngOnInit called");
    this.routeSubscription?.unsubscribe();
    this.subscribeToRoute();
  }

  private subscribeToRoute() {
    this.routeSubscription = this.route.queryParams.subscribe(params => {
      const key = params['key'];
      this.http.post<ConfirmEmailResponseDTO>(`${environment.serverUrl}/api/Users/emailConfirmation/${key}`, null)
        .subscribe((response) => {
          if (response.isConfirmed) {
            this.isConfirmed = true;
          } else {
            this.isConfirmed = false;
            this.message = response.error;
          }
        });
    });
  }

  ngOnDestroy(): void {
    console.log("ngOnDestroy called");
    if (this.routeSubscription) {
      this.routeSubscription.unsubscribe();
    }
  }
}

interface ConfirmEmailResponseDTO {
  isConfirmed: boolean;
  error: string;
}
