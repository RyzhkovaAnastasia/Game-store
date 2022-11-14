import { Game } from "./game";

export class FilteredGameModel {
  
  constructor(
    public games: Game[] = [],
    public allGameNumber: number = 0
  ) { }
}