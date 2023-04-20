import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TodayDsoComponent } from './today-dso.component';

describe('TodayDsoComponent', () => {
  let component: TodayDsoComponent;
  let fixture: ComponentFixture<TodayDsoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TodayDsoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TodayDsoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
