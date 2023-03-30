import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Wattmeter1Component } from './wattmeter1.component';

describe('Wattmeter1Component', () => {
  let component: Wattmeter1Component;
  let fixture: ComponentFixture<Wattmeter1Component>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ Wattmeter1Component ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Wattmeter1Component);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
