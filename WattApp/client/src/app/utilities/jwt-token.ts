import { Buffer } from "buffer";

export class JwtToken{
	private token:string="";
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
		console.log(exp)
		return exp<(new Date()).getTime();
	}
}