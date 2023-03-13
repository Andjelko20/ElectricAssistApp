export interface Users {
    id: string;
    name: string;
    userName: string;
    password:string;
    block: string;
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