import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NumberOfUsersMeterComponent } from './number-of-users-meter.component';

describe('NumberOfUsersMeterComponent', () => {
  let component: NumberOfUsersMeterComponent;
  let fixture: ComponentFixture<NumberOfUsersMeterComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ NumberOfUsersMeterComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NumberOfUsersMeterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
