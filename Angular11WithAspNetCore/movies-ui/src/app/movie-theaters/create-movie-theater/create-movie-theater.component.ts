import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { movieTheaterCreationDTO } from '../movie-theater.model';
import { MovieTheatersService } from '../movie-theaters.service';

@Component({
  selector: 'app-create-movie-theater',
  templateUrl: './create-movie-theater.component.html',
  styleUrls: ['./create-movie-theater.component.css']
})
export class CreateMovieTheaterComponent implements OnInit {

  constructor(
    private movieTheatersService: MovieTheatersService,
    private router: Router) { }

  ngOnInit(): void {
  }

  saveChanges(movieTheater: movieTheaterCreationDTO) {
    this.movieTheatersService.create(movieTheater).subscribe(() => {
      this.router.navigate(['/movietheaters']);
    });
  }

}
