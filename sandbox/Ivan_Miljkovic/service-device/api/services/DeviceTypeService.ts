/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { DeviceType } from '../models/DeviceType';

import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class DeviceTypeService {

    /**
     * @param typeId 
     * @returns string Success
     * @throws ApiError
     */
    public static getApiDeviceType(
typeId?: number,
): CancelablePromise<string> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/DeviceType',
            query: {
                'typeId': typeId,
            },
        });
    }

    /**
     * @param categoryId 
     * @returns DeviceType Success
     * @throws ApiError
     */
    public static getApiDeviceTypeTypes(
categoryId?: number,
): CancelablePromise<Array<DeviceType>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/DeviceType/types',
            query: {
                'categoryId': categoryId,
            },
        });
    }

}
