import { ComponentFixture, TestBed } from '@angular/core/testing';

import { BarYearProsumerComponent } from './bar-year-prosumer.component';

describe('BarYearProsumerComponent', () => {
  let component: BarYearProsumerComponent;
  let fixture: ComponentFixture<BarYearProsumerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ BarYearProsumerComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(BarYearProsumerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
