import { Model } from "./model.entity";

export interface User {
    id: number; 
    name: string; 
    lastName: string; 
    email: string; 
    password: string; 
    notifications: Notification[]; 
    models: Model[]; 
  }