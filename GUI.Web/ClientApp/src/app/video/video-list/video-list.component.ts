import { Component, OnInit, Input } from '@angular/core';
import { Video } from '../model';
import { VideoService } from '../video.service';

@Component({
  selector: 'vid-video-list',
  templateUrl: './video-list.component.html',
  styleUrls: ['./video-list.component.css']
})
export class VideoListComponent implements OnInit {
  @Input() class: string;
  title: string;
  videos: Video[];

  constructor(private videoService: VideoService) { }

  ngOnInit() {
    switch (this.class) {

      case 'movie':
      default:
        this.title = 'Movies';
        this.videoService.getMovies().subscribe(result => {
          this.videos = result;
        }, error1 => console.log(error1));
        break;
      case 'series':
        this.title = 'Series';
        this.videoService.getSeries().subscribe(result => {
          this.videos = result;
        }, error1 => console.log(error1));
        break;
    }
  }

}
