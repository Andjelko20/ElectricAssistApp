import { Component, OnInit } from '@angular/core';
import { Prosumers, Users } from 'src/app/models/users.model';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-all-prosumers',
  templateUrl: './all-prosumers.component.html',
  styleUrls: ['./all-prosumers.component.css']
})
export class AllProsumersComponent implements OnInit{

  prosumers:Prosumers[] = [];
  filteredProsumers: Prosumers[] = [];
  _listFilter = '';
  constructor(private userService:AuthService)
  {
    
  }
  
  get listFilter(): string {
    return this._listFilter;
  }
  performFilter(filterBy: string): Prosumers[] {
    filterBy = filterBy.toLocaleLowerCase();
    return this.prosumers.filter((prosumers: Prosumers) =>
    prosumers.name.toLocaleLowerCase().indexOf(filterBy) !== -1);
  }
  set listFilter(value: string) {
    this._listFilter = value;
    this.filteredProsumers = this.listFilter ? this.performFilter(this.listFilter) : this.prosumers;
  }
  ngOnInit(): void {
    this.getUsers();
  }
  getUsers(): void {
    this.userService.getAllProsumers().subscribe(prosumers => {
      this.prosumers=prosumers.data.map((u:any)=>({
        id: u.id,
        name: u.name,
        username: u.username,
        email: u.email,
        role: u.role,
        blocked: u.blocked,
        settlement:u.settlement,
        city:u.city,
        country: u.country
    }as Prosumers));
    this.filteredProsumers=prosumers.data.map((u:any)=>({
      id: u.id,
      name: u.name,
      username: u.username,
      email: u.email,
      role: u.role,
      blocked: u.blocked,
      settlement:u.settlement,
      city:u.city,
      country: u.country
     }as Prosumers));
    
     });
  }
}
