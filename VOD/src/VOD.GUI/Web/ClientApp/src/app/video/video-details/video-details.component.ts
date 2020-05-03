import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {VideoDetails} from '../model';

@Component({
  selector: 'vid-video-details',
  templateUrl: './video-details.component.html',
  styleUrls: ['./video-details.component.css']
})
export class VideoDetailsComponent implements OnInit {

  videoDetails: VideoDetails;
  isSeries = false;
  thumbnailLink = './assets/fake-images/fake-thumbnail-16x9.png';

  constructor(private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    this.activatedRoute.data.subscribe(data => {
      this.videoDetails = data.videoDetails;
    });

    if (this.videoDetails.episode != null || this.videoDetails.season != null) {
      this.isSeries = true;
      this.thumbnailLink = './assets/fake-images/fake-thumbnail-a0.png';
    }
  }

}
