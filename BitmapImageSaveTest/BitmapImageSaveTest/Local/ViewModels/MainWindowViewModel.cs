using BitmapImageSaveTest.Local.Features.ScreenCapture;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BitmapImageSaveTest.Local.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        private FrameworkElement _view;


        private bool _isScreenSaving;
        public bool IsScreenSaving
        {
            get => _isScreenSaving;
            set => SetProperty(ref _isScreenSaving, value);
        }


        private ScreenCaptureAgency _screenCaptureAgency;


        private ICommand _startSavingScreenCommand;
        public ICommand StartSavingScreenCommand => _startSavingScreenCommand ?? (_startSavingScreenCommand = new RelayCommand(StartSavingScreen));

        private ICommand _stopSavingScreenCommand;
        public ICommand StopSavingScreenCommand => _stopSavingScreenCommand ?? (_stopSavingScreenCommand = new RelayCommand(StopSavingScreen));


        public MainWindowViewModel()
        {
            Initialize();
        }

        /// <summary>
        /// 뷰에서 자신의 뷰모델에 참조를 넘긴다
        /// </summary>
        /// <param name="view"></param>
        public void SetView(FrameworkElement view)
        {
            _view = view;
        }

        private void Initialize()
        {
            _screenCaptureAgency = new ScreenCaptureAgency();
        }

        /// <summary>
        /// 화면 저장 시작 요청에 대한 메서드
        /// </summary>
        public void StartSavingScreen()
        {
            _screenCaptureAgency.StartCaptureScreen(_view);

            IsScreenSaving = true;
        }

        public void StopSavingScreen()
        {
            _screenCaptureAgency.StopCaptureScreen();

            IsScreenSaving = false;
        }
    }
}
