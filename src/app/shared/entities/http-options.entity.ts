export class HttpOptions {
    url: string;
    method: "GET" | "POST" | "PUT" | "PATCH" | "DELETE";
    auth: boolean;
    errorPage: string;
    bodyData: any;
    responseType: "arraybuffer" | "blob" | "json" | "text";
    private headers : Array<{ key: any; value: any }> = [];
    private params : Array<{ key: any; value: any }> = [];
    retry: boolean;
    timeoutTime: number;
  
    constructor(
      url: string = "",
      method: "GET" | "POST" | "PUT" | "PATCH" | "DELETE" = "GET",
      auth: boolean = true,
      errorPage: any = null,
      responseType: "arraybuffer" | "blob" | "json" | "text" = "json",
      retry: boolean = true,
      timeoutTime: number = 60000
    ) {
      this.url = url;
      this.method = method;
      this.auth = auth;
      this.errorPage = errorPage;
      this.responseType = responseType;
      this.retry = retry;
      this.timeoutTime = timeoutTime;
    }
  
    body(bodyData: any) {
      if (
        this.method.toUpperCase() === "GET" ||
        this.method.toUpperCase() === "DELETE"
      ) {
        const aux = [];
        for (const param in bodyData) {
          if (bodyData[param] !== undefined && bodyData[param] !== "") {
            aux.push([param, bodyData[param]].join("="));
          }
        }
        this.url = this.url + "?" + aux.join("&");
      } else {
        this.bodyData = bodyData;
      }
    }
  
    addHeader(key: string, value: string) {
      this.headers.push({ key, value });
    }
  
    addParams(key: string, value: string | number) {
      this.params.push({ key, value });
    }
  
    getHeaders() {
      return this.headers;
    }
  
    getParams() {
      return this.params;
    }
  }
  