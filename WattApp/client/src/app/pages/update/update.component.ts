import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { Users } from 'src/app/models/users.model';

@Component({
  selector: 'app-update',
  templateUrl: './update.component.html',
  styleUrls: ['./update.component.css']
})
export class UpdateComponent implements OnInit {
  updateUserRequest:Users={
    id:'',
    name:'',
    userName:'',
    block:'',
    role:''
  }
  constructor(private route:ActivatedRoute,private router:Router) { }

  ngOnInit(): void {
  }
  upDate()
  {

  }

}
