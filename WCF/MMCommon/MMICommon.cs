using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

using System.Drawing;
using System.Drawing.Imaging;

using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Reflection;

using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json.Utilities;

using System.Data;
using System.Data.SqlClient;
using System.Data.EntityClient;

using System.Windows.Forms;
using System.Configuration;
using System.Web.Configuration;

namespace MetricsManager.Common
{

    public static class MMIJSON
    {
        public static T StringToObject<T>(string jstr)
        {
            T obj = JsonConvert.DeserializeObject<T>(jstr);
            return obj;
        }

        public static string ObjectToString(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.None);
        }

        public static Stream ObjectToStream<T>(T tobj)
        {
            JObject jo = JObject.FromObject(tobj);
            // don't use  using ( ) , it will release ms. you will get nothing
            MemoryStream ms = new MemoryStream(ASCIIEncoding.ASCII.GetBytes(jo.ToString()));
            return ms;
        }

        public static T StreamToObject<T>(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                byte[] buf = new byte[1024];
                int recnt = 0;
                while ((recnt = stream.Read(buf, 0, buf.Length)) > 0)
                {
                    ms.Write(buf, 0, recnt);
                }
                ms.Position = 0;
                string objstr = ASCIIEncoding.ASCII.GetString(ms.GetBuffer());
                T obj = JsonConvert.DeserializeObject<T>(objstr);
                return obj;
            }
        }

    }

    public static class MMIIMAGE
    {
        public static Image BytesToImage(byte[] buf)
        {
            ImageConverter converter = new ImageConverter();
            return (Image)converter.ConvertFrom(buf);
            /*
            MemoryStream ms = new MemoryStream(buf);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
            */
        }

        public static Image StringToImage(string base64String)
        {
            byte[] buf = Convert.FromBase64String(base64String);
            return BytesToImage(buf);
        }

        public static void ImageToFile(Image img, string full_name)
        {
            img.Save(full_name);
        }

        public static Rectangle Bytes2Rect(int[] rectArr)
        {
            return new Rectangle(rectArr[0], rectArr[1], rectArr[2], rectArr[3]);
        }

        #region image to byte[]
        public static byte[] toBytes(Image image)
        {
            byte[] buf = new byte[0];
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, ImageFormat.Jpeg);
                buf = ms.ToArray();
                ms.Close();
            }
            return buf;
        }

        public static byte[] toBytes(Image image, ImageFormat imgType)
        {
            byte[] buf = new byte[0];
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, imgType);
                buf = ms.ToArray();
                ms.Close();
            }
            return buf;
        }

        public static byte[] toBytes(string image_path)
        {
            byte[] buf = new byte[0];
            if (File.Exists(image_path))
            {
                using (FileStream fr = File.OpenRead(image_path))
                using (MemoryStream ms = new MemoryStream())
                {
                    Image image = Bitmap.FromStream(fr);
                    image.Save(ms, ImageFormat.Jpeg);
                    buf = ms.ToArray();
                }
            }
            return buf;
        }

        public static byte[] toBytes(string image_path, ImageFormat imgType)
        {
            byte[] buf = new byte[0];
            if (File.Exists(image_path))
            {
                using (FileStream fr = File.OpenRead(image_path))
                using (MemoryStream ms = new MemoryStream())
                {
                    Image image = Bitmap.FromStream(fr);
                    image.Save(ms, imgType);
                    buf = ms.ToArray();
                }
            }
            return buf;
        }
        #endregion image to byte[]

        #region image base64 string
        public static string ImageToBase64(Image image)
        {
            byte[] buf = toBytes(image);
            return Convert.ToBase64String(buf);
        }

        public static string ImageToBase64(Image image, ImageFormat imgType)
        {
            byte[] buf = toBytes(image, imgType);
            return Convert.ToBase64String(buf);
        }

        public static string ImageToBase64(string image_path)
        {
            byte[] buf = toBytes(image_path);
            //return buf;
            return Convert.ToBase64String(buf);
        }

        public static string BinaryBlobToBase64(string binary_path)
        {
            byte[] buf = File.ReadAllBytes(binary_path);
            return Convert.ToBase64String(buf);
        }

        public static string ImageToBase64(string image_path, ImageFormat imgType)
        {
            byte[] buf = toBytes(image_path, imgType);
            return Convert.ToBase64String(buf);
        }
        #endregion image base64 string
        
        #region crop image function
        public static Image CropImage(Image image, int[] bound)
        {
            Rectangle srcRect = Bytes2Rect(bound);
            Bitmap newImage = new Bitmap(srcRect.Width, srcRect.Height, PixelFormat.Format24bppRgb);
            using (Graphics gp = Graphics.FromImage(newImage))
            {
                Rectangle dstRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height);
                gp.DrawImage(image, dstRect, srcRect, GraphicsUnit.Pixel);
            }
            return newImage;
        }

        public static Image CropImage(Image image, Rectangle srcRect)
        {
            Bitmap newImage = new Bitmap(srcRect.Width, srcRect.Height, PixelFormat.Format24bppRgb);
            using (Graphics gp = Graphics.FromImage(newImage))
            {
                Rectangle dstRect = new Rectangle(0, 0, srcRect.Width, srcRect.Height);
                gp.DrawImage(image, dstRect, srcRect,  GraphicsUnit.Pixel);
            }
            return newImage;
        }

        public static Image CropImage(Image image, int x, int y, int width, int height)
        {
            return CropImage(image, new Rectangle(x, y, width, height));
        }

        public static Image Resize(Image image, int[] new_size, bool stretch = false)
        {
            if (stretch)
            {
                return new Bitmap(image, new Size(new_size[0], new_size[1]));
            }
            else
            {
                int ow = image.Width;
                int oh = image.Height;
                int ew = new_size[0];
                int eh = new_size[1];
                double rate = Math.Min((double)ew / (double)ow, (double)eh / (double)oh);
                int nw = (int)(ow * rate);
                int nh = (int)(oh * rate);
                return new Bitmap(image, new Size(nw, nh));
            }
        }
        #endregion to Image function

        #region image metadata 
        public static string getComments(Image img)
        {
            string ret_str = string.Empty;
            PropertyItem p1 = img.GetPropertyItem(37510);
            ret_str = MMIUNICODE.getString(p1.Value);
            return ret_str;
        }

        public static string getMetaHead(Image img)
        {
            string ret_str = string.Empty;
            PropertyItem p1 = img.GetPropertyItem(37510);
            string meta_str = MMIUNICODE.getString(p1.Value);
            StringReader sr = new StringReader(meta_str);
            ret_str = sr.ReadLine();
            return ret_str;
        }

        public static int getVersion(Image img)
        {
            int ret_str = 0;
            string metaHead = getMetaHead(img);
            string[] hhArr = metaHead.Split(new char[] { ' ' }, StringSplitOptions.None);
            int.TryParse(hhArr[1], out ret_str);
            return ret_str;
        }

        public static string getMetaData(Image img)
        {
            string ret_str = string.Empty;
            PropertyItem p1 = img.GetPropertyItem(37510);
            string meta_str = MMIUNICODE.getString(p1.Value);
            StringReader sr = new StringReader(meta_str);

            sr.ReadLine();
            ret_str = sr.ReadLine();
            return ret_str;
        }

        public static string[] getMetaDataArray(Image img)
        {
            string[] ret_arr = new String[0];
            PropertyItem p1 = img.GetPropertyItem(37510);
            string meta_str = MMIUNICODE.getString(p1.Value);
            StringReader sr = new StringReader(meta_str);

            sr.ReadLine();
            string meta_dd = sr.ReadLine();
            ret_arr = meta_dd.Split(new char[] { ',' }, StringSplitOptions.None);
            return ret_arr;
        }
        #endregion image metadata
    }

    public static class MMIDATE
    {
        public static string Duration(DateTime start_date, DateTime end_date)
        {
            string ret = string.Empty;
            TimeSpan ts = end_date - start_date;
            if ((int)ts.Days != 0)
            {
                if (ts.Days != 1 || ts.Days != -1)
                {
                    ret = string.Format("{0} days {1}", ts.Days, ts.ToString(@"hh\:mm\:ss"));
                }
                else
                {
                    ret = string.Format("{0} day {1}", ts.Days, ts.ToString(@"hh\:mm\:ss"));
                }
            }
            else
            {
                if( ts.TotalSeconds < 10 )
                    if(ts.TotalSeconds<1)
                        ret = string.Format("{0}ms", ts.Milliseconds);
                    else 
                        ret = string.Format("{0}s", Math.Round(ts.TotalSeconds,3));
                else 
                    ret = string.Format("{0}", ts.ToString(@"hh\:mm\:ss"));
            }
            return ret;
        }

        public static int TimeOffset()
        {
            return (int)(DateTime.Now - DateTime.UtcNow).TotalSeconds;
        }

        public static string TimeZone(int timeoffset)
        {
            TimeSpan ts = TimeSpan.FromSeconds(timeoffset);
            if (ts.Hours > 0)
                return string.Format("+{0}:{1}", ts.Hours.ToString(), ts.Minutes.ToString().PadLeft(2, '0'));
            else
                return string.Format("-{0}:{1}", Math.Abs(ts.Hours).ToString(), ts.Minutes.ToString().PadLeft(2, '0'));
        }

        public static int Millisecond(DateTime start_date, DateTime end_date)
        {
            int ret = 0;
            TimeSpan ts = end_date - start_date;
            ret = (int)ts.TotalMilliseconds;
            return ret;
        }

        public static double toUTC(DateTime dt)
        {
            return (dt.ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))).TotalSeconds;
        }

        public static DateTime mkUTC(double utc_seconds)
        {
            DateTime ret_dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            ret_dt = ret_dt.AddSeconds(utc_seconds).ToUniversalTime();
            return ret_dt;
        }

        public static string utcFileName(DateTime dt)
        {
            return string.Format("{0}.{1}.{2}_{3}.{4}.{5}",
                                    dt.Year,
                                    dt.Month.ToString().PadLeft(2, '0'),
                                    dt.Day.ToString().PadLeft(2, '0'),
                                    dt.Hour.ToString().PadLeft(2, '0'),
                                    dt.Minute.ToString().PadLeft(2, '0'),
                                    dt.Second.ToString().PadLeft(2, '0')
                                 );
        }

        public static DateTime mktime(double utc_seconds, int timeoffset)
        {
            DateTime ret_dt = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            ret_dt = ret_dt.AddSeconds(utc_seconds + timeoffset);
            DateTime ret_dt1 = DateTime.SpecifyKind(ret_dt, DateTimeKind.Local);
            return ret_dt1;
        }

        public static string utcFileName(double utc_seconds, int timeoffset)
        {
            DateTime dt = mktime(utc_seconds, timeoffset);
            return utcFileName(dt);
        }

        /*
        DateTime dt = new DateTime(year,month, day, hour, minute, seconde,  DateTimeKind.UTC/LocalTime/UnSpecify)
        默认是  UnSpecify : dt != dt.ToLocalTime() != dt.ToUniversalTime()
        if new DateTime( ..., DateTimeKind.UnSpecify) 
        DateTime dt0 = new DateTime(2014, 2, 15, 15, 30, 30, DateTimeKind.Unspecified);
        Console.WriteLine("Date : {0} - {1} - {2}", dt0, dt0.ToLocalTime(), dt0.ToUniversalTime() );
        2014-2-15 3:30:30pm - 2014-2-15 7:30:30 AM (-8 hours)  - 2014-2-15 11:30:30pm (+8 hours)  
        此时如果  DateTime.ToLocalTime() - 它就是当着 UTC Time    转成 Local Time  ( UTC - 8)
        此时如果  DateTime.ToLocalTime() - 它就是当着 Local Time  转成 UTC Time    ( UTC + 8)
   
        DateTime.Now - LocalTime; 所以 DateTime.Now = DateTime.Now.ToLocalTime()   

        时间数值不变的转换类型
        DateTime dt0 = new DateTime(2014, 2, 15, 15, 30, 30, DateTimeKind.Local);
        dt0 = dt0.ToLocalTime()
        DateTime dt2 = DateTime.SpecifyKind(dt0, DateTimeKind.Utc);
        dt2 = dt2.ToUniversalTime()
        
        DateTime dt0 = new DateTime(2014, 2, 15, 15, 30, 30, DateTimeKind.Local);
        DateTime dt1 = dt0.ToLocalTime();
        DateTime dt11 = dt0.ToUniversalTime();
        DateTime dt2 = DateTime.SpecifyKind(dt0, DateTimeKind.Utc);
        DateTime dt3 = dt2.ToUniversalTime();
        DateTime dt33 = dt2.ToLocalTime();
         
        dt0: 2/15/2014 3:30:30 PM  - local
        dt1: 2/15/2014 3:30:30 PM  - local
        dt11: 2/15/2014 11:30:30 PM - +8hours 
        dt2: 2/15/2014 3:30:30 PM  - UTC
        dt3: 2/15/2014 3:30:30 PM  - UTC
        dt33: 2/15/2014 7:30:30 AM - -8hours
        */
    }
  
    public static class MMINET  
    {
        public static string getIPAddress()
        {
            // 获得本机局域网IP地址   
            //IPAddress[] AddressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            string ip_address = string.Empty;
            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            IPAddress[] AddressList = ipHost.AddressList;
            if (AddressList.Length > 0)
            {
                foreach (IPAddress ip in AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                        ip_address = ip.ToString();
                }
            }
            return ip_address;
        }

        public static byte[] getIPBytes()
        {
            // 获得本机局域网IP地址   
            //IPAddress[] AddressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            byte[] ip_address = new byte[4];
            IPAddress[] AddressList = Dns.GetHostEntry(Dns.GetHostName()).AddressList;
            if (AddressList.Length > 0)
            {
                foreach (IPAddress ip in AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                        ip_address = ip.GetAddressBytes();
                }
            }
            return ip_address;
        }

        public static string allowURL(string url)
        {
            string ret_str = string.Empty;
            string param_str = string.Format(@"netsh http add urlacl url={0} user=Users", url);
            ret_str = RunExternalExe("cmd", "/c " + param_str);
            return ret_str;
        }

        /* how to run commandline 
            string mss = MMNET.RunExternalExe("cmd", "/c ping 192.168.1.83");
            string mss = MMNET.RunExternalExe("cmd" , "/c notepad.exe");
            string mss = MMNET.RunExternalExe("notepad");
            string mss = MMNET.RunExternalExe("notepad", @"c:\fm\aaa.txt");
         
            string mss = MMNET.RunExternalExe("ping 192.168.1.83"); - 出错，不可以直接执行，需要先打开 cmd 程序。应该使用第一种方法。
        */
        public static string RunExternalExe(string file, string arguments = "")
        {
            string ret_str = string.Empty;

            Process ppp = new Process();

            ppp.StartInfo.FileName = file;
            ppp.StartInfo.WorkingDirectory = @"C:\Windows\System32\";
            if (!string.IsNullOrEmpty(arguments)) ppp.StartInfo.Arguments = arguments;

            ppp.StartInfo.CreateNoWindow = true;
            ppp.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            ppp.StartInfo.UseShellExecute = false;

            ppp.StartInfo.RedirectStandardError = true;
            ppp.StartInfo.RedirectStandardOutput = true;

            StringBuilder stdOut = new StringBuilder();
            ppp.OutputDataReceived += (sender, args) => stdOut.Append(args.Data);

            string stdErr = string.Empty;
            try
            {
                ppp.Start();
                ppp.BeginOutputReadLine();
                stdErr = ppp.StandardError.ReadToEnd();
                ppp.WaitForExit();

                if (ppp.ExitCode == 0)
                {
                    ret_str = stdOut.ToString();
                }
                else
                {
                    StringBuilder msg = new StringBuilder();
                    if (!string.IsNullOrEmpty(stdErr)) msg.AppendLine(stdErr);
                    if (stdOut.Length > 0) msg.AppendLine(stdOut.ToString());
                    ret_str = string.Format("Execute Command '{0} {1}' Finished With Error:{2}\nMessage{3}", file, arguments, ppp.ExitCode, msg.ToString());
                }
            }
            catch (Exception err)
            {
                ret_str = string.Format("Execute Command '{0} {1}' Error:{2}\nMessage{3}", file, arguments, err.Message, err.StackTrace);
            }
            return ret_str;
        }
    }

    public static class MMIUNICODE
    {
        public static bool isASCII(byte[] arr_byte)
        {
            bool ret_flag = true;
            if (arr_byte.Length > 0)
            {
                string ascii_str = UnicodeEncoding.ASCII.GetString(arr_byte);
                string unicode_str = UnicodeEncoding.Unicode.GetString(arr_byte);
                int uc = (int)unicode_str[0];
                int ac = (int)ascii_str[0];
                if (uc == ac) ret_flag = false; else ret_flag = true;
            }
            return ret_flag;
        }

        public static string getString(byte[] arr_byte)
        {
            string ret_str = string.Empty;
            if (isASCII(arr_byte))
            {
                ret_str = UnicodeEncoding.ASCII.GetString(arr_byte);
            }
            else
            {
                ret_str = UnicodeEncoding.Unicode.GetString(arr_byte);
            }
            return ret_str;
        }

        public static byte[] asciiBytes(string str)
        {
            return UnicodeEncoding.ASCII.GetBytes(str);
        }

        public static byte[] unicodeBytes(string str)
        {
            return UnicodeEncoding.Unicode.GetBytes(str);
        }
    }

    public static class MMISTRING
    {
        public static string[] slice(string[] sourceArray, int start, int length)
        {
            start = start < 0 ? 0 : start;
            length = length < 0 ? 0 : length;
            string[] ret = new string[length];
            if (start < sourceArray.Length)
            {
                if (start + length <= sourceArray.Length)
                {
                    Array.Copy(sourceArray, start, ret, 0, length);
                }
                else
                {
                    Array.Copy(sourceArray, start, ret, 0, sourceArray.Length - start);
                }
            }
            return ret;
        }

        public static int[] toInt(string[] sourceArray)
        {
            int[] ret = new int[sourceArray.Length];
            for (int i = 0; i < sourceArray.Length; i++)
            {
                int.TryParse(sourceArray[i], out ret[i]);
            }
            return ret;
        }

        public static double[] toDouble(string[] sourceArray)
        {
            double[] ret = new double[sourceArray.Length];
            for (int i = 0; i < sourceArray.Length; i++)
            {
                double.TryParse(sourceArray[i], out ret[i]);
            }
            return ret;
        }

        public static string[] toString(string[] sourceArray)
        {
            string[] ret = new string[sourceArray.Length];
            for (int i = 0; i < sourceArray.Length; i++)
            {
                if (string.IsNullOrEmpty(sourceArray[i]))
                    ret[i] = string.Empty;
                else
                    ret[i] = sourceArray[i];
            }
            return ret;
        }

        public static string getString(string[] sourceArray, int pos)
        {
            string ret = string.Empty;
            if (pos >= 0 && pos < sourceArray.Length)
            {
                ret = sourceArray[pos];
            }
            return ret;
        }

        public static int getInt(string[] sourceArray, int pos)
        {
            int ret = 0;
            if (pos >= 0 && pos < sourceArray.Length)
            {
                int.TryParse(sourceArray[pos], out ret);
            }
            return ret;
        }

        public static double getDouble(string[] sourceArray, int pos)
        {
            double ret = 0;
            if (pos >= 0 && pos < sourceArray.Length)
            {
                double.TryParse(sourceArray[pos], out ret);
            }
            return ret;
        }

        public static float getFloat(string[] sourceArray, int pos)
        {
            float ret = 0;
            if (pos >= 0 && pos < sourceArray.Length)
            {
                float.TryParse(sourceArray[pos], out ret);
            }
            return ret;
        }

        public static string getCSVcontent(string ofile)
        {
            string ret_str = string.Empty;
            if (File.Exists(ofile))
            {
                using (StreamReader sr = File.OpenText(ofile))
                {
                    string str = sr.ReadToEnd();
                    sr.Close();
                    string[] strs = str.Split(new string[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries);

                    var qq = from el in strs
                             select el.Split(new string[] { "," }, StringSplitOptions.None);

                    StringBuilder sb = new StringBuilder();
                    bool first_row = true;
                    foreach (string[] q in qq)
                    {
                        if (first_row)
                        {
                            sb.AppendLine(string.Join(",", q));
                            first_row = false;
                            continue;
                        }

                        sb.AppendLine(string.Join(",", q));
                    }
                    ret_str = sb.ToString();
                }
            }
            return ret_str;
        }

        public static void StringToFile(string content, string full_name)
        {
           
            using(StreamWriter sw = new StreamWriter(full_name)) {
                sw.Write(content);
                sw.Flush();
                sw.Close();
                sw.Dispose();
            }
           
        }
    }

    public class MMISQL
    {
        private SqlConnection _scon;
        private SqlCommand _scom;

        public string Server;
        public string Database;
        public string User;
        public string Password;
        public bool trusted;

        public MMISQL()
        {
        }

        public MMISQL(string server, string database, string user, string password, bool trusted = false)
            : this()
        {
            this.Server = server;
            this.Database = database;
            this.User = user;
            this.Password = password;
            this.trusted = trusted;

            this._scon = new SqlConnection(this.ConnectString);
            this._scom = new SqlCommand();
            this._scom.Connection = this._scon;
        }

        #region SQL Connection
        public string ConnectString
        {
            get
            {
                return this.trusted ? string.Format(@"Server={0};Database={1};Uid={2};Pwd={3};Connection Timeout=5;Trusted_Connection=Yes;Integrated Security=True;Persist Security Info=True;", this.Server, this.Database, this.User, this.Password) :
                                      string.Format(@"Server={0};Database={1};Uid={2};Pwd={3};Connection Timeout=5", this.Server, this.Database, this.User, this.Password);
            }
        }

        public string EntityString(string ef_model_name = "")
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
            sqlBuilder.DataSource = this.Server;
            sqlBuilder.InitialCatalog = this.Database;
            sqlBuilder.PersistSecurityInfo = true;
            sqlBuilder.IntegratedSecurity = this.trusted;
            sqlBuilder.MultipleActiveResultSets = true;
            sqlBuilder.UserID = this.User;
            sqlBuilder.Password = this.Password;
            //sqlBuilder.TrustServerCertificate = false;

            EntityConnectionStringBuilder entityBuilder = new EntityConnectionStringBuilder();
            entityBuilder.ProviderConnectionString = sqlBuilder.ToString();
            entityBuilder.Metadata = string.IsNullOrEmpty(ef_model_name) ? "res://*/" : string.Format("res://*/{0}.csdl|res://*/{0}.ssdl|res://*/{0}.msl", ef_model_name);
            entityBuilder.Provider = "System.Data.SqlClient";
          
            return entityBuilder.ToString();
        }

        public SqlConnection Connection
        {
            get
            {
                return this._scon;
            }
            set
            {
                this._scon = value;
            }
        }

        public void Open()
        {
            this.Close();
            this._scon.ConnectionString = this.ConnectString;
            this._scon.Open();
        }

        public void Close()
        {
            if (this._scon.State == System.Data.ConnectionState.Open)
            {
                this._scon.Close();
            }
        }
        #endregion SQL Connection

        #region  Database Operation
        public bool dbExists(string databaseName)
        {
            if (this._scon.State == System.Data.ConnectionState.Closed)
            {
                this.Open();
            }

            String check_db_str = string.Format("SELECT COUNT(*) FROM master.dbo.sysdatabases WHERE name = '{0}'", databaseName);
            this._scom.CommandText = check_db_str;

            int db_exists = (int)_scom.ExecuteScalar();
            if (db_exists > 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public bool dbChange(string databaseName)
        {
            if (this.dbExists(databaseName))
            {
                this.Close();
                this.Database = databaseName;
                this.Open();
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool objExists(string obj)
        {
            if (this._scon.State != System.Data.ConnectionState.Open)
            {
                this.Open();
            }
            string check_tb_str = string.Format(@"SELECT COUNT(*) FROM dbo.sysobjects WHERE id = object_id(N'[dbo].[{0}]')", obj);
            this._scom.CommandText = check_tb_str;
            int tb_exists = (int)this._scom.ExecuteScalar();
            if (tb_exists > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public int Query(string sql)
        {
            if (this._scon.State != System.Data.ConnectionState.Open)
            {
                this.Open();
            }
            this._scom.CommandText = sql;
            this._scom.CommandType = CommandType.Text;
            int sqlResult = this._scom.ExecuteNonQuery();
            return sqlResult;
        }
        #endregion  Database Operation
    }

    public class MMICONFIG
    {
        private Configuration Config;

        //AppDomain.CurrentDomain.BaseDirectory  = Only the directory
        //Application.ExecutablePath - for web application this will point to 
        // c:\windows\system32\inetsrv\w3wp.exe   IIS executable file path
        public MMICONFIG()
        {
            if (File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Web.config")))
            {
                this.Config = WebConfigurationManager.OpenWebConfiguration("~");
            }
            else
            {
                this.Config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            }
        }


        public string this[string key]
        {
            get
            {
                string ret_val = string.Empty;
                if (exists(key)) ret_val = this.Config.AppSettings.Settings[key].Value;
                return ret_val;
            }
            set
            {
                if (exists(key))
                {
                    this.Config.AppSettings.Settings[key].Value = value;
                }
                else
                {
                    this.Config.AppSettings.Settings.Add(key, value);
                }

                this.Config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }
        }

        #region get value of type
        public string getString(string key)
        {
            string ret_val = string.Empty;
            try
            {
                ret_val = this[key];
                return ret_val;
            }
            catch
            {
                return ret_val;
            }
        }

        public bool getBool(string key)
        {
            bool ret_val = false;
            try
            {
                ret_val = Convert.ToBoolean(this[key]);
                return ret_val;
            }
            catch
            {
                try
                {
                    return Convert.ToInt32(this[key]) > 0 ? true : false;
                }
                catch
                {
                    return ret_val;
                }
            }
        }

        public int getInt(string key)
        {
            int ret_val = 0;
            try
            {
                ret_val = (int)Convert.ToDouble(this[key]);
                return ret_val;
            }
            catch
            {
                return ret_val;
            }
        }

        public double getDouble(string key)
        {
            double ret_val = 0d;
            try
            {
                ret_val = Convert.ToDouble(this[key]);
                return ret_val;
            }
            catch
            {
                return ret_val;
            }
        }

        public DateTime getDate(string key)
        {
            DateTime ret_val = DateTime.MinValue;
            try
            {
                ret_val = Convert.ToDateTime(this[key]);
                return ret_val;
            }
            catch
            {
                return ret_val;
            }
        }

        public bool exists(string key)
        {
            return this.Config.AppSettings.Settings[key] == null ? false : true;
        }
        #endregion get value of type

        public bool copyTo(string file_path)
        {
            bool ret = false;
            try
            {
                if (File.Exists(file_path))
                {
                    XDocument edoc = new XDocument();
                    edoc = XDocument.Load(file_path);
                    IEnumerable<XElement> qq = from el in edoc.Element("configuration").Element("appSettings").Elements("add")
                                               select el;
                    foreach (XElement q in qq)
                    {
                        if (this[q.Attribute("key").Value.ToString()] != string.Empty)
                            q.Attribute("value").Value = this[q.Attribute("key").Value.ToString()];
                    }
                    edoc.Save(file_path);
                }
                ret = true;
            }
            catch { }
            return ret;
        }
    }

    public static class SINGLETON
    {
        [DllImport("User32.dll")]
        public static extern bool ShowWindowAsync(IntPtr hWnd, SW cmdShow);
        [DllImport("User32.dll")]
        public static extern bool ShowWindow(IntPtr hWnd, SW cmdShow);
        [DllImport("User32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("User32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);

        public static bool SingleCheck()
        {
            bool single_flag = true;
            Process curProcess = Process.GetCurrentProcess();
            int cur_pid = curProcess.Id;
            string cur_guid = getGUID(curProcess.MainModule.FileName);



            // if you change assembly name, it will treat as other assembly. 
            // if you want to check all list, just Process.GetProcessesByName()
            foreach (Process pel in Process.GetProcessesByName(curProcess.ProcessName))
            {
                if (pel.Id != cur_pid)
                {
                    try
                    {
                        string pel_guid = getGUID(pel.MainModule.FileName);
                        if (cur_guid != string.Empty && pel_guid != string.Empty && cur_guid == pel_guid)
                            single_flag = false;
                    }
                    catch
                    {
                        //throw new Exception(err.Message);
                        continue;
                    }
                }
            }
            return single_flag;
        }

        public static void SingleRunning()
        {
            Process curProcess = Process.GetCurrentProcess();
            int cur_pid = curProcess.Id;
            string cur_guid = getGUID(curProcess.MainModule.FileName);

            // if you change assembly name, it will treat as other assembly. 
            // if you want to check in all list, just Process.GetProcessesByName()
            foreach (Process pel in Process.GetProcessesByName(curProcess.ProcessName))
            {
                if (pel.Id != cur_pid)
                {
                    try
                    {
                        string pel_guid = getGUID(pel.MainModule.FileName);
                        if (cur_guid != string.Empty && pel_guid != string.Empty && cur_guid == pel_guid)
                        {
                            if (IsWindowVisible(pel.MainWindowHandle))
                            {
                                ShowWindow(pel.MainWindowHandle, SW.SW_SHOWNORMAL);
                                SetForegroundWindow(pel.MainWindowHandle);
                                Environment.Exit(Environment.ExitCode);
                            }
                            else
                            {

                                ShowWindow(pel.MainWindowHandle, SW.SW_SHOWNORMAL);
                                SetForegroundWindow(pel.MainWindowHandle);
                                Environment.Exit(Environment.ExitCode);
                            }
                            break;
                        }
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }

        public static string getGUID()
        {
            /* other identity
            // processName is not reliable if you change the assembly name "abc.exe" to "abcd.exe"
            string process_name = curProcess.ProcessName;
            
            Assembly assembly = Assembly.LoadFile(curProcess.MainModule.FileName);
            string guid1 = assembly.ManifestModule.ModuleVersionId.ToString();  // 只要每次编译（即使什么也没改），该值都会变
            */

            string guid = string.Empty;
            try
            {
                Attribute guid_attr = Attribute.GetCustomAttribute(Assembly.GetExecutingAssembly(), typeof(GuidAttribute));
                guid = ((GuidAttribute)guid_attr).Value;   // GUID， 重新编译过，或者版本号变了， 该值都不变
            }
            catch (Exception err)
            {
                throw new Exception(err.Message);
            }
            return guid;
        }

        public static string getGUID(string assembly_fileName)
        {
            string guid = string.Empty;
            try
            {
                Assembly assem = getAssembly(assembly_fileName);
                if (assem != null)
                {
                    Attribute guid_attr = Attribute.GetCustomAttribute(assem, typeof(GuidAttribute));
                    guid = ((GuidAttribute)guid_attr).Value;
                }
            }
            catch
            {
                //throw new Exception(err.Message);
            }
            return guid;
        }

        public static Assembly getAssembly(string fileName)
        {
            try
            {
                return Assembly.LoadFrom(fileName);
            }
            catch
            {
                return null;
            }
        }

        public enum SW
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_MAX = 10
        }
    }
}
