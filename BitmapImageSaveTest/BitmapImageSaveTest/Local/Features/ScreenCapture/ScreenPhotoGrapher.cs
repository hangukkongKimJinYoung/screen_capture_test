using BitmapImageSaveTest.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace BitmapImageSaveTest.Local.Features.ScreenCapture
{
    public class ScreenPhotoGrapher
    {
        private ScreenStorageService _storageService;


        public ScreenPhotoGrapher(ScreenStorageService storageService)
        {
            _storageService = storageService;
        }

        public void CaptureScreen(FrameworkElement subject)
        {
            BitmapSource bitmap = MakeScreenshot(subject);

            if (bitmap != null)
            {
                _storageService.SaveScreenshot(bitmap);
            }
        }

        public BitmapSource MakeScreenshot(FrameworkElement subject)
        {
            // ActualWidth, ActualHeight가 0이면 정상 캡처가 안 된다
            if (subject.ActualWidth == 0 || subject.ActualHeight == 0)
            {
                return null;
            }


            BitmapSource bitmapSource = null;


            // RenderTargetBitmap이 UI 요소를 렌더링해야 하므로 UI 스레드에서 실행한다
            // RenderTargetBitmap 자체도 UI 스레드 전용 객체여서 UI 스레드에서 생성해야 한다
            // DispatcherPriority.Render를 사용하여 렌더링 완료 이후 실행되도록 한다
            //
            // 함수 자체를 async에 Task를 반환하도록 하고 Dispatcher.Invoke 부분을 await Task.Run() 으로 감싸서 UI를 멈추지 않고 백그라운드에서 동작 수행이 가능
            // 호출한 곳에서는 await 함수() 해서 작업 끝나면 받음
            DispatcherHelper.Invoke(() =>
            {
                RenderTargetBitmap renderBitmap = new RenderTargetBitmap(
                    (int)subject.ActualWidth, (int)subject.ActualHeight,
                    96, 96, PixelFormats.Pbgra32);

                // RenderTargetBitmap.Render()는 현재 렌더링된 모습을 캡처한다. 이미 그려진 화면을 캡처한다는 뜻
                // Measure(), Arrange() 호출의 이유는 일부 컨트롤의 경우 화면에 표시되지 않으면 크기가 0으로 유지되기 때문
                // Visibility="Collapsed" 상태이거나 ContentPresenter 내부의 동적 컨텐츠일 때는 크기가 할당되지 않음
                subject.Measure(new Size(subject.ActualWidth, subject.ActualHeight));
                subject.Arrange(new Rect(new Size(subject.ActualWidth, subject.ActualHeight)));

                renderBitmap.Render(subject);
                

                // 중요:
                // BitmapSource로 변환하여 UI 스레드 종속성 제거
                // RenderTargetBitmap이 내부에서 렌더링 할 때 WPF 렌더링에 접근하기 때문에 UI 스레드 전용 객체이다
                // BitmapFrame.Create()는 새로 이미지를 복사해서 BitmapFrame 객체를 만든다. BitmapFrame은 이미지만 저장하기 때문에 UI 종속적이지 않음
                // 그럼에도 Freeze()를 호출하지 않으면 다른 스레드에서는 에러가 발생한다
                // 이때는 UI 스레드가 아니어서 발생하는 것이 아니고, Freezable 객체는 Freeze()호출 전까지 스레드의 변경을 검사하기 때문에 스레드 독립적이지 않다
                // 다른 스레드가 변경하려고 해서 그렇다는 뜻. Freeze()하면 읽기 전용이어서 다른 스레드에서도 접근이 가능하다
                bitmapSource = BitmapFrame.Create(renderBitmap);
                bitmapSource.Freeze();
            }, DispatcherPriority.Render);


            return bitmapSource;
        }
    }
}
