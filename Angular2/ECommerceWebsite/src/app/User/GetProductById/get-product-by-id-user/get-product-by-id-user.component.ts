import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductListingMOdel } from 'src/app/Models/ProductToListModel';
import { AdminServiceService } from 'src/app/Services/admin-service.service';

@Component({
  selector: 'app-get-product-by-id-user',
  templateUrl: './get-product-by-id-user.component.html',
  styleUrls: ['./get-product-by-id-user.component.css']
})
export class GetProductByIdUserComponent {

   constructor(private AService:AdminServiceService,private route:ActivatedRoute){}
  
    productList : ProductListingMOdel | null = null;
    errorMessage:any;
    id:any;

    ngOnInit()
    {
      this.GetProductById();
    }
  
    GetProductById()
    {
      this.id = this.route.snapshot.params["id"];
      this.AService.GetProductById(this.id).subscribe({
                                                        next:(res:any)=>
                                                        {
                                                          if(res.status)
                                                          {
                                                            console.log("product get succesfully but this product only have the imagename not the original image");
                                                            this.productList = {...res.data,Original_Image:""};
                                                            
                                                              const fileName = this.productList?.urlOfImage.split('/').pop();
                                                              this.AService.GetProductImage(fileName).subscribe({
                                                                                                                  next:(res1:any)=>
                                                                                                                  {
                                                                                                                    console.log("Image get successfully");
  
                                                                                                                    const objUrl = URL.createObjectURL(res1);
                                                                                                                    this.productList!.Original_Image = objUrl;
                                                                                                                    
                                                                                                                  },
                                                                                                                  error:error=>
                                                                                                                  {
                                                                                                                    this.errorMessage=error.message;
                                                                                                                    console.log(error);    
                                                                                                                  }
                                                                                                                 })
                                                          }
                                                          else
                                                          {
                                                            console.log("the product is not get");
                                                            this.errorMessage=res.message;        
                                                            
                                                          }
                                                        },
                                                        error:error=>
                                                        {
                                                          console.log("it comes to the error message of getProduct ByID")
                                                          this.errorMessage=error.message;
                                                        }
                                                      });
    }
}
