using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitmapImageSaveTest.Repositories
{
    public class MysqlQueryHelper
    {
        private const string TBL_IMAGE_ROW_INSERT = "INSERT INTO `tbl_image_row` (`application`, `savetime`, `image`)\r\nVALUES (@applicationName, @savetime, @image) AS new\r\nON DUPLICATE KEY UPDATE\r\n  `savetime` = new.`savetime`,\r\n  `image` = new.`image`;";


        public MySqlCommand MakeImageDataInsertCommand(string applicationName, byte[] imageBinary)
        {
            MySqlCommand comm = new MySqlCommand(TBL_IMAGE_ROW_INSERT);
            comm.Parameters.Add("@savetime", MySqlDbType.DateTime).Value = DateTime.Now;
            comm.Parameters.Add("@applicationName", MySqlDbType.VarChar).Value = applicationName;
            comm.Parameters.Add("@image", MySqlDbType.LongBlob).Value = imageBinary;


            return comm;
        }
    }
}
