import { Component, EventEmitter, Input, Output } from '@angular/core';
import { faComment, faDollar, faStar, faUpload } from '@fortawesome/free-solid-svg-icons';
import { GameSort } from 'src/app/classes/gameSort';
import { GameSortMethod } from 'src/app/enums/gameSortMethod';

@Component({
  selector: 'app-games-sort',
  templateUrl: './games-sort.component.html',
  styleUrls: ['./games-sort.component.css']
})
export class GamesSortComponent {

  @Output() passSortToParent = new EventEmitter<GameSort>();

  @Input() public sort: GameSort;
  public GameSortMethod = GameSortMethod;

  public faMoney = faDollar;
  public faPopular = faStar;
  public faComment = faComment;
  public faNewAdded = faUpload;

  constructor() {
    this.sort = new GameSort(GameSortMethod.None, false);
  }

  public click(method: GameSortMethod) {
    if (this.sort.gameSortMethod as GameSortMethod === method as GameSortMethod) {
      this.sort.isAscending = !this.sort.isAscending;
    } else {
      this.sort.gameSortMethod = method as GameSortMethod;
      this.sort.isAscending = false;
    }
    this.startSort();
  }

  public startSort() {
    this.passSortToParent.emit(this.sort);
  }
}