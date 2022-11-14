import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { faBan, faPencil, faQuoteLeft, faReply, faTrash } from '@fortawesome/free-solid-svg-icons';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { CommentService } from 'src/app/services/comment.service';
import { Comment } from 'src/app/classes/comment'
import { DeleteModalComponent } from '../../Shared/delete-modal/delete-modal.component';
import { ViewportScroller } from '@angular/common';
import { GameService } from 'src/app/services/game.service';
import { UserService } from 'src/app/services/user.service';
import { User } from 'src/app/classes/auth/user';
import { Role } from 'src/app/enums/role';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.css']
})
export class CommentsComponent implements OnInit {

  public modalRef?: BsModalRef;
  public comments: Comment[] = [];
  public gamekey: string = '';
  public gameId: string = '';
  public options;
  public user: User;
  public isInRole: boolean;

  public faDelete = faTrash;
  public faReply = faReply;
  public faQuote = faQuoteLeft;
  public faBan = faBan;
  public faEdit = faPencil;

  constructor(
    private readonly _commentService: CommentService,
    private readonly _userService: UserService,
    private readonly _gameService: GameService,
    private readonly _toastr: ToastrService,
    private readonly _modalService: BsModalService,
    private readonly _route: ActivatedRoute,
    private readonly _viewportScroller: ViewportScroller,
    private readonly _router: Router
  ) {
    this.gamekey = this._route.snapshot.paramMap.get('gamekey') ?? '';

    this.options = {
      displayField: 'name',
      isExpandedField: 'expanded',
      animateExpand: true,
      childrenField: 'childComments'
    };

    this.user = _userService.getUser;
    this.isInRole = this.user.role === Role.Manager || this.user.role === Role.Admin || this.user.role === Role.Moderator;
  }

  ngOnInit(): void {
    this.getGameId();
    this.getAll();
  }

  public getAll(): void {
    this._commentService.getComments(this.gamekey)
      .subscribe(
        {
          next: responseBody => this.comments = responseBody
        });
  }

  public getGameId(): void {
    this._gameService.getGame(this.gamekey)
      .subscribe(
        {
          next: responseBody => this.gameId = responseBody.id
        });
  }

  public banUser(): void {
    this._router.navigate(['commentban/duration']);
  }

  public deleteComment(id: string): void {
    let modalRef = this._modalService.show(DeleteModalComponent, { class: 'modal-sm' });

    modalRef?.content?.onClose.subscribe(
      () => {
        this._commentService.deleteComment(id)
          .subscribe({
            next: () => {
              this._toastr.success("Comment was deleted", "Success!");
              this.getAll();
            }
          });
      })
  }

  public onAuthorClick(elementId: string): void {
    this._viewportScroller.scrollToAnchor(elementId);
  }
}
