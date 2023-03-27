import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminDsoComponent } from './admin-dso.component';

describe('AdminDsoComponent', () => {
  let component: AdminDsoComponent;
  let fixture: ComponentFixture<AdminDsoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminDsoComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminDsoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
