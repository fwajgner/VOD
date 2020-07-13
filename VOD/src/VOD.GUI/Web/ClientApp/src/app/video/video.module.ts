import { NgModule } from '@angular/core';
import { VideoListComponent } from './video-list/video-list.component';
import { Routes, RouterModule } from '@angular/router';
import { SharedModule } from '../shared/shared.module';
import { NavMenuVideoComponent } from './nav-menu-video/nav-menu-video.component';
import { VideoDetailsComponent } from './video-details/video-details.component';
import { VideoService } from './video.service';
import { VideoResolver } from './video-resolver.service';
import { VideoHomeComponent } from './video-home/video-home.component';
import { MovieComponent } from './movie/movie.component';
import { SeriesComponent } from './series/series.component';

const routes: Routes = [
  {
    path: '', component: VideoHomeComponent, children: [
      { path: '', redirectTo: 'movies'},
      { path: 'movies', component: MovieComponent },
      { path: 'series', component: SeriesComponent }
    ]
  },
  {
    path: 'movies/:id', component: VideoDetailsComponent,
    resolve: {
      videoDetails: VideoResolver
    }
  },
  {
    path: 'series/:id', component: VideoDetailsComponent,
    resolve: {
      videoDetails: VideoResolver
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
    SeriesComponent
  ],
  imports: [
    SharedModule,
    RouterModule.forChild(routes)
  ],
  exports: [
    VideoHomeComponent
  ],
  providers: [
    VideoService
  ]
})
export class VideoModule { }
