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
        private uint _saveId = 0;


        public ScreenFileStorageService(string storagePath) : base(storagePath)
        {
            Initialize();
        }

        private void Initialize()
        {
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
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
            return Path.Combine(_storagePath, $"{_saveId++}.png");
        }
    }
}
