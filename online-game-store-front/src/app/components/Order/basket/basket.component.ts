
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { ToastrService } from 'ngx-toastr';
import { Order } from 'src/app/classes/order';
import { OrderState } from 'src/app/enums/orderState';
import { OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'app-basket',
  templateUrl: './basket.component.html',
  styleUrls: ['./basket.component.css']
})
export class BasketComponent {

  public faTrash = faTrash;
  public order;
  
  constructor(
    private readonly _orderService: OrderService,
    private readonly _toastr: ToastrService,
    private readonly _route: Router
  ) {
    this.order = new Order();
    this.order.orderState = OrderState.Opened;
    this.getOrder();
  }

  private getOrder(): void {
    this._orderService.getActiveOrder().subscribe({
      next: responseBody => this.order = responseBody
    }
    )
  }

  public deleteItem(id: string): void {
    this._orderService.deleteItemFromOrder(id).subscribe({
      next: responseBody => {
        this.getOrder();
      }
    })
  }

  public plusOneItem(id: string): void {
    let item = this.order.items.find(x => x.id == id);
    if (item?.game && item.game.unitsInStock >= item.quantity + 1) {
      item.quantity += 1;
    }
    else if (item?.game && item.game.unitsInStock < item.quantity + 1) {
      this._toastr.warning(`Only ${item?.game?.unitsInStock} games in stock`);
    }
  }

  public minusOneItem(id: string): void {
    let item = this.order.items.find(x => x.id == id);

    if (item?.game && item.quantity - 1 > 0) {
      item.quantity -= 1;
    }
    else if (item?.game && item.quantity - 1 <= 0) {
      this._toastr.warning(`Please, select minimum 1 game or delete it`);
    }
  }


  public round (num: number) {
     return num.toFixed(2);
  }

  public getTotal() {
    let sum = 0;
    this.order.items.forEach(x => sum += ((x.price - (x.price * x.discount * 0.01)) * x.quantity))
    return sum.toFixed(2);
  }

  public makeOrder(): void {
    this._orderService.editOrder(this.order)
    .subscribe({
      next: () => this._route.navigate(['order/delivery'])}
    )
  }
}
