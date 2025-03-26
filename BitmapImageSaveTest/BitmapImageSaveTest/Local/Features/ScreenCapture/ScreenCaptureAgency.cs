using BitmapImageSaveTest.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace BitmapImageSaveTest.Local.Features.ScreenCapture
{
    /// <summary>
    /// 객체 간의 연결, 동작 수행을 뷰모델에서 수행하면 나중에 이것저것 지저분해지는 경우가 많아서 사용 분야에서 동작하는 내용을 관리하는 것은 ~Agency 클래스가 담당한다
    /// </summary>
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
            // Debug/Capture/ 에서 이미지 저장하는 것을 확인하려면 주석 제거
            //ScreenStorageService storageService = new ScreenFileStorageService(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Capture"));
            ScreenStorageService storageService = new ScreenDbStorageService(
                new System.Net.IPEndPoint(IPAddress.Parse("127.0.0.1"), 3306),
                "komeri_ship",
                "root",
                "1234",
                1,
                (int)EApplicationCode.Ar);
            
            _screenPhotoGrapher = new ScreenPhotoGrapher(storageService);

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
