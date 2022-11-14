import { Game } from "./game";
import { Order } from "./order";

export class OrderDetail {
  constructor(
  public id: string = '',
  public gameId: string = '',
  public game: Game | null = null,
  public price: number = 0,
  public quantity: number = 0,
  public discount: number = 0,
  public orderId: string = '',
  public order: Order | null = null){ }
}