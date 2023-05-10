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
export interface updateDevices {
    id:number;
    userId: number;
    name: string ;
    energyInKwh: number;
    standByKwh: number;
    visibility: boolean;
    controlability: boolean;
    turnOn: boolean;
};

export interface DeviceBrand  {
    id: number;
    name: string;
};
export interface DeviceModel {
    Id: number;
    Mark: string;
};

export interface DeviceType {
    id: number;
    categoryId: number;
    name: string;
};


export interface WeekByDay {
    day: number;
    month: string;
    year : number;
    energyUsageResult: number;
}

export interface YearsByMonth {
    month: string;
    year : number;
    energyUsageResult: number;
}

export interface DayByHour {
    energyUsageResult: number;
    hour:any;
    day:any;
    month: string;
    year : number;
}
export interface Durations {
    startTime: string;
    endTime: string;
    duration: string;
};


