using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using WinMovieRack.Properties;
using System.Drawing;
using System.Text.RegularExpressions;

namespace WinMovieRack.Model {

	public class PictureHandler {

		public static string PATH_TO_DB = Directory.GetCurrentDirectory();

		public static string movieImagesPath = PATH_TO_DB + @"\img\mov\";
		public static string personImagesPath = PATH_TO_DB + @"\img\per\";


		public const int LIST_IMAGE_HEIGHT = 80;
		public const int LIST_IMAGE_WIDTH = 80;

		public const int PREVIEW_IMAGE_WIDTH = 200;
		public const int PREVIEW_IMAGE_HEIGHT = 200;

		public const string moviePosterName = "poster";
		public const string personPortraitName = "portrait";

		private PictureHandler () {
		}

		public static void savePicturePoster(Bitmap b, int idMovies) {
			saveMovieImageToHD(b, movieImagesPath + idMovies.ToString(), moviePosterName);
		}

		public static string getPicturePosterPath(int idMovies, EImageSizes size) {
			string path= buildPath(movieImagesPath + idMovies.ToString(), moviePosterName, size);
			if (File.Exists(path)) {
				return path;
			} else {
				return getNoPic(size);
			}
		}


		private static void saveMovieImageToHD(Bitmap b, string path, string filename) {
			Directory.CreateDirectory(path);

			b.Save(buildPath(path, filename,EImageSizes.FULL), System.Drawing.Imaging.ImageFormat.Jpeg);

			Bitmap scaledLIST = scaleImageProportianal(b, LIST_IMAGE_WIDTH, LIST_IMAGE_HEIGHT);
			scaledLIST.Save(buildPath(path, filename, EImageSizes.LIST), System.Drawing.Imaging.ImageFormat.Jpeg);

			Bitmap scaledPREVIEW = scaleImageProportianal(b, PREVIEW_IMAGE_WIDTH, PREVIEW_IMAGE_HEIGHT);
			scaledPREVIEW.Save(buildPath(path, filename, EImageSizes.PREVIEW), System.Drawing.Imaging.ImageFormat.Jpeg);
		}

		private static string getNoPic(EImageSizes size) {
			return @"Ressources\nopic" + size.ToString() + ".jpg";
		}

		private static string buildPath(string path, string filename, EImageSizes size) {
			return makePathStringSafe(path) + @"\" + filename + size.ToString() + ".jpg";
		}

		public static Bitmap scaleImageProportianal(Image image, int newMaxWidth, int newMaxHeight)
        {

			int scaleHeight, scaleWidth;

			if(image.Height > image.Width) {
				scaleHeight = newMaxHeight;
				scaleWidth = (int)(((double)image.Width* (double)scaleHeight) /(double) image.Height);
			} else {
				scaleWidth = newMaxWidth;
				scaleHeight =(int)(((double)image.Height * (double)scaleWidth)/ (double) image.Width);
			}
            //a holder for the result
            Bitmap result = new Bitmap(scaleWidth, scaleHeight);

            //use a graphics object to draw the resized image into the bitmap
            using (Graphics graphics = Graphics.FromImage(result))
            {
                //set the resize quality modes to high quality
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                //draw the image into the target bitmap
                graphics.DrawImage(image, 0, 0, result.Width, result.Height);
            }

            //return the resulting bitmap
            return result;
        }

		private static string makePathStringSafe(string path) {
			string safePath = Regex.Replace(path.Trim(), "\\$", "");
			return  Regex.Replace(safePath, @"\s", "\\ ");
		}
	}
}
