import { Component } from '@angular/core';
import { AdminServiceService } from 'src/app/Services/admin-service.service';
@Component({
  selector: 'app-admin-dash-board',
  templateUrl: './admin-dash-board.component.html',
  styleUrls: ['./admin-dash-board.component.css']
})
export class AdminDashBoardComponent {

  constructor(private adminService:AdminServiceService){}

  ngOnInit()
  {
   this.GetUserCountByAdmin();
   this.GetProductCount();
  }

  totalUsers:any;
  totalUsersErrorMessage:any;
  totalProducts:any;
  totalProductsErrorMessage:any;


  GetUserCountByAdmin() //to get the cout of total users
  {
    this.adminService.GettUserCount().subscribe({
      next:(res:any)=>
      {
        if(res.status)
        {
          this.totalUsers=res.data;
          console.log(res);        
        }
        else{
          this.totalUsersErrorMessage=res.message;
          console.log(res);         
        }
      },
      error:error=>
      {
        this.totalUsersErrorMessage=error.message;
        console.log(error);         
      }
    })
  }


  GetProductCount()
  {
    this.adminService.GetProductCountFn().subscribe({
      next:(res:any)=>
      {
        if(res.status)
        {
          this.totalProducts=res.data;
          console.log(res);        
        }
        else{
          this.totalProductsErrorMessage=res.message;
          console.log(res);         
        }
      },
      error:error=>
      {
        this.totalProductsErrorMessage=error.message;
        console.log(error);         
      }
    })
  }


}
