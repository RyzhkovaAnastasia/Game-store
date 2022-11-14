import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { faEnvelope, faLock } from '@fortawesome/free-solid-svg-icons';
import { ToastrService } from 'ngx-toastr';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-sign-in',
  templateUrl: './sign-in.component.html',
  styleUrls: ['./sign-in.component.css']
})
export class SignInComponent {

  faEnvelope = faEnvelope;
  faLock = faLock;

  public form = new FormGroup({
    email: new FormControl(null, Validators.required),
    password: new FormControl(null, Validators.required)
  });

  public isInvalidCredentials = false;

  constructor(
    private userService: UserService,
    private router: Router,
    private toasrt: ToastrService
  ) { }

  public submit(): void {
    if (!this.form.valid) {
      this.form.markAllAsTouched();
      return;
    }
    this.form.disable();
    
    this.userService.signIn(this.form.value)
      .subscribe(
        () => {
          this.router.navigate(['games/'])
        },
        error => {
            this.form.enable();
        }
      );
  }

}
