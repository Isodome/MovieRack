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
		public static string nopicPath = PATH_TO_DB + @"\img\nopic";


		public const int PREVIEW_IMAGE_WIDTH = 200;
		public const int PREVIEW_IMAGE_HEIGHT = 200;
		public const int LIST_IMAGE_HEIGHT = 80;
		public const int LIST_IMAGE_WIDTH = 80;
		public const int TINY_IMAGE_HEIGHT = 20;
		public const int TINY_IMAGE_WIDTH = 20;


		public const string moviePosterName = "poster";
		public const string personPortraitName = "portrait";
		public const string noPicName = "nopic";

		public static System.Drawing.Imaging.ImageFormat imageCode = System.Drawing.Imaging.ImageFormat.Jpeg;

		private PictureHandler () {
		}

		public static void savePicturePoster(Bitmap b, int idMovies) {
			savePosterToHD(b, movieImagesPath + idMovies.ToString(), moviePosterName);
		}

		public static string getPicturePosterPath(int idMovies, PosterSize size) {
			string path = buildPosterPath(movieImagesPath + idMovies.ToString(), moviePosterName, size);
			if (File.Exists(path)) {
				return path;
			} else {
				return getNoPic(size);
			}
		}

		public static void savePersonPortrait(Bitmap b, int idPerson) {
			savePosterToHD(b, personImagesPath + idPerson.ToString(), personPortraitName);
		}

		public static string getPersonPortraitPath(int idPerson, PosterSize size) {
			string path = buildPosterPath(personImagesPath + idPerson.ToString(), personPortraitName, size);
			if (File.Exists(path)) {
				return path;
			} else {
				return getNoPic(size);
			}
		}

		private static void savePosterToHD(Bitmap b, string path, string filename) {
			Directory.CreateDirectory(path);

			b.Save(buildPosterPath(path, filename, PosterSize.FULL), imageCode);

			Bitmap bPreview = scaleImageProportianal(b, PREVIEW_IMAGE_WIDTH, PREVIEW_IMAGE_HEIGHT);
			bPreview.Save(buildPosterPath(path, filename, PosterSize.PREVIEW), imageCode);

			Bitmap bList = scaleImageProportianal(b, LIST_IMAGE_WIDTH, LIST_IMAGE_HEIGHT);
			bList.Save(buildPosterPath(path, filename, PosterSize.LIST), imageCode);

			Bitmap bTiny = scaleImageProportianal(b, TINY_IMAGE_WIDTH, TINY_IMAGE_HEIGHT);
			bTiny.Save(buildPosterPath(path, filename, PosterSize.TINY), imageCode);

		}

		private static string buildPosterPath(string path, string filename, PosterSize size) {
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

		public static void initNoPic() {
			Directory.CreateDirectory(nopicPath);

			Bitmap noPicFull = WinMovieRack.Properties.Resources.nopicFULL;
			noPicFull.Save(buildPosterPath(nopicPath, noPicName, PosterSize.FULL), imageCode);

			Bitmap prev = WinMovieRack.Properties.Resources.nopicPREVIEW;
			prev.Save(buildPosterPath(nopicPath, noPicName, PosterSize.PREVIEW), imageCode);

			Bitmap list = WinMovieRack.Properties.Resources.nopicLIST;
			list.Save(buildPosterPath(nopicPath, noPicName, PosterSize.LIST), imageCode);

			Bitmap tiny = WinMovieRack.Properties.Resources.nopicTINY;
			tiny.Save(buildPosterPath(nopicPath, noPicName, PosterSize.TINY), imageCode);
		}

		private static string getNoPic(PosterSize size) {
			return buildPosterPath(nopicPath, noPicName, size);
		}
	}
}
