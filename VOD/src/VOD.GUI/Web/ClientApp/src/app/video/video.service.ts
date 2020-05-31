import { Injectable, Inject } from '@angular/core';
import {HttpClient, HttpParams} from '@angular/common/http';
import { CONFIG, Config } from '../model';
import {VideoDetails, VideoPage} from './model';

@Injectable({
  providedIn: 'root'
})
export class VideoService {

  constructor(private httpClient: HttpClient, @Inject(CONFIG) private config: Config) { }

  getVideos(params?: HttpParams) {
   return this.httpClient.get<VideoPage>(`${this.config.apiUrl}/videos`, { params });
  }

  getVideoById(id: string) {
    return this.httpClient.get<VideoDetails>(`${this.config.apiUrl}/videos/${id}`);
  }

  getMovies(params?: HttpParams) {
    return this.httpClient.get<VideoPage>(`${this.config.apiUrl}/kinds/13ae2883-f1c0-459a-01b5-08d804c04036/videos`, { params });
  }

  getSeries(params?: HttpParams) {
    return this.httpClient.get<VideoPage>(`${this.config.apiUrl}/kinds/78f4cb5f-121a-4fe2-01b6-08d804c04036/videos`, { params });
  }

  getMedia(id: string) {
    return `${this.config.apiUrl}/videos/${id}/media`;
  }
}
