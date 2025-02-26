import { Component } from '@angular/core';
import { AdminServiceService } from 'src/app/Services/admin-service.service';
import { AdminLayoutComponent } from 'src/app/Admin/AdminLayout/admin-layout/admin-layout.component';
import { ProductListingMOdel } from 'src/app/Models/ProductToListModel';
@Component({
  selector: 'app-search-category-comp',
  templateUrl: './search-category-comp.component.html',
  styleUrls: ['./search-category-comp.component.css']
})
export class SearchCategoryCompComponent {

 

  constructor(private service:AdminServiceService,private layout:AdminLayoutComponent){}

  productModel:ProductListingMOdel[] =[];

  data:any;
  errorMessage:any;
  searchCategory:any;

  ngOnInit()
  {
    this.getproductsByCategory()
  }

  getproductsByCategory()
  {
    this.searchCategory = this.layout.SearchCategoryForm.get('categoryName')?.value;
    this.service.GetProductsByCategory(this.searchCategory).subscribe(
    {
      next:(res:any)=>
      {
        console.log(res);
        
        if(res.status)
        {
          this.productModel=res.data.map((products:any)=>
                                            ({
                                                ...products, // ✅ Copy existing product properties
                                                Original_Image: "",   // ✅ Initialize an empty field for the image
                                            })
                                        );
                                        this.productModel.forEach( (products)=>
                                                                    {
                                                                      const filename = products.urlOfImage.split('/').pop();  // Extract filename
                                                                      this.service.GetProductImage(filename).subscribe(
                                                                                                                        {
                                                                                                                          next:(res1:any)=>
                                                                                                                          {
                                                                                                                             // // ✅ Directly convert Blob to object URL instead of using FileReader
                                                                                                                             const objectUrl = URL.createObjectURL(res1);
                                                                                                                             products.Original_Image = objectUrl;

                                                                                                                             console.log(`Image loaded for: ${products.idOfProduct}`, objectUrl);

                                                                                                                          },
                                                                                                                          error:error=>
                                                                                                                          {
                                                                                                                            console.log(`Error fetching image for product: ${products.idOfProduct}`, error);
                                                                                                                            products.Original_Image = undefined;
                                                                                                                          }
                                                                                                                        }
                                                                                                                      );
                                                                    }
                                                                  );
                // ✅ Reset the form field after search
                this.layout.SearchCategoryForm.reset();  // This ensures new searches work

        }
        else
        {
          console.log(res);        
          this.errorMessage=res.message;
        }

      },
      error:error=>
      {
          console.log(error);        
          this.errorMessage=error.message;
      }
    });

    
  }


}
