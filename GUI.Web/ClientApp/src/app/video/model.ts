export interface Video {
  title: string;
  altTitle: string;
  genres: string[];
  type: string;
}

export interface VideoDetails extends Video {
  duration: number;
  description: string;
  releaseYear: Date;
}

export interface Movie extends VideoDetails {

}

export interface Series extends VideoDetails {
  season: number;
  episode: number;
}
