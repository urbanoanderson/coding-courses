import { Component, OnInit } from '@angular/core';
import { movieTheaterCreationDTO } from '../movie-theater.model';

@Component({
  selector: 'app-create-movie-theater',
  templateUrl: './create-movie-theater.component.html',
  styleUrls: ['./create-movie-theater.component.css']
})
export class CreateMovieTheaterComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

  saveChanges(movieTheater: movieTheaterCreationDTO) {
    console.log(movieTheater);
  }

}
