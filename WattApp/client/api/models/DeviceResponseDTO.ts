/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

export type DeviceResponseDTO = {
    Id?: number;
    UserId?: number;
    DeviceCategory?: string | null;
    DeviceType?: string | null;
    DeviceBrand?: string | null;
    DeviceModel?: string | null;
    readonly Name?: string | null;
    EnergyInKwh?: number;
    StandByKwh?: number;
    Visibility?: boolean;
    Controlability?: boolean;
    TurnOn?: boolean;
};
