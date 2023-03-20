import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DsoHomePageComponent } from './dso-home-page.component';

describe('DsoHomePageComponent', () => {
  let component: DsoHomePageComponent;
  let fixture: ComponentFixture<DsoHomePageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ DsoHomePageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DsoHomePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
