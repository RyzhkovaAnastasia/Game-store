import { HttpResponse } from '@angular/common/http';
import { Component } from '@angular/core';
import { DomSanitizer, SafeUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';
import * as saveAs from 'file-saver';
import { ToastrService } from 'ngx-toastr';
import { PaymentMethod } from 'src/app/classes/paymentMethod';
import { PaymentService } from 'src/app/services/payment.service';

@Component({
  selector: 'app-payment-methods',
  templateUrl: './payment-methods.component.html',
  styleUrls: ['./payment-methods.component.css']
})
export class PaymentMethodsComponent {

  paymentMethods: PaymentMethod[] = [];
  imgSrc: Map<string, SafeUrl>;

  constructor(
    private readonly _paymentService: PaymentService,
    private readonly _toastr: ToastrService,
    private readonly _route: Router,
    private readonly _sanitizer: DomSanitizer
    ) {
    this.getPaymentMethods();
    this.imgSrc = new Map<string, SafeUrl>();
  }

  getPaymentMethods() {
    this._paymentService.getPayments()
    .subscribe({
      next: responseBody => {
        this.paymentMethods = responseBody; 
        this.getImages(); 
      },
      error: () => this._toastr.error("Sorry, payment is not available now", "Payment error")
    });
  }

  downloadFileFromHttp(httpResponse: HttpResponse<Blob>): void {
    const uriEncodedFileName = httpResponse?.headers?.get('content-disposition')?.match(/filename\*=UTF-8''(.*?)$/);
    if (uriEncodedFileName != null) {
      const fileName = decodeURI(uriEncodedFileName[1]);
      if (httpResponse.body != null) {
        const file = new File([httpResponse.body], fileName, { type: httpResponse.body.type });
        saveAs(file);
      }
    }
  }

  getImages() {
    this.paymentMethods.forEach(element => {
      this.getImageByPath(element.id, element.imageFileName);
    });
  }

  getImageByPath(paymentid: string, path: string): any {
    this._paymentService.getImage(path).subscribe(
      responseBody => {
        let objectURL = URL.createObjectURL(responseBody);
        this.imgSrc.set(paymentid, this._sanitizer.bypassSecurityTrustUrl(objectURL));
      });
  }

  pay(method: string) {
    switch (method) {

      case 'Bank':
        this._paymentService.startTimeout(3 * 24 * 60).subscribe();

        this._paymentService.getBankInvoice().subscribe({
          next: responseBody => this.downloadFileFromHttp(responseBody),
          error: error => this._toastr.error(error.error?.message, 'Error')
        });
        break;

      case 'IBox':
        this._paymentService.startTimeout(15).subscribe()
        this._route.navigate(['order/payment/ibox']);
        break;

      case 'Visa':
        this._paymentService.startTimeout(15).subscribe()
        this._route.navigate(['order/payment/visa']);
        break;
    }
  }
}
