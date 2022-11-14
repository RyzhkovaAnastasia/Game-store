
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { faMagnifyingGlass, faPencil, faTrash } from '@fortawesome/free-solid-svg-icons';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { User } from 'src/app/classes/auth/user';
import { Game } from 'src/app/classes/game';
import { GameFilter } from 'src/app/classes/gameFilter';
import { GameFilterComponents } from 'src/app/classes/gameFilterComponents';
import { GamePagination } from 'src/app/classes/gamePagination';
import { GameSort } from 'src/app/classes/gameSort';
import { Role } from 'src/app/enums/role';
import { GameService } from 'src/app/services/game.service';
import { UserService } from 'src/app/services/user.service';
import { DeleteModalComponent } from '../../Shared/delete-modal/delete-modal.component';

@Component({
  selector: 'app-games',
  templateUrl: './games.component.html'
})
export class GamesComponent implements OnInit {

  private uri: string = '';
  public gameNumber: number = 0;
  public games: Game[] = [];
  public gameFilterComponents: GameFilterComponents;
  public user: User;
  public isInRole;

  public faPencil = faPencil;
  public faTrash = faTrash;
  public faMagnifier = faMagnifyingGlass;

  constructor(
    private readonly _gameService: GameService,
    private readonly _userService: UserService,
    private readonly _toastr: ToastrService,
    private readonly _modalService: BsModalService,
    private readonly _router: Router) {
      this.gameFilterComponents = new GameFilterComponents(new GameFilter(), new GameSort(), new GamePagination());
      this.user = _userService.getUser;
      this.isInRole = this.user.role === Role.Admin || this.user.role === Role.Manager;
  }

  ngOnInit(): void {
    const uri = new URLSearchParams(location.search).get('filters');

    if (!uri) {
      this.gameFilterComponents = new GameFilterComponents(new GameFilter(), new GameSort(), new GamePagination());
      this.uri = `${location.origin}${location.pathname}?filters=${encodeURIComponent(JSON.stringify(this.gameFilterComponents))}`;

      window.history.pushState({ path: this.uri }, '', this.uri);
    } else {
      this.gameFilterComponents = JSON.parse(uri);
    }

    this.filter();
  }

  private getAll() {
    this._gameService.getGames()
      .subscribe({
        next: responseBody => {
          this.games = responseBody.games;
          this.gameNumber = responseBody.allGameNumber;
          this.games.forEach(x => x.description = x.description.slice(0, 150) + '...');
        }
      });
  }

  public deleteGame(id: string): void {
    let modalRef = this._modalService.show(DeleteModalComponent, { class: 'modal-sm' });

    modalRef?.content?.onClose.subscribe(
      () => {
        this._gameService.deleteGame(id)
          .subscribe({
            next: () => {
              this._toastr.success("Game was deleted", "Success!");
              this.getAll();
            }
          })
      });
  }


  public editGame(gamekey: string): void {
    this._router.navigate([`games/update/${gamekey}`]);
  }

  public detailsGame(gamekey: string): void {
    this._router.navigate([`games/${gamekey}`]);
  }

  public getWithDiscount(price: number, discount: number): string {
    return (price - (price * discount / 100)).toFixed(2);
  }

  public filterBuilder(filter: GameFilter) {
      this.gameFilterComponents.filter = Object.assign({}, filter);
      this.gameFilterComponents.pagination.currentPage = 1;
      this.filter();
  }

  public sortBuilder(sort: GameSort) {
      this.gameFilterComponents.sort = Object.assign({}, sort);
      this.gameFilterComponents.pagination.currentPage = 1;
      this.filter();
  }

  public paginationBuilder(pagination: GamePagination) {
      this.gameFilterComponents.pagination = Object.assign({}, pagination);
      this.filter();
  }

  private filter() {
    this.uri = `${location.origin}${location.pathname}?filters=${encodeURIComponent(JSON.stringify(this.gameFilterComponents))}`
    window.history.pushState({ path: this.uri }, '', this.uri);

    this._gameService.filterGames(JSON.stringify(this.gameFilterComponents)).subscribe(
      {
        next: responseBody => {
          this.games = responseBody.games;
          this.gameNumber = responseBody.allGameNumber;
          this.games.forEach(x => x.description = x.description.slice(0, 150) + '...');
        }
      }
    )
  }
}
