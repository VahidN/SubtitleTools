using System;
using System.Threading.Tasks;
using SubtitleTools.Common.MVVM;
using SubtitleTools.Infrastructure.Core;
using SubtitleTools.Infrastructure.Models;

namespace SubtitleTools.Infrastructure.ViewModels
{
    public class TranslatorViewModel
    {
        private readonly Translate _translate = new Translate();

        public TranslatorViewModel()
        {
            TranslatorData = new TranslatorModel();
            setupCommands();
        }

        public DelegateCommand<string> DoCloseTranslateView { set; get; }
        public DelegateCommand<string> DoTranslate { set; get; }
        public DelegateCommand<string> DoStopTranslation { set; get; }

        public TranslatorModel TranslatorData { set; get; }

        void doStopTranslation(string data)
        {
            _translate.StopTranslation = true;
        }

        bool canDoStopTranslation(string data)
        {
            return true;
        }

        bool canDoCloseTranslateView(string data)
        {
            return true;
        }

        static bool canDoTranslate(string filePatha)
        {
            return true;
        }

        void doCloseTranslateView(string data)
        {
            App.Messenger.NotifyColleagues("doCloseTranslateView");
        }

        void doTranslate(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return;

            if (TranslatorData.FromLanguage == null || TranslatorData.ToLanguage == null)
            {
                LogWindow.AddMessage(LogType.Alert, "Please select the From and To languages.");
                return;
            }

            Task.Factory.StartNew(() =>
            {
                try
                {
                    _translate.TranslateAll(filePath, TranslatorData.FromLanguage.ISO639, TranslatorData.ToLanguage.ISO639);
                }
                catch (Exception ex)
                {
                    LogWindow.AddMessage(LogType.Error, ex.Message);
                }
            });
        }

        private void setupCommands()
        {
            DoTranslate = new DelegateCommand<string>(doTranslate, canDoTranslate);
            DoCloseTranslateView = new DelegateCommand<string>(doCloseTranslateView, canDoCloseTranslateView);
            DoStopTranslation = new DelegateCommand<string>(doStopTranslation, canDoStopTranslation);
        }
    }
}