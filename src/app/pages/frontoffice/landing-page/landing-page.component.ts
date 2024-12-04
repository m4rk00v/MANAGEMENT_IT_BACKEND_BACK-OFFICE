import { Component, inject } from '@angular/core';
import { StorageService } from 'src/app/shared/services/storage.service';

@Component({
  selector: 'app-landing-page',
  templateUrl: './landing-page.component.html',
  styleUrls: ['./landing-page.component.css'],
  standalone: true,
})
export class LandingPageComponent {

  private storageService = inject(StorageService)

  info : any = null

  ngOnInit(): void {
    this.getInfo();
  }

  getInfo(){

    this.info = this.storageService.get('User')
    console.log("info",this.info)

  }

}
