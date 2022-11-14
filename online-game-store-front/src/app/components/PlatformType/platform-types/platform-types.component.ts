import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { faPencil, faTrash } from '@fortawesome/free-solid-svg-icons';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { PlatformType } from 'src/app/classes/platformType';
import { PlatformTypeService } from 'src/app/services/platform-type.service';
import { DeleteModalComponent } from '../../Shared/delete-modal/delete-modal.component';

@Component({
  selector: 'app-platform-types',
  templateUrl: './platform-types.component.html',
  styleUrls: ['./platform-types.component.css']
})
export class PlatformTypesComponent implements OnInit {

  public platforms: PlatformType[] = [];

  public faTrash = faTrash;
  public faPencil = faPencil;

  constructor(
    private readonly _platformService: PlatformTypeService,
    private readonly _toastr: ToastrService,
    private readonly _modalService: BsModalService,
    private readonly _router: Router) { }

  ngOnInit(): void {
    this.getAll();
  }

  private getAll(): void {
    this._platformService.getPlatformTypes()
      .subscribe({
        next: responseBody => this.platforms = responseBody
      });
  }

  public editPlatform(id: string): void {
    this._router.navigate([`platforms/update/${id}`]);
  }

  public deletePlatform(id: string): void {
    let modalRef = this._modalService.show(DeleteModalComponent, { class: 'modal-sm' });

    modalRef?.content?.onClose.subscribe(
      () => {
        this._platformService.deletePlatformType(id)
          .subscribe({
            next: () => {
              this._toastr.success("Platform was deleted", "Success!");
              this.getAll();
            }
          });
      });
  }
}