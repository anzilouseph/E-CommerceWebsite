import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { CartProdcutsModel } from 'src/app/Models/CartProductModel';
import { AdminServiceService } from 'src/app/Services/admin-service.service';
import { UserServiceService } from 'src/app/Services/user-service.service';
import Swal from 'sweetalert2';
import { UserLayoutCompComponent } from '../../UserLayout/user-layout-comp/user-layout-comp.component';
@Component({
  selector: 'app-cart-comp',
  templateUrl: './cart-comp.component.html',
  styleUrls: ['./cart-comp.component.css']
})
export class CartCompComponent {

  constructor(private uservice:UserServiceService,private Aservice:AdminServiceService,private router:Router,private userlayout:UserLayoutCompComponent){}

  cartProducts :CartProdcutsModel [] = [] ;
  errorMessage:any;

                            Toast = Swal.mixin({  //for toast
                              toast: true,
                              position: 'top-right',
                              iconColor: 'white',
                              customClass: {
                                popup: 'colored-toast',
                              },
                              showConfirmButton: false,
                              timer: 1500,
                              timerProgressBar: true,
                            });

  ngOnInit()
  {
    this.GetProducts();
  }

  GetProducts()
  {
    this.uservice.GetProductsFromCart().subscribe({
      next:(res:any)=>
      {
        if(res.status)
        {
          console.log(res);
          
          this.cartProducts = res.data.map((product:any)=>({
                                                              ...product,  
                                                              Original_Image:"",
                                                              QuantityToBuy : 1,
                                                              TotalPrice : product.priceOfProduct,
                                                          })
                                                        );
          this.cartProducts.forEach(product=>
                                            {
                                              const fileName = product.urlOfImage.split('/').pop();
                                              this.Aservice.GetProductImage(fileName).subscribe({
                                                                                                  next:(res1:any)=>
                                                                                                  {
                                                                                                    const objUrl = URL.createObjectURL(res1);
                                                                                                   product.Original_Image=objUrl;
                                                                                                  },
                                                                                                  error:error=>
                                                                                                  {
                                                                                                    console.log(`Error fetching image for product: ${product.idOfProduct}`, error);
                                                                                                    product.Original_Image = undefined;
                                                                                                  }
                                                                                                  });
                                            }
                                      )
                                      console.log(this.cartProducts);

                                     this.calculateCartTotal()   //this fn is to SHOW THE ACTUAL PRICE FOR BUY INCLUDING ALL THE PRODUCTS PRICE

                                      
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
    });
  }


    singleProduct : CartProdcutsModel | null = null  //this fn is for when i click on a product in the cart it ned to dispkay the full details of the product
      singleView(id: any) {
        this.singleProduct = this.cartProducts.find((product) => product.idOfProduct == id) || null;
      }

      cart()
      {
        this.singleProduct=null;
      }


//for delete a prodcut from the CART            
  DeleteProductFromCart(pid:any)
  {
    this.uservice.DeleteFromCart(pid).subscribe({
      next:(res:any)=>
      {
        if(res.status)
        {
          this.cartProducts=this.cartProducts.filter((products)=> products.idOfProduct!= pid);
          console.log(res);
          this.userlayout.GetCountOfCart();
          this.Toast.fire({
                            position:'bottom',
                            icon: 'success',
                            title: res.message,
                            background: 'rgba(0, 0, 0, 0.8)', // Black with slight transparency
                            color: 'white', // White text
                            toast: true,
                            showConfirmButton: false,
                            timerProgressBar: false,
                          });

          this.calculateCartTotal(); //when the productis removed frm cart the price must need to minus that price
        }
        else
        {
          console.log(res);
          
          this.Toast.fire({
                            position:'bottom',
                            icon: 'error',
                            title: res.message,
                            background: 'rgba(0, 0, 0, 0.8)', // Black with slight transparency
                            color: 'white', // White text
                            toast: true,
                            showConfirmButton: false,
                            timerProgressBar: false,
                          });
        }
      },
      error:error=>
      {
        console.log(error);
        alert(error.message);
      }
    });
  }


  //this fn is to show the tota price a single product based on their quantity
  updateTotalPrice(product: CartProdcutsModel) {
    product.TotalPrice = product.QuantityToBuy * product.priceOfProduct;
    this.calculateCartTotal() ; //here we are the the beloow fn cuz when ever we make a change in the quantity the total price that product and the tital price of the Overall cart both changes;
  }


  //this fn is to SHOW THE ACTUAL PRICE FOR BUY INCLUDING ALL THE PRODUCTS PRICE
  cartTotal:number=0;
  calculateCartTotal()
  {
    let total = 0;
    for(let product of this.cartProducts)
    {
      total = total + product.TotalPrice;
    }
    this.cartTotal = total;
  }
}
