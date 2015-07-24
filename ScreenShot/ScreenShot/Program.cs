using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
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
        public static void CaptureScreen(int seconds, int limit, string destDir)
        {
            int id = 0;
            Directory.CreateDirectory(destDir);
            String[] filePaths = Directory.GetFiles(destDir);
            foreach (String filePath in filePaths)
                File.Delete(filePath);
            StreamWriter outfile = new StreamWriter(destDir + "\\log.txt", false);
            while (id < limit || limit == -1)
            {
                try
                {
                    Rectangle bounds = Screen.GetBounds(Point.Empty);
                    Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height);
                    Graphics g = Graphics.FromImage(bitmap);
                    g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                    string name = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
                    bitmap.Save(destDir + "\\" + name + ".png", ImageFormat.Png);
                    Console.WriteLine(destDir + "\\" + name + ".png saved.");
                    outfile.WriteLine(destDir + "\\" + name + ".png saved.");
                    outfile.Flush();
                    id++;
                    Thread.Sleep(seconds*1000);
                }
                catch (Exception e)
                {
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
                Console.WriteLine("Usage: ScreenShot seconds limit destDir");
                Console.WriteLine("seconds: int");
                Console.WriteLine("limit: int");
                Console.WriteLine("destDir: valid path");
                return;
            }
            try
            {
                ScreenShot.CaptureScreen(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), args[2]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                Console.WriteLine("Usage: ScreenShot seconds limit destDir");
                Console.WriteLine("seconds: int");
                Console.WriteLine("limit: int");
                Console.WriteLine("destDir: valid path");
            }
        }
    }
}
