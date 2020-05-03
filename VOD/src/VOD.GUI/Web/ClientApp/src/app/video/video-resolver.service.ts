import { Injectable } from '@angular/core';
import { VideoService } from './video.service';
import { VideoDetails } from './model';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class VideoResolver implements Resolve<VideoDetails> {

  constructor(private videoService: VideoService) { }

  resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): VideoDetails | Observable<VideoDetails> | Promise<VideoDetails> {
    const id = route.params['id'];
    return this.videoService.getVideoById(id);
    }
}
