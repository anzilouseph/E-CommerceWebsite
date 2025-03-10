import { Component } from '@angular/core';
import { UserLayoutCompComponent } from '../../UserLayout/user-layout-comp/user-layout-comp.component';
import { SharedServicesService } from '../../SharedServices/shared-services.service';
import { ProductListingMOdel } from 'src/app/Models/ProductToListModel';
import { UserDashboardCompComponent } from '../../UserDashboard/user-dashboard-comp/user-dashboard-comp.component';
@Component({
  selector: 'app-get-product-by-category-comp',
  templateUrl: './get-product-by-category-comp.component.html',
  styleUrls: ['./get-product-by-category-comp.component.css']
})
export class GetProductByCategoryCompComponent {

  constructor(private layout:UserLayoutCompComponent,private sharedservices:SharedServicesService) {}

  ngOnInit()
  {
   
  }


  filteredProductModel : ProductListingMOdel[] = [];  //NAMMAL ALREADY ELLAAA PRODUCTS INEEM "productModel" enna oru variable il eduth veykkunund in USERDASHBOARD, so ini avdunn nammlde category il ulla products ine maathrtam filter cheyth ee variabke il veykkunnu,so oryu api call pooavnda aavshym illa

  getProductsByCategory(idOfCategory:any) //this is for gettting all teh products in a specuific category
  {
    
  }
}
