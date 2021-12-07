using System;
using System.IO;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace test2
{
    public class CalcCheckSum
    {
        public static byte[] OneThreadCalculation(string path)
        {
           if (Directory.Exists(path) | File.Exists(path))
           {
               using var md5 = MD5.Create();
               if (Directory.Exists(path))
               {
                   var arrayOfFiles = Directory.GetFiles(path).OrderBy(file => file).ToArray();
                   var res = new StringBuilder();
                   res.Append(Path.GetFileName(Path.GetDirectoryName(path)));
                   foreach (var file in arrayOfFiles)
                   {
                       res.Append(Encoding.ASCII.GetString(OneThreadCalculation(file)));
                   }
                   return md5.ComputeHash(Encoding.ASCII.GetBytes(res.ToString()));
               }
               else
               {
                   var bytes = File.ReadAllBytes(path);
                   return md5.ComputeHash(bytes);
               }    
           }
           throw new FileNotFoundException("Directory doesn't exist");
           
        }
        
        public static async Task<byte[]> MultThreadCalculation(string path)
        {
            if (Directory.Exists(path) | File.Exists(path))
            {
                using var md5 = MD5.Create();
                if (Directory.Exists(path))
                {
                    var arrayOfFiles = Directory.GetFiles(path).OrderBy(file => file).ToArray();
                    var res = new StringBuilder();
                    res.Append(Path.GetFileName(Path.GetDirectoryName(path)));
                    foreach (var file in arrayOfFiles)
                    {
                        res.Append(Encoding.ASCII.GetString(OneThreadCalculation(file)));
                    }
                    return md5.ComputeHash(Encoding.ASCII.GetBytes(res.ToString()));
                }
                else
                {
                    var bytes = await File.ReadAllBytesAsync(path);
                    return md5.ComputeHash(bytes);
                }    
            }
            throw new FileNotFoundException("Directory doesn't exist");
            
        }
        
        public static void Comparison(string path)
        {
            var stopWatchOneThread = new Stopwatch();
            stopWatchOneThread.Start();
            OneThreadCalculation(path);
            stopWatchOneThread.Stop();

            var stopWatchMultThread = new Stopwatch();
            stopWatchMultThread.Start();
            var task = MultThreadCalculation(path);
            task.Wait();
            stopWatchMultThread.Stop();
            var time1 = stopWatchOneThread.ElapsedMilliseconds;
            var time2 = stopWatchMultThread.ElapsedMilliseconds;
            if (time2 > time1)
            {
                Console.WriteLine($"Time of single-threaded calculation faster than mult-threaded: {time1}");
            }
            if (time1 > time2)
            {
                Console.WriteLine($"Result of single-threaded calculation faster than mult-threaded : {time2}");
            }

        }
    }
}