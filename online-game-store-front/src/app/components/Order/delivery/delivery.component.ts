import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Order } from 'src/app/classes/order';
import { OrderService } from 'src/app/services/order.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-delivery',
  templateUrl: './delivery.component.html',
  styleUrls: ['./delivery.component.css']
})
export class DeliveryComponent {

  public order: Order = new Order();
  
  constructor(
    private readonly _orderService: OrderService,
    private readonly _userService: UserService,
    private readonly _toastr: ToastrService,
    private readonly _route: Router
  ) {
    this.getOrder();
  }

  private getOrder(): void {
    this._orderService.getActiveOrder().subscribe({
      next: responseBody => 
      {
        this.order = responseBody;
      this.order.shipName = this._userService.getUser.username;
      }
    })
  }

  public makeOrder(): void {
    this._orderService.editOrder(this.order)
    .subscribe({
      next: () => this._route.navigate(['order'])}
    )
  }

}
