import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Guid } from 'guid-typescript';
import { ToastrService } from 'ngx-toastr';
import { PlatformType } from 'src/app/classes/platformType';
import { FormService } from 'src/app/services/form.service';
import { PlatformTypeService } from 'src/app/services/platform-type.service';

@Component({
  selector: 'app-platform-type-editor',
  templateUrl: './platform-type-editor.component.html',
  styleUrls: ['./platform-type-editor.component.css']
})
export class PlatformTypeEditorComponent {

  public platformForm: FormGroup;

  constructor(
    private readonly _platformService: PlatformTypeService,
    private readonly _toastr: ToastrService,
    private readonly _route: ActivatedRoute,
    private readonly _formService: FormService
  ) {
    let id = this._route.snapshot.paramMap.get('id') ?? '';
    this.getPlatform(id);
    this.platformForm = this.formInit();
  }

  private getPlatform(id: string): void {
    this._platformService.getPlatformType(id)
      .subscribe({
        next: responseBody => this._formService.setToForm(responseBody, this.platformForm)
      })
  }

  public editPlatform(): void {
    if (this.platformForm.valid) {
      let platform = new PlatformType('', '');
      this._formService.getFromForm(platform, this.platformForm);
      this._platformService.editPlatformType(platform)
        .subscribe({
          next: () => this._toastr.success("Platform type was created", "Success")
        })
    } else {
      this.platformForm.markAllAsTouched();
    }
  }

  private formInit(): FormGroup {
    return new FormGroup(
      {
        id: new FormControl(Guid.EMPTY),
        type: new FormControl('', Validators.required)
      }
    );
  }
}
