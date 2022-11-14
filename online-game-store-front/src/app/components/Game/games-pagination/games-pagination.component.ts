import { Component, EventEmitter, Input, Output } from '@angular/core';
import { GamePagination } from 'src/app/classes/gamePagination';

@Component({
  selector: 'app-games-pagination',
  templateUrl: './games-pagination.component.html',
  styleUrls: ['./games-pagination.component.css']
})
export class GamesPaginationComponent {

  @Output() passPaginationToParent = new EventEmitter<GamePagination>();
  public maxPageNumber: number = 0;

  @Input() pagination: GamePagination;

  @Input()
  get gameNumber(): number { return this._gameNumber; }
  set gameNumber(value: number) {
    this._gameNumber = value;
    this.calculateMaxPageNumber();
  }
  private _gameNumber: number = 0;

  constructor() {
    this.pagination = new GamePagination();
  }

  private calculateMaxPageNumber() {
    this.maxPageNumber = Math.ceil(this.gameNumber / this.pagination.itemsPerPage);
  }

  public clickPage(currentPage: number): void {
    if (this.pagination.currentPage <= this.maxPageNumber && this.pagination.currentPage > 0) {
      this.pagination.currentPage = currentPage;
    }
    else if (this.pagination.currentPage > this.maxPageNumber) {
      this.pagination.currentPage = this.maxPageNumber;
    }
    else {
      this.pagination.currentPage = 1;
    }
    this.startPagination();
  }

  public changeItemsPerPage(value: number): void {
    this.pagination.itemsPerPage = value;
    this.pagination.currentPage = 1;
    this.calculateMaxPageNumber();
    this.startPagination();
  }

  public startPagination() {
    this.passPaginationToParent.emit(this.pagination);
  }
}