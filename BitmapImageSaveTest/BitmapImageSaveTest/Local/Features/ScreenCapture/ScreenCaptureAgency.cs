using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace BitmapImageSaveTest.Local.Features.ScreenCapture
{
    public class ScreenCaptureAgency
    {
        private FrameworkElement _subject;

        private Timer _captureScreenTimer;
        private int _captureScreenPeriod_ms = 1000 * 60;


        private ScreenPhotoGrapher _screenPhotoGrapher;


        public ScreenCaptureAgency()
        {
            Initialize();
        }

        private void Initialize()
        {
            _screenPhotoGrapher = new ScreenPhotoGrapher(new ScreenFileStorageService());

            _captureScreenTimer = new Timer(CaptureScreenTimerCallback);
        }

        public void StartCaptureScreen(FrameworkElement subject)
        {
            _subject = subject;

            _captureScreenTimer.Change(0, _captureScreenPeriod_ms);
            //_captureScreenTimer.Change(0, 1000*5);
            //Task.Run(() => { _screenPhotoGrapher.CaptureScreen(subject); });
        }

        public void StopCaptureScreen()
        {
            _captureScreenTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void CaptureScreenTimerCallback(object state)
        {
            _screenPhotoGrapher.CaptureScreen(_subject);
        }
    }
}
