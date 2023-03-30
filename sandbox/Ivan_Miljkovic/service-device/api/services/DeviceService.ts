/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { DeviceRequestDTO } from '../models/DeviceRequestDTO';
import type { DeviceResponseDTO } from '../models/DeviceResponseDTO';

import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class DeviceService {

    /**
     * Get device details by device id.
     * @param id 
     * @returns DeviceResponseDTO Success
     * @throws ApiError
     */
    public static getApiDevice(
id: number,
): CancelablePromise<DeviceResponseDTO> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Device/{id}',
            path: {
                'id': id,
            },
        });
    }

    /**
     * @param id 
     * @returns any Success
     * @throws ApiError
     */
    public static deleteApiDevice(
id: number,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'DELETE',
            url: '/api/Device/{id}',
            path: {
                'id': id,
            },
        });
    }

    /**
     * @param requestBody 
     * @returns any Success
     * @throws ApiError
     */
    public static postApiDevice(
requestBody?: DeviceRequestDTO,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Device',
            body: requestBody,
            mediaType: 'application/json',
        });
    }

    /**
     * Get all visible devices for every user if you are DSO and only your devices if you are PROSUMER.
     * @returns any Success
     * @throws ApiError
     */
    public static getApiDevice1(): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Device',
        });
    }

    /**
     * @param requestBody 
     * @returns any Success
     * @throws ApiError
     */
    public static putApiDevice(
requestBody?: DeviceRequestDTO,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'PUT',
            url: '/api/Device',
            body: requestBody,
            mediaType: 'application/json',
        });
    }

    /**
     * @param deviceId 
     * @returns any Success
     * @throws ApiError
     */
    public static putApiDeviceTurnOn(
deviceId: number,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'PUT',
            url: '/api/Device/turnOn{deviceId}',
            path: {
                'deviceId': deviceId,
            },
        });
    }

    /**
     * @param deviceId 
     * @returns any Success
     * @throws ApiError
     */
    public static putApiDeviceControlability(
deviceId: number,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'PUT',
            url: '/api/Device/controlability{deviceId}',
            path: {
                'deviceId': deviceId,
            },
        });
    }

    /**
     * @param deviceId 
     * @returns any Success
     * @throws ApiError
     */
    public static putApiDeviceVisibility(
deviceId: number,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'PUT',
            url: '/api/Device/visibility{deviceId}',
            path: {
                'deviceId': deviceId,
            },
        });
    }

}
