import { Component, EventEmitter, Input, Output } from '@angular/core';
import { faMagnifyingGlass } from '@fortawesome/free-solid-svg-icons';
import { ToastrService } from 'ngx-toastr';
import { GameFilter } from 'src/app/classes/gameFilter';
import { Genre } from 'src/app/classes/genre';
import { PlatformType } from 'src/app/classes/platformType';
import { Publisher } from 'src/app/classes/publisher';
import { GenreService } from 'src/app/services/genre.service';
import { PlatformTypeService } from 'src/app/services/platform-type.service';
import { PublisherService } from 'src/app/services/publisher.service';

@Component({
  selector: 'app-games-filter',
  templateUrl: './games-filter.component.html',
  styleUrls: ['./games-filter.component.css']
})
export class GamesFilterComponent {

  @Output() passFilterToParent = new EventEmitter<GameFilter>();

  @Input() public filter: GameFilter;
  public genres: Genre[] = [];
  public platforms: PlatformType[] = [];
  public publishers: Publisher[] = [];

  public faSearch = faMagnifyingGlass;

  constructor(
    private readonly _publisherService: PublisherService,
    private readonly _genreService: GenreService,
    private readonly _platformService: PlatformTypeService,
    private readonly _toastr: ToastrService
  ) {
    this.filter = new GameFilter();
    this.getPublishers();
    this.getGenres();
    this.getPlatforms();
  }

  private getPublishers(): void {
    this._publisherService.getPublishers()
      .subscribe({
        next: responseBody => this.publishers = responseBody
      });
  }

  private getGenres(): void {
    this._genreService.getGenres()
      .subscribe({
        next: responseBody => this.genres = responseBody.filter(x => x.parentGenre == null)
      });
  }

  private getPlatforms(): void {
    this._platformService.getPlatformTypes()
      .subscribe({
        next: responseBody => this.platforms = responseBody
      });
  }

  public filterGames(): void {
    this.passFilterToParent.emit(this.filter);
  }

  public changeSelectedPlatforms(platform: PlatformType) {
    if (this.filter.platforms.includes(platform)) {
      this.filter.platforms = this.filter.platforms.filter(x => x.id != platform.id);
    } else {
      this.filter.platforms.push(platform);
    }
  }

  public changeSelectedGenres(genre: Genre) {
    if (this.filter.genres.includes(genre)) {
      this.filter.genres = this.filter.genres.filter(x => x.id != genre.id);
    } else {
      this.filter.genres.push(genre);
    }
  }

  public changeSelectedPublishers(publisher: Publisher) {
    if (this.filter.publishers.includes(publisher)) {
      this.filter.publishers = this.filter.publishers.filter(x => x.id != publisher.id);
    } else {
      this.filter.publishers.push(publisher);
    }
  }

  public isPublisherChecked = (publisherId: string) => this.filter.publishers.find(x => x.id === publisherId) ? true : false;
  public isPlatformChecked = (platformId: string) => this.filter.platforms.find(x => x.id === platformId) ? true : false;
  public isGenreChecked = (genreId: string) => this.filter.genres.find(x => x.id === genreId) ? true : false;
}
