import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { movieTheaterCreationDTO, movieTheaterDTO } from './movie-theater.model';

@Injectable({
  providedIn: 'root'
})

export class MovieTheatersService {

  private apiURL = environment.apiURL + '/movietheaters';

  constructor(private http: HttpClient) { }

  get(): Observable<movieTheaterDTO[]> {
    return this.http.get<movieTheaterDTO[]>(this.apiURL);
  }

  getById(id: number): Observable<movieTheaterDTO> {
    return this.http.get<movieTheaterDTO>(`${this.apiURL}/${id}`);
  }

  create(movieTheaterCreationDTO: movieTheaterCreationDTO) {
    return this.http.post(this.apiURL, movieTheaterCreationDTO);
  }

  edit(id: number, movieTheater: movieTheaterCreationDTO) {
    return this.http.put(`${this.apiURL}/${id}`, movieTheater);
  }

  delete(id: number) {
    return this.http.delete(`${this.apiURL}/${id}`);
  }

}
