using System;
using Foundation;
using UIKit;
using UserNotifications;

namespace NotificationTestServiceExtension
{
    [Register("NotificationService")]
    public class NotificationService : UNNotificationServiceExtension
    {
        Action<UNNotificationContent> ContentHandler { get; set; }
        UNMutableNotificationContent BestAttemptContent { get; set; }

        protected NotificationService(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void DidReceiveNotificationRequest(UNNotificationRequest request, Action<UNNotificationContent> contentHandler)
        {
            ContentHandler = contentHandler;
            BestAttemptContent = (UNMutableNotificationContent)request.Content.MutableCopy();

            // Modify the notification content here...
            var plist = new NSUserDefaults("group.com.libin.sharing", NSUserDefaultsType.SuiteName);
            nint badgeCount = plist.IntForKey("BadgeCount");
            badgeCount++;

            var newAlertContent = new UNMutableNotificationContent
            {
                Body = BestAttemptContent.Body,
                Title = BestAttemptContent.Title,
                Sound = BestAttemptContent.Sound,
                Badge = new NSNumber(badgeCount)
            };
            plist.SetInt(badgeCount, "BadgeCount");
            ContentHandler(newAlertContent);
        }

        public override void TimeWillExpire()
        {
            // Called just before the extension will be terminated by the system.
            // Use this as an opportunity to deliver your "best attempt" at modified content, otherwise the original push payload will be used.

            ContentHandler(BestAttemptContent);
        }
    }
}
