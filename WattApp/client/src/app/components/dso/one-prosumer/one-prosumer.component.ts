import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { JwtToken } from 'src/app/utilities/jwt-token';
import { Prosumers } from 'src/app/models/users.model'
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-one-prosumer',
  templateUrl: './one-prosumer.component.html',
  styleUrls: ['./one-prosumer.component.css']
})
export class OneProsumerComponent implements OnInit{

  id?:number;
  dso!: Prosumers;
  constructor(private authService:AuthService,private route:ActivatedRoute) {
    
  }
  ngOnInit(): void {
    let token=new JwtToken();
    this.id=token.data.id as number;

    this.authService.getProsumer(Number(this.route.snapshot.paramMap.get('id'))).subscribe(user=>{
      this.dso = user;
      console.log(this.dso);
    })

    console.log(this.id);
  }
}
