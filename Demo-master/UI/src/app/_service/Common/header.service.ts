import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { HttpHeaders } from "@angular/common/http";
import { UserSession } from './../../models/UserSession/userSession';

@Injectable({
  providedIn: 'root'
})
export class HeaderService {
  usession: UserSession; 
  constructor(@Inject(PLATFORM_ID) private platformId: Object) {
    this.usession = {
      userId: '',
      token: '',
      tokenExpireTime: '',
      status: 0,
      message: '',
      loginTime: ''
    };
  }

  getTenantKeyHeaders() {  
    let headers = new HttpHeaders();
    headers = headers.set('Content-Type', 'application/json');   
    this.usession = JSON.parse((localStorage.getItem('session') || '{}'));
    if (this.usession != null) {
      headers = headers.set('Authorization', 'Bearer ' + this.usession.token);
    }
   return headers;
  }

getAPIHeaders() {
    let headers = new HttpHeaders().set('Content-Type', 'application/json');  
    return headers;
  }
}
