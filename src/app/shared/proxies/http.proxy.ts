import { HttpClient, HttpHeaders, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { HttpOptions } from "../entities/http-options.entity";
import { catchError, finalize, mergeMap, Observable, retryWhen, throwError, timeout, timer } from "rxjs";
import { Router } from "@angular/router";
import { StorageService } from "../services/storage.service";

@Injectable()
export class HttpProxy {
  constructor(private storage: StorageService, private router: Router, private httpClient: HttpClient) { }

  makeCall(options: HttpOptions) {
    return new Promise<any>(async (resolve, reject) => {
      let req: Observable<any>;
      req = new Observable(); // Inicializa con un Observable vacío
      try {
        let headers = new HttpHeaders();
       
        const optionsHeaders = options.getHeaders();
        if (optionsHeaders.findIndex(o => o.key.toLowerCase() === 'content-type') === -1) {
          optionsHeaders.push({ key: 'Content-Type', value: 'application/json' });
        }
        // Add Auth header
        if (options.auth) {
          const tokenInfo = await this.storage.get('Token');
          const token = tokenInfo !== null ? tokenInfo.access_token : null;
          if (token != null) {
            optionsHeaders.push({ key: 'Authorization', value: 'Bearer ' + token });
          }
        }
        // create call options
        
        optionsHeaders.forEach(item => {
          headers = (headers as HttpHeaders).append(item.key, item.value);
        });

        const params = new HttpParams();
        options.getParams().forEach(item => {
          params.append(item.key, item.value);
        });

        if (options.method === 'GET') {
          req = this.httpClient.get(options.url);
          if (optionsHeaders.length > 0 || options.auth) {
            if (options.responseType === 'text') {
              req = this.httpClient.get(options.url, { headers: headers as HttpHeaders, params, responseType: 'text' });
            } else {
              req = this.httpClient.get(options.url, { headers: headers as HttpHeaders, params });
            }
          }
        } 
        else if (options.method === 'DELETE') {
          req = this.httpClient.delete(options.url);
          if (optionsHeaders.length > 0 || options.auth) {
            req = this.httpClient.delete(options.url, { headers: headers as HttpHeaders, params });
          }
        } 
        else if (options.method === 'POST') {
          if (headers.get('Content-Type') === 'multipart/form-data') {
            headers = headers.delete('Content-Type');
            const body: FormData = options.bodyData;
            req = this.httpClient.post(options.url, body, { headers: headers as HttpHeaders, params });
          } else {
            let body = {};
            if (options.bodyData !== undefined) {
              body = options.bodyData;
            }
            req = this.httpClient.post(options.url, body, { headers: headers as HttpHeaders, params });
          }
        } 
        else if (options.method === 'PUT') {
          let body = {};
          if (options.bodyData !== undefined) {
            body = options.bodyData;
          }
          req = this.httpClient.put(options.url, body, { headers: headers as HttpHeaders, params });
        } 
        else if (options.method === 'PATCH') {
          let body = {};
          if (options.bodyData !== undefined) {
            body = options.bodyData;
          }
          req = this.httpClient.patch(options.url, body, { headers: headers as HttpHeaders, params });
        }
        
        req
          .pipe(
            timeout(options.timeoutTime),
            retryWhen(errors => options.retry ? errors.pipe(this.genericRetryStrategy()) : new Observable()),
            catchError(error => throwError(error)),
          )
          .subscribe(
            async res => {
              if (res) {
                // return resolve(res);
                if (res.status != null && res.status !== undefined && res.status !== 200 && res.status !== 201 && res.status !== 401) {
                  await this._handleError(res, options);
                  reject(res);
                } else {
                  return resolve(res);
                }
              } else {
                reject(null);
              }
            },
            async err => {
              if (err && err.status === 401) {
                options.errorPage = 'login';
              }
              await this._handleError(err, options);
              reject(err);
            },
          );
      } catch (e) {
        return reject(e);
      }
    });
  }

  private async _handleError(err: any, options: HttpOptions) {
    console.log(err);
    if (options.errorPage) {
      let message = '';
      switch (err.name) {
        case 'HttpErrorResponse':
          switch (err.status) {
            case 0:
              if (err.message.match(/unknown url/)) {
                message = 'Lo sentimos, al parecer no tienes conexión a internet';
              } else {
                message = 'Request error unknown';
              }
              break;
            case 400:
              message = err.message;
              break;
            case 401:
              
              break;
            case 403:
              message = 'Oops!...';
              break;
            case 404:
              message = 'No se encontro el recurso!';
              break;
            case 500:
              message = 'Ocurrio un error interno en el servidor!';
              break;
            case 503:
              message = 'Ocurrio un error interno en el servidor!';
              break;
            default:
              message = 'Request error status ' + err.status;
              break;
          }
          break;
        case 'TimeoutError':
          message = 'En este momento tenemos problemas para cargar la aplicación.<br>Vuelve a visitarnos en unos instantes';
          break;
      }
      this.router.navigate([options.errorPage]);
    }
    return true;
  }


  private genericRetryStrategy = ({
    maxRetryAttempts = 3,
    scalingDuration = 1000,
    excludedStatusCodes = [400, 200, 401],
  }: {
    maxRetryAttempts?: number;
    scalingDuration?: number;
    excludedStatusCodes?: number[];
  } = {}) => (attempts: Observable<any>) => {
    return attempts.pipe(
      mergeMap((error, i) => {
        const retryAttempt = i + 1;
        if (retryAttempt > maxRetryAttempts || excludedStatusCodes.includes(error.status)) {
          return throwError(error);
        }
        console.log(`Attempt ${retryAttempt}: retrying in ${retryAttempt * scalingDuration}ms`);
        // retry after 1s, 2s, etc...
        return timer(retryAttempt * scalingDuration);
      }),
      finalize(() => console.log('We are done!')),
    );
  };
}
