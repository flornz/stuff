using System;
using System.Collections.Generic;
using System.Text;
using Google.GData.Photos;
using Google.GData.Extensions.MediaRss;
using Google.Picasa;

namespace WindowsFormsApplication1
{
    class CreateNewAlbum
    {

        private PicasaService service;
        private PicasaFeed feed;
        private PicasaEntry newEntry = null;

        public CreateNewAlbum(PicasaService service, PicasaFeed feed)
		{
			//
			// Required for Windows Form Designer support
			//
//			InitializeComponent();
            this.service = service;
            this.feed = feed;
        }

        private void doCreate(
            string AlbumName
            , string AlbumDescription
            )
        {
            doCreate(
                AlbumName
                , AlbumDescription
                , ""
                , ""
                , true
                , true);
        }


        private void doCreate(
            string AlbumName
            , string AlbumDescription
            , string AlbumLocation
            , string AlbumKeywords
            , bool AlbumPublic
            , bool AllowComments
            )
        {

            Album acc = new Album();
            acc.Title = AlbumName;
            acc.Summary = AlbumDescription;
            if (AlbumLocation.Length > 0)
            {
                acc.Location = AlbumLocation;
            }
            if (AlbumKeywords.Length > 0)
            {
                acc.PicasaEntry.Media = new MediaGroup();
                MediaKeywords keywords = new MediaKeywords(AlbumKeywords);
                acc.PicasaEntry.Media.Keywords = keywords;
            }
            acc.Access = AlbumPublic ? "public" : "private";
            acc.CommentingEnabled = AllowComments;

            this.newEntry = this.service.Insert(this.feed, acc.PicasaEntry);
            //this.Close();

        }

    }
}
