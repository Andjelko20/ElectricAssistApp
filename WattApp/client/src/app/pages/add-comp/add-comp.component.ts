import { Component, OnInit } from '@angular/core';
import { Users } from 'src/app/models/users.model';

@Component({
  selector: 'app-add-comp',
  templateUrl: './add-comp.component.html',
  styleUrls: ['./add-comp.component.css']
})
export class AddCompComponent implements OnInit {
  addUserRequest:Users={
    id:'',
    name:'',
    userName:'',
    block:'',
    role:''
  }
  constructor() { }

  ngOnInit(): void {
  }

  addUsers()
  {
    console.log(this.addUserRequest);
  }
}
