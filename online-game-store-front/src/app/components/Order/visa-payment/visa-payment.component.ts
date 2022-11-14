import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Visa } from 'src/app/classes/visa';
import { Order } from 'src/app/classes/order';
import { OrderState } from 'src/app/enums/orderState';
import { OrderService } from 'src/app/services/order.service';
import { PaymentService } from 'src/app/services/payment.service';
import * as moment from 'moment';

@Component({
  selector: 'app-visa-payment',
  templateUrl: './visa-payment.component.html',
  styleUrls: ['./visa-payment.component.css']
})
export class VisaPaymentComponent implements OnInit {

  public clicked: Boolean = false;
  public visaForm: FormGroup;
  private order: Order = new Order();

  constructor(
    private readonly _orderService: OrderService,
    private readonly _paymentService: PaymentService,
    private readonly _toastr: ToastrService) {

    this.visaForm = this.formInit();
  }

  ngOnInit(): void {
    this._orderService.getActiveOrder().subscribe(responseBody => this.order = responseBody);
  }

  private formInit(): FormGroup {
    return new FormGroup(
      {
        name: new FormControl('', Validators.required),
        cardNumber: new FormControl('', [Validators.required, Validators.pattern('^4[0-9]{12}(?:[0-9]{3})?$')]),
        cvv2: new FormControl('', [Validators.required, Validators.maxLength(3), Validators.minLength(3)]),
        dateOfExpiry: new FormControl('', [Validators.required, Validators.pattern('[0-9]{2}/[0-9]{4}'), this.validDateOfExpiry()])
      });
  }

  public getTotal() {
    let sum = 0;
    this.order.items.forEach(x => sum += ((x.price - (x.price * x.discount * 0.01)) * x.quantity))
    return sum.toFixed(2);
  }

  private getFromForm(): Visa {
    return new Visa(
      this.visaForm.controls['name'].value,
      this.visaForm.controls['cardNumber'].value,
      this.visaForm.controls['cvv2'].value,
      this.visaForm.controls['dateOfExpiry'].value);
  }

  public pay(): void {
    if (this.visaForm.valid) {
      let requestBody = this.getFromForm();

      this._paymentService.visaPayment(requestBody)
      .subscribe({
        next: () => this._toastr.success("Payment success!", "Success"),
        error: (error) => {
          this.clicked = false;
        }
    })
    } else {
      this.clicked = false;
      this.visaForm.markAllAsTouched();
    }
  }

  private validDateOfExpiry(): ValidatorFn {
    return (control: AbstractControl): ValidationErrors | null => {

      let dateOfExpiry = control.value;

      let nowYear = moment().format('YYYY');
      let nowMonth = moment().format('MM');
      let month = dateOfExpiry.substring(0, 2);
      let year = dateOfExpiry.substring(3, 4);

      let isValid = year > nowYear || (year == nowYear && month >= nowMonth);

      return !isValid ? { validDateOfExpiry: true } : null;
    }
  }
}
