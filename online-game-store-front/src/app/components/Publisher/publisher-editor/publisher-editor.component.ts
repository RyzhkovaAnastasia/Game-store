import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Guid } from 'guid-typescript';
import { ToastrService } from 'ngx-toastr';
import { Publisher } from 'src/app/classes/publisher';
import { FormService } from 'src/app/services/form.service';
import { PublisherService } from 'src/app/services/publisher.service';

@Component({
  selector: 'app-publisher-editor',
  templateUrl: './publisher-editor.component.html',
  styleUrls: ['./publisher-editor.component.css']
})

export class PublisherEditorComponent {

  public publisherForm: FormGroup;

  constructor(
    private readonly _publisherService: PublisherService,
    private readonly _toastr: ToastrService,
    private readonly _route: ActivatedRoute,
    private readonly _formService: FormService
  ) {
    let companyName = this._route.snapshot.paramMap.get('companyName') ?? '';
    this.getPublisher(companyName);
    this.publisherForm = this.formInit();
  }

  private getPublisher(companyName: string): void {
    this._publisherService.getPublisherByCompanyName(companyName)
      .subscribe({
        next: responseBody => this._formService.setToForm(responseBody, this.publisherForm)
      });
  }

  public editPublisher(): void {
    if (this.publisherForm.valid) {
      let publisher = new Publisher(Guid.EMPTY, '', '', '');
      this._formService.getFromForm(publisher, this.publisherForm);
      this._publisherService.editPublisher(publisher)
        .subscribe({
          next: () => this._toastr.success("Publisher was edited", "Success")
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
