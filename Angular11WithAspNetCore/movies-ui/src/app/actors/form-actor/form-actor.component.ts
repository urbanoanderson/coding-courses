import { EventEmitter, Input } from '@angular/core';
import { Component, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { actorCreationDTO } from '../actor.model';

@Component({
  selector: 'app-form-actor',
  templateUrl: './form-actor.component.html',
  styleUrls: ['./form-actor.component.css']
})
export class FormActorComponent implements OnInit {

  form: FormGroup;

  @Input()
  model: actorCreationDTO;
  urlCurrentImage: string;

  @Output()
  onSaveChanges = new EventEmitter<actorCreationDTO>();

  constructor(private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      name: ['', {
        validators: [Validators.required]
      }],
      dateOfBirth: '',
      picture: '',
      biography: ''
    });

    if (this.model !== undefined) {
      this.form.patchValue(this.model);
    }
  }

  onImageSelected(image) {
    this.form.get('picture').setValue(image);
  }

  changeMarkdown(content) {
    this.form.get('biography').setValue(content);
  }

  saveChanges() {
    this.onSaveChanges.emit(this.form.value);
  }

}
