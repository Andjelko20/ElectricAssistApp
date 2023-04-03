import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DsoOneProsumerDevicesPageComponent } from './dso-one-prosumer-devices-page.component';

describe('DsoOneProsumerDevicesPageComponent', () => {
  let component: DsoOneProsumerDevicesPageComponent;
  let fixture: ComponentFixture<DsoOneProsumerDevicesPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DsoOneProsumerDevicesPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DsoOneProsumerDevicesPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
