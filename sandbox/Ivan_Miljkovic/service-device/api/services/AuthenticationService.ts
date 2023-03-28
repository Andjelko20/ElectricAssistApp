/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { EmailRequestDTO } from '../models/EmailRequestDTO';
import type { LoginDTO } from '../models/LoginDTO';
import type { MessageResponseDTO } from '../models/MessageResponseDTO';
import type { ResetPasswordRequestDTO } from '../models/ResetPasswordRequestDTO';
import type { TokenResponseDTO } from '../models/TokenResponseDTO';
import type { UserCreateDTO } from '../models/UserCreateDTO';

import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';

export class AuthenticationService {

    /**
     * Login
     * @param requestBody 
     * @returns TokenResponseDTO Success
     * @throws ApiError
     */
    public static postApiAuthenticationLogin(
requestBody?: LoginDTO,
): CancelablePromise<TokenResponseDTO> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Authentication/login',
            body: requestBody,
            mediaType: 'application/json',
            errors: {
                400: `Bad Request`,
                401: `Unauthorized`,
            },
        });
    }

    /**
     * Register as guest
     * @param requestBody 
     * @returns MessageResponseDTO Success
     * @throws ApiError
     */
    public static postApiAuthenticationRegister(
requestBody?: UserCreateDTO,
): CancelablePromise<MessageResponseDTO> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Authentication/register',
            body: requestBody,
            mediaType: 'application/json',
            errors: {
                400: `Bad Request`,
            },
        });
    }

    /**
     * Send reset password token on mail
     * @param requestBody 
     * @returns MessageResponseDTO Success
     * @throws ApiError
     */
    public static postApiAuthenticationGenerateResetToken(
requestBody?: EmailRequestDTO,
): CancelablePromise<MessageResponseDTO> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Authentication/generate_reset_token',
            body: requestBody,
            mediaType: 'application/json',
            errors: {
                400: `Bad Request`,
                500: `Server Error`,
            },
        });
    }

    /**
     * Reset password
     * @param requestBody 
     * @returns MessageResponseDTO Success
     * @throws ApiError
     */
    public static postApiAuthenticationResetPassword(
requestBody?: ResetPasswordRequestDTO,
): CancelablePromise<MessageResponseDTO> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Authentication/reset_password',
            body: requestBody,
            mediaType: 'application/json',
            errors: {
                400: `Bad Request`,
                500: `Server Error`,
            },
        });
    }

}
