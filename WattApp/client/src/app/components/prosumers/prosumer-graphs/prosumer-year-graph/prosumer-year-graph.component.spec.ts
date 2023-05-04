import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProsumerYearGraphComponent } from './prosumer-year-graph.component';

describe('ProsumerYearGraphComponent', () => {
  let component: ProsumerYearGraphComponent;
  let fixture: ComponentFixture<ProsumerYearGraphComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProsumerYearGraphComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProsumerYearGraphComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
