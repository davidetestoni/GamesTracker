import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {
  requestForm!: FormGroup;
  resetForm!: FormGroup;
  username: string = '';
  validationErrors: string[] = [];
  tempCodeSent: boolean = false;

  constructor(private accountService: AccountService, private router: Router,
    private fb: FormBuilder, private toastr: ToastrService) {}

  ngOnInit(): void {
    this.initializeForms();
  }

  initializeForms() {
    this.requestForm = this.fb.group({
      username: ['', Validators.required]
    });
    this.resetForm = this.fb.group({
      password: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(32)]],
      confirmPassword: ['', [Validators.required, this.matchValues('password')]],
      tempCode: ['', Validators.required]
    });
    this.resetForm.controls.password.valueChanges.subscribe(() => {
      this.resetForm.controls.confirmPassword.updateValueAndValidity();
    });
  }

  getControlFromRequestForm(name: string): FormControl {
    return this.requestForm.controls[name] as FormControl;
  }
  
  getControlFromResetForm(name: string): FormControl {
    return this.resetForm.controls[name] as FormControl;
  }

  requestPasswordReset() {
    this.accountService.requestPasswordReset(this.requestForm.value).subscribe(response => {
      this.toastr.success('Password reset code sent to your email address');
      this.username = this.requestForm.value.username;
      this.tempCodeSent = true;
    });
  }

  submitCode() {
    this.accountService.resetPassword({ 
      username: this.username,
      newPassword: this.resetForm.value.password,
      tempCode: this.resetForm.value.tempCode
    }).subscribe(response => {
      this.toastr.success('Password reset successfully');
      this.router.navigateByUrl('/library');
    });
  }

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      const controls = control?.parent?.controls as any;
      if (controls) {
        return control?.value === controls[matchTo]?.value
          ? null 
          : { isMatching: true }; // Dummy object
      }
      return null;
    }
  }

}
