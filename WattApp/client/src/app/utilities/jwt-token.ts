import { Buffer } from "buffer";

export class JwtToken{
	private token:string="";
	/**
	 * @summary Get token from localStorage or param token 
	 */
	constructor();
	constructor(token:string);
	constructor(token?:string|undefined){
		if(token===undefined){
			let localToken=localStorage.getItem("token");
			if(localToken===null)
				this.token="";
			else
				this.token=localToken;
			return;
		}
		this.token=token;
	}

	/**
	 * @summary Check if token format is valid
	 * @param token 
	 * @returns 
	 */
	public isJwtToken(token: string): boolean {
		const jwtRegex = /^([A-Za-z0-9-_=]+\.){2}([A-Za-z0-9-_=]+)$/;
		return jwtRegex.test(token);
	}
	public get data(){
		if(!this.isJwtToken(this.token))
			throw new Error("Invalid token format");
		return JSON.parse(Buffer.from(this.token.split('.')[1],"base64").toString());
	}
	public get expired(){
		let {exp}=this.data;
		let date=new Date(0);
		date.setUTCSeconds(exp);
		return date.getTime()<(new Date()).getTime();
	}
}