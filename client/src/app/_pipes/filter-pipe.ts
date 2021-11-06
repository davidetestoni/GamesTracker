import { Pipe, PipeTransform } from '@angular/core'

@Pipe({ name: 'filterBy' }) 
export class FilterPipe implements PipeTransform { 
  transform(value: any[], fieldName: string, fieldValue: string): any { 
    return value.filter(e => e[fieldName] == fieldValue);
  }
}