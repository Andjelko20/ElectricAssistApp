import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllProsumersComponent } from './all-prosumers.component';

describe('AllProsumersComponent', () => {
  let component: AllProsumersComponent;
  let fixture: ComponentFixture<AllProsumersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AllProsumersComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AllProsumersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
