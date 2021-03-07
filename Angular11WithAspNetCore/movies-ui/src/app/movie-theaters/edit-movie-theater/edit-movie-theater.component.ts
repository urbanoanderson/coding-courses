import { Component, Input, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { movieTheaterCreationDTO, movieTheaterDTO } from '../movie-theater.model';

@Component({
  selector: 'app-edit-movie-theater',
  templateUrl: './edit-movie-theater.component.html',
  styleUrls: ['./edit-movie-theater.component.css']
})
export class EditMovieTheaterComponent implements OnInit {

  model: movieTheaterDTO = { name: 'Agora', latitude: 18.483541, longitude: -69.939275 };

  constructor(private activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(params => {
    });
  }

  saveChanges(movieTheater: movieTheaterCreationDTO) {
    console.log(movieTheater);
  }
}
