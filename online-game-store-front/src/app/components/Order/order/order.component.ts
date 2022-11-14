import { Component } from '@angular/core';
import { Order } from 'src/app/classes/order';
import { OrderState } from 'src/app/enums/orderState';
import { OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']
})
export class OrderComponent {

  public order: Order;
  
  constructor(
    private readonly _orderService: OrderService
    ) {
    this.order = new Order();
    this.getOrder();
  }

  private getOrder() {
    this._orderService.getActiveOrder().subscribe(
      responseBody => this.order = responseBody
    )
  }

  public round(num: number){
    return num.toFixed(2)
  }

  public getTotal() {
    let sum = 0;
    this.order.items.forEach(x => sum += ((x.price - (x.price * x.discount * 0.01)) * x.quantity))
    return sum.toFixed(2);
  }
}
