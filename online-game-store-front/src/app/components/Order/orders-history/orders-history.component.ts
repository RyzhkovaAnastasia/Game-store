
import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Order } from 'src/app/classes/order';
import { OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'app-orders-history',
  templateUrl: './orders-history.component.html',
  styleUrls: ['./orders-history.component.css']
})
export class OrdersHistoryComponent implements OnInit {

  public orders: Order[] = [];
  selectedDateRange!: { startDate: Date; endDate: Date; };
  public isFiltering = false;

  constructor(
    private readonly _orderService: OrderService,
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
      let date = new Date(Date.now());
      this._orderService.getOrdersByDate('', new Date(date.setMonth(-1)).toISOString())
        .subscribe({
          next: responseBody => this.orders = responseBody
        });
    }
  }

}
