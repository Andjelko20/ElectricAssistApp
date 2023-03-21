import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.css']
})
export class SidebarComponent {
  constructor(private router:Router,private usersService:AuthService,
    private route:ActivatedRoute) { }
  logout()
  {
    localStorage.removeItem('token');
    localStorage.clear();
    this.usersService.isLoginSubject.next(false)
    this.router.navigate(['/login']);
  }
}
