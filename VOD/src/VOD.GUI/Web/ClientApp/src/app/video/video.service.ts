import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CONFIG, Config } from '../model';
import {Video, VideoDetails} from './model';

@Injectable({
  providedIn: 'root'
})
export class VideoService {

  constructor(private httpClient: HttpClient, @Inject(CONFIG) private config: Config) { }

  getVideos() {
   return this.httpClient.get<Video[]>(`${this.config.apiUrl}/videos`);
  }

  getVideoById(id: string) {
    return this.httpClient.get<VideoDetails>(`${this.config.apiUrl}/videos/${id}`);
  }

  getMovies() {
    return this.httpClient.get<Video[]>(`${this.config.apiUrl}/kinds/4e3b9799-4abc-4db1-734b-08d7eccf2be6/videos`);
  }

  getSeries() {
    return this.httpClient.get<Video[]>(`${this.config.apiUrl}/kinds/ba9e4454-4f3f-4a55-734c-08d7eccf2be6/videos`);
  }
}
