import { bootstrapApplication }
  from '@angular/platform-browser';
import { AppComponent }
  from './app/app.component';
import { appConfig }
  from './app/app.config';
import { provideHttpClient, withInterceptors }
  from '@angular/common/http';
import { authInterceptor }

  from './app/core/interceptors/auth.interceptor';
bootstrapApplication(AppComponent,
  { providers: [ provideHttpClient(withInterceptors([authInterceptor])),
      ...appConfig.providers ] });
