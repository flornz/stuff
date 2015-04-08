using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;

namespace ClassLibrary1
{
	class ImageResize
	{
		enum Dimensions 
		{
			Width,
			Height
		}
		enum AnchorPosition
		{
			Top,
			Center,
			Bottom,
			Left,
			Right
		}
		[STAThread]
        //static void Main(string[] args)
        //{
        //    //set a working directory
        //    string WorkingDirectory = @"C:\Projects\Tutorials\ImageResize";

        //    //create a image object containing a verticel photograph
        //    Image imgPhotoVert = Image.FromFile(WorkingDirectory + @"\imageresize_vert.jpg");
        //    Image imgPhotoHoriz = Image.FromFile(WorkingDirectory + @"\imageresize_horiz.jpg");
        //    Image imgPhoto = null;

        //    imgPhoto = ScaleByPercent(imgPhotoVert, 50);
        //    imgPhoto.Save(WorkingDirectory + @"\images\imageresize_1.jpg", ImageFormat.Jpeg);
        //    imgPhoto.Dispose();

        //    imgPhoto = ConstrainProportions(imgPhotoVert, 200, Dimensions.Width);
        //    imgPhoto.Save(WorkingDirectory + @"\images\imageresize_2.jpg", ImageFormat.Jpeg);
        //    imgPhoto.Dispose();

        //    imgPhoto = FixedSize(imgPhotoVert, 200, 200);
        //    imgPhoto.Save(WorkingDirectory + @"\images\imageresize_3.jpg", ImageFormat.Jpeg);
        //    imgPhoto.Dispose();

        //    imgPhoto = Crop(imgPhotoVert, 200, 200, AnchorPosition.Center);
        //    imgPhoto.Save(WorkingDirectory + @"\images\imageresize_4.jpg", ImageFormat.Jpeg);
        //    imgPhoto.Dispose();

        //    imgPhoto = Crop(imgPhotoHoriz, 200, 200, AnchorPosition.Center);
        //    imgPhoto.Save(WorkingDirectory + @"\images\imageresize_5.jpg", ImageFormat.Jpeg);
        //    imgPhoto.Dispose();

        //}

		static Image ScaleByPercent(Image imgPhoto, int Percent)
		{
			float nPercent = ((float)Percent/100);

			int sourceWidth = imgPhoto.Width;
			int sourceHeight = imgPhoto.Height;
			int sourceX = 0;
			int sourceY = 0;

			int destX = 0;
			int destY = 0; 
			int destWidth  = (int)(sourceWidth * nPercent);
			int destHeight = (int)(sourceHeight * nPercent);

			Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
			bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

			grPhoto.DrawImage(imgPhoto, 
				new Rectangle(destX,destY,destWidth,destHeight),
				new Rectangle(sourceX,sourceY,sourceWidth,sourceHeight),
				GraphicsUnit.Pixel);

			grPhoto.Dispose();
			return bmPhoto;
		}

		static Image ConstrainProportions(Image imgPhoto, int Size, Dimensions Dimension)
		{
			int sourceWidth = imgPhoto.Width;
			int sourceHeight = imgPhoto.Height;
			int sourceX = 0;
			int sourceY = 0;
			int destX = 0;
			int destY = 0; 
			float nPercent = 0;

			switch(Dimension)
			{
				case Dimensions.Width:
					nPercent = ((float)Size/(float)sourceWidth);
					break;
				default:
					nPercent = ((float)Size/(float)sourceHeight);
					break;
			}
				
			int destWidth  = (int)(sourceWidth * nPercent);
			int destHeight = (int)(sourceHeight * nPercent);

			Bitmap bmPhoto = new Bitmap(destWidth, destHeight, PixelFormat.Format24bppRgb);
			bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

			grPhoto.DrawImage(imgPhoto, 
			new Rectangle(destX,destY,destWidth,destHeight),
			new Rectangle(sourceX,sourceY,sourceWidth,sourceHeight),
			GraphicsUnit.Pixel);

			grPhoto.Dispose();
			return bmPhoto;
		}

