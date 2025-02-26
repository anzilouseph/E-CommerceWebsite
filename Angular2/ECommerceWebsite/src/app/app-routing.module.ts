import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginCompComponent } from './login-comp/login-comp.component';
import { RegistrationCompComponent } from './registration-comp/registration-comp.component';
import { AdminLayoutComponent } from './Admin/AdminLayout/admin-layout/admin-layout.component';
import { AdminDashBoardComponent } from './Admin/AdminDashBoard/admin-dash-board/admin-dash-board.component';
import { AddUserCompComponent } from './Admin/AddUser/add-user-comp/add-user-comp.component';
import { UserMangementCompComponent } from './Admin/UserManagement/user-mangement-comp/user-mangement-comp.component';
import { GetByIdForAdminCompComponent } from './Admin/UserManagement/get-by-id-for-admin-comp/get-by-id-for-admin-comp.component';
import { SearchUserCompComponent } from './Admin/UserManagement/SearchUser/search-user-comp/search-user-comp.component';
import { AddProductComponent } from './Admin/ProductManagement/add-product/add-product.component';
import { GetAllProductsCompComponent } from './Admin/ProductManagement/GetAllProducts/get-all-products-comp/get-all-products-comp.component';
import { SearchProductCompComponent } from './Admin/ProductManagement/SearchProduct/search-product-comp/search-product-comp.component';
import { SearchCategoryCompComponent } from './Admin/ProductManagement/SearchCategory/search-category-comp/search-category-comp.component';
import { GetProductByIdCompComponent } from './Admin/ProductManagement/GetProductById/get-product-by-id-comp/get-product-by-id-comp.component';



const routes: Routes = [
  {path:"",component:LoginCompComponent},
  {path:"UserRegistration",component:RegistrationCompComponent},


  //for admin things
  {
    //here we are actually creating the childerns
    path:"admin",component:AdminLayoutComponent,children:[
                                                            { path: "", redirectTo: "dashboard", pathMatch: "full" }, // Redirect /admin to /admin/dashboard
                                                            { path: "dashboard", component: AdminDashBoardComponent } ,// redirect to dashboard when admin login
                                                           
                                                           //user managemnt paths
                                                            {path:"adduser",component:AddUserCompComponent}, //to redirect to Add User By Admin
                                                            
                                                            {path:"usermanagement",component:UserMangementCompComponent}, // THIS IS FOR GETALL
                                                            {path:"getbyid/:id",component:GetByIdForAdminCompComponent}, //for get by id
                                                            {path:"searchuser",component:SearchUserCompComponent} , //for search user by their name
                                                            {path:"addProduct",component:AddProductComponent}, //for add Product By The Admin
                                                          

                                                            
                                                            //product management paths
                                                            {path:"getAllProduct",component:GetAllProductsCompComponent},  //for get all the products
                                                            {path:"searchproduct",component:SearchProductCompComponent} , //for search a product
                                                            {path:"searchcategory",component:SearchCategoryCompComponent}, //for search a category
                                                            {path:"getProductById/:id",component:GetProductByIdCompComponent} , //for get the product by its id
                                                          ]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
