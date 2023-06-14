import { Component, } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { AuthenticationService } from '../_service/authentication.service';
import { Router } from '@angular/router';
import { Login } from '../models/Login/login';
import { UserSession } from './../models/UserSession/userSession';
import {
  MatSnackBar, MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition } from '@angular/material/snack-bar';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent {
  horizontalPosition: MatSnackBarHorizontalPosition = 'right';
  verticalPosition: MatSnackBarVerticalPosition = 'top';
  myForm: FormGroup; 
  loginInfo: Login;
  hide: boolean;
  usession: UserSession;
  constructor(private fb: FormBuilder, private router: Router, private snackBar: MatSnackBar, private _auth: AuthenticationService) {
    if (this._auth.loggedIn) {
      this.router.navigate(['']);
    }
    this.loginInfo = {
      userName: '',
      password: ''
    };
    this.usession = {
      userId: '', 
      token: '',
      tokenExpireTime: '',
      status: 0,
      message: '',
      loginTime:''
    };
    this.myForm = this.fb.group({
      username: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required]],
    });
    this.hide = true;
  }  
  onSubmit(): void {
    if (!this.myForm.valid) {    
      return;
    }       
    this._auth.getLoginUser(this.loginInfo).subscribe(      
      (result: any) => {      
        if (result != null) {
          if (result['status'] != 200) {             
            this.snackBar.open(result['message'], "OK", {
              horizontalPosition: this.horizontalPosition,
              verticalPosition: this.verticalPosition,
              duration: 4000,             
            });
            return;
          }
          this.usession = result;
          this.usession.loginTime = new Date().toString();
          window.localStorage.setItem('session', JSON.stringify(this.usession));
          this.router.navigate(['/user']);        
          this.snackBar.open("Login Successful", "OK", {
            horizontalPosition: this.horizontalPosition,
            verticalPosition: this.verticalPosition,
            duration: 4000,           
          });
        }
      },
      (error: any) => {       
        this.snackBar.open("Somting Wrong", "OK", {
          horizontalPosition: this.horizontalPosition,
          verticalPosition: this.verticalPosition,
          duration: 4000,
        });
      });
  }

}

