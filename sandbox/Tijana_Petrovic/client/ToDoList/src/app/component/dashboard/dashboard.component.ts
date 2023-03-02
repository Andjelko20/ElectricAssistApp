import { Component, OnInit } from '@angular/core';
import { Item } from 'src/app/model/item';
import { ItemServiceService } from 'src/app/service/item-service.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit{
  itemObject : any;
  items : Item[] = [];

  addItemValue : string = '';

  constructor(private ItemServiceService : ItemServiceService){}

  ngOnInit(): void {
    this.items = [];
    this.getAllItems();
  }

  getAllItems(){
    this.ItemServiceService.getAllItems().subscribe(res => {
      this.items = res;
      console.log(this.items.values);
    }, err => {
      alert("NOPE!");
    })
  }

  addItem(){
    this.itemObject.name = this.addItemValue;
    this.ItemServiceService.addItem(this.itemObject).subscribe(res => {
      this.ngOnInit();
      this.addItemValue = "";
    }, err => {
      alert(err);
    })
  }

  deleteItem(mitem : Item){
    this.ItemServiceService.deleteItem(mitem).subscribe(res => {
      this.ngOnInit();
    }, err => {
      alert("NOPE!");
    })
  }
}
