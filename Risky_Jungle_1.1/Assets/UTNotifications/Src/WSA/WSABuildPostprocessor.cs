#if UNITY_EDITOR && UNITY_WSA

using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Xml;

namespace UTNotifications
{
    class WSABuildPostprocessor
    {
    //public
        [UnityEditor.Callbacks.PostProcessBuildAttribute(0)]
        public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
        {
            if (target == BuildTarget.WSAPlayer)
            {
                PatchManifest(Path.Combine(pathToBuiltProject, Application.productName + "/Package.appxmanifest"));
                PatchManifest(Path.Combine(pathToBuiltProject, Application.productName + "/" + Application.productName + ".Windows/Package.appxmanifest"));
                PatchManifest(Path.Combine(pathToBuiltProject, Application.productName + "/" + Application.productName + ".WindowsPhone/Package.appxmanifest"));
            }
        }

    //private
        private static void PatchManifest(string manifestFileName)
        {
            if (!File.Exists(manifestFileName))
            {
                return;
            }

            XmlDocument xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.Load(manifestFileName);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                return;
            }

            XmlNode packageNode = XmlUtils.FindChildNode(xmlDocument, "Package");
            XmlNode applicationsNode = XmlUtils.FindChildNode(packageNode, "Applications");
            XmlNode applicationNode = XmlUtils.FindChildNode(applicationsNode, "Application");

            string ns;
            XmlNode previous;
            if (XmlUtils.FindElement(out previous, applicationNode, "m2:VisualElements") != null)
            {
                //Windows manifest
                ns = "m2";
                PatchLockScreen(xmlDocument, packageNode, applicationNode, ns);
            }
            else if (XmlUtils.FindElement(out previous, applicationNode, "m3:VisualElements") != null)
            {
                //Windows Phone manifest
                ns = "m3";
            }
            else
            {
                //Windows 10 Universal Build
                ns = "uap";
                PatchLockScreen(xmlDocument, packageNode, applicationNode, ns);
            }

            PatchIdentity(xmlDocument, packageNode);
            PatchCapabilities(xmlDocument, packageNode, applicationNode, ns);
            PatchExtensions(xmlDocument, packageNode, applicationNode);

            xmlDocument.Save(manifestFileName);

            DeleteInvalidXmlns(manifestFileName);
        }

        private static void PatchIdentity(XmlDocument xmlDocument, XmlNode packageNode)
        {
            if (Settings.Instance.PushNotificationsEnabledWindows)
            {
                XmlNode previous;
                XmlElement identityNode = XmlUtils.FindElement(out previous, packageNode, "Identity");
                string identityName = Settings.Instance.WindowsIdentityName;

                if (!string.IsNullOrEmpty(identityName) && identityName != PlayerSettings.WSA.applicationDescription)
                {
                    identityNode.SetAttribute("Name", identityName);
                }
                else
                {
                    identityName = identityNode.GetAttribute("Name");
                }

                if (string.IsNullOrEmpty(identityName) || identityName == PlayerSettings.WSA.applicationDescription)
                {
                    Debug.LogWarning("Please specify Windows Store Identity Name in the UTNotifications Settings!");
                }

                string publisher = identityNode.GetAttribute("Publisher").Replace("CN=", "");
                if (!Settings.Instance.WindowsCertificateIsCorrect(publisher))
                {
                    Debug.LogWarning(Settings.WRONG_CERTIFICATE_MESSAGE);
                }
            }
        }

        private static void PatchLockScreen(XmlDocument xmlDocument, XmlNode packageNode, XmlNode applicationNode, string ns)
        {
            //<Package>
            //  <Applications>
            //    <Application>
            //      <m2/uap:VisualElements ToastCapable="true">
            //        <m2/uap:LockScreen Notification="badge" BadgeLogo="Assets\MediumTile.png" />
            //      </m2/uap:VisualElements>
            //    </Application>
            //  </Applications>
            //</Package>

            XmlNode previous;
            XmlElement visualElementsNode = XmlUtils.FindElement(out previous, applicationNode, ns + ":VisualElements");
            XmlElement lockScreenNode = XmlUtils.UpdateOrCreateElement(xmlDocument, visualElementsNode, ns + ":LockScreen", null, null, null, null, packageNode.GetNamespaceOfPrefix(ns));
            if (string.IsNullOrEmpty(lockScreenNode.GetAttribute("Notification")))
            {
                lockScreenNode.SetAttribute("Notification", "badge");
            }
            if (string.IsNullOrEmpty(lockScreenNode.GetAttribute("BadgeLogo")))
            {
                lockScreenNode.SetAttribute("BadgeLogo", visualElementsNode.GetAttribute("Square150x150Logo"));
            }
        }

