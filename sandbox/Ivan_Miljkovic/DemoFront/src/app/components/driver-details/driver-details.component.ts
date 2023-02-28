import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Driver } from 'src/app/models/driver';
import { DriverService } from 'src/app/services/driver.service';

@Component({
  selector: 'app-driver-details',
  templateUrl: './driver-details.component.html',
  styleUrls: ['./driver-details.component.css']
})
export class DriverDetailsComponent implements OnInit{

  pageTitle = 'Driver Detail';
  errorMessage = '';
  driver: Driver | undefined;

  constructor(private route: ActivatedRoute, private router: Router, private driverService: DriverService) {
   }

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.getDriver(id);
    }
  }

  getDriver(id: string) {
    this.driverService.getDriver(id).subscribe({
        next: (driver) => this.driver = driver,
        error: (err) => this.errorMessage = <any>err,
        complete: () => console.info('getDriver details works')
      });
  }

  goBack(): void {
    this.router.navigate(['/drivers']);
  }
}
