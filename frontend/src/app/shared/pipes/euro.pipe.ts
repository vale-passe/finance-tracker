import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
  name: 'euro',
})
export class EuroPipe implements PipeTransform {
  transform(value: number, showSign: boolean = false) {
    const formatted = Intl.NumberFormat('it-IT', {
      style: 'currency',
      currency: 'EUR',
      minimumFractionDigits: 2,
      maximumFractionDigits: 2
    }).format(Math.abs(value));

    if (showSign && value < 0) {
      return `-${formatted}`;
    }

    if (showSign && value > 0) {
      return `+${formatted}`;
    }

    return value < 0 ? `-${formatted}` : formatted;
  }

}