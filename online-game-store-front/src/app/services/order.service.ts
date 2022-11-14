
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Order } from '../classes/order';
import { OrderDetail } from '../classes/orderDetail';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class OrderService {

  private apiUrl: string = environment.apiUrl + 'api/';

  constructor( 
    private httpClient: HttpClient
    ) { }

  getOrder(id: string): Observable<Order> {
    return this.httpClient.get<Order>(this.apiUrl + "order/" + id, { withCredentials: true });
  }

  getOrdersByDate(startDate: string, endDate: string): Observable<Order[]> {
    return this.httpClient.get<Order[]>(this.apiUrl + `order/filter/`, 
    { params: new HttpParams()
        .append('startDate', startDate)
        .append('endDate', endDate), 
        withCredentials: true 
    });
  }

  getActiveOrder(): Observable<Order> {
    return this.httpClient.get<Order>(this.apiUrl + "order", { withCredentials: true });
  }

  editOrder(order: Order): Observable<Order> {
    return this.httpClient.put<Order>(this.apiUrl + "order", order, { withCredentials: true });
  }

  addItemToOrder(orderDetail : OrderDetail): Observable<any> {
    return this.httpClient.post(this.apiUrl + 'orderdetail', orderDetail, { withCredentials: true });
  }

  deleteItemFromOrder(id: string): Observable<any> {
    return this.httpClient.delete(this.apiUrl + `orderdetail/${id}`, { withCredentials: true });
  }
}