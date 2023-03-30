import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MapInputComponent } from './map-input.component';

describe('ProsumersMapComponent', () => {
  let component: MapInputComponent;
  let fixture: ComponentFixture<MapInputComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ MapInputComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MapInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
