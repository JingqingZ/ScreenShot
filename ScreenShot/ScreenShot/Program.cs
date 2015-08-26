using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace ScreenShot
{
    class ScreenShot
    {
        public static void deleteOldShot(int weeks, string destDir, StreamWriter outfile)
        {
            DirectoryInfo info = new DirectoryInfo(destDir);
            FileInfo[] fileInfos = info.GetFiles().OrderBy(p => p.CreationTime).ToArray();
            foreach (FileInfo fileInfo in fileInfos)
            {
                string imagePath = fileInfo.Name.Replace(destDir + "\\", "").Replace(".png", "");
                try
                {
                    DateTime datePath = DateTime.ParseExact(imagePath, "yyyy-MM-dd-HH-mm-ss",
                        CultureInfo.InvariantCulture);
                    if ((DateTime.Now - datePath).Days > weeks*7)
                    {
                        File.Delete(destDir + "\\" + fileInfo.Name);
                        Console.WriteLine(destDir + "\\" + fileInfo.Name + " deleted!");
                        outfile.WriteLine(destDir + "\\" + fileInfo.Name + " deleted!");
                    }
                    else break;
                }
                catch (Exception e)
                {
                    continue;
                }
            }
            return;
        }

        // capture screens every X seconds
        // delete images older than Y weeks automatically
        // save images in the destination directory
        public static void CaptureScreen(int seconds, int weeks, string destDir)
        {
            // create destination folders and log.txt
            String imageDir = destDir + "\\images";
            Directory.CreateDirectory(imageDir);
            StreamWriter outfile = new StreamWriter(destDir + "\\log.txt", true);
            // delete images older than X weeks
            deleteOldShot(weeks, imageDir, outfile);

            DateTime currentDate = DateTime.Now;  
            while (true)
            {
                try
                {
                    // delete old images everyday
                    if ((DateTime.Now - currentDate).Days >= 1)
                    {
                        currentDate = DateTime.Now;
                        deleteOldShot(weeks, imageDir, outfile);
                    }
                    // capture screen
                    Rectangle bounds = Screen.GetBounds(Point.Empty);
                    Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);
                    Graphics g = Graphics.FromImage(bitmap);
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                    string name = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
                    bitmap.Save(imageDir + "\\" + name + ".png", ImageFormat.Png);
                    Console.WriteLine(imageDir + "\\" + name + ".png saved.");
                    outfile.WriteLine(imageDir + "\\" + name + ".png saved.");
                    outfile.Flush();
                    
                    Thread.Sleep(seconds*1000);
                }
                catch (Exception e)
                {
                    // exception handlers
                    outfile.WriteLine(e.ToString());
                    outfile.Flush();
                    if (e.GetType() == new ExternalException().GetType() ||
                        e.GetType() == new IOException().GetType() ||
                        e.GetType() == new EncoderFallbackException().GetType())
                    {
                        break;
                    }
                }
            }
            outfile.Close();
        }
        static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("Usage: ScreenShot seconds weeks destDir");
                Console.WriteLine("seconds: int, capture screen each X seconds");
                Console.WriteLine("weeks: int, delete images older than X weeks");
                Console.WriteLine("destDir: string, save images to this folder");
                return;
            }
            while (true)
            {
                try
                {
                    ScreenShot.CaptureScreen(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), args[2]);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    Thread.Sleep(120 * 1000);
                }
            }
        }
    }
}
