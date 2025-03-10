import { Injectable } from '@angular/core';
import { EventEmitter } from '@angular/core';
import { BehaviorSubject, Subject } from 'rxjs';
@Injectable({
  providedIn: 'root'
})
export class SharedServicesService {

  private resetStateSubject = new Subject<void>();

  // Observable to listen for reset state events
  resetState$ = this.resetStateSubject.asObservable();

  // Method to trigger reset state
  triggerResetState() {
    this.resetStateSubject.next();
  }



  //code to searchproduct STARTS HERE

        // Create a Subject to emit events (like a radio broadcaster)
        private buttonClickSource = new Subject<string>();

        // Create an observable for other components to subscribe to (like a radio listener)
        buttonClicked$ = this.buttonClickSource.asObservable();

        // Method to emit an event with data
        emitButtonClick(data:string)
        {
          this.buttonClickSource.next(data);  // Send the data to all subscribers
        }
//code to searchproduct ENDS HERE


}
