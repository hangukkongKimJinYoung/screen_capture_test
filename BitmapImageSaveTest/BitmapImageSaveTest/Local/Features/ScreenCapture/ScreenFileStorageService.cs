using BitmapImageSaveTest.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BitmapImageSaveTest.Local.Features.ScreenCapture
{
    public class ScreenFileStorageService : ScreenStorageService
    {
        private string _baseStorageFolderPath;
        private uint _saveId = 0;


        public ScreenFileStorageService()
        {
            Initialize();
        }

        private void Initialize()
        {
            _baseStorageFolderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Capture");

            if (!Directory.Exists(_baseStorageFolderPath))
            {
                Directory.CreateDirectory(_baseStorageFolderPath);
            }
        }

        // 추상 클래스 사용하기 이전 구조에서 사용한 함수
        //public override void SaveScreenshot(RenderTargetBitmap screenshot)
        //{
        //    using (FileStream outStream = new FileStream(CalculateStoragePath(), FileMode.Create))
        //    {
        //        DispatcherHelper.Invoke(() =>
        //        {
        //            PngBitmapEncoder encoder = new PngBitmapEncoder();
        //            encoder.Frames.Add(BitmapFrame.Create(screenshot));
        //            encoder.Save(outStream);
        //        });
        //    }
        //}

        protected override void SaveToStorage(BitmapEncoder encoder)
        {
            using (FileStream outStream = new FileStream(CalculateStoragePath(), FileMode.Create))
            {
                encoder.Save(outStream);
            }
        }

        private string CalculateStoragePath()
        {
            return Path.Combine(_baseStorageFolderPath, $"{_saveId++}.png");
        }
    }
}
