/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */

/**
 * Data get from database using pagination ( only one page )
 */
export type ObjectDataPage = {
    PreviousPage?: number | null;
    NextPage?: number | null;
    NumberOfPages?: number;
    Data?: Array<any> | null;
};