		public Image FixedSize(Image imgPhoto, int Width, int Height)
		{
			int sourceWidth = imgPhoto.Width;
			int sourceHeight = imgPhoto.Height;
			int sourceX = 0;
			int sourceY = 0;
			int destX = 0;
			int destY = 0; 

			float nPercent = 0;
			float nPercentW = 0;
			float nPercentH = 0;

			nPercentW = ((float)Width/(float)sourceWidth);
			nPercentH = ((float)Height/(float)sourceHeight);

			//if we have to pad the height pad both the top and the bottom
			//with the difference between the scaled height and the desired height
			if(nPercentH < nPercentW)
			{
				nPercent = nPercentH;
				destX = (int)((Width - (sourceWidth * nPercent))/2);
			}
			else
			{
				nPercent = nPercentW;
				destY = (int)((Height - (sourceHeight * nPercent))/2);
			}
		
			int destWidth  = (int)(sourceWidth * nPercent);
			int destHeight = (int)(sourceHeight * nPercent);

			Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
			bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			grPhoto.Clear(Color.Red);
			grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

			grPhoto.DrawImage(imgPhoto, 
				new Rectangle(destX,destY,destWidth,destHeight),
				new Rectangle(sourceX,sourceY,sourceWidth,sourceHeight),
				GraphicsUnit.Pixel);

			grPhoto.Dispose();
			return bmPhoto;
		}

		static Image Crop(Image imgPhoto, int Width, int Height, AnchorPosition Anchor)
		{
			int sourceWidth = imgPhoto.Width;
			int sourceHeight = imgPhoto.Height;
			int sourceX = 0;
			int sourceY = 0;
			int destX = 0;
			int destY = 0;

			float nPercent = 0;
			float nPercentW = 0;
			float nPercentH = 0;

			nPercentW = ((float)Width/(float)sourceWidth);
			nPercentH = ((float)Height/(float)sourceHeight);

			if(nPercentH < nPercentW)
			{
				nPercent = nPercentW;
				switch(Anchor)
				{
					case AnchorPosition.Top:
						destY = 0;
						break;
					case AnchorPosition.Bottom:
						destY = (int)(Height - (sourceHeight * nPercent));
						break;
					default:
						destY = (int)((Height - (sourceHeight * nPercent))/2);
						break;
				}				
			}
			else
			{
				nPercent = nPercentH;
				switch(Anchor)
				{
					case AnchorPosition.Left:
						destX = 0;
						break;
					case AnchorPosition.Right:
						destX = (int)(Width - (sourceWidth * nPercent));
						break;
					default:
						destX = (int)((Width - (sourceWidth * nPercent))/2);
						break;
				}			
			}

			int destWidth  = (int)(sourceWidth * nPercent);
			int destHeight = (int)(sourceHeight * nPercent);

			Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
			bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

			Graphics grPhoto = Graphics.FromImage(bmPhoto);
			grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

			grPhoto.DrawImage(imgPhoto, 
				new Rectangle(destX,destY,destWidth,destHeight),
				new Rectangle(sourceX,sourceY,sourceWidth,sourceHeight),
				GraphicsUnit.Pixel);

			grPhoto.Dispose();
			return bmPhoto;
		}

        public Image FixedSize1(Image imgPhoto, int Width, int Height)
        {
            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);

            //if we have to pad the height pad both the top and the bottom
            //with the difference between the scaled height and the desired height
            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;
                destX = (int)((Width - (sourceWidth * nPercent)) / 2);
            }
            else
            {
                nPercent = nPercentW;
                destY = (int)((Height - (sourceHeight * nPercent)) / 2);
            }

            int destWidth = (int)(sourceWidth * nPercent);
            int destHeight = (int)(sourceHeight * nPercent);

            Bitmap bmPhoto = new Bitmap(Width, Height, PixelFormat.Format48bppRgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            Graphics grPhoto = Graphics.FromImage(bmPhoto);
            grPhoto.Clear(Color.Gray);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new Rectangle(destX, destY, destWidth, destHeight),
                new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                GraphicsUnit.Pixel);

