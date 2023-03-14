export interface Users {
    id: number;
    name: string;
    userName: string;
    password:string;
    block: boolean;
    role:string;


}
export interface Register{
    name: string;
    userName: string;
    password:string;
}
export interface Token {
    sub: string;
    iat: number;
    exp: number;
  }