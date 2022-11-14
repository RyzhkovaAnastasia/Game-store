import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Guid } from 'guid-typescript';
import { ToastrService } from 'ngx-toastr';
import { Publisher } from 'src/app/classes/publisher';
import { FormService } from 'src/app/services/form.service';
import { PublisherService } from 'src/app/services/publisher.service';

@Component({
  selector: 'app-publisher-creator',
  templateUrl: './publisher-creator.component.html',
  styleUrls: ['./publisher-creator.component.css']
})
export class PublisherCreatorComponent {

  publisherForm: FormGroup;

  constructor(
    private readonly _publisherService: PublisherService,
    private readonly _toastr: ToastrService,
    private readonly _formService: FormService
    ) {
    this.publisherForm = this.formInit();
  }

  public createPublisher(): void {
    if (this.publisherForm.valid) {

      let publisher = new Publisher(Guid.EMPTY, '', '', '');
      this._formService.getFromForm(publisher, this.publisherForm);

      this._publisherService.createPublisher(publisher)
        .subscribe({
          next: () => this._toastr.success("Publisher was created", "Success")
        });
    } else {
      this.publisherForm.markAllAsTouched();
    }
  }

  private formInit(): FormGroup {
    return new FormGroup(
      {
        id: new FormControl(Guid.EMPTY),
        companyName: new FormControl('', Validators.required),
        description: new FormControl('', [Validators.required, Validators.max(1000)]),
        homePage: new FormControl('', [Validators.required]),
      });
  }
}
