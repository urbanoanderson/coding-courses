import { ILogger, LogLevel } from "@microsoft/signalr";

export class CustomLogger implements ILogger {
    log(logLevel: LogLevel, message: string): void {
        console.log(`${logLevel} :: ${message}`);
    }
}