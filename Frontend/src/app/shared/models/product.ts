import { DecimalPipe } from "@angular/common";

export class Product{
    productId: number = 0;
    name: string = '';
    productNumber: string = '';
    color: string ='';
    standardCost: number = 0.0;
    listPrice: number =0;
    size: string = '';
    weight: number =0;
    productCategoryId: number = 0;
    productModelId: number = 0;
    sellStartDate?: Date;
    sellEndDate?:Date;
    discontinuedDate?:Date;
    thumbNailPhoto?: number[];
    thumbnailPhotoFileName:string ='';
    rowguid: any;
    modifiedDate?:Date;
    productCategory:any;
    productModel:any;
    salesOrderDetails:any;

    
}