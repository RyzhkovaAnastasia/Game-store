
import { Component } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { CommentBanDuration } from 'src/app/enums/commentBanDuration';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-user-comment-ban',
  templateUrl: './user-comment-ban.component.html',
  styleUrls: ['./user-comment-ban.component.css']
})

export class UserCommentBanComponent {

  commentBanDuration: CommentBanDuration = CommentBanDuration.oneDay;

  constructor(
    private readonly _userService: UserService,
    private readonly _toastr: ToastrService
  ) {
  }

  banUser() {
    this._userService.banUserComments(this.commentBanDuration)
    .subscribe({
        next: responseBody => {
          this._toastr.success("Duration accepted", "Success");
          this.commentBanDuration = responseBody;
        } }
      )
  }
}
