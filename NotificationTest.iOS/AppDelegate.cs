using System;
using System.Collections.Generic;
using System.Linq;

using Foundation;
using UIKit;
using UserNotifications;
using WindowsAzure.Messaging;

namespace NotificationTest.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        private SBNotificationHub _hub;
        //TODO: Change HubName and URL
        private const string notificationUrl = @"Endpoint=sb://*************************";
        private const string notificationHubName = "*****PushNotificationHub-Dev";

        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound,
                (granted, error) =>
                {
                    if (granted)
                        InvokeOnMainThread(UIApplication.SharedApplication.RegisterForRemoteNotifications);
                });
            }

            return base.FinishedLaunching(app, options);
        }

        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            _hub = new SBNotificationHub(notificationUrl, notificationHubName);

            _hub.UnregisterAllAsync(deviceToken);

            var deviceId = Convert.ToBase64String(deviceToken.ToArray());
            var tag = Guid.NewGuid().ToString();
            var tags = new List<string> { tag };

            _hub.RegisterNativeAsync(deviceToken, new NSSet(tags.ToArray()));

        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
        }
    }
}
