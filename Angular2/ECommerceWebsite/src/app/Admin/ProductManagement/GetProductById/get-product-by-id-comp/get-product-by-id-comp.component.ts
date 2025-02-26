import { Component } from '@angular/core';
import { AdminServiceService } from 'src/app/Services/admin-service.service';
import { ActivatedRoute } from '@angular/router';
import { ProductListingMOdel } from 'src/app/Models/ProductToListModel';
@Component({
  selector: 'app-get-product-by-id-comp',
  templateUrl: './get-product-by-id-comp.component.html',
  styleUrls: ['./get-product-by-id-comp.component.css']
})
export class GetProductByIdCompComponent {

  constructor(private service:AdminServiceService,private route:ActivatedRoute){}

  productModel :ProductListingMOdel[] =[];
  idOfProduct:any;
  errorMessage:any;
  data:any;
  getProductById()
  {
    this.idOfProduct = this.route.snapshot.params['id'];
    this.service.GetProductById(this.idOfProduct).subscribe({
      next:(res:any)=>
      {
        if(res.status)
        {
          
        }
        else{
          console.log(res.message);
          this.errorMessage=res.message;
        }
      }
    })
  }
}
