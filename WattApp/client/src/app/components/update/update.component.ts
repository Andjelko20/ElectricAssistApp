import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { Users } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-update',
  templateUrl: './update.component.html',
  styleUrls: ['./update.component.css']
})
export class UpdateComponent implements OnInit {
  updateUserDetail:Users={
    id:0,
    name:'',
    userName:'',
    password:'',
    block:false,
    role:''
  }
  constructor(private route:ActivatedRoute,private router:Router,private updateService:AuthService) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe({
      next:(params)=>
      {
        if(this.updateUserDetail.id)
        {
          this.updateService.getUsers(this.updateUserDetail.id)
          .subscribe({
            next:(response)=>{
              this.updateUserDetail=response;
            }
          });
        }
      }
    })
  }
  
  
  upDate()
  {
    // this.onSelectedBlock(this.updateUserDetail.block);
    // console.log("Azuriran objekat: ",this.updateUserDetail);
    this.updateService.upDate(this.updateUserDetail.id,this.updateUserDetail)
    .subscribe({
      next:(response)=>{
        this.router.navigate(['home']);
      }
    });
  }

}
