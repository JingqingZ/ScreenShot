using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
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
            try
            {
                while (id < limit || limit == -1)
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
                    Thread.Sleep(seconds * 1000);
                } 
            }
            catch (Exception e)
            {
                outfile.WriteLine(e.ToString());
                outfile.Flush();
            }
            outfile.Close();
          
        }
        static void Main(string[] args)
        {
            //Console.WriteLine(args[1]);
            ScreenShot.CaptureScreen(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), args[2]);
        }
    }
}
