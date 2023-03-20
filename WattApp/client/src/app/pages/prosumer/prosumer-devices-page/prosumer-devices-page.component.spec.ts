import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProsumerDevicesPageComponent } from './prosumer-devices-page.component';

describe('ProsumerDevicesPageComponent', () => {
  let component: ProsumerDevicesPageComponent;
  let fixture: ComponentFixture<ProsumerDevicesPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProsumerDevicesPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProsumerDevicesPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
