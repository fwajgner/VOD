import { TestBed } from '@angular/core/testing';

import { SeriesResolverService } from './series-resolver.service';

describe('SeriesResolverService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: SeriesResolverService = TestBed.get(SeriesResolverService);
    expect(service).toBeTruthy();
  });
});
