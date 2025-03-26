using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace BitmapImageSaveTest.Utils
{
    public class DispatcherHelper
    {
        public static void Invoke(Action action, DispatcherPriority priority = DispatcherPriority.Send)
        {
            Dispatcher dispatchObject = Application.Current != null ? Application.Current.Dispatcher : null;
            if (dispatchObject == null || dispatchObject.CheckAccess())
                action();
            else
                dispatchObject.Invoke(action, priority);
        }
    }
}
