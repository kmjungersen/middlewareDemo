import { Component, Inject } from '@angular/core';
// import { Http } from '@angular/http';
// import { HttpHeaders } from '@angular/common/http';
import { HttpClient } from '@angular/common/http';
// import { RequestMethod } from '@angular/http';

@Component({
    selector: 'route-test',
    templateUrl: './route-test.component.html'
})
export class RouteTestComponent {
    routes: Route[];
    http: HttpClient;
    baseUrl: string;

    constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        this.http = http;
        this.baseUrl = baseUrl;

        this.http.get(this.baseUrl + 'api/TestRoute/GetRoutes').subscribe(data => {
            this.routes = data as Route[];
        }, error => console.error(error));
    }

    callApi(name: string) {
        return this.http.post(this.baseUrl + 'api/TestRoute/PostRoute?Name=' + name, JSON.stringify(name))
            .subscribe(data => {
                let result = data as Route;
                let existingRoute = this.routes.filter(x => x.name == result.name);

                if (existingRoute.length != 0) {
                    existingRoute[0].points += 1;
                }
            });
    }
}

interface Route {
    name: string;
    points: number;
}
