import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProsumerHomePageComponent } from './prosumer-home-page.component';

describe('ProsumerHomePageComponent', () => {
  let component: ProsumerHomePageComponent;
  let fixture: ComponentFixture<ProsumerHomePageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProsumerHomePageComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProsumerHomePageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
