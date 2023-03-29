import { TestBed } from '@angular/core/testing';

import { DispatcherGuard } from './dispatcher.guard';

describe('DispatcherGuard', () => {
  let guard: DispatcherGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(DispatcherGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
