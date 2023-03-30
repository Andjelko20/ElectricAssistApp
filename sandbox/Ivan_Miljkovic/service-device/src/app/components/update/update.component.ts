import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { Users } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-update',
  templateUrl: './update.component.html',
  styleUrls: ['./update.component.css']
})
export class UpdateComponent implements OnInit {
	roles:Array<any>=[];
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
		fetch(environment.serverUrl+"/api/users/roles",{headers:{"Authorization":"Bearer "+localStorage.getItem("token")}})
		.then(res=>res.json())
		.then(res=>{
			this.roles=res;
          this.updateService.getUser( Number(this.route.snapshot.paramMap.get('id')) )
          .subscribe({
            next:(response)=>{
              this.updateUserDetail={
				id:Number(this.route.snapshot.paramMap.get('id')),
				name:response.name,
				userName:response.username,
				password:"",
				block:response.blocked,
				role:this.roles.find(r=>r.name==response.role)?.id
			  };
            },
			error:(response)=>{
				this.router.navigate(["/home"]);
			}
          });
		});
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
