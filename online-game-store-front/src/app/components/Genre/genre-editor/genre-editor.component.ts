import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Guid } from 'guid-typescript';
import { ToastrService } from 'ngx-toastr';
import { Genre } from 'src/app/classes/genre';
import { FormService } from 'src/app/services/form.service';

import { GenreService } from 'src/app/services/genre.service';
@Component({
  selector: 'app-genre-editor',
  templateUrl: './genre-editor.component.html',
  styleUrls: ['./genre-editor.component.css']
})
export class GenreEditorComponent {

  public genreForm: FormGroup;
  public genres: Genre[] = [];

  constructor(
    private readonly _genreService: GenreService,
    private readonly _toastr: ToastrService,
    private readonly _formService: FormService,
    _route: ActivatedRoute
  ) {
    const id = _route.snapshot.paramMap.get('id') ?? '';
    this.getGenresExceptCurrent(id);
    this.getGenre(id);
    this.genreForm = this.formInit();
  }

  public editGenre(): void {
    if (this.genreForm.valid) {
      let genre = new Genre();
      this._formService.getFromForm(genre, this.genreForm);
      genre.parentGenre = null;

      this._genreService.editGenre(genre)
        .subscribe({
          next: () =>
            this._toastr.success("Genre was edited", "Success!")
        });
    } else {
      this.genreForm.markAllAsTouched();
    }
  }

  private getGenre(id: string): void {
    this._genreService.getGenre(id).subscribe({
      next: (responseBody) => this._formService.setToForm(responseBody, this.genreForm)
    });
  }

  private getGenresExceptCurrent(id: string) {
    let genres: Genre[] = [];
    this._genreService.getGenres()
      .subscribe({
        next: responseBody =>
          this.genres = responseBody.filter(x => x.id != id)});
    return genres;
  }

  private formInit(): FormGroup {
    return new FormGroup(
      {
        id: new FormControl(),
        name: new FormControl('', Validators.required),
        parentGenreId: new FormControl(''),
        parentGenre: new FormControl(''),
        children: new FormControl(''),
      });
  }
}