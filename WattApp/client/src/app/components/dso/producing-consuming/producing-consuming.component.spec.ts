import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ProducingConsumingComponent } from './producing-consuming.component';

describe('ProducingConsumingComponent', () => {
  let component: ProducingConsumingComponent;
  let fixture: ComponentFixture<ProducingConsumingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ProducingConsumingComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ProducingConsumingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
