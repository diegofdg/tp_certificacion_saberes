import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { ConfigApp } from '../classs/ConfigApp';


@Injectable({
  providedIn: 'root'
})
export class LoginService {
  private urlApi:string;


  constructor(private http:HttpClient) { 
    let config=new ConfigApp();
    this.urlApi= config.urlApi+'Login/';    
  }

  login(email: string, password: string): Observable<any> {
    const body = {
      email,
      password
    }
        
    return this.http.post(this.urlApi, body);
  }
}
