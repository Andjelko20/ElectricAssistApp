import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, NavigationEnd, NavigationStart, Router } from '@angular/router';
import { Location } from '@angular/common';
@Component({
  selector: 'app-prosumer-footer',
  templateUrl: './prosumer-footer.component.html',
  styleUrls: ['./prosumer-footer.component.css']
})
export class ProsumerFooterComponent implements OnInit {
  showLink:boolean=false;
  showLink1:boolean=false;
  id?:number;
  previousRoute:string=''
  constructor(public router:Router,
    public route:ActivatedRoute,private location: Location) { 
      
    }
  ngOnInit(): void {
    
    this.id=Number(this.route.snapshot.paramMap.get('id'))
    if(this.location.path()==='/devices')
    {
      this.showLink=false;
    }
    else if(this.location.path()==='/device/'+this.id || this.location.path()==='/device-update/'+this.id || this.location.path()==='/device-add')
    {
      this.showLink=true;
    }
    else if(this.location.path()==='/profile')
    {
      this.showLink1=false
    }
    else if(this.location.path()==='/profile-edit' || this.location.path()==='/prosumer-change-password')
    {
      this.showLink1=true
    }
    
  }
  
  
}
