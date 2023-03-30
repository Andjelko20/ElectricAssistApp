/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { City } from '../models/City';
import type { Country } from '../models/Country';
import type { DeviceBrand } from '../models/DeviceBrand';
import type { DeviceCategory } from '../models/DeviceCategory';
import type { DeviceModel } from '../models/DeviceModel';
import type { DeviceType } from '../models/DeviceType';
import type { Settlement } from '../models/Settlement';

import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class DropDownService {

    /**
     * @returns DeviceCategory Success
     * @throws ApiError
     */
    public static getCategories(): CancelablePromise<Array<DeviceCategory>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/categories',
        });
    }

    /**
     * @param categoryId 
     * @returns DeviceType Success
     * @throws ApiError
     */
    public static getTypes(
categoryId?: number,
): CancelablePromise<Array<DeviceType>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/types',
            query: {
                'categoryId': categoryId,
            },
        });
    }

    /**
     * @param typeId 
     * @returns DeviceBrand Success
     * @throws ApiError
     */
    public static getBrands(
typeId?: number,
): CancelablePromise<Array<DeviceBrand>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/brands',
            query: {
                'typeId': typeId,
            },
        });
    }

    /**
     * @param typeId 
     * @param brandId 
     * @returns DeviceModel Success
     * @throws ApiError
     */
    public static getModels(
typeId?: number,
brandId?: number,
): CancelablePromise<Array<DeviceModel>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/models',
            query: {
                'typeId': typeId,
                'brandId': brandId,
            },
        });
    }

    /**
     * @param cityId 
     * @returns Settlement Success
     * @throws ApiError
     */
    public static getSettlements(
cityId?: number,
): CancelablePromise<Array<Settlement>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/settlements',
            query: {
                'cityId': cityId,
            },
        });
    }

    /**
     * @param countryId 
     * @returns City Success
     * @throws ApiError
     */
    public static getCities(
countryId?: number,
): CancelablePromise<Array<City>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/cities',
            query: {
                'countryId': countryId,
            },
        });
    }

    /**
     * @returns Country Success
     * @throws ApiError
     */
    public static getCountries(): CancelablePromise<Array<Country>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/countries',
        });
    }

}
