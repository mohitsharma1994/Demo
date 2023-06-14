import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort, Sort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import {
  MatSnackBar, MatSnackBarHorizontalPosition,
  MatSnackBarVerticalPosition
} from '@angular/material/snack-bar';
import { UserService } from '../_service/User/user.service';
import { Pagination } from '../models/Pagination/pagination';
@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent implements OnInit {
  horizontalPosition: MatSnackBarHorizontalPosition = 'right';
  verticalPosition: MatSnackBarVerticalPosition = 'top';
  displayedColumns: string[] = ['id', 'firstName', 'lastName', 'email', 'phoneNumber'];
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

  constructor(private snackBar: MatSnackBar,private userservice: UserService) {
    this.pagination = {
      id: '',
      PageNumber: 1,
      PageSize: 10,
      Sort: '',
      SortDirection: '',
      Search: ''
    };
  }

  ngOnInit(): void {
    this.getuserlist();
  }

  getuserlist(): void {
    this.pagination.PageNumber = this.currentPage;
    this.pagination.PageSize = this.pageSize;
    this.pagination.Sort = this.sortColumnName;
    this.pagination.SortDirection = this.sortDirection;
    this.pagination.Search = this.pagination.Search
    this.userservice.getUserList(this.pagination).subscribe(
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
          duration: 4000,
        });
      });
  }

  openEditForm(data: any) {
    console.log(data);
  }

  applyFilter() {   
    this.sortDirection = this.sortDirection;
    this.sortColumnName = this.sortColumnName;
    this.pagination.Search = this.pagination.Search;
    this.getuserlist()
  }

  onPageChange(event: any) {
    console.log(event)
    this.length = this.length;

    if (event.previousPageIndex === 0 && event.pageIndex === 1) {
      event.pageIndex = 2
    }
    this.currentPage = event.pageIndex == 0 ? 1 : event.pageIndex;
    event.previousPageIndex = event.pageIndex -1
    this.pageSize = event.pageSize
    this.getuserlist()
  }

  sortData(sort: Sort) {
    this.sortDirection = sort.direction;
    this.sortColumnName = sort.active;
    this.getuserlist()
  }

}
