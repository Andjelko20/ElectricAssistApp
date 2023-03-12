import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddCompComponent } from './add-comp.component';

describe('AddCompComponent', () => {
  let component: AddCompComponent;
  let fixture: ComponentFixture<AddCompComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AddCompComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddCompComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
