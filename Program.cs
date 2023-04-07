using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace KFin.Mara.ImageRemover
{
    class Program
    {
        static void Main(string[] args)
        {

            //string WebImageFolder  = ConfigurationManager.AppSettings["WebImageFolder"].ToString();
            //Int32 PrevDays = Convert.ToInt32(ConfigurationManager.AppSettings["PrevDays"].ToString());

            //DirectoryInfo objDirectoryInfo = new DirectoryInfo(WebImageFolder);

            //FileInfo[] fileInfo = objDirectoryInfo.GetFiles().Where(x => x.CreationTime < DateTime.Now.AddDays(PrevDays)).ToArray();

            ImageRemover objImageRemover = new ImageRemover();
            //foreach (var fname in fileInfo)
            //{
            //    if (objImageRemover.CheckForPlazaImage(fname.Name))
            //    {
            //        objImageRemover.DeleteFile(fname.FullName);
            //    }
            //}

            DataTable dt = objImageRemover.GetFileData();

            foreach (DataRow dr in dt.Rows)
            {
                string ImageDemoPath = dr["ImageDemoPath"].ToString();
                string PlazaPath = dr["PlazaPath"].ToString();

                if (objImageRemover.isImageAvailable(ImageDemoPath))
                {
                    if (objImageRemover.isImageAvailable(PlazaPath))
                    {
                        objImageRemover.DeleteFile(ImageDemoPath);
                    }
                }
            }

        }
    }
    class ImageRemover
    {
        public DataTable GetFileData()
        {
            try
            {
                DataSet ds;
                var _connString = ConfigurationManager.ConnectionStrings["ConStr"].ToString();
                using (SqlConnection _conn = new SqlConnection(_connString))
                {
                    SqlCommand _cmd = new SqlCommand();
                    _cmd.Connection = _conn;
                    _cmd.CommandType = CommandType.StoredProcedure;
                    _cmd.CommandText = "usp_GetPathForImgDemoRemove";

                    SqlDataAdapter da = new SqlDataAdapter(_cmd); 

                    ds=new DataSet();
                    da.Fill(ds);
                }
                return ds.Tables[0];
            }
            catch (Exception)
            {
                
                throw;
            }

        }
        public bool DeleteFile(string ImagePath)
        {
            try
            {
                System.IO.File.Delete(ImagePath);
                return true;
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        public bool isImageAvailable(string ImagePath)
        {
            try
            {

                if (System.IO.File.Exists(ImagePath))
                    return true;
                else
                    return false;

            }
            catch (Exception)
            {
                
                throw;
            }
        }
      

    }
}
