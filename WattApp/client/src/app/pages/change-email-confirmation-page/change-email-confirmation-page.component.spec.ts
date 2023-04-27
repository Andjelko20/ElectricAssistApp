import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ChangeEmailConfirmationPageComponent } from './change-email-confirmation-page.component';

describe('ChangeEmailConfirmationPageComponent', () => {
  let component: ChangeEmailConfirmationPageComponent;
  let fixture: ComponentFixture<ChangeEmailConfirmationPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ChangeEmailConfirmationPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ChangeEmailConfirmationPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
