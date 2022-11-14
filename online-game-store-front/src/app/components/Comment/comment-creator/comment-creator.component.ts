import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Guid } from 'guid-typescript';
import { ToastrService } from 'ngx-toastr';
import { Comment } from 'src/app/classes/comment';
import { CommentService } from 'src/app/services/comment.service';
import { FormService } from 'src/app/services/form.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-comment-creator',
  templateUrl: './comment-creator.component.html',
  styleUrls: ['./comment-creator.component.css']
})

export class CommentCreatorComponent {

  public commentForm: FormGroup;
  public gamekey: string;

  @Input() isQuoted: boolean = false;
  @Input() parentCommentId: string | null;
  @Input() gameId: string;

  @Output() updateCommentList = new EventEmitter();

  constructor(
    private readonly _commentService: CommentService,
    private readonly _userService: UserService,
    private readonly _toastr: ToastrService,
    private readonly _route: ActivatedRoute,
    private readonly _formService: FormService
  ) {
    this.gameId = '';
    this.parentCommentId = null;
    this.gamekey = this._route.snapshot.paramMap.get('gamekey') ?? '';
    this.commentForm = this.formInit();
  }

  getCommentList(): void {
    this.updateCommentList.emit();
  }

  createComment(): void {
    if (this.commentForm.valid) {
      const requestBody = this.getFromForm();

      this._commentService.createComment(requestBody)
        .subscribe({
          next: () => {
            this._toastr.success("Comment was created", "Success!");
            this.getCommentList();
          }
        });

    } else {
      this.commentForm.markAllAsTouched();
    }
  }

  formInit(): FormGroup {
    return new FormGroup(
      {
        id: new FormControl(Guid.EMPTY),
        name: new FormControl(this._userService.getUser.username ?? '', [Validators.required, Validators.maxLength(100)]),
        body: new FormControl('', [Validators.required, Validators.maxLength(1000)]),
        isQuoted: new FormControl(this.isQuoted),
        gameId: new FormControl(this.gameId),
        parentCommentId: new FormControl(this.parentCommentId),
        parentCommentName: new FormControl(''),
        parentCommentBody: new FormControl(''),
        childComments: new FormControl([]),
      });
  }

  getFromForm(): Comment {
    let comment: Comment = new Comment(Guid.EMPTY, '', '', false, '', '', '', '', []);

    this._formService.getFromForm(comment, this.commentForm);

    comment.parentCommentId = this.parentCommentId;
    comment.isQuoted = this.isQuoted;
    comment.gameId = this.gameId;

    return comment;
  }
}
