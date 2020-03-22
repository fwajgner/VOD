import { NgModule } from '@angular/core';
import { VideoListComponent } from './video-list/video-list.component';
import { Routes, RouterModule } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { NavMenuVideoComponent } from './nav-menu-video/nav-menu-video.component';
import { VideoDetailsComponent } from './video-details/video-details.component';
import { VideoService } from './video.service';
import { MovieResolver } from './movie-resolver.service';
import { VideoHomeComponent } from './video-home/video-home.component';
import { SeriesResolver } from './series-resolver.service';
import { MovieComponent } from './movie/movie.component';
import { SeriesComponent } from './series/series.component';
import {MaterialModule} from '../material/material.module';

const routes: Routes = [
  {
    path: '', component: VideoHomeComponent, children: [
      { path: '', redirectTo: 'movie'},
      { path: 'movie', component: MovieComponent },
      { path: 'series', component: SeriesComponent }
    ]
  },
  {
    path: 'movie/:altTitle', component: VideoDetailsComponent,
    resolve: {
      videoDetails: MovieResolver
    }
  },
  {
    path: 'series/:altTitle', component: VideoDetailsComponent,
    resolve: {
      videoDetails: SeriesResolver
    }
  }
];

@NgModule({
  declarations: [
    VideoListComponent,
    NavMenuVideoComponent,
    VideoDetailsComponent,
    VideoHomeComponent,
    MovieComponent,
    SeriesComponent,
  ],
  imports: [
    SharedModule,
    RouterModule.forChild(routes),
  ],
  exports: [
    VideoHomeComponent
  ],
  providers: [
    VideoService
  ]
})
export class VideoModule { }
