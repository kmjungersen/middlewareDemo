import { Component, Inject } from '@angular/core';
import { HttpHeaders } from '@angular/common/http';
import { HttpClient } from '@angular/common/http';
import { RequestMethod } from '@angular/http';

@Component({
    selector: 'results',
    templateUrl: './results.component.html'
})
export class ResultsComponent {
    logs: Log[];

    constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        http.get(baseUrl + 'api/Logger/GetLogs').subscribe(data => {
            this.logs = data as Log[];
            this.computeStats();
        }, error => console.error(error));
    }

    computeStats() {
        this.logs = this.logs.sort((a, b) => a.requestTime.getTime() - b.requestTime.getTime());
        // let posts = this.logs.filter(x => x.type == 'POST');
    }
}

interface Log {
    statusCode: string,
    method: string,
    path: string,
    size: string,
    requestTime: Date,
    responseTime: Date,
    totalRequestTime: Date,
    processingTime: Date,
}
