import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastModule } from 'primeng/toast';
import { MessageService } from 'primeng/api';


@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  exports:[
	ToastModule
  ],
  providers:[MessageService]
})
export class PrimeModule { }
