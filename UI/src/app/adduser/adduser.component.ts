import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { User } from '../models/User/user';
import { UserService } from '../_service/User/user.service';
import { Router, ActivatedRoute } from '@angular/router';
import { first } from 'rxjs/operators';
import {
  MatSnackBar, MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition
} from '@angular/material/snack-bar';
@Component({
  selector: 'app-adduser',
  templateUrl: './adduser.component.html',
  styleUrls: ['./adduser.component.scss']
})
export class AdduserComponent implements OnInit {
  form: FormGroup = new FormGroup({});
  user: User;
  isAddMode: boolean;
  hide: boolean;
  horizontalPosition: MatSnackBarHorizontalPosition = 'right';
  verticalPosition: MatSnackBarVerticalPosition = 'top';
  constructor(private fb: FormBuilder, private userservice: UserService, private route: ActivatedRoute, private router: Router, private snackBar: MatSnackBar  
  ) {
    this.hide = true;
    this.isAddMode = false;
    this.user = {
      id: '',
      firstName: '',
      lastName: '',
      userName: '',
      email: '',
      phoneNumber: '',
      password:'',
    };
}

  ngOnInit(): void {   
    this.form = this.fb.group({
      fname: [null, [Validators.required]],
      lname: [null, [Validators.required]],
      email: [null, [Validators.required, Validators.email]],
      mobile: [null, [Validators.required, Validators.pattern("^((\\+91-?)|0)?[0-9]{10}$")]],
      password: [null, [Validators.required, Validators.minLength(6), Validators.pattern('(?=.*[A-Z])(?=.*[0-9])[A-Za-z\d$@$!%*?&].{5,}')]]     
    });
   
    
  }

  saveDetails(form: any) {   
    if (!this.form.valid) {
      return;
    }
    this.userservice.addUser(this.user).subscribe(
      (result: any) => {
        console.log(result);
        if (result != null) {
          if (result['status'] != 201) {         
            this.snackBar.open(result['message'], "OK", {
              horizontalPosition: this.horizontalPosition,
              verticalPosition: this.verticalPosition,
              duration: 5000,
            });
            return;
          }
          this.router.navigate(['/user']);
          this.snackBar.open("User Add Successful", "OK", {
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
          duration: 5000,
        });
      });
  }  
}
