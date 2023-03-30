/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

export type DeviceRequestDTO = {
    Id?: number;
    UserId?: number;
    DeviceCategoryId?: number;
    DeviceTypeId?: number;
    DeviceBrandId?: number;
    DeviceModelId?: number;
    Name?: string | null;
    EnergyInKwh?: number;
    StandByKwh?: number;
    Visibility?: boolean;
    Controlability?: boolean;
    TurnOn?: boolean;
};