        private static void PatchCapabilities(XmlDocument xmlDocument, XmlNode packageNode, XmlNode applicationNode, string ns)
        {
            //<Package>
            //  <Applications>
            //    <Application>
            //      <m3:VisualElements ToastCapable="true">
            //      </m3:VisualElements>
            //    </Application>
            //  </Applications>
            //  <Capabilities>
            //    <Capability Name="internetClientServer" /> / <Capability Name="internetClient" />
            //  </Capabilities>
            //</Package>

            XmlNode previous;
            XmlElement visualElementsNode = XmlUtils.FindElement(out previous, applicationNode, ns + ":VisualElements");
            visualElementsNode.SetAttribute("ToastCapable", "true");

            if (Settings.Instance.PushNotificationsEnabledWindows)
            {
                XmlElement capabilitiesNode = XmlUtils.UpdateOrCreateElement(xmlDocument, packageNode, "Capabilities");

                string requiredCapability = (ns == "m3" ? "internetClientServer" : "internetClient");
                XmlUtils.UpdateOrCreateElement(xmlDocument, capabilitiesNode, "Capability", "Name", null, requiredCapability);
            }
        }

        private static void PatchExtensions(XmlDocument xmlDocument, XmlNode packageNode, XmlNode applicationNode)
        {
            //<Package>
            //  <Applications>
            //    <Applications>
            //      <Extensions>
            //        <Extension Category="windows.backgroundTasks" EntryPoint="UTNotifications.WSA.BackgroundTask">
            //          <BackgroundTasks>
            //            <Task Type="systemEvent"/>
            //          </BackgroundTasks>
            //        </Extension>
            //        <Extension Category="windows.backgroundTasks" EntryPoint="UTNotifications.WSA.PushBackgroundTask">
            //          <BackgroundTasks>
            //            <Task Type="pushNotification"/>
            //          </BackgroundTasks>
            //        </Extension>
            //      </Extensions>
            //    </Application>
            //  </Applications>
            //</Package>

            //<Extensions>
            XmlElement extensionsNode = XmlUtils.UpdateOrCreateElement(xmlDocument, applicationNode, "Extensions");

            //<Extension Category="windows.backgroundTasks" EntryPoint="UTNotifications.WSA.BackgroundTask">
            //  <BackgroundTasks>
            //    <Task Type="systemEvent"/>
            //  </BackgroundTasks>
            //</Extension>
            {
                XmlElement extensionNode = XmlUtils.UpdateOrCreateElement(xmlDocument, extensionsNode, "Extension", "EntryPoint", null, "UTNotifications.WSA.BackgroundTask");
                extensionNode.SetAttribute("Category", "windows.backgroundTasks");
                XmlElement backgroundTasksNode = XmlUtils.UpdateOrCreateElement(xmlDocument, extensionNode, "BackgroundTasks");
                XmlUtils.UpdateOrCreateElement(xmlDocument, backgroundTasksNode, "Task", "Type", null, "systemEvent");
            }

            //<Extension Category="windows.backgroundTasks" EntryPoint="UTNotifications.WSA.PushBackgroundTask">
            //  <BackgroundTasks>
            //    <Task Type="pushNotification"/>
            //  </BackgroundTasks>
            //</Extension>
            if (Settings.Instance.PushNotificationsEnabledWindows)
            {
                XmlElement extensionNode = XmlUtils.UpdateOrCreateElement(xmlDocument, extensionsNode, "Extension", "EntryPoint", null, "UTNotifications.WSA.PushBackgroundTask");
                extensionNode.SetAttribute("Category", "windows.backgroundTasks");
                XmlElement backgroundTasksNode = XmlUtils.UpdateOrCreateElement(xmlDocument, extensionNode, "BackgroundTasks");
                XmlUtils.UpdateOrCreateElement(xmlDocument, backgroundTasksNode, "Task", "Type", null, "pushNotification");
            }
            else
            {
                XmlUtils.RemoveElement(xmlDocument, extensionsNode, "Extension", "EntryPoint", null, "UTNotifications.WSA.PushBackgroundTask");
            }
        }

        private static void DeleteInvalidXmlns(string manifestFileName)
        {
            string contents = File.ReadAllText(manifestFileName);
            contents = contents.Replace(" xmlns=\"\"", "");
            File.WriteAllText(manifestFileName, contents);
        }
    }
}
#endif