import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllDevicesDsoComponent } from './all-devices-dso.component';

describe('AllDevicesDsoComponent', () => {
  let component: AllDevicesDsoComponent;
  let fixture: ComponentFixture<AllDevicesDsoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AllDevicesDsoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AllDevicesDsoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
