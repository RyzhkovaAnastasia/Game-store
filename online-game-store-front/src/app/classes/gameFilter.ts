import { GamePublishedDate } from "../enums/gamePublishedDate";
import { Genre } from "./genre";
import { PlatformType } from "./platformType";
import { Publisher } from "./publisher";

export class GameFilter {
  [prop: string]: any;
constructor(
  public name: string | null = null,
  public published: GamePublishedDate = GamePublishedDate.allTime,
  public minPrice: number | null = null,
  public maxPrice: number | null = null,
  public genres: Genre [] = [],
  public platforms: PlatformType[] = [],
  public publishers: Publisher[] = []
){ }
}