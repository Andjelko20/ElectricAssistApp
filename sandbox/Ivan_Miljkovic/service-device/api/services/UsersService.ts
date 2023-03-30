/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { BlockedStatusDTO } from '../models/BlockedStatusDTO';
import type { ChangePasswordDTO } from '../models/ChangePasswordDTO';
import type { MessageResponseDTO } from '../models/MessageResponseDTO';
import type { ObjectDataPage } from '../models/ObjectDataPage';
import type { RoleModel } from '../models/RoleModel';
import type { UserCreateDTO } from '../models/UserCreateDTO';
import type { UserUpdateDTO } from '../models/UserUpdateDTO';

import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class UsersService {

    /**
     * Get 20 users per page
     * @param page 
     * @returns ObjectDataPage Success
     * @throws ApiError
     */
    public static getApiUsersPage(
page: number,
): CancelablePromise<ObjectDataPage> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Users/page/{page}',
            path: {
                'page': page,
            },
            errors: {
                400: `Bad Request`,
                500: `Server Error`,
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
            errors: {
                400: `Bad Request`,
                500: `Server Error`,
            },
        });
    }

    /**
     * Update user by admin
     * @param id 
     * @param requestBody 
     * @returns MessageResponseDTO Success
     * @throws ApiError
     */
    public static putApiUsers(
id: number,
requestBody?: UserUpdateDTO,
): CancelablePromise<MessageResponseDTO> {
        return __request(OpenAPI, {
            method: 'PUT',
            url: '/api/Users/{id}',
            path: {
                'id': id,
            },
            body: requestBody,
            mediaType: 'application/json',
            errors: {
                400: `Bad Request`,
                500: `Server Error`,
            },
        });
    }

    /**
     * Delete user
     * @param id 
     * @returns MessageResponseDTO Success
     * @throws ApiError
     */
    public static deleteApiUsers(
id: number,
): CancelablePromise<MessageResponseDTO> {
        return __request(OpenAPI, {
            method: 'DELETE',
            url: '/api/Users/{id}',
            path: {
                'id': id,
            },
            errors: {
                400: `Bad Request`,
                500: `Server Error`,
            },
        });
    }

    /**
     * Get all roles
     * @returns RoleModel Success
     * @throws ApiError
     */
    public static getApiUsersRoles(): CancelablePromise<Array<RoleModel>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Users/roles',
            errors: {
                400: `Bad Request`,
                500: `Server Error`,
            },
        });
    }

    /**
     * Create user
     * @param requestBody 
     * @returns MessageResponseDTO Success
     * @throws ApiError
     */
    public static postApiUsers(
requestBody?: UserCreateDTO,
): CancelablePromise<MessageResponseDTO> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Users',
            body: requestBody,
            mediaType: 'application/json',
            errors: {
                400: `Bad request`,
                500: `Server Error`,
            },
        });
    }

    /**
     * Update logged in user data
     * @param requestBody 
     * @returns MessageResponseDTO Success
     * @throws ApiError
     */
    public static putApiUsers1(
requestBody?: UserUpdateDTO,
): CancelablePromise<MessageResponseDTO> {
        return __request(OpenAPI, {
            method: 'PUT',
            url: '/api/Users',
            body: requestBody,
            mediaType: 'application/json',
            errors: {
                400: `Bad Request`,
                500: `Server Error`,
            },
        });
    }

    /**
     * Block or unblock user
     * @param id 
     * @param requestBody 
     * @returns MessageResponseDTO Success
     * @throws ApiError
     */
    public static putApiUsersSetBlockedStatus(
id: number,
requestBody?: BlockedStatusDTO,
): CancelablePromise<MessageResponseDTO> {
        return __request(OpenAPI, {
            method: 'PUT',
            url: '/api/Users/set_blocked_status/{id}',
            path: {
                'id': id,
            },
            body: requestBody,
            mediaType: 'application/json',
            errors: {
                400: `Bad Request`,
                500: `Server Error`,
            },
        });
    }

    /**
     * Change password
     * @param requestBody 
     * @returns MessageResponseDTO Success
     * @throws ApiError
     */
    public static putApiUsersChangePassword(
requestBody?: ChangePasswordDTO,
): CancelablePromise<MessageResponseDTO> {
        return __request(OpenAPI, {
            method: 'PUT',
            url: '/api/Users/change_password',
            body: requestBody,
            mediaType: 'application/json',
            errors: {
                400: `Bad Request`,
                500: `Server Error`,
            },
        });
    }

}
