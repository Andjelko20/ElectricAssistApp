import { Component, OnInit } from '@angular/core';
import { Users } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-all-prosumers',
  templateUrl: './all-prosumers.component.html',
  styleUrls: ['./all-prosumers.component.css']
})
export class AllProsumersComponent implements OnInit{

  users:Users[] = [{
    id: 1,
    name: '',
    userName: '',
    password:'',
    block: false,
    role:''
  }];
  filteredDrivers: Users[] = [];
  _listFilter = '';
  constructor(private userService:AuthService)
  {
    
  }
  
  get listFilter(): string {
    return this._listFilter;
  }
  performFilter(filterBy: string): Users[] {
    filterBy = filterBy.toLocaleLowerCase();
    return this.users.filter((users: Users) =>
    users.name.toLocaleLowerCase().indexOf(filterBy) !== -1);
  }
  set listFilter(value: string) {
    this._listFilter = value;
    this.filteredDrivers = this.listFilter ? this.performFilter(this.listFilter) : this.users;
  }
  ngOnInit(): void {
    this.getUsers();
  }
  getUsers(): void {
    this.userService.getAllUsers().subscribe(users => {
      this.users=users.data.map((u:any)=>({
     id:u.id,
     name:u.name,
     userName:u.username,
     password:u.password,
     block:u.blocked,
     role:u.role
    }as Users));
    this.filteredDrivers=users.data.map((u:any)=>({
      id:u.id,
      name:u.name,
      userName:u.username,
      password:u.password,
      block:u.blocked,
      role:u.role
     }as Users));
    
     });
  }
}
