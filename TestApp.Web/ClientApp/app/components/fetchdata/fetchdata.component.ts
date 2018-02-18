import { Component, Inject } from '@angular/core';
import { Http } from '@angular/http';

@Component({
    selector: 'fetchdata',
    templateUrl: './fetchdata.component.html'
})
export class FetchDataComponent {
    routes: Route[];

    constructor(http: Http, @Inject('BASE_URL') baseUrl: string) {

        http.get(baseUrl + 'api/TestRoute/GetRoutes').subscribe(result => {
            this.routes = result.json() as Route[];
        }, error => console.error(error));
    }
}

interface Route {
    Name: string;
    Points: number;
    Blocker: boolean;
}
