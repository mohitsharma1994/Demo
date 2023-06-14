import { Component, OnInit, Input } from '@angular/core';
import { AuthenticationService } from '../_service/authentication.service';
import { Router } from '@angular/router';
import {
  MatSnackBar, MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition
} from '@angular/material/snack-bar';
@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.scss']
})
export class NavbarComponent implements OnInit {
  horizontalPosition: MatSnackBarHorizontalPosition = 'right';
  verticalPosition: MatSnackBarVerticalPosition = 'top';
  user: any = {};
  showNavbar: boolean = false;
  progressbar: number = 0;
  @Input() public isUserLoggedIn: boolean = false;;
  constructor(private router: Router, private _auth: AuthenticationService,
    private snackBar: MatSnackBar
  ) {
  }

  ngOnInit(): void {
    
  }

  logout() {
    this._auth.logout().subscribe(
      (result: any) => {      
        if (result != null) {        
          window.localStorage.removeItem('session');
          this.showNavbar = false;
          this.router.navigate(['']);
        }
      },
      (error: any) => {       
        this.snackBar.open("Somting Wrong", "OK", {
          horizontalPosition: this.horizontalPosition,
          verticalPosition: this.verticalPosition,
          duration: 4000,
        });
      });;

  }
}
