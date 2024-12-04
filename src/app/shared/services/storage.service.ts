import { Injectable } from '@angular/core';

import { BehaviorSubject, Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class StorageService {
  private languaje: BehaviorSubject<any> = new BehaviorSubject(null); // starting app default as 'es'
    private sessionId: BehaviorSubject<any> = new BehaviorSubject(null); // starting app default as null
  private profile: BehaviorSubject<any> = new BehaviorSubject(null); // starting app default as null


  constructor() {
    const sessionId = this.get('sessionId');
    this.sessionId.next(sessionId);
    const profile = this.get('profile');
    this.profile.next(profile);
    }

  getLanguaje(): Observable<any> {
    return this.languaje.asObservable();
  }

getSessionId(): Observable<string> {
    return this.sessionId.asObservable();
  }

  getProfile(): Observable<any> {
    return this.profile.asObservable();
  }


    get(key: string) {
        let result: any = null;
        try {
        result = localStorage.getItem(key);
        return JSON.parse(result);
        } catch (error) {
        console.log('Error StorageService get - ' + key, error);
        return result;
        }
    }

  set(key: string, obj: any) {
    try {
      localStorage.setItem(key, JSON.stringify(obj));
      this.next(key, obj);
      return obj;
    } catch (error) {
      console.log(
        'Error StorageService set - ' + key + ': ' + JSON.stringify(obj),
        error
      );
      return null;
    }
  }

  clear(key: string) {
    try {
      localStorage.removeItem(key);
      this.next(key, null);
      return true; // Devuelve un valor en caso de éxito
    } catch (error) {
      console.log('Error StorageService clear - ' + key, error);
      return false; // Devuelve un valor en caso de error
    }
  }
  

  clearAll() {
    try {
      localStorage.clear();
      this.next('', null);
      return true; // Devuelve un valor en caso de éxito
    } catch (error) {
      console.log('Error StorageService clearAll', error);
      return false; // Devuelve un valor en caso de error
    }
  }
  

  next(key: string, value: any) {
    switch (key) {
        case 'sessionId':
            this.sessionId.next(value);
            break;
        case 'profile':
            this.profile.next(value);
            break;
    }
  }
}
