import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProsumerHouseComponent } from './prosumer-house.component';

describe('ProsumerHouseComponent', () => {
  let component: ProsumerHouseComponent;
  let fixture: ComponentFixture<ProsumerHouseComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProsumerHouseComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProsumerHouseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
