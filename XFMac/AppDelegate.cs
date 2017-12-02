using System;
using System.IO;
using AppKit;
using Foundation;
using LocalData;
using Xamarin.Forms;
using Xamarin.Forms.Platform.MacOS;
using XFCore;

namespace XFMac
{
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        NSWindow _window;
        public AppDelegate()
        {
            var style = NSWindowStyle.Closable | NSWindowStyle.Resizable | NSWindowStyle.Titled;

            var rect = new CoreGraphics.CGRect(200, 1000, 1280, 800);

            _window = new NSWindow(rect, style, NSBackingStore.Buffered, false);
            _window.MinSize = new CoreGraphics.CGSize(400, 300);
            _window.Title = "Translate.ge";
            _window.TitleVisibility = NSWindowTitleVisibility.Hidden;
        }

        public override NSWindow MainWindow => _window;

        public override void DidFinishLaunching(NSNotification notification)
        {
            SQLitePCL.Batteries.Init();
            var dbPath = Path.Combine(NSSearchPath.GetDirectories(NSSearchPathDirectory.DocumentDirectory, NSSearchPathDomain.User, true)[0], "words.db3");

            SQLitePCL.raw.SetProvider(new SQLitePCL.SQLite3Provider_e_sqlite3());

            Forms.Init();
            LoadApplication(new App(new WordRepository(dbPath)));
            base.DidFinishLaunching(notification);
        }

    }
}
