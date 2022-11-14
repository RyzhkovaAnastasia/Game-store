import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Guid } from 'guid-typescript';
import { ToastrService } from 'ngx-toastr';
import { Comment } from 'src/app/classes/comment';
import { CommentService } from 'src/app/services/comment.service';
import { FormService } from 'src/app/services/form.service';

@Component({
  selector: 'app-comment-editor',
  templateUrl: './comment-editor.component.html',
  styleUrls: ['./comment-editor.component.css']
})

export class CommentEditorComponent implements OnInit {

  public commentForm: FormGroup;
  public gamekey: string;

  @Input() commentId: string;
  @Output() updateCommentList = new EventEmitter();

  constructor(
    private _commentService: CommentService,
    private _toastr: ToastrService,
    private _route: ActivatedRoute,
    private _formService: FormService
  ) {
    this.commentId = '';
    this.gamekey = this._route.snapshot.paramMap.get('gamekey') ?? '';
    this.commentForm = this.formInit(new Comment(Guid.EMPTY, '', '', false, '', '', '', '', []));
  }

  ngOnInit(): void {
    this.getComment();
  }

  emitUpdateCommentList(): void {
    this.updateCommentList.emit();
  }

  getComment(): void {
    this._commentService.getComment(this.gamekey, this.commentId)
      .subscribe({
        next: responseBody => this._formService.setToForm(responseBody, this.commentForm)
      });
  }

  editComment(): void {
    if (this.commentForm.valid) {

      let commentObj = new Comment(Guid.EMPTY, '', '', false, '', '', '', '', []);
      const requestBody =  this._formService.getFromForm(commentObj, this.commentForm) as Comment;

      this._commentService.editComment(requestBody)
        .subscribe({
          next: () => {
            this._toastr.success("Comment was created", "Success!");
            this.emitUpdateCommentList();
          }
        });

    } else {
      this.commentForm.markAllAsTouched();
    }
  }

  formInit(comment: Comment): FormGroup {
    return new FormGroup(
      {
        id: new FormControl(comment.id),
        name: new FormControl(comment.name, [Validators.required, Validators.maxLength(100)]),
        body: new FormControl(comment.body, [Validators.required, Validators.maxLength(1000)]),
        isQuoted: new FormControl(comment.isQuoted),
        gameId: new FormControl(comment.gameId),
        parentCommentId: new FormControl(comment.parentCommentId),
        parentCommentName: new FormControl(comment.parentCommentName),
        parentCommentBody: new FormControl(comment.parentCommentBody),
        childComments: new FormControl(null),
      });
  }
}