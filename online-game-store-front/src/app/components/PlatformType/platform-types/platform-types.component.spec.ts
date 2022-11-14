import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PlatformTypesComponent } from './platform-types.component';

describe('PlatformTypesComponent', () => {
  let component: PlatformTypesComponent;
  let fixture: ComponentFixture<PlatformTypesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ PlatformTypesComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(PlatformTypesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
