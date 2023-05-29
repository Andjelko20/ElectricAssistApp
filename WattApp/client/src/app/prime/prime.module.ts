import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastModule } from 'primeng/toast';
import { ConfirmationService, MessageService } from 'primeng/api';
import { DialogModule } from 'primeng/dialog';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmPopupModule } from 'primeng/confirmpopup';
import { PasswordModule } from 'primeng/password';

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  exports:[
	ToastModule,
	DialogModule,
	ConfirmDialogModule,
	ConfirmPopupModule,
	PasswordModule
  ],
  providers:[MessageService,ConfirmationService]
})
export class PrimeModule { }
