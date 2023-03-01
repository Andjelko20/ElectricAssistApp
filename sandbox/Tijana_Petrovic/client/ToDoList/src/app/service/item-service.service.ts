import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import {Item} from '../model/item';

@Injectable({
  providedIn: 'root'
})
export class ItemServiceService {

  serviceURL : string;
  constructor(private http : HttpClient) { 
    this.serviceURL = "https://localhost:7140/api/Item";
  }

  addItem(item : Item) : Observable<Item>{
    return this.http.post<Item>(this.serviceURL, item);

  }

  getAllItems() : Observable<Item[]>{
    return this.http.get<Item[]>(this.serviceURL);
  }

  deleteItem(item : Item) : Observable<Item>{
    return this.http.delete<Item>(this.serviceURL + '/' + item.id);

  }
}
