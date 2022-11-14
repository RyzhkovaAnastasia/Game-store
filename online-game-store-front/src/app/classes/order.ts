import { OrderState } from "../enums/orderState";
import { OrderDetail } from "./orderDetail";

export class Order 
{
  constructor(
  public id: string = '',
  public userId: string = '',
  public orderDate: string| null = null,
  public requiredDate: string = '',
  public shippedDate: string | null = null,
  public shipperId: number | null = null,
  public shipName: string | null = null,
  public shipAddress: string | null = null,
  public shipCity: string | null = null,
  public shipRegion: string | null  = null,
  public shipPostalCode: string | null  = null,
  public shipCountry: string | null  = null,
  public orderState: OrderState = OrderState.Opened,
  public items: OrderDetail[] = []
  )
  { }
}