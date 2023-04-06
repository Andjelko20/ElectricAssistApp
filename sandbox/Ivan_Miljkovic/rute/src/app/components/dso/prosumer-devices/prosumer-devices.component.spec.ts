import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProsumerDevicesComponent } from './prosumer-devices.component';

describe('ProsumerDevicesComponent', () => {
  let component: ProsumerDevicesComponent;
  let fixture: ComponentFixture<ProsumerDevicesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProsumerDevicesComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProsumerDevicesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
