import {Component, OnInit, Input, ViewChild} from '@angular/core';
import { Video } from '../model';
import { VideoService } from '../video.service';
import {PageEvent, MatPaginator} from '@angular/material/paginator';
import { HttpParams } from '@angular/common/http';

@Component({
  selector: 'vid-video-list',
  templateUrl: './video-list.component.html',
  styleUrls: ['./video-list.component.css']
})
export class VideoListComponent implements OnInit {
  @Input() class: string;
  title: string;
  videos: Video[];
  defaultPageIndex = 0;
  defaultPageSize = 30;
  imgSrc = './assets/fake-images/fake-thumbnail-16x9.png';
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(private videoService: VideoService) { }

  ngOnInit() {
     this.loadPage();
    }

  loadPage() {
      var pageEvent = new PageEvent();
      pageEvent.pageIndex = this.defaultPageIndex;
      pageEvent.pageSize = this.defaultPageSize;
      this.getData(pageEvent);
    }

  getData(event: PageEvent) {
    var params = new HttpParams()
      .set('pageIndex', event.pageIndex.toString())
      .set('pageSize', event.pageSize.toString());

  switch (this.class) {
    case 'movie':
      this.title = 'Movies';
      this.videoService.getMovies(params).subscribe(result => {
        this.paginator.length = result.total;
        this.paginator.pageIndex = result.pageIndex;
        this.paginator.pageSize = result.pageSize;
        this.videos = result.data;
      }, error1 => console.log(error1));
      break;
    case 'series':
      this.title = 'Series';
      this.videoService.getSeries(params).subscribe(result => {
        this.paginator.length = result.total;
        this.paginator.pageIndex = result.pageIndex;
        this.paginator.pageSize = result.pageSize;
        this.videos = result.data;
      }, error1 => console.log(error1));
      break;
    default:
      this.title = 'Videos';
      this.videoService.getVideos(params).subscribe(result => {
        this.paginator.length = result.total;
        this.paginator.pageIndex = result.pageIndex;
        this.paginator.pageSize = result.pageSize;
        this.videos = result.data;
      }, error1 => console.log(error1));
      break;
    }
  }
}
