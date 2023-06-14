import {Component, OnInit, ViewChild } from '@angular/core';
import { MatSort, Sort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Pagination } from '../models/Pagination/pagination';
import { UserService } from '../_service/User/user.service';
import { User } from '../models/User/user';
import {
  MatSnackBar, MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition
} from '@angular/material/snack-bar';
@Component({
  selector: 'app-loginlog',
  templateUrl: './loginlog.component.html',
  styleUrls: ['./loginlog.component.scss']
})
export class LoginlogComponent implements OnInit {
  horizontalPosition: MatSnackBarHorizontalPosition = 'right';
  verticalPosition: MatSnackBarVerticalPosition = 'top';
  user: User;
  displayedColumns: string[] = ['isSuccess', 'loginTime', 'logoutTime']; 
  userlist: any[] = [];
  dataSource!: MatTableDataSource<any>;
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;
  sortDirection: any = 'asc';
  sortColumnName: any = '';
  pagination: Pagination;
  currentPage: number = 1;
  length = 100;
  pageSize = 5;
  pageSizeOptions: number[] = [5, 10, 25, 100];
  pageNumber: any; 
  constructor(private userservice: UserService, private snackBar: MatSnackBar) {   
    this.pagination = {
      id: '',
      PageNumber: 1,
      PageSize: 10,
      Sort: '',
      SortDirection: '',
      Search: ''
    };
    this.user = {
      id: '',
    };
  }
  ngOnInit(): void {   
    this.getuserData();
  }
  getuserData(): void {
    this.pagination.PageNumber = 1;
    this.pagination.PageSize = 100;
    this.userservice.getUserList(this.pagination).subscribe(
      (result: any) => {     
        if (result != null) {
          this.userlist = result.items;
           this.length = result.metaData.totalItems
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
  selectUser(id:any) {
    console.log(id);
    this.getuserLog(id);
  }  
  getuserLog(userid: any): void {
    this.pagination.id = userid;
    this.pagination.PageNumber = this.currentPage;
    this.pagination.PageSize = this.pageSize;
    this.pagination.Sort = this.sortColumnName;
    this.pagination.SortDirection = this.sortDirection;
    this.userservice.getUserLog(this.pagination).subscribe(
      (result: any) => {       
        if (result != null) {
          this.dataSource = new MatTableDataSource(result.items);
          this.length = result.metaData.totalItems
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

  onPageChange(event: any) {
    console.log(event)
    this.length = this.length;
    this.currentPage = event.pageIndex == 0 ? 1 : event.pageIndex;
    this.pageSize = event.pageSize
    this.getuserLog(this.user.id)
  }

  sortData(sort: Sort) {  
    this.sortDirection = sort.direction;
    this.sortColumnName = sort.active;
    this.getuserLog(this.user.id)
  }
}

