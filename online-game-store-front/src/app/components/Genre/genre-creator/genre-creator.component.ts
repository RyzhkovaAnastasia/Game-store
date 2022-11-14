import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Guid } from 'guid-typescript';
import { ToastrService } from 'ngx-toastr';
import { Genre } from 'src/app/classes/genre';
import { FormService } from 'src/app/services/form.service';

import { GenreService } from 'src/app/services/genre.service';
@Component({
  selector: 'app-genre-creator',
  templateUrl: './genre-creator.component.html',
  styleUrls: ['./genre-creator.component.css']
})
export class GenreCreatorComponent {

  public genreForm: FormGroup;
  public genres: Genre[] = [];

  constructor(
    private readonly _genreService: GenreService,
    private readonly _toastr: ToastrService,
    private readonly _formService: FormService
  ) {
    this.getGenres();
    this.genreForm = this.formInit();
    this._formService.setToForm(new Genre(), this.genreForm);
  }

  public createGenre(): void {
    if (this.genreForm.valid) {
      let genre = new Genre();
      this._formService.getFromForm(genre, this.genreForm);

      this._genreService.createGenre(genre)
        .subscribe({
          next: () => this._toastr.success("Genre was created", "Success!")
        });

    } else {
      this.genreForm.markAllAsTouched();
    }
  }

  private getGenres(): void {
    this._genreService.getGenres()
      .subscribe({
        next: responseBody => this.genres = responseBody});
  }

  formInit(): FormGroup {
    return new FormGroup(
      {
        id: new FormControl(Guid.EMPTY),
        mongoId: new FormControl(Guid.EMPTY),
        name: new FormControl('', Validators.required),
        parentGenreId: new FormControl(''),
        parentGenre: new FormControl(''),
        children: new FormControl(''),
      });
  }
}