import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { UpdateProfileImageModel } from 'src/app/Models/UpdateProfileImage';
import { UserToModel } from 'src/app/Models/UserModel';
import { UserServiceService } from 'src/app/Services/user-service.service';
@Component({
  selector: 'app-profile-comp',
  templateUrl: './profile-comp.component.html',
  styleUrls: ['./profile-comp.component.css']
})
export class ProfileCompComponent {
  constructor(private uservice :UserServiceService,private router:Router){}

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


  logout() {
    // ✅ Clear user session (optional: JWT, tokens, etc.)
    localStorage.clear();

    // ✅ Navigate to the login page
    this.router.navigate(['']);

    // ✅ Close the modal manually (if Bootstrap modal stays open)
    const modalBackdrop = document.querySelector('.modal-backdrop');
    if (modalBackdrop) {
      modalBackdrop.remove();
    }

    // ✅ Remove modal-open class from body
    document.body.classList.remove('modal-open');
  }


  //FOR DELETE PROFILE IMAGE OF USER STARTS HERE
  imageErrorMessage:any;

  deleteProfilePic()
  {
    this.uservice.DeleteProfileImage().subscribe({
      next:(res:any)=>
      {
        if(res.status)
        {
          console.log(res);   
          alert(res.message);
          this.getProfile();

        }
        else
        {
          console.log (res);        
          alert(res.message)
        }
      },
      error:error=>
      {
        alert(error.message)
      }
    })
  }
//FOR DELETE PROFILE IMAGE OF USER ENDS HERE




//FOR UPDATE PROFILE IMAGE STARTS HERE
newProfileImage:File | null = null;

onFileSelected(event:any)
{
  const file = event.target.files[0] ; //here we are storing the image to variable file
  if(file)
  {
    this.newProfileImage = file;  // we are storing that image into the variable selected file;
  }
}

updateProfileModel : UpdateProfileImageModel | null = null

 updateProfilePhoto()
 {
  if (!this.newProfileImage) {
    alert('No image selected');
    return;
  }

  const formData = new FormData();
  formData.append('profileimage', this.newProfileImage);
  this.uservice.updateProfileImage(formData).subscribe({
    next:(res:any)=>
    {
      if(res.status)
      {
        alert(res.message);
        console.log(res);
        this.getProfile();        
      }
      else
      {
        console.log(res);
        alert(res.message)
      }
    },
    error:error=>
    {
      console.log(error);
      alert(error.message);
    }
  })
 }




  isEdit:boolean=false;
  onEdit()
  {
    this.isEdit=true;
  }
  goback()
  {
    this.isEdit=false;
  }

  clear()
  {
    this.populateForm();
  }
  update()
  {
    if(this.profileForm.valid)
    {
      const profileData = this.profileForm.value;
      this.uservice.UpdateProfileDetails(profileData).subscribe({
        next:(res:any)=>
        {
          if(res.status)
          {
            this.isEdit=false;
            console.log(res);
            alert(res.message);
            this.getProfile();
          }
          else
          {
            alert(res.message);
          }
        },
        error:error=>
        {
          console.log(error);
          alert(error.message)
        }
      })
    }
  }

}
