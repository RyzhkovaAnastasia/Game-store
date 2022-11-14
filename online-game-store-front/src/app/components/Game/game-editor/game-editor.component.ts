
import { Component } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { ToastrService } from 'ngx-toastr';
import { Game } from 'src/app/classes/game';
import { Genre } from 'src/app/classes/genre';
import { PlatformType } from 'src/app/classes/platformType';
import { Publisher } from 'src/app/classes/publisher';
import { FormService } from 'src/app/services/form.service';
import { GameService } from 'src/app/services/game.service';
import { GenreService } from 'src/app/services/genre.service';
import { PlatformTypeService } from 'src/app/services/platform-type.service';
import { PublisherService } from 'src/app/services/publisher.service';

@Component({
  selector: 'app-game-editor',
  templateUrl: './game-editor.component.html'
})
export class GameEditorComponent {

  gameForm: FormGroup;

  genres: Genre[] = [];
  publishers: Publisher[] = [];
  platforms: PlatformType[] = [];
  
  dropdownGenresSettings: IDropdownSettings = {};
  dropdownPlatformsSettings: IDropdownSettings = {};

  constructor(
    private _gameService: GameService,
    private readonly _toasrt: ToastrService,
    private readonly _publihserService: PublisherService,
    private readonly _genreService: GenreService,
    private readonly _platformService: PlatformTypeService,
    private readonly _route: ActivatedRoute,
    private readonly _formService: FormService
    ) {
    this.getPublishers();
    this.getGenres();
    this.getPlatforms();
    this.dropdownSettingsInit();

    this.gameForm = this.formInit();

    let gamekey = this._route.snapshot.paramMap.get('gamekey') ?? '';
    this.getGame(gamekey);
  }

  public editGame(): void {
    if (this.gameForm.valid) {
      let requestBody = this._formService.getFromForm(new Game(), this.gameForm) as Game;

      this._gameService.editGame(requestBody)
        .subscribe({
          next: () => this._toasrt.success("Game was edited", "Success")});

    } else {
      this.gameForm.markAllAsTouched();
    }
  }

  public onNameChange(): void {
    this.gameForm.controls['key'].setValue(
      encodeURIComponent(this.gameForm.controls['name'].value.toLowerCase().replace(/[^a-z0-9]+/gi, '-'))
    );
  }

  private getPublishers() {
    this._publihserService.getPublishers()
      .subscribe({
        next: responseBody => this.publishers = responseBody
      });
  }

  private getGenres() {
    this._genreService.getGenres()
      .subscribe({
        next: responseBody => this.genres = responseBody
      });
  }

  private getPlatforms() {
    this._platformService.getPlatformTypes()
      .subscribe({
        next: responseBody => this.platforms = responseBody
      });
  }

  private formInit(): FormGroup {
    return new FormGroup(
      {
        id: new FormControl(''),
        name: new FormControl('', Validators.required),
        key: new FormControl('', [Validators.required, Validators.pattern('[a-z0-9-]+')]),
        description: new FormControl('', [Validators.required, Validators.maxLength(1000)]),

        viewsNumber: new FormControl(0),
        commentsNumber: new FormControl(0),
        published: new FormControl(null),
        added: new FormControl(null),

        price: new FormControl('', [Validators.required, Validators.min(0), Validators.pattern('^([0-9]+)(\\.|$)([0-9]{0,2}|)$')]),
        discount: new FormControl('', [Validators.max(100), Validators.min(0), Validators.required]),
        unitsInStock: new FormControl('', [Validators.required, Validators.min(0)]),

        discontinued: new FormControl('', Validators.required),
        isCommented: new FormControl(true),
        isDownloaded: new FormControl(true),
        publisherId: new FormControl(''),
        publisher: new FormControl(null),
        genres: new FormControl([]),
        platformTypes: new FormControl([])
      });
  }
  
  private dropdownSettingsInit(): void {
    this.dropdownGenresSettings = {
      singleSelection: false,
      idField: 'id',
      textField: 'name',
      itemsShowLimit: 5,
      allowSearchFilter: true
    };

    this.dropdownPlatformsSettings = {
      singleSelection: false,
      idField: 'id',
      textField: 'type',
      itemsShowLimit: 5,
      allowSearchFilter: true
    };
  }

  private getGame(gamekey: string): void{
    this._gameService.getGame(gamekey)
      .subscribe({
        next: responseBody => {
          this._formService.setToForm(responseBody, this.gameForm);
          if(responseBody.publisher != null) {
          this.gameForm.controls['publisher'].setValue(this.publishers?.find(x=> x.id == responseBody.publisher?.id))
          }
        }
      });
  }
}
