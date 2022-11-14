import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-delete-modal',
  templateUrl: './delete-modal.component.html',
  styleUrls: ['./delete-modal.component.css']
})
export class DeleteModalComponent {

  public onClose: Subject<boolean> = new Subject();;
  
  constructor(public _bsModalRef: BsModalRef) { }

  public onConfirm(): void {
    this.onClose.next(true);
    this._bsModalRef.hide();
}
}
