 export interface ProductListingMOdel  // this is the model and this name is not same as the filename(optional)
 {
    idOfProduct : string,
    nameOfProduct : string,
    descriptionOfProduct : string,
    priceOfProduct : number,
    availableQuantity : number,
    urlOfImage : string,
    Original_Image?: File | Blob | string;  //   '?' means it  may or maynot be present
 }