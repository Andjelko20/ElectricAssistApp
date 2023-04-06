import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DsoOneProsumerPageComponent } from './dso-one-prosumer-page.component';

describe('DsoOneProsumerPageComponent', () => {
  let component: DsoOneProsumerPageComponent;
  let fixture: ComponentFixture<DsoOneProsumerPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DsoOneProsumerPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DsoOneProsumerPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
