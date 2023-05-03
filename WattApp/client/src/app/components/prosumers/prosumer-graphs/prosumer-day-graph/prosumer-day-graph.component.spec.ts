import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProsumerDayGraphComponent } from './prosumer-day-graph.component';

describe('ProsumerDayGraphComponent', () => {
  let component: ProsumerDayGraphComponent;
  let fixture: ComponentFixture<ProsumerDayGraphComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProsumerDayGraphComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProsumerDayGraphComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
