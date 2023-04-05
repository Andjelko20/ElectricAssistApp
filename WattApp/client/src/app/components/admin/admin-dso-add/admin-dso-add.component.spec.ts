import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminDsoAddComponent } from './admin-dso-add.component';

describe('AdminDsoAddComponent', () => {
  let component: AdminDsoAddComponent;
  let fixture: ComponentFixture<AdminDsoAddComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminDsoAddComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminDsoAddComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
