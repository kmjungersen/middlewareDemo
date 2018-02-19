import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpModule } from '@angular/http';
import { RouterModule } from '@angular/router';
import { HttpHeaders } from '@angular/common/http';
import { HttpClientModule } from '@angular/common/http';
import { DatePipe } from '@angular/common';

import { AppComponent } from './components/app/app.component';
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { RouteTestComponent } from './components/route-test/route-test.component';
import { ResultsComponent } from './components/results/results.component';

@NgModule({
    declarations: [
        AppComponent,
        NavMenuComponent,
        RouteTestComponent,
        HomeComponent,
        ResultsComponent,
    ],
    imports: [
        CommonModule,
        HttpModule,
        FormsModule,
        HttpClientModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'route-tests', component: RouteTestComponent },
            { path: 'results', component: ResultsComponent },
            { path: '**', redirectTo: 'home' },
        ])
    ]
})
export class AppModuleShared {
}
