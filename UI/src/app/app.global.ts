import { Injectable } from "@angular/core";
import { HttpParams, HttpHeaders } from '@angular/common/http';
import { environment } from '../environments/environment';

@Injectable()

export class GlobalConstants {

  public static API_BASE_URL: string = environment.API_URL; 
}
