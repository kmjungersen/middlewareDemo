import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
    selector: 'results',
    templateUrl: './results.component.html'
})
export class ResultsComponent {
    logs: Log[];
    resultStats: ResultStatistics;

    constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        http.get(baseUrl + 'api/Logger/GetLogs').subscribe(data => {
            this.logs = data as Log[];
        }, error => console.error(error));

        http.get(baseUrl + 'api/Logger/GetStatistics').subscribe(data => {
            this.resultStats = data as ResultStatistics;
        }, error => console.error(error));

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

interface ResultStatistics {
    minRequest: string,
    maxRequest: string,
    avgRequest: string,
}
