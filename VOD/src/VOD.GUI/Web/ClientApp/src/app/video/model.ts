export interface VideoPage {
  total: number;
  pageSize: number;
  pageIndex: number;
  data: Video[];
}

export interface Video {
  id: string;
  pictureUri: string;
  title: string;
  altTitle: string;
  genre: Genre;
  kind: Kind;
}

export interface VideoDetails extends Video {
  duration: number;
  description: string;
  releaseYear: Date;
  season: number;
  episode: number;
}

export interface Genre {
  id: string;
  name: string;
}

export interface Kind {
  id: string;
  name: string;
}
