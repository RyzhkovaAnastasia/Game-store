import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { ToastrService } from 'ngx-toastr';
import { User } from 'src/app/classes/auth/user';
import { FormService } from 'src/app/services/form.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-user-settings',
  templateUrl: './user-settings.component.html',
  styleUrls: ['./user-settings.component.css']
})
export class UserSettingsComponent {
  public userForm: FormGroup;

  constructor(
    private readonly _userService: UserService,
    private readonly _translator: TranslateService,
    private readonly _formService: FormService,
    private readonly _toastrService: ToastrService
  ) {
    this.userForm = this.formInit();
    this.getUser();
   }

   public getUser(){
    this._formService.setToForm(this._userService.getUser, this.userForm);
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

  public changeLanguage(langCode: string){
    this._translator.setDefaultLang(langCode);
    this._translator.use(langCode);
    localStorage.setItem("language", langCode);
  }
}
