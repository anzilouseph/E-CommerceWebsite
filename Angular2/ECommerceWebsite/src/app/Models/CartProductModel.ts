export interface CartProdcutsModel 
{
    idOfProduct: string,
    nameOfProduct: string,
    descriptionOfProduct: string,
    priceOfProduct: number,
    availableQuantity: number,
    idOfCategory: string,
    urlOfImage: string,
    nameOfCategory: string,
    Original_Image?: File | Blob | string;  //   '?' means it  may or maynot be present
    QuantityToBuy:number //this field is also not in the db, this field is when our product is in the cart we can increse the quantity of each producyts uniquly without affecting each other ,for that we use this field
    TotalPrice : number //quantity * price

}