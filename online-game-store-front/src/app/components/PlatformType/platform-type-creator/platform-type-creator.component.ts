import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Guid } from 'guid-typescript';
import { ToastrService } from 'ngx-toastr';
import { PlatformType } from 'src/app/classes/platformType';
import { PlatformTypeService } from 'src/app/services/platform-type.service';

@Component({
  selector: 'app-platform-type-creator',
  templateUrl: './platform-type-creator.component.html',
  styleUrls: ['./platform-type-creator.component.css']
})
export class PlatformTypeCreatorComponent {

  public platformForm: FormGroup;

  constructor(
    private readonly _platformService: PlatformTypeService,
    private readonly _toastr: ToastrService
  ) {
    this.platformForm = this.formInit();
  }

  public createPlatform(): void {
    if (this.platformForm.valid) {
      let platform = new PlatformType(Guid.EMPTY, this.platformForm.controls['type'].value);
      this._platformService.createPlatformType(platform)
        .subscribe({
          next: () => {
            this._toastr.success("Platform type was created", "Success");
          }
        })
    } else {
      this.platformForm.markAllAsTouched();
    }
  }

  private formInit(): FormGroup {
    return new FormGroup(
      {
        type: new FormControl('', Validators.required)
      }
    );
  }
}
