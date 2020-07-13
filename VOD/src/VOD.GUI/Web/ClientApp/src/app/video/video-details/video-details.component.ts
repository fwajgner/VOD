import {Component, Inject, OnInit} from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {VideoDetails} from '../model';
import {Config, CONFIG} from '../../model';
import {VideoService} from '../video.service';
import {AuthService} from '../../user/auth.service';
import {User} from '../../user/model';

@Component({
  selector: 'vid-video-details',
  templateUrl: './video-details.component.html',
  styleUrls: ['./video-details.component.css']
})
export class VideoDetailsComponent implements OnInit {
  videoDetails: VideoDetails;
  isSeries = false;
  mediaUrl: string;
  thumbnailLink = './assets/fake-images/fake-thumbnail-16x9.png';
  currentUser: User;

  constructor(private activatedRoute: ActivatedRoute,
              @Inject(CONFIG) private config: Config,
              private videoService: VideoService,
              private authService: AuthService) { }

  ngOnInit() {
    this.activatedRoute.data.subscribe(data => {
      this.videoDetails = data.videoDetails;
    });

    this.authService.currentUser.subscribe(x => this.currentUser = x);

    this.mediaUrl = this.videoService.getMedia(this.videoDetails.id);

    if (this.videoDetails.episode != null || this.videoDetails.season != null) {
      this.isSeries = true;
      this.thumbnailLink = './assets/fake-images/fake-thumbnail-a0.png';
    }
  }

}
