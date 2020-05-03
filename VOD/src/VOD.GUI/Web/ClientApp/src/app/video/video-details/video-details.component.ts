import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import {Movie, Series} from '../model';

@Component({
  selector: 'vid-video-details',
  templateUrl: './video-details.component.html',
  styleUrls: ['./video-details.component.css']
})
export class VideoDetailsComponent implements OnInit {

  videoDetails: Movie | Series;
  isSeries = false;

  constructor(private activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    this.activatedRoute.data.subscribe(data => {
      this.videoDetails = data.videoDetails;
    });

    if ('episode' in this.videoDetails && 'season' in this.videoDetails) {
      this.isSeries = true;
    }
  }

}
