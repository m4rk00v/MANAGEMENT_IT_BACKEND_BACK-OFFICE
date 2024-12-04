import { Component, inject } from '@angular/core';
import { MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { FormsModule } from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { AuthProxy } from '../../../shared/proxies/authentication.proxy'; // Ajusta la ruta según tu estructura
import { Login } from '../../../shared/entities/login.entity'; // Ajusta la ruta según tu estructura


import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { HttpProxy } from 'src/app/shared/proxies/http.proxy';
import { CommonModule } from '@angular/common';
import { throwError } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { AuthService } from 'src/app/shared/services/authentication.service';
import { Router } from '@angular/router';


@Component({
  selector: 'pop-up-login',
  templateUrl: './pop-up-login.component.html',
  styleUrls: ['./pop-up-login.component.css'],
  standalone: true,
  imports: [
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule, // Importa el módulo del spinner
    MatSnackBarModule,
    CommonModule
  ],
  providers: [AuthService,HttpProxy],
})
export class PopUpLoginComponent {
  readonly dialogRef = inject(MatDialogRef<PopUpLoginComponent>);
  private authService = inject(AuthService)
  private snackBar = inject(MatSnackBar);
  private router = inject(Router)

  loginData: Login = { email: '', password: '' };
  isLoading = false; // Controla el estado del spinner

  onNoClick(): void {
    this.dialogRef.close();
  }

  async onLoginClick(): Promise<void> {
    this.isLoading = true; // Muestra el spinner
    try {
      const info = await this.authService.login(this.loginData);
    
      if (info!= undefined){
        this.snackBar.open('Welcome!!  '+ info.name, 'Close', { duration: 7000 }); // Muestra notificación
        this.dialogRef.close(info); // Cierra el popup con los datos
        this.router.navigate(['/landing']); // Redirige automáticamente
        
      }else{
        this.snackBar.open('Login failed. Please try again.', 'Close', { duration: 7000 }); // Notificación de error;
      }
       
      
    } catch (error) {
      this.snackBar.open('Login failed. Please try again.', 'Close', { duration: 7000 }); // Notificación de error
      console.error('Login failed:', error);
    } finally {
      this.isLoading = false; // Oculta el spinner
    }
  }
}
