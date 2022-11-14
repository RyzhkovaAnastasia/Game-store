import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { PaymentMethod } from '../classes/paymentMethod';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { Visa } from '../classes/visa';
import { IBox } from '../classes/ibox';
import { duration } from 'moment';

@Injectable({
  providedIn: 'root',
})
export class PaymentService {

  private apiUrl: string = environment.apiUrl + "api/payment";

  constructor(
    private httpClient: HttpClient
  ) { }

  getPayments(): Observable<PaymentMethod[]> {
    return this.httpClient.get<PaymentMethod[]>(this.apiUrl,  { withCredentials: true });
  }

  getBankInvoice(): Observable<HttpResponse<Blob>> {
    return this.httpClient.get(this.apiUrl + "/bank",
      {
        responseType: 'blob',
        observe: 'response',
        withCredentials : true
      });
  }

  getImage(path: string): Observable<Blob> {
    return this.httpClient.get(this.apiUrl + "/images/" + path,
      {
        responseType: 'blob'
      });
  }

  visaPayment(visa: Visa): Observable<any> {
    return this.httpClient.post(this.apiUrl + "/visa", visa,  { withCredentials: true });
  }

  iboxPayment(ibox: IBox): Observable<any> {
    return this.httpClient.post(this.apiUrl + "/ibox", ibox,  { withCredentials: true });
  }

  startTimeout(timeSpan: number): Observable<any> {
    return this.httpClient.post(this.apiUrl + "/timeout", timeSpan,  { withCredentials: true });
  }
}