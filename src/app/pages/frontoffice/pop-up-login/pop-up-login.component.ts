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
  providers: [AuthProxy,HttpProxy],
})
export class PopUpLoginComponent {
  readonly dialogRef = inject(MatDialogRef<PopUpLoginComponent>);
  private authProxy = inject(AuthProxy);
  private snackBar = inject(MatSnackBar);

  loginData: Login = { email: '', password: '' };
  isLoading = false; // Controla el estado del spinner

  onNoClick(): void {
    this.dialogRef.close();
  }

  async onLoginClick(): Promise<void> {
    this.isLoading = true; // Muestra el spinner
    try {
      const result = await this.authProxy.login(this.loginData);
      this.snackBar.open('Login successful!', 'Close', { duration: 3000 }); // Muestra notificación
      this.dialogRef.close(result); // Cierra el popup con los datos
    } catch (error) {
      this.snackBar.open('Login failed. Please try again.', 'Close', { duration: 3000 }); // Notificación de error
      console.error('Login failed:', error);
    } finally {
      this.isLoading = false; // Oculta el spinner
    }
  }
}
