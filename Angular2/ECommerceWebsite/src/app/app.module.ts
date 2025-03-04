import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import {HttpClientModule} from '@angular/common/http'
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginCompComponent } from './login-comp/login-comp.component';
import { ReactiveFormsModule } from '@angular/forms';
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
import { FormsModule } from '@angular/forms';
import { UserDashboardCompComponent } from './User/UserDashboard/user-dashboard-comp/user-dashboard-comp.component';
import { UserLayoutCompComponent } from './User/UserLayout/user-layout-comp/user-layout-comp.component';
import { GetProductByIdUserComponent } from './User/GetProductById/get-product-by-id-user/get-product-by-id-user.component';
@NgModule({
  declarations: [
    AppComponent,
    LoginCompComponent,
    RegistrationCompComponent,
    AdminLayoutComponent,
    AdminDashBoardComponent,
    AddUserCompComponent,
    UserMangementCompComponent,
    GetByIdForAdminCompComponent,
    SearchUserCompComponent,
    AddProductComponent,
    GetAllProductsCompComponent,
    SearchProductCompComponent,
    SearchCategoryCompComponent,
    
    GetProductByIdCompComponent,
         UserDashboardCompComponent,
         UserLayoutCompComponent,
         GetProductByIdUserComponent,
      ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    ReactiveFormsModule,
    FormsModule,
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
