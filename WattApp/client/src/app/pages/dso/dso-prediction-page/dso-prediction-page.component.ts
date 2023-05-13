import { Component, OnInit } from '@angular/core';
import { Prosumers } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { JwtToken } from 'src/app/utilities/jwt-token';

@Component({
  selector: 'app-dso-prediction-page',
  templateUrl: './dso-prediction-page.component.html',
  styleUrls: ['./dso-prediction-page.component.css']
})
export class DsoPredictionPageComponent implements OnInit{
  
  graph:boolean = true;
  tabelar:boolean = false;
  currentTime!: Date;
  updateUserDetail:Prosumers={
    id: 0,
    name: '',
    username: '',
    email: '',
    role: '',
    blocked: false,
    settlement:'',
    city:'',
    country: '',
    address:''
  }
  idUser!: number;
  role!: string;
  constructor(private authService:AuthService){}
  ngOnInit(): void {
    this.updateTime();
    setInterval(() => {
      this.updateTime();
    }, 1000);
    const { id, role } = new JwtToken().data;
    this.idUser = id as number;
    this.role = role as string;
    this.authService.getlogInUser().subscribe(response=>{
      this.updateUserDetail = {
        id: this.idUser,
        name: response.name,
        username: response.username,
        email: response.email,
        blocked: response.blocked,
        role: this.role,
        settlement: response.settlement,
        city: response.city,
        country: response.country,
        address: response.address
      }
     }
    )
  }
  updateTime() {
    this.currentTime = new Date();
  }
  showGraph(){
    this.graph = true;
    this.tabelar = false;
  }
  showTable(){
    this.graph = false;
    this.tabelar = true;
  }

  
}
