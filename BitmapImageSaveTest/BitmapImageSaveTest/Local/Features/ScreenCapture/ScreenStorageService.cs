using BitmapImageSaveTest.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace BitmapImageSaveTest.Local.Features.ScreenCapture
{
    public abstract class ScreenStorageService
    {
        protected string _storagePath;

        protected ScreenStorageService(string storagePath)
        {
            _storagePath = storagePath;
        }

        public void SaveScreenshot(BitmapSource screenshot)
        {
            SaveToStorage(EncodeScreen(screenshot));
        }

        protected BitmapEncoder EncodeScreen(BitmapSource screenshot)
        {
            PngBitmapEncoder encoder = new PngBitmapEncoder();

            encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(screenshot));

            //DispatcherHelper.Invoke(() =>
            //{
            //    encoder = new PngBitmapEncoder();
            //    encoder.Frames.Add(BitmapFrame.Create(screenshot));
            //});
            return encoder;
        }

        protected abstract void SaveToStorage(BitmapEncoder encoder);
    }
}
