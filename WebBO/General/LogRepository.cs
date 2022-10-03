using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebBO.General
{
    public class LogRepository
    {
        /// <summary>
        /// 紀錄使用者動作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        public static void CreateActionLog<T>(string actionName, T instance)
        {
            var json = JsonConvert.SerializeObject(instance);

            using (NT_PlantTreeEntities _context = new NT_PlantTreeEntities())
            {
                try
                {
                    //var blog = new LogSystemError
                    //{
                    //    ProjectId = 1,
                    //    Application = route,
                    //    SQLJson = json,
                    //    Message = e.Message,
                    //    Source = e.Source,
                    //    StackTrace = e.StackTrace,
                    //    TargetSite = "",
                    //    DateTime = DateTime.Now
                    //};
                    //_context.LogSystemError.Add(blog);
                    //_context.SaveChanges();

                }
                catch (DbUpdateException)
                {
                    throw;
                }
            }
        }

        public static void CreateErrorLog(Exception e)
        {
            //using (NT_PlantTreeEntities _context = new NT_PlantTreeEntities())
            //{
            //    try
            //    {
            //        var blog = new LogSystemError
            //        {
            //            ProjectId = 1,
            //            Application = "",
            //            Message = e.InnerException.Message,
            //            Source = e.Source,
            //            StackTrace = e.InnerException.StackTrace,
            //            TargetSite = "",
            //            DateTime = DateTime.Now
            //        };
            //        _context.LogSystemError.Add(blog);
            //        _context.SaveChanges();

            //    }
            //    catch (DbUpdateException ex)
            //    {
            //        throw;
            //    }
            //}
        }
    }
}
