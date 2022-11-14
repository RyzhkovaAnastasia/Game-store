import { GameSortMethod } from "../enums/gameSortMethod";

export class GameSort {
  [prop: string]: any;
  constructor(
    public gameSortMethod: GameSortMethod = GameSortMethod.None,
    public isAscending: Boolean = true
  ){}
}