            grPhoto.Dispose();
            return bmPhoto;
        }
    
    }


    /// <summary>
    /// objective
    /// function that will take a folder and resize image(s) in the folder to a subfolder inside the input
    /// also generate thumbnails for each of the resized images
    /// </summary>
    class ra_resize
    {
        private int m_id = -1;
        public int ID
        {
            get { return m_id; }
            set { m_id = value; }
        }

        private string m_subdir_name = string.Empty;
        public string subdir_name
        {
            get { return m_subdir_name; }
            set { m_subdir_name = value; }
        }

        private string m_files_processed = string.Empty;
        public string files_processed
        {
            get { return m_files_processed; }
            set { m_files_processed = value; }
        }

        public void resize_files_in_folder(
            string str_src_dir
            , int rsz_w, int rsz_h
            , int thumb_w, int thumb_h
            )
        {
            System.IO.DirectoryInfo objDirInfo = null;
            ImageResize obj_rsz = null;

            string dest_thumbs_dir = "";
            string csv_fn = "";
            //string subdir_name = "";
            string rszd_fpn = "";
            Image img_src = null;
            Image img_gen = null;

            try
            {
                // get a dest dir under source
                do
                {
                    //subdir_name = DateTime.UtcNow.ToString("yyyyMMdd-hhmmss");
                    subdir_name = String.Format(@"{0}\{1:yyyyMMdd-hhmmss}", str_src_dir, DateTime.UtcNow);
                } while (Directory.Exists(subdir_name));

                dest_thumbs_dir = subdir_name + @"\thumbs";
                Directory.CreateDirectory(subdir_name);
                Directory.CreateDirectory(dest_thumbs_dir);

                obj_rsz = new ImageResize();

                // traverse, generate slide image
                objDirInfo = new System.IO.DirectoryInfo(str_src_dir);
                foreach (System.IO.FileInfo objFil in objDirInfo.GetFiles("*"))
                {
                    // resize image for slide
                    img_src = Image.FromFile(String.Format(@"{0}\{1}", str_src_dir, objFil.Name));
                    rszd_fpn = String.Format(@"{0}\{1}", subdir_name, objFil.Name);

                    if (img_src.Width == rsz_w && img_src.Height == rsz_h)
                    // do not resize (but copy), if target dimensions are the same as original
                    {
                        System.IO.File.Copy(String.Format(@"{0}\{1}", str_src_dir, objFil.Name), rszd_fpn);
                    }
                    else 
                    {
                        img_gen = obj_rsz.FixedSize1(img_src, rsz_w, rsz_h);
                        img_gen.Save(rszd_fpn, ImageFormat.Jpeg);
                    }

                    img_src.Dispose();// = null;
                    img_gen.Dispose();

                    // generate a thumb for the resize image
                    img_src = Image.FromFile(rszd_fpn);
                    img_gen = obj_rsz.FixedSize1(img_src, thumb_w, thumb_h);
                    rszd_fpn = String.Format(@"{0}\{1}", dest_thumbs_dir, objFil.Name);
                    img_gen.Save(rszd_fpn, ImageFormat.Jpeg);

                    img_src.Dispose();// = null;
                    img_gen.Dispose();

                    // update csv_fn; will be used for html slideshow (ron-anne)
                    csv_fn += String.Format(", '{0}'", objFil.Name);
                }

                // assemble slide js
                string jsSlideDir = subdir_name.Replace(Path.GetDirectoryName(subdir_name) + Path.DirectorySeparatorChar, "");
                string slideJS = String.Format("\t\t\t\tvar slideDir = '{0}'", jsSlideDir);
                slideJS += Environment.NewLine + "\t\t\t\tvar gal_imgs = new Array(";
                slideJS += csv_fn.Length > 2 ? String.Format("{0}\t\t\t\t\t{1}", Environment.NewLine, csv_fn.Substring(1)) : Environment.NewLine + "no-img";
                slideJS += Environment.NewLine + "\t\t\t\t);";

                // clip slide js
                ClassLibrary1.cboard slideCB = new ClassLibrary1.cboard { BoredText = slideJS };
                slideCB.clip_text();
            }

            finally
            {
                if (objDirInfo != null) objDirInfo = null;
                if (obj_rsz != null) obj_rsz = null;
            }
        }
    
    }

}
