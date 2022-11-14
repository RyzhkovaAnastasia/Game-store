import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { faMagnifyingGlass, faPencil, faTrash } from '@fortawesome/free-solid-svg-icons';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { User } from 'src/app/classes/auth/user';
import { Publisher } from 'src/app/classes/publisher';
import { Role } from 'src/app/enums/role';
import { PublisherService } from 'src/app/services/publisher.service';
import { UserService } from 'src/app/services/user.service';
import { DeleteModalComponent } from '../../Shared/delete-modal/delete-modal.component';

@Component({
  selector: 'app-publishers',
  templateUrl: './publishers.component.html'
})
export class PublishersComponent {

  public publishers: Publisher[] = [];
  public user: User;
  public isInRole: boolean;

  public faPencil = faPencil;
  public faTrash = faTrash;
  public faMagnifier = faMagnifyingGlass;

  constructor(
    private readonly _publisherService: PublisherService,
    private readonly _userService: UserService,
    private readonly _toastr: ToastrService,
    private readonly _modalService: BsModalService,
    private readonly _router: Router
  ) {
    this.getAll();
    this.user = _userService.getUser;
    this.isInRole = this.user.role === Role.Admin || this.user.role === Role.Manager;
  }

  public detailsPublisher(companyName: string): void {
    this._router.navigate([`publishers/${companyName}`]);
  }

  private getAll(): void {
    this._publisherService.getPublishers()
      .subscribe({
        next: responseBody => this.publishers = responseBody
      });
  }

  public editPublisher(companyName: string): void {
    this._router.navigate([`publishers/update/${companyName}`]);
  }

  public deletePublisher(id: string): void {
    let modalRef = this._modalService.show(DeleteModalComponent, { class: 'modal-sm' });

    modalRef?.content?.onClose.subscribe(
      () => {
        this._publisherService.deletePublisher(id)
          .subscribe({
            next: () => {
              this._toastr.success("Publisher was deleted", "Success!");
              this.getAll();
            }
          });
      });
  }
}
