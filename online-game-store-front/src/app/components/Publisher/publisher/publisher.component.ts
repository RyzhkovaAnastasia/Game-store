import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { faPencil, faTrash } from '@fortawesome/free-solid-svg-icons';
import { Guid } from 'guid-typescript';
import { ToastrService } from 'ngx-toastr';
import { Publisher } from 'src/app/classes/publisher';

import { PublisherService } from 'src/app/services/publisher.service';

@Component({
  selector: 'app-publisher',
  templateUrl: './publisher.component.html',
  styleUrls: ['./publisher.component.css']
})
export class PublisherComponent {

  public faPencil = faPencil;
  public faTrash = faTrash;
  
  public publisher: Publisher;

  constructor(
    private readonly _publisherService: PublisherService,
    private readonly _toastr: ToastrService,
    private readonly _route: ActivatedRoute
  ) {
    this.publisher = new Publisher('', '', '', '');
    let companyName = this._route.snapshot.paramMap.get('companyName') ?? '';
    this.getPublisher(companyName);
  }

  private getPublisher(companyName: string) {
    this._publisherService.getPublisherByCompanyName(companyName)
      .subscribe({
        next: responseBody => this.publisher = responseBody
      })
  }
}
