import { Component } from '@angular/core';
import * as signalR from "@microsoft/signalr";
import { CustomLogger } from './customLogger';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {
  title: string = 'signal-r-app-frontend';
  viewCounter: number = 0;
  sampleMethodThatTakesParameters: string = '';
  connectionStatus: string = '';
  private connection: signalR.HubConnection;

  constructor() {
    this.connection = new signalR.HubConnectionBuilder()
      .configureLogging(new CustomLogger())
      .configureLogging(signalR.LogLevel.Information)
      .withUrl("http://localhost:5000/hubs/view", {
        transport: signalR.HttpTransportType.WebSockets
        | signalR.HttpTransportType.ServerSentEvents
      })
      .withAutomaticReconnect()
      .build();

    this.connection.onclose(() => { this.connectionStatus = "Disconnected"; });
    this.connection.onreconnected(() => { this.connectionStatus = "Connected"; });
    this.connection.onreconnecting(() => { this.connectionStatus = "Reconnecting"; });

    // subscribe to server events
    this.connection.on("viewCountUpdate", (value: number) => {
      this.viewCounter = value;
    });

    //start connection
    this.connection.start().then(this.startSuccess.bind(this), this.startFail.bind(this));
  }

  startSuccess() {
    this.connectionStatus = "Connected";
    console.log("Connected.");
  }

  startFail() {
    this.connectionStatus = "Disconnected";
    console.log("Connection failed.");
  }

  button_call_methods_click() {
    this.connection.send("sampleVoidMethod");
    this.connection.invoke("sampleMethodThatTakesParameters", "Anderson", 11)
      .then(
        (result) => { this.sampleMethodThatTakesParameters = result },
        (error) => { console.log(`An error ocurred ${error}`) });
  }
}
