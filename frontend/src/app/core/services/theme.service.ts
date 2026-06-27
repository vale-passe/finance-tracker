import { effect, Injectable, signal } from "@angular/core";

export type Theme = 'light' | 'dark';

@Injectable({ providedIn: 'root' })
export class ThemeService {
  readonly theme = signal<Theme>(this.getInitialTheme());

  constructor() {
    effect(() => { 
      const current = this.theme();
      document.documentElement.setAttribute('data-theme', current);
      localStorage.setItem('theme', current);
    });
  }

  toggle(): void {
    this.theme.update(t => t === 'light' ? 'dark' : 'light');
  }

  private getInitialTheme(): Theme {
    const saved = localStorage.getItem('theme') as Theme | null;
    
    if (saved === 'light' || saved === 'dark') {
      return saved;
    }

    const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
    return prefersDark ? 'dark' : 'light';
  }
}