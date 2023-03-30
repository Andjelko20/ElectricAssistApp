import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProsumerOneDevicePageComponent } from './prosumer-one-device-page.component';

describe('ProsumerOneDevicePageComponent', () => {
  let component: ProsumerOneDevicePageComponent;
  let fixture: ComponentFixture<ProsumerOneDevicePageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProsumerOneDevicePageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProsumerOneDevicePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
