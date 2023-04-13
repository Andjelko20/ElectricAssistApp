import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProsumerAccountPageComponent } from './prosumer-account-page.component';

describe('ProsumerAccountPageComponent', () => {
  let component: ProsumerAccountPageComponent;
  let fixture: ComponentFixture<ProsumerAccountPageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProsumerAccountPageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProsumerAccountPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
