import { Component, OnInit } from '@angular/core';
import { Driver } from 'src/app/models/driver';
import { DriverService } from 'src/app/services/driver.service';

@Component({
  selector: 'app-driver-list',
  templateUrl: './driver-list.component.html',
  styleUrls: ['./driver-list.component.css']
})
export class DriverListComponent implements OnInit {

  pageTitle = 'Driver List';
  filteredDrivers: Driver[] = [];
  drivers: Driver[] = [];
  errorMessage = '';
  _listFilter = '';


  constructor(private driverService: DriverService) {
  }

 
  performFilter(filterBy: string): Driver[] {
    filterBy = filterBy.toLocaleLowerCase();
    return this.drivers.filter((driver: Driver) =>driver.name.toLocaleLowerCase().indexOf(filterBy) !== -1);
  }
  get listFilter(): string {
    return this._listFilter;
  }
  set listFilter(value: string) {
    this._listFilter = value;
    this.filteredDrivers = this.listFilter ? this.performFilter(this.listFilter) : this.drivers;
  }
  

  ngOnInit() :  void{
    this.getDriver();

  }
  public getDriver()
  {
    this.driverService.getDrivers().subscribe({next:(drivers)=>{
      this.drivers=drivers;
      this.filteredDrivers=drivers;
    },
    error: (err) => this.errorMessage = <any>err,
    complete: () => console.info('getDrivers works')
  })
  }
  deleteDriver(id: string, name: string): void {
    if (id === '') {
      this.onSaveComplete();
    } else {
      if (confirm(`Are you sure want to delete this Driver: ${name}?`)) {
        this.driverService.deleteDriver(id)
          .subscribe({
            next: () => this.onSaveComplete(),
            error: (err) => this.errorMessage = <any>err,
            complete: () => console.info('deleteDriver works')
          });
      }
    }
  }
  onSaveComplete(): void {
    this.driverService.getDrivers()
      .subscribe({
        next: (drivers) => {
          this.drivers = drivers;
          this.filteredDrivers = drivers;
        },
        error: (err) => this.errorMessage = <any>err,
        complete: () => console.info('getDrivers works')
      });
  }
}
