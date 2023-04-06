import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProsumerDevicePageComponent } from './prosumer-device-page.component';

describe('ProsumerDevicePageComponent', () => {
  let component: ProsumerDevicePageComponent;
  let fixture: ComponentFixture<ProsumerDevicePageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProsumerDevicePageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProsumerDevicePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
