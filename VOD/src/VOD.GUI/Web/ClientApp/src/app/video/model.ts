export interface Video {
  id: string;
  pictureUri: string;
  title: string;
  altTitle: string;
  genre: string;
  kind: string;
}

export interface VideoDetails extends Video {
  duration: number;
  description: string;
  releaseYear: Date;
  season: number;
  episode: number;
}
