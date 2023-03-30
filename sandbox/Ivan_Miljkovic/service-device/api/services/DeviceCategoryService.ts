/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { DeviceCategoryDTO } from '../models/DeviceCategoryDTO';

import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class DeviceCategoryService {

    /**
     * @returns DeviceCategoryDTO Success
     * @throws ApiError
     */
    public static getApiDeviceCategoryCategories(): CancelablePromise<Array<DeviceCategoryDTO>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/DeviceCategory/categories',
        });
    }

}
