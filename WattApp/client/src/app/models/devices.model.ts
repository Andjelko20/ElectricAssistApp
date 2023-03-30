export interface ShowDevices {
    id:number;
    userId: number;
    deviceCategory: string ;
    deviceType: string ;
    deviceBrand: string ;
    deviceModel: string ;
    name: string ;
    energyInKwh: number;
    standByKwh: number;
    visibility: boolean;
    controlability: boolean;
    turnOn: boolean;
};
export interface Devices {
    id:number;
    userId: number;
    deviceCategoryId: number ;
    deviceTypeId: number ;
    deviceBrandId: number ;
    deviceModelId: number ;
    name: string ;
    energyInKwh: number;
    standByKwh: number;
    visibility: boolean;
    controlability: boolean;
    turnOn: boolean;
};
