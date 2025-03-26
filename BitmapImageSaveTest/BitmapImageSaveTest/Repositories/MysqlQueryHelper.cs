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
        private const string TBL_IMAGE_ROW_INSERT = "INSERT INTO `tbl_image_row` (`ship_code`, `sensor_code`, `sensor_instance`, `savetime`, `image`)\r\nVALUES (@shipCode, @sensorCode, @sensorInstance, @savetime, @image) AS new\r\nON DUPLICATE KEY UPDATE\r\n  `savetime` = new.`savetime`,\r\n  `image` = new.`image`;";


        public MySqlCommand MakeImageDataInsertCommand(int shipCode, int sensorCode, int sensorInstance, byte[] imageBinary)
        {
            MySqlCommand comm = new MySqlCommand(TBL_IMAGE_ROW_INSERT);
            comm.Parameters.Add("@shipCode", MySqlDbType.Int32).Value = shipCode;
            comm.Parameters.Add("@sensorCode", MySqlDbType.Int32).Value = sensorCode;
            comm.Parameters.Add("@sensorInstance", MySqlDbType.Int32).Value = sensorInstance;
            comm.Parameters.Add("@savetime", MySqlDbType.DateTime).Value = DateTime.Now;
            comm.Parameters.Add("@image", MySqlDbType.LongBlob).Value = imageBinary;


            return comm;
        }
    }
}
