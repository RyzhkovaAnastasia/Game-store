import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Guid } from 'guid-typescript';
import * as moment from 'moment';
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
  selector: 'app-game-creator',
  templateUrl: './game-creator.component.html',
  styleUrls: ['./game-creator.component.css']
})
export class GameCreatorComponent {

  public gameForm: FormGroup;

  public genres: Genre[] = [];
  public publishers: Publisher[] = [];
  public platforms: PlatformType[] = [];

  public dropdownGenresSettings: IDropdownSettings = {};
  public dropdownPlatformsSettings: IDropdownSettings = {};

  constructor(
    private readonly _gameService: GameService,
    private readonly _toasrt: ToastrService,
    private readonly _publihserService: PublisherService,
    private readonly _genreService: GenreService,
    private readonly _platformService: PlatformTypeService,
    private readonly _router: Router,
    private readonly _formService: FormService
  ) {
    this.getPublishers();
    this.getGenres();
    this.getPlatforms();

    this.gameForm = this.formInit();
    this.dropdownSettingsInit();
  }

  private formInit(): FormGroup {
    return new FormGroup(
      {
        id: new FormControl(Guid.EMPTY),
        name: new FormControl('', Validators.required),
        key: new FormControl('', [Validators.required, Validators.pattern('[a-z0-9-]+')]),
        description: new FormControl('', [Validators.required, Validators.maxLength(1000)]),

        viewsNumber: new FormControl(0),
        commentsNumber: new FormControl(0),
        published: new FormControl(Date.UTC),
        added: new FormControl(Date.UTC),

        price: new FormControl('0', [Validators.required, Validators.min(0), Validators.pattern('^([0-9]+)(\\.|$)([0-9]{0,2}|)$')]),
        discount: new FormControl('0', [Validators.max(100), Validators.min(0), Validators.required]),
        unitsInStock: new FormControl('0', [Validators.required, Validators.min(0)]),

        discontinued: new FormControl(false, Validators.required),
        isCommented: new FormControl(true),
        isDownloaded: new FormControl(true),
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

  public createGame(): void {
    if (this.gameForm.valid) {
      let game: Game = new Game();
      this._formService.getFromForm(game, this.gameForm);

      this._gameService.createGame(game)
        .subscribe({
          next: () => {
            this._toasrt.success("Game was created", "Success");
            this._router.navigate(['/games']);
          }
        })
    } else {
      this.gameForm.markAllAsTouched();
    }
  }

  public getPublishers(): void {
    this._publihserService.getPublishers()
      .subscribe({
        next: responseBody => this.publishers = responseBody
      });
  }

  public getGenres(): void {
    this._genreService.getGenres()
      .subscribe({
        next: responseBody => this.genres = responseBody
      });
  }

  public getPlatforms(): void {
    this._platformService.getPlatformTypes()
      .subscribe({
        next: responseBody => this.platforms = responseBody
      });
  }

  public onNameChange(): void {
    this.gameForm.controls['key'].setValue(
      encodeURIComponent(this.gameForm.controls['name'].value.toLowerCase().replace(/[^a-z0-9]+/gi, '-'))
    );
  }
}
