using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RFIDBLL.HelperClasses
{
    public class ClientDateService
    {
        public static int h = 0; //onvert.ToInt32(ConfigurationManager.AppSettings["TimeDiff.Hours"]);
        public static int m = 0; //onvert.ToInt32(ConfigurationManager.AppSettings["TimeDiff.Minutes"]);
        public static int s = 0; //Convert.ToInt32(ConfigurationManager.AppSettings["TimeDiff.Seconds"]);
        public static DateTime ClientDateTime()
        {

            TimeSpan span;
            span = new TimeSpan(h, m, s);//(hh,mm,ss)
            DateTime SystemDate = DateTime.Now.Add(span);
            return SystemDate;
        }
        public static string GetYear()
        {
            TimeSpan span;
            span = new TimeSpan(h, m, s);//(hh,mm,ss)
            string SystemDate = DateTime.Now.Add(span).Year.ToString();
            return SystemDate;
        }
        public static string GetMonth()
        {
            TimeSpan span;
            span = new TimeSpan(h, m, s);//(hh,mm,ss)
            string SystemDate = DateTime.Now.Add(span).Month.ToString();
            return SystemDate;
        }
        public static string GetDay()
        {
            TimeSpan span;
            span = new TimeSpan(h, m, s);//(hh,mm,ss)
            string SystemDate = DateTime.Now.Add(span).Day.ToString();
            return SystemDate;
        }

        public static string HostingYear()
        {
            TimeSpan span;
            span = new TimeSpan(0, 0, 0);//(hh,mm,ss)
            string SystemDate = DateTime.Now.Add(span).Year.ToString();
            return SystemDate;
        }
        public static string HostingMonth()
        {
            TimeSpan span;
            span = new TimeSpan(0, 0, 0);//(hh,mm,ss)
            string SystemDate = DateTime.Now.Add(span).Month.ToString();
            return SystemDate;
        }
        public static string HostingDay()
        {
            TimeSpan span;
            span = new TimeSpan(0, 0, 0);//(hh,mm,ss)
            string SystemDate = DateTime.Now.Add(span).Day.ToString();
            return SystemDate;
        }
        public static string HostingTime()
        {
            TimeSpan span;
            span = new TimeSpan(0, 0, 0);//(hh,mm,ss)
            string SystemDate = DateTime.Now.Add(span).ToLongTimeString();
            return SystemDate;
        }

        public static TimeSpan HostingTime7()
        {
            TimeSpan span;
            span = new TimeSpan(0, 0, 0);//(hh,mm,ss)
            TimeSpan SystemDate = DateTime.Now.TimeOfDay;
            return SystemDate;
        }
        public static bool CheckDirectory(string path)
        {

            // Specify the directories you want to manipulate.

            DirectoryInfo di = new DirectoryInfo(@path);
            try
            {
                // Determine whether the directory exists.
                if (di.Exists)
                {
                    // Indicate that the directory already exists.
                    // Response.Write("That path exists already.");
                    return true;
                }
                else
                {

                    // Try to create the directory.
                    di.Create();
                    // Response.Write("The directory was created successfully.");
                }

            }
            catch (Exception e)
            {
                //Response.Write("The process failed: {0}", e.ToString());
            }
            finally { }
            return true;
        }
        public static string FileRename(string fName, string fExtention)
        {
            string newFileName = fName + "_" + DateTime.Now.Second + "." + fExtention;
            return newFileName;
        }
        public static string FileRenameWithOutExtenision(string fName)
        {
            string[] nameWithExt = fName.Split('.');
            string newFileName = nameWithExt[0] + "_" + DateTime.Now.Second + "." + nameWithExt[1];
            return newFileName;
        }
        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }
    }
}
