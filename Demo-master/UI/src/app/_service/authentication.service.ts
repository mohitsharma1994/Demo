import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GlobalConstants } from './../app.global';
import { HeaderService } from './Common/header.service';
import { Login } from './../models/Login/login';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  constructor(private http: HttpClient, private headerService: HeaderService) { }
  getLoginUser(loginInfo: Login): Observable<any> {  
    let url: string = GlobalConstants.API_BASE_URL + 'Application/Login';
    return this.http.post(url, loginInfo, { headers: this.headerService.getAPIHeaders() });
  }
  logout(): Observable<any> {
    let url: string = GlobalConstants.API_BASE_URL + 'Application/LogOut';
    return this.http.post(url, {}, { headers: this.headerService.getTenantKeyHeaders() });    

  }
  public get loggedIn(): boolean {
    return (localStorage.getItem('session') !== null);
  }
} 
