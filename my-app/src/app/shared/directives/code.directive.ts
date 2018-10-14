import { Injectable } from '@angular/core';
import { FormControl } from '@angular/forms';
import { ClientService } from '../../core/services/client.service';

@Injectable()
export class CodeValidator {

  debouncer: any;

  constructor(public clientService: ClientService) { }

  checkUsername(control: FormControl): any {

    clearTimeout(this.debouncer);

    return new Promise(resolve => {

      this.debouncer = setTimeout(() => {
        if (control.value.length == 3) {
          this.clientService.isNameExist(control.value).subscribe((res) => {
            if (res) {
              resolve({ 'validCode': true });
            }
            else {
              resolve(null);
            }
          }, (err) => {
            resolve(null);
          });
        }
      }, 1000);
    });
  }
}