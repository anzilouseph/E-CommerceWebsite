import { Component } from '@angular/core';
import { ProductListingMOdel } from 'src/app/Models/ProductToListModel';
import { AdminServiceService } from 'src/app/Services/admin-service.service';
@Component({
  selector: 'app-get-all-products-comp',
  templateUrl: './get-all-products-comp.component.html',
  styleUrls: ['./get-all-products-comp.component.css']
})
export class GetAllProductsCompComponent {

  productModel : ProductListingMOdel[] = [];
  
  constructor(private service:AdminServiceService){}
  image:any;
  data:any;
  errorMessage:any;

  async ngOnInit():Promise<void>
  {
    await this.getAllProducts();
  }

  async getAllProducts():Promise<void>
  {
    await this.service.GetAllProducts().subscribe({
      next:(res:any)=>
      {
        if(res.status)
        {
          console.log(res.data);
          
          this.productModel = res.data.map((product:any)=>
                ({
                  ...product, // ✅ Copy existing product properties
                  Original_Image:""  // ✅ Initialize an empty field for the image
                })
            );
            console.log(this.productModel);
            
          this.productModel.forEach((product)=>   //for every product
            {
              // product.Original_Image="https://localhost:7165/"+product.urlOfImage

              console.log(product.urlOfImage);
                        const fileName = product.urlOfImage.split('/').pop(); // Extract filename
                        this.service.GetProductImage(fileName).subscribe({
                            next:(res1:any)=>
                              {
                                
                                console.log(`Blob received for ${product.idOfProduct}:`, res1);

                                // // ✅ Directly convert Blob to object URL instead of using FileReader
                                const objectURL = URL.createObjectURL(res1);
                                product.Original_Image = objectURL;

                                console.log(`Image loaded for: ${product.idOfProduct}`, objectURL);
                              
                              },
                            error:error=>
                              {
                                console.log(`Error fetching image for product: ${product.idOfProduct}`, error);
                                product.Original_Image = undefined;
                              }
                        });
            });
          
        }
        else
        {
          this.errorMessage = res.message
          console.log(res);          
        }
      },
      error:error=>
      {
        console.log(error);
        this.errorMessage=error.message;        
      }
    })
  }

}
