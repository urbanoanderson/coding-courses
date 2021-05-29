import { actorsMovieDTO } from "../actors/actor.model";
import { genreDTO } from "../genres/genre.model";
import { movieTheaterDTO } from "../movie-theaters/movie-theater.model";

export interface movieCreationDTO {
  title: string;
  summary: string;
  poster: File;
  inTheaters: boolean;
  releaseDate: Date;
  trailer: string;
  genresIds: number[];
  movieTheatersIds: number[];
  actors: actorsMovieDTO[];
}

export interface movieDTO {
  title: string;
  summary: string;
  poster: string;
  inTheaters: boolean;
  releaseDate: Date;
  trailer: string;
}

export interface moviePostGetDTO {
  genres: genreDTO[];
  movieTheaters: movieTheaterDTO[];
}
