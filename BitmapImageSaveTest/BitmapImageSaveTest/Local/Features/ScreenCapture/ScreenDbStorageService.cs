using BitmapImageSaveTest.Repositories;
using BitmapImageSaveTest.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace BitmapImageSaveTest.Local.Features.ScreenCapture
{
    public class ScreenDbStorageService : ScreenStorageService
    {
        string _applicationName;


        private MysqlDataService _mysqlService;
        private MysqlQueryHelper _mysqlQueryHelper;


        public ScreenDbStorageService(string applicationName, IPEndPoint remoteEP, string storagePath, string user, string password) : base(storagePath)
        {
            _applicationName = applicationName;
            _mysqlService = new MysqlDataService("komsa", remoteEP, storagePath, user, password);
            _mysqlQueryHelper = new MysqlQueryHelper();
        }

        /// <summary>
        /// 저장소에 인코딩된 내용을 저장
        /// </summary>
        protected override void SaveToStorage(BitmapEncoder encoder)
        {
            byte[] bytes = ConvertToBytesFrom(encoder);
            InsertImageBinary(bytes);
        }

        private byte[] ConvertToBytesFrom(BitmapEncoder encoder)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                encoder.Save(stream);
                return stream.ToArray();
            }
        }

        private void InsertImageBinary(byte[] bytes)
        {
            _mysqlService.ExecuteQuery(_mysqlQueryHelper.MakeImageDataInsertCommand(_applicationName, bytes));
        }
    }
}
