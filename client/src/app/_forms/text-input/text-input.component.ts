import { Component, Input, Self } from '@angular/core';
import { ControlValueAccessor, FormControl, NgControl } from '@angular/forms';

@Component({
  selector: 'app-text-input',
  templateUrl: './text-input.component.html',
  styleUrls: ['./text-input.component.css']
})

// Bridge between angular and native component in DOM (input)
export class TextInputComponent implements ControlValueAccessor {
  @Input() label: string = '';
  @Input() type: string = 'text';

  // We want to inject the control into the constructor of this component
  // The Self decorator ensures that angular doesn't take an existing one from DI, we want
  // this to be self-contained. Self ensures that angular will always inject what we're doing locally.
  constructor(@Self() public ngControl: NgControl) {
    this.ngControl.valueAccessor = this;
  }

  getControl(): FormControl {
    return this.ngControl.control as FormControl;
  }

  writeValue(obj: any): void {
    
  }

  registerOnChange(fn: any): void {
    
  }

  registerOnTouched(fn: any): void {
    
  }

}
