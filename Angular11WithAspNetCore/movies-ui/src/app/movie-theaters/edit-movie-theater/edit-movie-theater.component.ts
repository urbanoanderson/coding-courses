import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { movieTheaterCreationDTO, movieTheaterDTO } from '../movie-theater.model';
import { MovieTheatersService } from '../movie-theaters.service';

@Component({
  selector: 'app-edit-movie-theater',
  templateUrl: './edit-movie-theater.component.html',
  styleUrls: ['./edit-movie-theater.component.css']
})
export class EditMovieTheaterComponent implements OnInit {

  model: movieTheaterDTO;

  constructor(
    private activatedRoute: ActivatedRoute,
    private movieTheatersService: MovieTheatersService,
    private router: Router) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(params => {
      this.movieTheatersService.getById(params.id).subscribe(movieTheater => {
        this.model = movieTheater;
      });
    });
  }

  saveChanges(movieTheater: movieTheaterCreationDTO) {
    this.movieTheatersService.edit(this.model.id, movieTheater).subscribe(() => {
      this.router.navigate(['/movietheaters']);
    });
  }
}
