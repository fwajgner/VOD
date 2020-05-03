import { Injectable, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CONFIG, Config } from '../model';
import { Video, Movie, Series } from './model';

@Injectable({
  providedIn: 'root'
})
export class VideoService {

  constructor(private httpClient: HttpClient, @Inject(CONFIG) private config: Config) { }

 // getVideos() {
 //   return this.httpClient.get<Video[]>(`${this.config.apiUrl}/video`);
 // }

  getMovies() {
    return this.httpClient.get<Video[]>(`${this.config.apiUrl}/video/movie`);
  }

  getMoviesWithTitle(title: string) {
    return this.httpClient.get<Movie[]>(`${this.config.apiUrl}/video/movie/with-title/${title}`);
  }

  getMoviesWithAltTitle(altTitle: string) {
    return this.httpClient.get<Movie>(`${this.config.apiUrl}/video/movie/${altTitle}`);
  }

  getSeries() {
    return this.httpClient.get<Video[]>(`${this.config.apiUrl}/video/series`);
  }

  getSeriesWithTitle(title: string) {
    return this.httpClient.get<Series[]>(`${this.config.apiUrl}/video/series/with-title/${title}`);
  }

  getSeriesWithAltTitle(altTitle: string) {
    return this.httpClient.get<Series>(`${this.config.apiUrl}/video/series/${altTitle}`);
  }
}
