import { GameFilter } from "./gameFilter";
import { GamePagination } from "./gamePagination";
import { GameSort } from "./gameSort";

export class GameFilterComponents {
  
  constructor(
    public filter: GameFilter,
    public sort: GameSort,
    public pagination: GamePagination
  ) { }
}