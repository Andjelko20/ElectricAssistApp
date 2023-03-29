import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProsumerTowerComponent } from './prosumer-tower.component';

describe('ProsumerTowerComponent', () => {
  let component: ProsumerTowerComponent;
  let fixture: ComponentFixture<ProsumerTowerComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProsumerTowerComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProsumerTowerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
