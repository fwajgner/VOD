import { Injectable } from '@angular/core';
import { VideoService } from './video.service';
import { Series } from './model';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SeriesResolver implements Resolve<Series> {

  constructor(private videoService: VideoService) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Series | Observable<Series> | Promise<Series> {
    const altTitle = route.params['altTitle'];
    return this.videoService.getSeriesWithAltTitle(altTitle);
  }
}
