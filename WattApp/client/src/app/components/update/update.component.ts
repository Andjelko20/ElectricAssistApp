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
    id:'',
    name:'',
    userName:'',
    password:'',
    block:'',
    role:''
  }
  constructor(private route:ActivatedRoute,private router:Router,private updateService:AuthService) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe({
      next:(params)=>
      {
        const id = params.get('id');
        if(id)
        {
          this.updateService.getUsers(id)
          .subscribe({
            next:(response)=>{
              this.updateUserDetail=response;
            }
          });
        }
      }
    })
  }
  onSelectedBlock(value:string):void
  {
    this.updateUserDetail.block = value;
  }
  onSelectedRole(value:string):void
  {
    this.updateUserDetail.role = value;
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
