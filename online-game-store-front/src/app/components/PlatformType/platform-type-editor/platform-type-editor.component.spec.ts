import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlatformTypeEditorComponent } from './platform-type-editor.component';

describe('PlatformTypeEditorComponent', () => {
  let component: PlatformTypeEditorComponent;
  let fixture: ComponentFixture<PlatformTypeEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlatformTypeEditorComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PlatformTypeEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
