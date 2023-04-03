import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {

  oldPassword!: string;
  newPassword!: string;
  confirmPassword!: string;
  constructor(private userService: AuthService) { }

  ngOnInit(): void {
  }
  changePassword() {
    this.userService.changePassword(this.oldPassword, this.newPassword)
      .subscribe(
        data => {
         alert('Uspješno promijenjena lozinka');
        },
        error => {
          alert('Došlo je do greške prilikom promjene lozinke');
        }
      );
  }
}
