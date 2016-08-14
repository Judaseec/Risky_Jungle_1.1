using UnityEngine;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System;
using System.ComponentModel;

public class TapjoyEvent
{
	private string myGuid;
	private string myName;
	private string myParameter;
	private ITapjoyEvent myCallback;
	
	public TapjoyEvent(string eventName, ITapjoyEvent callback) : this(eventName, null, callback) {}
	public TapjoyEvent(string eventName, string eventParameter, ITapjoyEvent callback)
	{
		myName = eventName;
		myParameter = eventParameter;
		myCallback = callback;
		myGuid = TapjoyPlugin.CreateEvent(this, eventName, eventParameter);
		
		Debug.Log(String.Format("C#: Event {0} created with GUID:{1} with Param:{2}", myName, myGuid, myParameter));
	}
	
	public void Send()
	{
		Debug.Log(String.Format("C#: Sending event {0} ", myName));
		
		TapjoyPlugin.SendEvent(myGuid);
	}
	
	public void Show()
	{
		TapjoyPlugin.ShowEvent(myGuid);
	}
	
	public void EnableAutoPresent(bool autoPresent)
	{
		TapjoyPlugin.EnableEventAutoPresent(myGuid, autoPresent);
	}

	public void TriggerSendEventSucceeded(bool contentIsAvailable)
	{
		Debug.Log("C#: TriggerSendEventSucceeded");
		myCallback.SendEventSucceeded(this, contentIsAvailable);
	}
	
	public void TriggerSendEventFailed(string errorMsg)
	{
		Debug.Log("C#: TriggerSendEventFailed");
		myCallback.SendEventFailed(this, errorMsg);
	}

	public void TriggerContentDidAppear()
	{
		Debug.Log("C#: TriggerContentDidAppear");
		myCallback.ContentDidAppear(this);
	}
	
	public void TriggerContentDidDisappear()
	{
		Debug.Log("C#: TriggerContentDidDisappear");
		myCallback.ContentDidDisappear(this);
	}

	public void TriggerDidRequestAction(int type, string identifier, int quantity)
	{
		Debug.Log("C#: TriggerDidRequestAction");

		TapjoyEventRequest eventRequest = new TapjoyEventRequest(myGuid, type, identifier, quantity);

		myCallback.DidRequestAction(this, eventRequest);
	}
}

