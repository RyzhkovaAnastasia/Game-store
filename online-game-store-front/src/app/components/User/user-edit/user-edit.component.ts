import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Route, Router } from '@angular/router';
import { BsModalService } from 'ngx-bootstrap/modal';
import { ToastrService } from 'ngx-toastr';
import { Role } from 'src/app/classes/auth/role';
import { User } from 'src/app/classes/auth/user';
import { Publisher } from 'src/app/classes/publisher';
import { FormService } from 'src/app/services/form.service';
import { PublisherService } from 'src/app/services/publisher.service';
import { RoleService } from 'src/app/services/role.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent {

  public roles: Role[] = [];
  public publishers: Publisher[] = [];
  public userForm: FormGroup;

  constructor(
    private readonly _userService: UserService,
    private readonly _roleService: RoleService,
    private readonly _publisherService: PublisherService,
    private readonly _formService: FormService,
    private readonly _toastrService: ToastrService,
    private readonly _route: ActivatedRoute
  ) {
    const username = _route.snapshot.paramMap.get('username') ?? '';
    this.getRoles();
    this.getPublishers();
    this.userForm = this.formInit();
    this.getUser(username);
   }

   public getUser(username: string){
    this._userService.getUserByName(username)
    .subscribe(
      (data)=> {
        this._formService.setToForm(data, this.userForm);
      }
    );
  }

   public editUser(){
    let user = new User();
    this._formService.getFromForm(user, this.userForm);
    this._userService.editUser(user).subscribe(
      ()=> {
        this._toastrService.success("Edit was successful!", "Success!");
      }
    );
  }
  
  public getRoles(){
    this._roleService.getRoles()
    .subscribe(
      (data) => this.roles = data
    );
  }

  public getPublishers(){
    this._publisherService.getPublishers()
    .subscribe(
      (data) => this.publishers = data
    );
  }

  private formInit(): FormGroup {
    return new FormGroup(
      {
        id: new FormControl(),
        username: new FormControl('', Validators.required),
        role: new FormControl('', Validators.required),
        email: new FormControl('', [Validators.email, Validators.required]),
        publisherId: new FormControl('')
      });
  }
}
