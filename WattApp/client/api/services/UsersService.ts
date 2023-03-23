/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { BlockedStatusDTO } from '../models/BlockedStatusDTO';
import type { ChangePasswordDTO } from '../models/ChangePasswordDTO';
import type { UserCreateDTO } from '../models/UserCreateDTO';
import type { UserUpdateDTO } from '../models/UserUpdateDTO';

import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class UsersService {

    /**
     * Get 20 users per page
     * @param page 
     * @returns any Success
     * @throws ApiError
     */
    public static getApiUsersPage(
page: number,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Users/page/{page}',
            path: {
                'page': page,
            },
        });
    }

    /**
     * Get single user
     * @param id 
     * @returns any Success
     * @throws ApiError
     */
    public static getApiUsers(
id: number,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Users/{id}',
            path: {
                'id': id,
            },
        });
    }

    /**
     * Update user by admin
     * @param id 
     * @param requestBody 
     * @returns any Success
     * @throws ApiError
     */
    public static putApiUsers(
id: number,
requestBody?: UserUpdateDTO,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'PUT',
            url: '/api/Users/{id}',
            path: {
                'id': id,
            },
            body: requestBody,
            mediaType: 'application/json',
        });
    }

    /**
     * Delete user
     * @param id 
     * @returns any Success
     * @throws ApiError
     */
    public static deleteApiUsers(
id: number,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'DELETE',
            url: '/api/Users/{id}',
            path: {
                'id': id,
            },
        });
    }

    /**
     * Get all roles
     * @returns any Success
     * @throws ApiError
     */
    public static getApiUsersRoles(): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Users/roles',
        });
    }

    /**
     * Create user
     * @param requestBody 
     * @returns any Success
     * @throws ApiError
     */
    public static postApiUsers(
requestBody?: UserCreateDTO,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Users',
            body: requestBody,
            mediaType: 'application/json',
            errors: {
                400: `Bad request`,
            },
        });
    }

    /**
     * Update logged in user data
     * @param requestBody 
     * @returns any Success
     * @throws ApiError
     */
    public static putApiUsers1(
requestBody?: UserUpdateDTO,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'PUT',
            url: '/api/Users',
            body: requestBody,
            mediaType: 'application/json',
        });
    }

    /**
     * Block or unblock user
     * @param id 
     * @param requestBody 
     * @returns any Success
     * @throws ApiError
     */
    public static putApiUsersSetBlockedStatus(
id: number,
requestBody?: BlockedStatusDTO,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'PUT',
            url: '/api/Users/set_blocked_status/{id}',
            path: {
                'id': id,
            },
            body: requestBody,
            mediaType: 'application/json',
        });
    }

    /**
     * Change password
     * @param requestBody 
     * @returns any Success
     * @throws ApiError
     */
    public static putApiUsersChangePassword(
requestBody?: ChangePasswordDTO,
): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'PUT',
            url: '/api/Users/change_password',
            body: requestBody,
            mediaType: 'application/json',
        });
    }

}
