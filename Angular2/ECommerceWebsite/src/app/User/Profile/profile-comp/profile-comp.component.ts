import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { UserToModel } from 'src/app/Models/UserModel';
import { UserServiceService } from 'src/app/Services/user-service.service';
@Component({
  selector: 'app-profile-comp',
  templateUrl: './profile-comp.component.html',
  styleUrls: ['./profile-comp.component.css']
})
export class ProfileCompComponent {
  constructor(private uservice :UserServiceService){}

  ngOnInit()
  {
    this.getProfile();
  }
  userModel : UserToModel | null = null;
  errorMessage:any;

  profileForm = new FormGroup(
  {
    nameOfUser:new FormControl("",[Validators.required,Validators.pattern("^[a-zA-Z ]+$")]),
    phoneOfUser: new FormControl("",[Validators.required,Validators.pattern("^[0-9]{10}$")]),
    emailOfUser:new FormControl("",[Validators.required, Validators.pattern("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$")]),
  }
  );


  getProfile()
  {
    this.uservice.GetOwnProfileServiceFn().subscribe({
      next:(res:any)=>
      {
        if(res.status)
        {
          this.userModel  = {...res.data,OriginalImage:""};
          this.populateForm();
          const fileName = this.userModel?.profileImage.split('/').pop();
          this.uservice.GetProfileImage(fileName).subscribe({
                                                              next:(res1:any)=>
                                                              {
                                                                console.log("Image get successfully");
                                                                const objUrl = URL.createObjectURL(res1);
                                                                this.userModel!.OriginalImage=objUrl;
                                                              },
                                                              error:error=>
                                                                {
                                                                  this.errorMessage=error.message;
                                                                  console.log(error);    
                                                                }
                                                            });
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


  populateForm()
  {
    this.profileForm.patchValue(
      {
        nameOfUser:this.userModel?.nameOfUser,
        phoneOfUser:this.userModel?.phoneOfUser,
        emailOfUser:this.userModel?.emailOfUser,
      }
    );
  }

  isEdit:boolean=false;
}
