import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginlogComponent } from './loginlog.component';

describe('LoginlogComponent', () => {
  let component: LoginlogComponent;
  let fixture: ComponentFixture<LoginlogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ LoginlogComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(LoginlogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
