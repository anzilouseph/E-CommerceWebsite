export interface UserToModel
{
    idOfUser:string,
    nameOfUser:string,
    phoneOfUser:string,
    emailOfUser:string,
    roleOfUser:string,
    profileImage:string,
    OriginalImage?: File | Blob | string , //   '?' means it  may or maynot be present
}