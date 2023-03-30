export interface Devices {
    id:number,
    userId: number;
    deviceCategory: string ;
    deviceType: string ;
    deviceBrand: string ;
    deviceModel: string ;
    readonly name: string ;
    energyInKwh: number;
    standByKwh: number;
    visibility: boolean;
    controlability: boolean;
    turnOn: boolean;
};
