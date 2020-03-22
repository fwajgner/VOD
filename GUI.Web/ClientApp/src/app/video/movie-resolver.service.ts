import { Injectable } from '@angular/core';
import { VideoService } from './video.service';
import { Movie } from './model';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class MovieResolver implements Resolve<Movie> {

  constructor(private videoService: VideoService) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Movie | Observable<Movie> | Promise<Movie> {
    const altTitle = route.params['altTitle'];
    return this.videoService.getMoviesWithAltTitle(altTitle);
    }
}
