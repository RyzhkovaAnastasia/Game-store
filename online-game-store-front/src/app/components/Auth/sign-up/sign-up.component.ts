import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { faEnvelope, faLock, faUser } from '@fortawesome/free-solid-svg-icons';
import { ToastrService } from 'ngx-toastr';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-sign-up',
  templateUrl: './sign-up.component.html',
  styleUrls: ['./sign-up.component.css']
})
export class SignUpComponent {

  public form = new FormGroup({
    username: new FormControl(null, Validators.required),
    email: new FormControl(null, [Validators.required, Validators.email]),
    password: new FormControl(null, [Validators.required, Validators.minLength(6)])
  });

  faEnvelope = faEnvelope;
  faLock = faLock;
  faUser = faUser;
  
  public isEmailExsists = false;

  constructor(
    private userService: UserService,
    private router: Router,
    private toastr: ToastrService
  ) { }

  public submit(): void {
    if (!this.form.valid) {
      this.form.markAllAsTouched();
      return;
    }

    this.form.disable();

    this.userService.signUp(this.form.value)
      .subscribe(
        () => {
          this.router.navigate(['games/'])
          this.toastr.success("Sign up was successful", "Success");
        },
        error => {
            this.form.enable();
        }
      );
  }
}
