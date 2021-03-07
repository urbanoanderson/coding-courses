import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { coordinatesMap } from 'src/app/utilities/map/coordinate';
import { movieTheaterCreationDTO, movieTheaterDTO } from '../movie-theater.model';

@Component({
  selector: 'app-form-movie-theater',
  templateUrl: './form-movie-theater.component.html',
  styleUrls: ['./form-movie-theater.component.css']
})
export class FormMovieTheaterComponent implements OnInit {

  form: FormGroup;

  @Input()
  model: movieTheaterDTO;

  @Output()
  onSaveChanges = new EventEmitter<movieTheaterCreationDTO>();

  initialCoordinates: coordinatesMap[] = [];

  constructor(private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      name: ['', {
        validators: [Validators.required]
      }],
      latitude: ['', {
        validators: [Validators.required]
      }],
      longitude: ['', {
        validators: [Validators.required]
      }]
    });

    if (this.model !== undefined) {
      this.form.patchValue(this.model);
      this.initialCoordinates.push({latitude: this.model.latitude, longitude: this.model.longitude});
    }
  }

  onSelectedLocation(coordinates: coordinatesMap) {
    this.form.patchValue(coordinates);
  }

  saveChanges() {
    this.onSaveChanges.emit(this.form.value);
  }

}
