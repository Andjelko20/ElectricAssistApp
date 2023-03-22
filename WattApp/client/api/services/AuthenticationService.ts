/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { LoginDTO } from '../models/LoginDTO';
import type { UserCreateDTO } from '../models/UserCreateDTO';

import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class AuthenticationService {

    /**
     * Login
     * @param requestBody 
     * @returns any Success
     * @throws ApiError
     */
    public static postApiAuthenticationLogin(
requestBody?: LoginDTO,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Authentication/login',
            body: requestBody,
            mediaType: 'application/json',
            errors: {
                400: `Bad request`,
            },
        });
    }

    /**
     * Register as guest
     * @param requestBody 
     * @returns any Success
     * @throws ApiError
     */
    public static postApiAuthenticationRegister(
requestBody?: UserCreateDTO,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Authentication/register',
            body: requestBody,
            mediaType: 'application/json',
        });
    }

}
