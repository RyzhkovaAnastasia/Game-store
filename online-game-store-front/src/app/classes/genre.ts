import { Guid } from "guid-typescript";

export class Genre {
  [prop: string]: any;
  constructor(
  public id : string = Guid.EMPTY,
  public name : string = '',
  public parentGenreId: string = '',
  public parentGenre: Genre | null = null,
  public children : Genre[] | null = []
  ) { }
}