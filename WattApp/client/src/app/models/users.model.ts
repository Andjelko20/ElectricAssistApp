export interface Users {
    id: number;
    name: string;
    username: string;
    password:string;
    email:string;
    block: boolean;
    roleId:number;


}
export interface ShowUsers {
    id: number;
    name: string;
    username: string;
    email:string;
    block: boolean;
    role:string;


}
export interface Register{
    name: string;
    username: string;
    password:string;
}
export interface Token {
    sub: string;
    iat: number;
    exp: number;
  }