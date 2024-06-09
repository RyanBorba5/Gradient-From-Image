using System.Collections.Generic;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
namespace SortingPixelsByColor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Please provide the image you'd like to sort the pixels from. Example: C:\\Users\\Hungr\\Desktop\\pixelmessing\\Bunny.png");
            String imagepath = Console.ReadLine();
            SortingPixels(imagepath);
        }

        public static void SortingPixels(String imagepath)
        {
            string tempFilePath = Path.Combine(Path.GetDirectoryName(imagepath), "Temporary_File.png");
            Bitmap mybitmap;
            using (Image imagetorandomize = Image.FromFile(imagepath))
            {
                mybitmap = new Bitmap(imagetorandomize);
            }

            List<Color> pixels = new List<Color>();
            List<Color> finalpixels = new List<Color>();

            Console.WriteLine("Collecting pixels...");
            // Collect all pixel colors
            for (int y = 0; y < mybitmap.Height; y++)
            {
                for (int x = 0; x < mybitmap.Width; x++)
                {
                    pixels.Add(mybitmap.GetPixel(x, y));
                }
                Console.WriteLine(y + "/" + mybitmap.Height);
            }

            Console.WriteLine("Sorting pixels...");
            // Sorting the list of pixels
            List<double> pixelstrengths = new List<double>();
            for (int i = 0; i < pixels.Count; i++)
            {
                double pixelsum = ((pixels[i].R*1.00) + (pixels[i].B*1.00) + pixels[i].G*1.00); // For customizing color disparities (big word fuh wut)
                pixelstrengths.Add(pixelsum);
            }

            //Console.WriteLine("FPS: " + finalpixels.Count);
            for (int j = 0; j < (mybitmap.Height * mybitmap.Width); j++)
            {
                if (pixelstrengths.Count > 0)
                {
                    int lowestindex = pixelstrengths.IndexOf(pixelstrengths.Max());
                    //Console.WriteLine(lowestindex);
                    finalpixels.Add(pixels[lowestindex]);
                    //Console.WriteLine(pixelstrengths.Count + "/" + (mybitmap.Height * mybitmap.Width));
                    pixels.RemoveAt(lowestindex);
                    pixelstrengths.RemoveAt(lowestindex);
                    Console.WriteLine(j + "/" + (mybitmap.Height * mybitmap.Width));
                }
            }

            Console.WriteLine("Adding sorted pixels and finishing up...");
            // Apply sorted pixels back to the bitmap
            int k = 0;
            //Console.WriteLine("{0} pixels in finalpixels. {1} pixels in image.",finalpixels.Count,(mybitmap.Height * mybitmap.Width));
            //Console.ReadLine();
            for (int y = 0; y < mybitmap.Height; y++)
            {
                for (int x = 0; x < mybitmap.Width; x++)
                {
                    mybitmap.SetPixel(x, y, finalpixels[k]);
                    k++;
                }
                Console.WriteLine(y + "/" + mybitmap.Height);
            }

            // Save the edited image to a temporary file
            mybitmap.Save(tempFilePath);
            mybitmap.Dispose();

            // Ensure all resources are released before replacing the file
            GC.Collect();
            GC.WaitForPendingFinalizers();

            Console.WriteLine("Replacing Original Image...");
            // Replace the original image with the temporary file
            File.Delete(imagepath);
            File.Move(tempFilePath, imagepath);

            Console.WriteLine("Image replaced successfully.");
        }
    }
}
