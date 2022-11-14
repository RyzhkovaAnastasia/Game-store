
import { Guid } from "guid-typescript";
import * as moment from "moment";
import { Moment } from "moment";
import { Genre } from "./genre";
import { PlatformType } from "./platformType";
import { Publisher } from "./publisher";

export class Game {
  [prop: string]: any;
  constructor(
  public id: string = Guid.EMPTY,
  public key: string = '',
  public name: string = '',
  public description: string = '',
  public viewsNumber: number = 0,
  public commentsNumber: number = 0,
  public added: Moment = moment.utc(),
  public published: Moment = moment.utc(),
  public price: number = 0,
  public discount: number = 0,
  public unitsInStock: number = 0,
  public discontinued: boolean = false,
  public isCommented: boolean = true,
  public isDownloaded: boolean = true,
  public publisher: Publisher | null = null,
  public genres: Genre[] = [],
  public platformTypes: PlatformType[] = []
  ) { }
}