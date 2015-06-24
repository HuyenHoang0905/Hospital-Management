using System;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace DevComponents.DotNetBar
{
#if FRAMEWORK20
    internal static class ImageHelper
#else
	internal class ImageHelper
#endif
    {
        public static Bitmap CreateReflectionImage(Image inputImage)
        {
            if (inputImage == null) return null;
            Bitmap tmp = new Bitmap(inputImage.Width, inputImage.Height, PixelFormat.Format32bppArgb);
            using (Graphics g = Graphics.FromImage(tmp))
                g.DrawImage(inputImage, 0, 0, inputImage.Width, inputImage.Height);
            Bitmap image = tmp;
            image.RotateFlip(RotateFlipType.Rotate180FlipX);

            Bitmap returnMap = new Bitmap(image.Width, image.Height,
                                   PixelFormat.Format32bppArgb);
            BitmapData bitmapData1 = image.LockBits(new Rectangle(0, 0,
                                     image.Width, image.Height),
                                     ImageLockMode.ReadOnly,
                                     PixelFormat.Format32bppArgb);
            BitmapData bitmapData2 = returnMap.LockBits(new Rectangle(0, 0,
                                     returnMap.Width, returnMap.Height),
                                     ImageLockMode.ReadOnly,
                                     PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* imagePointer1 = (byte*)bitmapData1.Scan0;
                byte* imagePointer2 = (byte*)bitmapData2.Scan0;
                int strideOffset = bitmapData1.Stride - (bitmapData1.Width * 4);
                int height = bitmapData1.Height, width = bitmapData1.Width;
                for (int i = 0; i < height; i++)
                {
                    int alpha = Math.Max(195, (int)(265 * ((i + bitmapData1.Height * .4) / (float)bitmapData1.Height)));
                    for (int j = 0; j < width; j++)
                    {
                        imagePointer2[0] = imagePointer1[0];
                        imagePointer2[1] = imagePointer1[1];
                        imagePointer2[2] = imagePointer1[2];
                        imagePointer2[3] = (byte)(Math.Max(0, imagePointer1[3] - alpha));
                        //4 bytes per pixel
                        imagePointer1 += 4;
                        imagePointer2 += 4;
                    }
                    //4 bytes per pixel
                    imagePointer1 += strideOffset;
                    imagePointer2 += strideOffset;
                }
            }
            returnMap.UnlockBits(bitmapData2);
            image.UnlockBits(bitmapData1);
            return returnMap;
        }

        public static Bitmap CreateGrayScaleImage(Image inputImage)
        {
            if (inputImage == null) return null;
            Bitmap image = inputImage as Bitmap;
            if (inputImage.PixelFormat != PixelFormat.Format32bppArgb || image == null)
            {
                Bitmap tmp = new Bitmap(inputImage.Width, inputImage.Height, PixelFormat.Format32bppArgb);
                using (Graphics g = Graphics.FromImage(tmp))
                    g.DrawImage(inputImage, 0, 0, inputImage.Width, inputImage.Height);
                image = tmp;
            }

            Bitmap returnMap = new Bitmap(image.Width, image.Height,
                                   PixelFormat.Format32bppArgb);
            BitmapData bitmapData1 = image.LockBits(new Rectangle(0, 0,
                                     image.Width, image.Height),
                                     ImageLockMode.ReadOnly,
                                     PixelFormat.Format32bppArgb);
            BitmapData bitmapData2 = returnMap.LockBits(new Rectangle(0, 0,
                                     returnMap.Width, returnMap.Height),
                                     ImageLockMode.ReadOnly,
                                     PixelFormat.Format32bppArgb);
            unsafe
            {
                byte* imagePointer1 = (byte*)bitmapData1.Scan0;
                byte* imagePointer2 = (byte*)bitmapData2.Scan0;
                int strideOffset = bitmapData1.Stride - (bitmapData1.Width * 4);
                int height = bitmapData1.Height, width = bitmapData1.Width;
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        byte nB = (byte)((imagePointer1[2] + 255) / 2);
                        byte nG = (byte)((imagePointer1[1] + 255) / 2);
                        byte nR = (byte)((imagePointer1[0] + 255) / 2);
                        nR = nG = nB = (byte)(nR * 0.299 + nG * 0.587 + nB * 0.114);

                        imagePointer2[0] = nR;
                        imagePointer2[1] = nG;
                        imagePointer2[2] = nB;
                        imagePointer2[3] = imagePointer1[3];

                        //4 bytes per pixel
                        imagePointer1 += 4;
                        imagePointer2 += 4;
                    }
                    //4 bytes per pixel
                    imagePointer1 += strideOffset;
                    imagePointer2 += strideOffset;
                }
            }
            returnMap.UnlockBits(bitmapData2);
            image.UnlockBits(bitmapData1);
            return returnMap;
        }
    }
}
