import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProsumerChangePasswordComponent } from './prosumer-change-password.component';

describe('ProsumerChangePasswordComponent', () => {
  let component: ProsumerChangePasswordComponent;
  let fixture: ComponentFixture<ProsumerChangePasswordComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProsumerChangePasswordComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProsumerChangePasswordComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
