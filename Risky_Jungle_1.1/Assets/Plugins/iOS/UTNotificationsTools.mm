#import <Foundation/Foundation.h>

extern "C"
{
	bool _UT_RegisterForIOS8(bool remote)
	{
		float version = [[[UIDevice currentDevice] systemVersion] floatValue];
		if (version >= 8.0)
		{
			UIUserNotificationType types = UIRemoteNotificationTypeBadge | UIRemoteNotificationTypeAlert | UIRemoteNotificationTypeSound;
			UIUserNotificationSettings* settings = [UIUserNotificationSettings settingsForTypes:types categories:nil];
			[[UIApplication sharedApplication] registerUserNotificationSettings:settings];

			if (remote)
			{
				[[UIApplication sharedApplication] registerForRemoteNotifications];
			}

			return true;
		}
		else
		{
			return false;
		}
	}

	int _UT_GetIconBadgeNumber()
	{
		return [UIApplication sharedApplication].applicationIconBadgeNumber;
	}

	void _UT_SetIconBadgeNumber(int value)
	{
		[UIApplication sharedApplication].applicationIconBadgeNumber = value;
	}

	void _UT_HideAllPushNotifications()
	{
		int oldBadgeNumber = _UT_GetIconBadgeNumber();
		_UT_SetIconBadgeNumber(0);
		_UT_SetIconBadgeNumber(oldBadgeNumber);
	}
}