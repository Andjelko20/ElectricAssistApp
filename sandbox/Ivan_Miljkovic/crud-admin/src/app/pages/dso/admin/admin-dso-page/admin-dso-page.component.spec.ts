import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminDsoPageComponent } from './admin-dso-page.component';

describe('AdminDsoPageComponent', () => {
  let component: AdminDsoPageComponent;
  let fixture: ComponentFixture<AdminDsoPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ AdminDsoPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdminDsoPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
