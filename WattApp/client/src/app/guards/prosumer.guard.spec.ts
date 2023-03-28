import { TestBed } from '@angular/core/testing';

import { ProsumerGuard } from './prosumer.guard';

describe('ProsumerGuard', () => {
  let guard: ProsumerGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(ProsumerGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
