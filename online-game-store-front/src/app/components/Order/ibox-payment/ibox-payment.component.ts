import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { IBox } from 'src/app/classes/ibox';
import { Order } from 'src/app/classes/order';
import { OrderState } from 'src/app/enums/orderState';
import { FormService } from 'src/app/services/form.service';
import { OrderService } from 'src/app/services/order.service';
import { PaymentService } from 'src/app/services/payment.service';

@Component({
  selector: 'app-ibox-payment',
  templateUrl: './ibox-payment.component.html',
  styleUrls: ['./ibox-payment.component.css']
})
export class IboxPaymentComponent implements OnInit {

  public clicked: Boolean = false;
  public iboxForm: FormGroup;
  private order;

  constructor(
    private readonly _orderService: OrderService,
    private readonly _paymentService: PaymentService,
    private readonly _toastr: ToastrService
    ) {
    this.order = new Order();
    this.iboxForm = this.formInit();
  }

  formInit(): FormGroup {
    return new FormGroup(
      {
        accountNumber: new FormControl(this.order.userId, Validators.required),
        invoiceNumber: new FormControl(this.order.id, Validators.required)
      });
  }

  ngOnInit(): void {
    this._orderService.getActiveOrder()
    .subscribe(responseBody => { 
      this.order = responseBody; 
      this.setToForm(); 
    });
  }

  public getTotal() {
    let sum = 0;
    this.order.items.forEach(x => sum += ((x.price - (x.price * x.discount * 0.01)) * x.quantity))
    return sum.toFixed(2);
  }

  private setToForm() {
    this.iboxForm.controls['accountNumber'].setValue(this.order.userId);
    this.iboxForm.controls['invoiceNumber'].setValue(this.order.id);
  }

  private getFromForm(): IBox {
    let ibox: IBox = new IBox(this.iboxForm.controls['accountNumber'].value, this.iboxForm.controls['invoiceNumber'].value, +this.getTotal());
    return ibox;
  }

  pay() {
    if (this.iboxForm.valid) {
      this._paymentService.iboxPayment(this.getFromForm())
      .subscribe({
        next: () => this._toastr.success("Payment success!", "Success"),
        error: error => {
          this.clicked = false;
        }
    });
    } else {
      this.clicked = false;
      this.iboxForm.markAllAsTouched();
    }
  }
}
