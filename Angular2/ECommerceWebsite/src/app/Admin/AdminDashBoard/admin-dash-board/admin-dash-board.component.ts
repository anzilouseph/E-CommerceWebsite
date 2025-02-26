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
  }

  totalUsers:any;
  errorMessage:any;
  GetUserCountByAdmin()
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
          this.errorMessage=res.message;
          console.log(res);         
        }
      },
      error:error=>
      {
        this.errorMessage=error.message;
        console.log(error);         
      }
    })
  }

}
