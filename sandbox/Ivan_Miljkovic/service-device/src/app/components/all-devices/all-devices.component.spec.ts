import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllDevicesComponent } from './all-devices.component';

describe('AllDevicesComponent', () => {
  let component: AllDevicesComponent;
  let fixture: ComponentFixture<AllDevicesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AllDevicesComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AllDevicesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
