import { TestBed } from '@angular/core/testing';

import { VideoResolver } from './video-resolver.service';

describe('VideoResolverService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: VideoResolver = TestBed.get(VideoResolver);
    expect(service).toBeTruthy();
  });
});
