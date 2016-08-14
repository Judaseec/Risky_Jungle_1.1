using System;

public interface ITapjoyEvent
{
	void SendEventSucceeded(TapjoyEvent tapjoyEvent, bool contentIsAvailable);
	void SendEventFailed(TapjoyEvent tapjoyEvent, string error);
	void ContentDidAppear(TapjoyEvent tapjoyEvent);
	void ContentDidDisappear(TapjoyEvent tapjoyEvent);
	void DidRequestAction(TapjoyEvent tapjoyEvent, TapjoyEventRequest tapjoyEventRequest);
}

