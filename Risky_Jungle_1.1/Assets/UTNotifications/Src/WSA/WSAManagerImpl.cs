#if !UNITY_EDITOR && UNITY_WSA

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UTNotifications.WSA;

namespace UTNotifications
{
    public class ManagerImpl : Manager, IInitializeHandler
    {
    //public
        public override bool Initialize(bool willHandleReceivedNotifications, int startId, bool incrementalId)
        {
            NotificationTools.Initialize(willHandleReceivedNotifications, startId, incrementalId, Application.version, this, Settings.Instance.PushNotificationsEnabledWindows, Settings.Instance.WindowsDontShowWhenRunning);
            return m_initialized = true;
        }

        public void OnInitialized(string registrationId)
        {
            m_registrationId = registrationId;
        }

        public override void PostLocalNotification(string title, string text, int id, IDictionary<string, string> userData, string notificationProfile)
        {
            NotificationTools.PostLocalNotification(title, text, id, userData, notificationProfile);
        }

        public override void ScheduleNotification(int triggerInSeconds, string title, string text, int id, IDictionary<string, string> userData, string notificationProfile)
        {
            NotificationTools.ScheduleNotification(triggerInSeconds, title, text, id, userData, notificationProfile);
        }

        public override void ScheduleNotificationRepeating(int firstTriggerInSeconds, int intervalSeconds, string title, string text, int id, IDictionary<string, string> userData, string notificationProfile)
        {
            NotificationTools.ScheduleNotificationRepeating(firstTriggerInSeconds, intervalSeconds, title, text, id, userData, notificationProfile);
        }

        public override bool NotificationsEnabled()
        {
            return NotificationTools.NotificationsEnabled();
        }

        public override void SetNotificationsEnabled(bool enabled)
        {
            NotificationTools.SetNotificationsEnabled(enabled);
        }

        public override void CancelNotification(int id)
        {
            NotificationTools.CancelNotification(id);
        }

        public override void CancelAllNotifications()
        {
            NotificationTools.CancelAllNotifications();
        }

        public override void HideNotification(int id)
        {
            NotSupported();
        }

        public override void HideAllNotifications()
        {
            NotSupported();
        }

        public override int GetBadge()
        {
            //Not implemented yet
			NotSupported("badges");
            return 0;
        }

        public override void SetBadge(int bandgeNumber)
        {
			//Not implemented yet
			NotSupported("badges");
        }

    //protected
        protected void Update()
        {
            if (!m_initialized)
            {
                return;
            }

            if (m_registrationId != null && OnSendRegistrationIdHasSubscribers())
            {
                _OnSendRegistrationId(m_providerName, m_registrationId);
                m_registrationId = null;
            }

            if (Time.time - m_lastTimeUpdated >= m_updateEverySeconds)
            {
                var received = NotificationTools.HandleReceivedNotifications();
                if (OnNotificationsReceivedHasSubscribers() && received != null && received.Count > 0)
                {
                    List<UTNotifications.ReceivedNotification> receivedNotifications = new List<UTNotifications.ReceivedNotification>();
                    foreach (var it in received)
                    {
                        receivedNotifications.Add(new UTNotifications.ReceivedNotification(it.Title, it.Text, it.Id, it.UserData, it.NotificationProfile));
                    }

                    _OnNotificationsReceived(receivedNotifications);
                }

                NotificationTools.UpdateWhenRunning();
                m_lastTimeUpdated = Time.time;
            }
        }

    //private
        private volatile string m_registrationId = null;
        private float m_lastTimeUpdated = 0;
        private bool m_initialized = false;
        private const float m_updateEverySeconds = 2.0f;
        private const string m_providerName = "Windows";
    }
}
#endif