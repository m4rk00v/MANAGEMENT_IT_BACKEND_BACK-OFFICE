import { Injectable } from "@angular/core";
import { HttpProxy } from "./http.proxy";
import { Login } from "../entities/login.entity";
import { HttpOptions } from "../entities/http-options.entity";

@Injectable()
export class AuthProxy{

    private api : string

    /**
     *
     */
    constructor(private http: HttpProxy) {
        this.api = 'https://localhost:7291/api'
        
    }

    async login(data: Login) {
        try {
            const url = `${this.api}/authentication/login`;
            const options = new HttpOptions(url, 'POST', false);
            options.body(data);
            const result = await this.http.makeCall(options);
            return result;
        } catch (err) {
            console.log('error', err);
        }
    }

}