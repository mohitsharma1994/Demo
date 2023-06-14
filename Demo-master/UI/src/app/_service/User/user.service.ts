import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { GlobalConstants } from './../../app.global';
import { HeaderService } from '../Common/header.service';
import { User } from '../../models/User/user';
import { Pagination } from '../../models/Pagination/pagination';

@Injectable({
  providedIn: 'root'
})
export class UserService {

  constructor(private http: HttpClient, private headerService: HeaderService) { }
  getUserList(pageinfo: Pagination): Observable<any> {   
    let url: string = GlobalConstants.API_BASE_URL + 'User?PageNumber=' + pageinfo.PageNumber + '&PageSize=' + pageinfo.PageSize + '&Sort=' + pageinfo.Sort + '&SortDirection=' + pageinfo.SortDirection + '&Search=' + pageinfo.Search;     
    return this.http.get(url,{ headers: this.headerService.getTenantKeyHeaders() });
  }
  getUserLog(pageinfo: Pagination): Observable<any> {   
    let url: string = GlobalConstants.API_BASE_URL + 'User/' + pageinfo.id + '/logs?PageNumber=' + pageinfo.PageNumber + '&PageSize=' + pageinfo.PageSize + '&Sort=' + pageinfo.Sort + '&SortDirection=' + pageinfo.SortDirection + '&Search=' + pageinfo.Search;
    return this.http.get(url, { headers: this.headerService.getTenantKeyHeaders() });
  }
  addUser(user: User): Observable<any> {  
    user.phoneNumber=user.phoneNumber?.toString();
    let url: string = GlobalConstants.API_BASE_URL + 'User';
    return this.http.post(url, user, { headers: this.headerService.getTenantKeyHeaders() });
  }
  getUserById(Id: string): Observable<any> {  
    let url: string = GlobalConstants.API_BASE_URL + 'User/' + Id;
    return this.http.get(url, { headers: this.headerService.getTenantKeyHeaders() });
  }  
} 
