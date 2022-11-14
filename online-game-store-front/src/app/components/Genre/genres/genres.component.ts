import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { faPencil, faPlus, faTrash } from '@fortawesome/free-solid-svg-icons';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Genre } from 'src/app/classes/genre';
import { GenreService } from 'src/app/services/genre.service';
import { DeleteModalComponent } from '../../Shared/delete-modal/delete-modal.component';


@Component({
  selector: 'app-genres',
  templateUrl: './genres.component.html',
  styleUrls: ['./genres.component.css']
})

export class GenresComponent implements OnInit {

  public modalRef?: BsModalRef;
  public genres: Genre[] = [];

  public faPencil = faPencil;
  public faTrash = faTrash;
  public faPlus = faPlus;

  public options: any;

  constructor(
    private readonly _genreService: GenreService,
    private readonly _toastr: ToastrService,
    private readonly _modalService: BsModalService,
    private readonly _router: Router) { }

  ngOnInit(): void {
    this.getAll();

    this.options = {
      displayField: 'name',
      isExpandedField: 'expanded',
      animateExpand: true
    };
  }

  private getAll(): void {
    this._genreService.getGenres()
      .subscribe(responseBody => {
        this.genres = responseBody.filter(x => x.parentGenre == null);
      });
  }

  public editGenre(id: string): void {
    this._router.navigate([`genres/update/${id}`]);
  }

  public deleteGenre(id: string): void {
    let modalRef = this._modalService.show(DeleteModalComponent, { class: 'modal-sm' });

    modalRef?.content?.onClose.subscribe(
      () => {
        this._genreService.deleteGenre(id)
          .subscribe({
            next: () => {
              this._toastr.success("Genre was deleted", "Success!");
              this.getAll();
            }
          });
      })
  }
}