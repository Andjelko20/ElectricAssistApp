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
    else if(this.location.path()==='/device/'+this.id)
    {
      this.showLink=true;
    }
    
  }
  
  
}
