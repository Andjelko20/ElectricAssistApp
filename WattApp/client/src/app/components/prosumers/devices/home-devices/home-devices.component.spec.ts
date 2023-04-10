import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeDevicesComponent } from './home-devices.component';

describe('HomeDevicesComponent', () => {
  let component: HomeDevicesComponent;
  let fixture: ComponentFixture<HomeDevicesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HomeDevicesComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(HomeDevicesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
