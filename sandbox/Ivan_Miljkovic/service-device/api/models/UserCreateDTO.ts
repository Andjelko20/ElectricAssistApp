/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

export type UserCreateDTO = {
    Name: string;
    Username: string;
    Password: string;
    Blocked?: boolean;
    Email: string;
    RoleId?: number;
};