import { AfterViewInit, Component } from '@angular/core';

declare const $: any;

@Component({
  selector: 'app-landing',
  imports: [],
  templateUrl: './landing.component.html',
  styleUrl: './landing.component.css'
})
export class LandingComponent implements AfterViewInit {
  ngAfterViewInit(): void {
    // re-load template scripts AFTER the landing HTML exists
    this.loadScripts([
      'assets/landing/js/jquery.min.js',
      'assets/landing/js/jquery.scrolly.min.js',
      'assets/landing/js/jquery.scrollex.min.js',
      'assets/landing/js/jquery.dropotron.min.js',
      'assets/landing/js/browser.min.js',
      'assets/landing/js/breakpoints.min.js',
      'assets/landing/js/util.js',
      'assets/landing/js/main.js',
    ]);
  }

  private loadScripts(urls: string[]) {
    // remove old ones if they exist (avoid duplicates)
    urls.forEach(u => {
      const old = document.querySelector(`script[data-landing="${u}"]`);
      if (old) old.remove();
    });

    // load sequentially (order matters!)
    const loadNext = (i: number) => {
      if (i >= urls.length) return;

      const s = document.createElement('script');
      s.src = urls[i];
      s.defer = true;
      s.setAttribute('data-landing', urls[i]);
      s.onload = () => loadNext(i + 1);
      document.body.appendChild(s);
    };

    loadNext(0);
  }
}
