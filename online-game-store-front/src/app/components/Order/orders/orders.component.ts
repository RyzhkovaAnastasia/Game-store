
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { faPencil } from '@fortawesome/free-solid-svg-icons';
import { ToastrService } from 'ngx-toastr';
import { Order } from 'src/app/classes/order';
import { OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements OnInit {

  public orders: Order[] = [];
  selectedDateRange!: { startDate: Date; endDate: Date; };
  public isFiltering = false;
  faPencil = faPencil;

  constructor(
    private readonly _orderService: OrderService,
    private readonly _router: Router,
    private readonly _toastr: ToastrService) { }

  ngOnInit(): void {
    this.getAll(false);
  }

  public getTotalSumOfOrder(id: string){
    let order = this.orders?.find(x => x.id == id);
    let totalSum = 0;
    order?.items.forEach(x => totalSum += ((x.price - (x.price * x.discount * 0.01)) * x.quantity));
    return totalSum.toFixed(2);
  }

  public getAll(isFiltering: boolean): void {
    if(isFiltering == true && this.selectedDateRange != null && this.selectedDateRange.endDate != null && this.selectedDateRange.endDate != null){
      this._orderService.getOrdersByDate(this.selectedDateRange.startDate.toISOString(), this.selectedDateRange.endDate.toISOString())
      .subscribe({
        next: responseBody => this.orders = responseBody
      });
    } else {
      let startDate = new Date();
      let endDate = new Date();
      startDate.setMonth(startDate.getMonth() - 1);

      this._orderService.getOrdersByDate(startDate.toISOString(), endDate.toISOString())
        .subscribe({
          next: responseBody => this.orders = responseBody//.filter(x => x.orderDate)
        });
      }
  }
  editOrder(id: string){
    this._router.navigate([`orders/update/${id}`]);
  }
}
