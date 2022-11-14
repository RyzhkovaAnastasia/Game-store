import { Injectable } from '@angular/core';
import { FormGroup } from '@angular/forms';

@Injectable({
  providedIn: 'root',
})

export class FormService {

  constructor() { }

  public getFromForm(obj: any, formGroup: FormGroup): object {
    for (let prop in obj) {
      obj[prop] = formGroup.controls[prop].value;
    }
    return obj;
  }

  public setToForm(obj: any, formGroup: FormGroup): void {
    for (let prop in obj) {
      formGroup.controls[prop].setValue(obj[prop]);
    }
  }
}