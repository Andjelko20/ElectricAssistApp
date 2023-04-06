import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminDsoUpdateComponent } from './admin-dso-update.component';

describe('AdminDsoUpdateComponent', () => {
  let component: AdminDsoUpdateComponent;
  let fixture: ComponentFixture<AdminDsoUpdateComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminDsoUpdateComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminDsoUpdateComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
