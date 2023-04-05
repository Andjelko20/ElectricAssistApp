import { Component, OnInit } from '@angular/core';
import { JwtToken } from 'src/app/utilities/jwt-token';

@Component({
  selector: 'app-prosumer-home-page',
  templateUrl: './prosumer-home-page.component.html',
  styleUrls: ['./prosumer-home-page.component.css']
})
export class ProsumerHomePageComponent implements OnInit {
  
  id?:number
  ngOnInit(): void {
    let token=new JwtToken();
    this.id=token.data.id as number;
    
    

  }

}
