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

            while (id < limit)
            {
                Rectangle bounds = Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                    }
                    bitmap.Save(destDir + "\\" + id + ".png", ImageFormat.Png);
                    id ++;
                    Thread.Sleep(seconds * 1000);
                }

            } 
        }
        static void Main(string[] args)
        {
            //Console.WriteLine(args[1]);
            ScreenShot.CaptureScreen(Convert.ToInt32(args[0]), Convert.ToInt32(args[1]), args[2]);
        }
    }
}
