import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { AdminServiceService } from 'src/app/Services/admin-service.service';
import { FormControl,FormGroup,Validators } from '@angular/forms';
@Component({
  selector: 'app-get-by-id-for-admin-comp',
  templateUrl: './get-by-id-for-admin-comp.component.html',
  styleUrls: ['./get-by-id-for-admin-comp.component.css']
})
export class GetByIdForAdminCompComponent {

  constructor(private adminService:AdminServiceService,private router:Router,private route:ActivatedRoute) {}

data:any;
errorMessage:any;
userData:any;
id:any;

getByIdUserForm = new FormGroup(  // this form is to show the details of the user in a form format
  {
    nameOfUser:new FormControl({value:"",disabled:true}),
    phoneOfUser: new FormControl({value:"",disabled:true}),
    emailOfUser:new FormControl({value:"",disabled:true}),
    passwordOfUser:new FormControl({value:"",disabled:true}),
    roleOfUser : new FormControl({value:"",disabled:true}),
  });

ngOnInit()
{
  this.getUserByAdmin();
}

getUserByAdmin()
{
  this.id=this.route.snapshot.params["id"];
  this.adminService.GetUserByAdminServiceFn(this.id).subscribe(
    {
      next:(res:any)=>
      {
        if(res.status)
        {
          this.userData=res.data;
          console.log(res);
          this.populateForm();
        }
        else
        {
          this.errorMessage = res.message;
          console.log(res);        
        }
      },
      error:error=>
      {
        this.errorMessage=error.message;
      }
    });
  }

  
  populateForm() //this fn is to generate the form with corresponding data
  {
    this.getByIdUserForm.patchValue(
      {
        nameOfUser:this.userData.nameOfUser,
        phoneOfUser:this.userData.phoneOfUser,
        emailOfUser:this.userData.emailOfUser,
      }
    )
  }

  //Get The Users Details while clicking the view button ENDS here


}
