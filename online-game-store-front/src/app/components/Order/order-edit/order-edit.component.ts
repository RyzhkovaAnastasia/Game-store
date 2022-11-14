import { Component, Injectable, OnInit } from '@angular/core';
import { ActivatedRoute, provideRoutes } from '@angular/router';
import { faTrash } from '@fortawesome/free-solid-svg-icons';
import { ToastrService } from 'ngx-toastr';
import { User } from 'src/app/classes/auth/user';
import { Order } from 'src/app/classes/order';
import { OrderState } from 'src/app/enums/orderState';
import { OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'app-order-edit',
  templateUrl: './order-edit.component.html',
  styleUrls: ['./order-edit.component.css']
})
export class OrderEditComponent implements OnInit {

  public order: Order = new Order();
  public orderState;
  public faTrash = faTrash;

  constructor(
    private readonly _orderService: OrderService,
    private readonly _toastrService: ToastrService,
    private readonly _route: ActivatedRoute
  ) {
    this.orderState = OrderState;
   }
  ngOnInit(): void {
    let id = this._route.snapshot.paramMap.get('id') ?? '';
    this.getOrder(id);
  }

   public getOrder(id: string){
    this._orderService.getOrder(id)
    .subscribe({
      next: data => this.order = data
    });
  }

  public deleteItem(id: string): void {
    this._orderService.deleteItemFromOrder(id).subscribe({
      next: responseBody => {
        this.getOrder(this._route.snapshot.paramMap.get('id') ?? '');
      }
    })
  }

   public editOrder(){
    this._orderService.editOrder(this.order)
    .subscribe(
      ()=> {
        this._toastrService.success("Edit was successful!", "Success!");
      }
    );
  }
}
