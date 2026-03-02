import { AfterViewInit, Component, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NavigationEnd, Router, RouterLink } from '@angular/router';
import { filter, Subscription } from 'rxjs';
import { AuthService } from '../core/services/auth/auth.service';

@Component({
  selector: 'app-landing',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.css'
})
export class LandingComponent implements AfterViewInit, OnDestroy {
  constructor(public auth: AuthService, private router: Router) {}

  private sub?: Subscription;

  
  private readonly landingScripts = [
    'assets/landing/js/jquery.min.js',
    'assets/landing/js/jquery.scrolly.min.js',
    'assets/landing/js/jquery.scrollex.min.js',
    'assets/landing/js/browser.min.js',
    'assets/landing/js/breakpoints.min.js',
    'assets/landing/js/util.js',
    'assets/landing/js/main.js',
  ];

  logout() {
    this.auth.logout();
    this.router.navigateByUrl('/').then(() => {
      window.scrollTo({ top: 0, behavior: 'auto' });
    });
  }

  ngAfterViewInit(): void {
   
    this.initLanding(true);

    
    this.sub = this.router.events
      .pipe(
        filter((e): e is NavigationEnd => e instanceof NavigationEnd),
        filter(e => this.isLandingUrl(e.urlAfterRedirects))
      )
      .subscribe(() => {
        setTimeout(() => this.initLanding(true), 0);
      });
  }

  ngOnDestroy(): void {
    this.sub?.unsubscribe();
  }

  private isLandingUrl(url: string): boolean {
    return url === '/' || url.startsWith('/#') || url.startsWith('/?');
  }

  private initLanding(forceReload: boolean): void {
    const w = window as any;

    
    document.body.classList.add('is-preload');

   
    if (forceReload) {
      this.removeScripts(this.landingScripts.filter(s => !s.endsWith('/jquery.min.js')));
    }


    const urlsToLoad = w.jQuery
      ? this.landingScripts.filter(s => !s.endsWith('/jquery.min.js'))
      : this.landingScripts;

    this.loadScriptsSequential(urlsToLoad)
      .then(() => {
    
        if (typeof w.landingInit === 'function') w.landingInit();

        setTimeout(() => document.body.classList.remove('is-preload'), 120);
      })
      .catch(err => console.error('Landing init error:', err));
  }

  private removeScripts(urls: string[]): void {
    urls.forEach(url => {
      const el = document.querySelector(`script[data-landing="${url}"]`);
      if (el) el.remove();
    });
  }

  private loadScriptsSequential(urls: string[]): Promise<void> {
    const loadNext = (i: number, resolve: () => void, reject: (e: any) => void) => {
      if (i >= urls.length) return resolve();

      const url = urls[i];
      if (document.querySelector(`script[data-landing="${url}"]`)) {
        return loadNext(i + 1, resolve, reject);
      }

      const s = document.createElement('script');
      s.src = url;
      s.async = false;

      s.setAttribute('data-landing', url);

      s.onload = () => loadNext(i + 1, resolve, reject);
      s.onerror = () => reject(new Error(`Failed to load: ${url}`));

      document.body.appendChild(s);
    };

    return new Promise((resolve, reject) => loadNext(0, resolve, reject));
  }
}