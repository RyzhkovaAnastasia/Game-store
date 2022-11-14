import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { faPencil } from '@fortawesome/free-solid-svg-icons';
import { ToastrService } from 'ngx-toastr';
import { Role } from 'src/app/classes/auth/role';
import { User } from 'src/app/classes/auth/user';
import { Publisher } from 'src/app/classes/publisher';
import { PublisherService } from 'src/app/services/publisher.service';
import { RoleService } from 'src/app/services/role.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-users',
  templateUrl: './users.component.html',
  styleUrls: ['./users.component.css']
})
export class UsersComponent {

  public users: User[] = [];
  public faPencil = faPencil;

  constructor(
    private readonly _userService: UserService,
    private readonly _publisherService: PublisherService,
    private readonly _toastrService: ToastrService,
    private readonly _router: Router
  ) { 
    this.getUsers();
  }

  getUsers(){
    this._userService.getAllUsers()
      .subscribe(
          (data) => this.users = data
      );
  }

  editUser(username: string){
    this._router.navigate([`users/update/${username}`]);
  }
}
