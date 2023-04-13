import { TestBed } from '@angular/core/testing';

import { HistoryPredictionService } from './history-prediction.service';

describe('HistoryPredictionService', () => {
  let service: HistoryPredictionService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(HistoryPredictionService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
