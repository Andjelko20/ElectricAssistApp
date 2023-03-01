import { Component, ElementRef, OnDestroy, OnInit, ViewChildren } from '@angular/core';
import { FormBuilder, FormControlName, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { Driver } from 'src/app/models/driver';
import { DriverService } from 'src/app/services/driver.service';

@Component({
  selector: 'app-driver-edit',
  templateUrl: './driver-edit.component.html',
  styleUrls: ['./driver-edit.component.css']
})
export class DriverEditComponent implements OnInit, OnDestroy {


  @ViewChildren(FormControlName, { read: ElementRef }) formInputElements!: ElementRef[];
  pageTitle = 'Driver Edit';
  errorMessage!: string;
  driverForm!: FormGroup;
  tranMode!: string;
  driver!: Driver;
  private sub!: Subscription;

  displayMessage: { [key: string]: string } = {};
  private validationMessages: { [key: string]: { [key: string]: string } };

  constructor(private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private driverService: DriverService) {

    this.validationMessages = {
      name: {
        required: 'Driver name is required.',
        minlength: 'Driver name minimum 3 characters',
        maxlength: 'Driver name maximum 20 characters.'
      }
    };
  }

  ngOnInit() {
    this.tranMode = "new";
    this.driverForm = this.fb.group({
      name: ['', [Validators.required, Validators.minLength(3), Validators.maxLength(20)]],nationality: "", championships: 0, number: '', yearOfBirth: 0, team: ''
    });

    this.sub = this.route.paramMap.subscribe(
      params => {
        const id = params.get('id');

        if (id == '0') {
          const driver: Driver = { id: "0", name: "", nationality: "", championships: 0, number: 0, yearOfBirth: 0,team: ""};
          this.displayDriver(driver);
        }
        else {
          this.getDriver(id);
        }
      }
    );
  }

  ngOnDestroy(): void {
    this.sub.unsubscribe();
  }

  getDriver(id: string | null): void {
    this.driverService.getDriver(id)
      .subscribe({
        next: (driver: Driver) => this.displayDriver(driver),
        error: (err) => this.errorMessage = <any>err,
        complete: () => console.info('getDriver works')
      });
  }

  displayDriver(driver: Driver): void {
    if (this.driverForm) {
      this.driverForm.reset();
    }
    this.driver = driver;
    if (this.driver.id == '0') {
      this.pageTitle = 'Add Driver';
    } else {
      this.pageTitle = `Edit Driver: ${this.driver.name}`;
    }
    this.driverForm.patchValue({
      name: this.driver.name,
      nationality: this.driver.nationality,
      championships: this.driver.championships,
      yearOfBirth: this.driver.yearOfBirth,
      number: this.driver.number,
      team: this.driver.team,
     
    });
  }

  deleteDriver(): void {
    if (this.driver.id == '0') {
      this.onSaveComplete();
    } else {
      if (confirm(`Are you sure want to delete this Driver: ${this.driver.name}?`)) {
        this.driverService.deleteDriver(this.driver.id)
          .subscribe({
            next: () => this.onSaveComplete(),
            error: (err) => this.errorMessage = <any>err,
            complete: () => console.info('deleteDriver works')
          });
      }
    }
  }

  saveDriver(): void {
    if (this.driverForm.valid) {
      if (this.driverForm.dirty) {
        const d = { ...this.driver, ...this.driverForm.value };
        if (d.id === '0') {
          this.driverService.createDriver(d)
            .subscribe({
              next: () => this.onSaveComplete(),
              error: (err) => this.errorMessage = <any>err,
              complete: () => console.info('createDriver works')
            });
        } else {
          this.driverService.updateDriver(d)
            .subscribe({
              next: () => this.onSaveComplete(),
              error: (err) => this.errorMessage = <any>err,
              complete: () => console.info('updateDriver works')
            });
        }
      } else {
        this.onSaveComplete();
      }
    } else {
      this.errorMessage = 'Error with create or update';
    }
  }

  onSaveComplete(): void {
    this.driverForm.reset();
    this.router.navigate(['/drivers']);
  }
}
