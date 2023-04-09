import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-prosumer-account-page',
  templateUrl: './prosumer-account-page.component.html',
  styleUrls: ['./prosumer-account-page.component.css']
})
export class ProsumerAccountPageComponent {
  constructor(private router:Router,private usersService:AuthService,
    private route:ActivatedRoute) { 
     
    }
  ngOnInit(): void {

  }
  logout()
  {
    localStorage.removeItem('token');
    this.usersService.isLoginSubject.next(false)
    this.router.navigate(['/login']);
  }
}
