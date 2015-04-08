using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using Google.GData.Photos;
using Google.GData.Extensions.MediaRss;
using Google.GData.Tools;
using Google.Picasa;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace WindowsFormsApplication1
{
    /// <summary>
    /// Summary description for Form1.
    /// </summary>
    public class PhotoBrowser : System.Windows.Forms.Form
    {
        private int progCtr;
        public int ProgCtr
        {
            // note: max progCtr = 8
            get { return progCtr; }
            set
            {
                int maxProgCtrIncrease = 8;
                int maxProgValue = 100;                //can be had from Alert form?
                int resetVal = maxProgValue - maxProgCtrIncrease;

                if (progCtr >= resetVal)
                {
                    // progress bar max value reached, restart
                    progCtr = 0;
                }
                else
                {
                    progCtr = value;
                }
                //progCtr = value;
            }
        }

        public string BaseZipDir  { get
            {
                return (@"D:\stuff\loads\ra");}}


        public List<string[]> list1 { get; set; }


        //public List<string[]> list1
        //{
        //    get
        //    {
        //        return (
        //            new List<string[]> {
        //            //new string[]{ "", "" }
        //            //, new string[]{ "", "" }
        //            new string[]{ "Kalbarri to Geraldton", "kalbarritogeraldton", "https://plus.google.com/photos/118173727612809541873/albums/6060791042233349153?banner=pwa" }
        //            }
        //        );
        //    }
        //}

        string currAlbum { get; set; }
        string currFile { get; set; }

        ProgressForm alert;

        private System.ComponentModel.IContainer components;
        private System.Windows.Forms.ListView AlbumList;
        private System.Windows.Forms.PictureBox AlbumPicture;
        private String googleAuthToken = null;
        private String user = null;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PropertyGrid AlbumInspector;
        private System.Windows.Forms.Button AddAlbum;
        private System.Windows.Forms.Button DeleteAlbum;
        private PicasaService picasaService = new PicasaService("PhotoBrowser");
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private Label label3;
        private Button button1;
        private Label labelStatus;
        private Button buttonStart;
        private BackgroundWorker backgroundWorker1;
        private PicasaFeed picasaFeed = null;


        public PhotoBrowser()
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AlbumList = new System.Windows.Forms.ListView();
            this.AlbumPicture = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.AlbumInspector = new System.Windows.Forms.PropertyGrid();
            this.AddAlbum = new System.Windows.Forms.Button();
            this.DeleteAlbum = new System.Windows.Forms.Button();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.labelStatus = new System.Windows.Forms.Label();
            this.buttonStart = new System.Windows.Forms.Button();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.AlbumPicture)).BeginInit();
            this.SuspendLayout();
            // 
            // AlbumList
            // 
            this.AlbumList.FullRowSelect = true;
            this.AlbumList.GridLines = true;
            this.AlbumList.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.AlbumList.LabelEdit = true;
            this.AlbumList.Location = new System.Drawing.Point(12, 33);
            this.AlbumList.Name = "AlbumList";
            this.AlbumList.Size = new System.Drawing.Size(431, 554);
            this.AlbumList.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.AlbumList.TabIndex = 0;
            this.AlbumList.UseCompatibleStateImageBehavior = false;
            this.AlbumList.View = System.Windows.Forms.View.List;
            this.AlbumList.SelectedIndexChanged += new System.EventHandler(this.AlbumList_SelectedIndexChanged);
            this.AlbumList.DoubleClick += new System.EventHandler(this.OnBrowseAlbum);
            // 
            // AlbumPicture
            // 
            this.AlbumPicture.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.AlbumPicture.Location = new System.Drawing.Point(456, 50);
            this.AlbumPicture.Name = "AlbumPicture";
            this.AlbumPicture.Size = new System.Drawing.Size(146, 131);
            this.AlbumPicture.TabIndex = 1;
            this.AlbumPicture.TabStop = false;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 21);
            this.label1.TabIndex = 2;
            this.label1.Text = "Albums:";
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(453, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 14);
            this.label2.TabIndex = 3;
            this.label2.Text = "Cover:";
            // 
            // AlbumInspector
            // 
            this.AlbumInspector.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.AlbumInspector.Location = new System.Drawing.Point(456, 204);
            this.AlbumInspector.Name = "AlbumInspector";
            this.AlbumInspector.Size = new System.Drawing.Size(406, 201);
            this.AlbumInspector.TabIndex = 4;
            // 
            // AddAlbum
            // 
            this.AddAlbum.Location = new System.Drawing.Point(677, 426);
            this.AddAlbum.Name = "AddAlbum";
            this.AddAlbum.Size = new System.Drawing.Size(74, 35);
            this.AddAlbum.TabIndex = 6;
            this.AddAlbum.Text = "&Add a new Album";
            this.AddAlbum.Click += new System.EventHandler(this.AddAlbum_Click);
            // 
            // DeleteAlbum
            // 
            this.DeleteAlbum.Location = new System.Drawing.Point(782, 426);
            this.DeleteAlbum.Name = "DeleteAlbum";
            this.DeleteAlbum.Size = new System.Drawing.Size(74, 35);
            this.DeleteAlbum.TabIndex = 8;
            this.DeleteAlbum.Text = "&Delete Album";
            this.DeleteAlbum.Click += new System.EventHandler(this.DeleteAlbum_Click);
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.RootFolder = System.Environment.SpecialFolder.MyPictures;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(453, 494);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 21);
            this.label3.TabIndex = 10;
            this.label3.Text = "Debug stuff:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(782, 518);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(74, 35);
            this.button1.TabIndex = 11;
            this.button1.Text = "&For Testing Only";
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelStatus
            // 
            this.labelStatus.AutoSize = true;
            this.labelStatus.Location = new System.Drawing.Point(453, 572);
            this.labelStatus.Name = "labelStatus";
            this.labelStatus.Size = new System.Drawing.Size(115, 13);
            this.labelStatus.TabIndex = 13;
            this.labelStatus.Text = "Work Progress /Status";
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(456, 518);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(170, 35);
            this.buttonStart.TabIndex = 12;
            this.buttonStart.Text = "&Create/Upload from Folders (Data Hardcoded)";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // PhotoBrowser
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(874, 602);
            this.Controls.Add(this.labelStatus);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.DeleteAlbum);
            this.Controls.Add(this.AddAlbum);
            this.Controls.Add(this.AlbumInspector);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AlbumPicture);
            this.Controls.Add(this.AlbumList);
            this.Name = "PhotoBrowser";
            this.Text = "RA Picasa Uploader";
            this.Load += new System.EventHandler(this.OnLoad);
            ((System.ComponentModel.ISupportInitialize)(this.AlbumPicture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.Run(new PhotoBrowser());
        }

        void LogToConsole(string logMsg)
        {
            Console.WriteLine(String.Format("{0}: {1}", System.DateTime.Now, logMsg));
        }

        bool runInit()
        {
            if (!Directory.Exists(BaseZipDir))
            {
                LogToConsole("Base dir does not exists.");
                return (false);
            }

            try
            {
                if (!Directory.Exists(Path.Combine(BaseZipDir, "zipBak")))
                {
                    Directory.CreateDirectory(Path.Combine(BaseZipDir, "zipBak"));
                }
            }
            catch (Exception)
            {
                LogToConsole("Backup zip dir not created.");
                return (false);
            }


            return (true);
        }

        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                return ("");
                //throw new ArgumentException("ARGH!");
            return (input.Substring(0,1).ToUpper() + input.Substring(1));
        }

        void PopulateUploadList()
        {
            list1 = new List<string[]>();
            // populate list2 here
            UnzipAlbumsGetAlbumName();
        }

        void UnzipAlbumsGetAlbumName()
        {
            List<string[]> listHC = null;
                                    //new List<string[]> {
                                    //    new string[]{ "", "" }
                                    //    , new string[]{ "", "" }
                                    //};

            string inputAN = "";
            string zipFileNoX = "";
            string suggAN = "";
            string extractDir = "";

            string[] fileEntries = Directory.GetFiles(BaseZipDir, "*.zip");
            foreach (string zipFile in fileEntries)
            {
                zipFileNoX = Path.GetFileNameWithoutExtension(zipFile).Trim();

                extractDir = Path.Combine(BaseZipDir, zipFileNoX);
                if (Directory.Exists(extractDir))
                    Directory.Delete(extractDir, true);

                LogToConsole("Unpacking zip file \"" + zipFileNoX + "\"");
                ZipFile.ExtractToDirectory(zipFile, extractDir);

                if (listHC == null)
                {
                    /*
                     * to do:
                     * if names are specified (ie from a textfile or from a local variable in this sub),
                     * don't ask for names anymore
                     * display a editable listview and match names to directory with user having ability to edit
                     * http://stackoverflow.com/questions/1824758/databind-listbox-selected-item-to-textboxes
 usersBindingSource = new BindingSource();
 usersBindingSource.DataSource = _presenter.Users;

 usersListBox.DataSource = usersBindingSource;
 usersListBox.DisplayMember = "Name";
 usersListBox.ValueMember = "Id";

 nameTextBox.DataBindings.Add("Text", usersBindingSource, "Name", true, DataSourceUpdateMode.OnPropertyChanged);
 loginTextBox.DataBindings.Add("Text", usersBindingSource, "Login", true, DataSourceUpdateMode.OnPropertyChanged);                     * 
                    */

                    suggAN = zipFileNoX.StartsWith("fwd") ? zipFileNoX.Substring(3) : zipFileNoX;
                    suggAN = FirstCharToUpper(suggAN);

                    inputAN = Microsoft.VisualBasic.Interaction.InputBox("Album name for \"" + zipFileNoX + "\"", "Enter album name", suggAN);
                }
                else
                {
                    inputAN = "";
                    /* 
                find match of extractDir from listHC, then assign that to inputAN
                    that will be added to list1
                    */
                }

                LogToConsole("Inputted/hardcodedd album name for \"" + zipFileNoX + "\" is: " + inputAN);

                list1.Add(new string[] { inputAN, extractDir, "" });

                //File.Move(zipFile, Path.Combine(BaseZipDir, "zipBak"));
            }
        }

        public static string ReadFileToStr1(string filePath)
        {
            using (StreamReader streamReader = new StreamReader(filePath))
            {
                string text = streamReader.ReadToEnd();
                streamReader.Close();
                return (text);
            }
        }

        private void RefreshIncJS()
        {
            string incJSLoc = @"D:\stuff\net\adsl6lcp\includes.js";
            string incJSLocBak = @"D:\stuff\net\adsl6lcp\lwv\includes.js";
            TestFtp ftp1 = new TestFtp(Constants1.ftpURL, Constants1.ftpun, Constants1.ftppw);

            LogToConsole("Downloading includes.js from ftp");
            ftp1.download(@"/includes.js", incJSLocBak);

            LogToConsole("Backing up includes.js");
            File.Copy(incJSLocBak, incJSLoc, true);

            LogToConsole("Generating js for new albums");
            string albLstMarker = @"/**************************** ALBUM LIST, NEWEST ON TOP *****************************/";
            string albLstDecl = @"var bums = new Array(";

            string incJSTxt = ReadFileToStr1(incJSLoc);
            int idxOfCurrTopAlbum = incJSTxt.IndexOf(albLstMarker) + albLstMarker.Length + albLstDecl.Length + 2;

            string incJSTxt1 = incJSTxt.Substring(0, idxOfCurrTopAlbum);
            string incJSTxt2 = incJSTxt.Substring(idxOfCurrTopAlbum + 3);
            //use list1 items to add to album list1

            string newAlbumsJS = "\t" + @"new Array(""" + list1[0][0] + "\", \"" + list1[0][2] + "\")";
            for (int i = 1; i < list1.Count; i++)
                newAlbumsJS = newAlbumsJS + Environment.NewLine + "\t, new Array(\"" + list1[i][0] + "\", \"" + list1[i][2] + "\")";

            incJSTxt = incJSTxt1 + newAlbumsJS + Environment.NewLine + "\t, " + incJSTxt2;
            Console.WriteLine(newAlbumsJS);

            // clip slide js
            ClassLibrary1.cboard slideCB = new ClassLibrary1.cboard { BoredText = newAlbumsJS };
            slideCB.clip_text();

            //File.WriteAllText(incJSLoc, incJSTxt, System.Text.Encoding.UTF8);

            //LogToConsole("Uploading new includes.js");
            //ftp1.upload(@"/includes.js", incJSLoc);
        }

        private void OnLoad(object sender, System.EventArgs e)
        {
            // clip slide js
            ClassLibrary1.cboard slideCB = new ClassLibrary1.cboard { BoredText = "NATOLIA TILE & STONE 18x18 Ivory Classic Travertine - 2.25 Sq. Feet Per EachModel: 92-333 | Store SKU: 1000720924SOLD IN-STORE ONLYCheck Store InventoryBe the first to write a review." };
            slideCB.clip_text();


            if (!runInit()) {
                MessageBox.Show("Error.");
                Application.Exit();
            }
            //createArgs();
            //List<int> cfg = ReadEntityConfigurationFile(@"D:\0_cases\0-GDEMO-GFI\2-ds-classifier-list.xml");

            bool debug = true;
            debug = false;
            //RefreshIncJS();
            PopulateUploadList();

            int numList = 0;
            numList = list1.Count;
            if (numList > 0 || debug)
            {
                if (debug)
                    LogToConsole("Debug mode...");
                else
                    LogToConsole("There are " + numList.ToString() + " albums specified:");

                for (int i = 0; i < list1.Count; i++)
                    LogToConsole("Album " + (i+1).ToString() + ": \"" + list1[i][0] + "\"");
                
            }
            else
            {
                LogToConsole("No albums found." + Constants1.un);
            }

            if (this.googleAuthToken == null)
            {
                LogToConsole("Logging " + Constants1.un);
                using (GoogleClientLogin loginDialog = new GoogleClientLogin(new PicasaService("PhotoBrowser"), Constants1.un + "@gmail.com", Constants1.pw))
                {
                    //loginDialog.ShowDialog();
                    this.googleAuthToken = loginDialog.AuthenticationToken;
                    this.user = loginDialog.User;
                    if (this.googleAuthToken != null)
                    {
                        LogToConsole(Constants1.un + " logged in, updating album feed...");
                        picasaService.SetAuthenticationToken(loginDialog.AuthenticationToken);
                        UpdateAlbumFeed1();
                    }
                    else
                        this.Close();
                }
            }
        
        }

        private void UpdateAlbumFeed()
        {
            AlbumQuery query = new AlbumQuery();

            this.AlbumList.Clear();


            query.Uri = new Uri(PicasaQuery.CreatePicasaUri(this.user));

            this.picasaFeed = this.picasaService.Query(query);

            if (this.picasaFeed != null && this.picasaFeed.Entries.Count > 0)
            {
                foreach (PicasaEntry entry in this.picasaFeed.Entries)
                {
                    ListViewItem item = new ListViewItem(entry.Title.Text +
                                    " (" + entry.GetPhotoExtensionValue(GPhotoNameTable.NumPhotos) + " )");
                    item.Tag = entry;
                    this.AlbumList.Items.Add(item);
                }
            }
            this.AlbumList.Update();
        }


        private void UpdateAlbumFeed1()
        {
            AlbumList.View = View.Details;
            AlbumList.FullRowSelect = true;

            //this.AlbumList.Clear();

            // Create columns for the items and subitems.
            AlbumList.Columns.Add("Album Name", 360 , HorizontalAlignment.Left);
            AlbumList.Columns.Add("Pics", 40, HorizontalAlignment.Right);

            ListViewItem lvItm = null;//ew ListViewItem("lvItm", 0);
            int albumCtr = 0;
            AlbumQuery query = new AlbumQuery();
            query.Uri = new Uri(PicasaQuery.CreatePicasaUri(this.user));
            this.picasaFeed = this.picasaService.Query(query);

            if (this.picasaFeed != null && this.picasaFeed.Entries.Count > 0)
            {
                foreach (PicasaEntry entry in this.picasaFeed.Entries)
                {
                    lvItm = new ListViewItem(entry.Title.Text, albumCtr++);
                    lvItm.SubItems.Add(entry.GetPhotoExtensionValue(GPhotoNameTable.NumPhotos));

                    this.AlbumList.Items.Add(lvItm);
                }
            }
            //this.AlbumList.Update();
        }


        private void AlbumList_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            foreach (ListViewItem item in this.AlbumList.SelectedItems)
            {
                PicasaEntry entry = item.Tag as PicasaEntry;
                setSelection(entry);
            }
        }

        private void OnBrowseAlbum(object sender, System.EventArgs e)
        {
            foreach (ListViewItem item in this.AlbumList.SelectedItems)
            {
                PicasaEntry entry = item.Tag as PicasaEntry;
                string photoUri = entry.FeedUri;
                if (photoUri != null)
                {
                    PictureBrowser b = new PictureBrowser(this.picasaService, false);
                    b.Show();
                    b.StartQuery(photoUri, entry.Title.Text);
                }
            }
        }

        private void AddAlbum_Click(object sender, System.EventArgs e)
        {
            using (NewAlbumDialog dialog = new NewAlbumDialog(this.picasaService, this.picasaFeed))
            {
                dialog.ShowDialog();
                PicasaEntry entry = dialog.CreatedEntry;
                if (entry != null)
                {
                    ListViewItem item = new ListViewItem(entry.Title.Text + " (" + entry.GetPhotoExtensionValue(GPhotoNameTable.NumPhotos) + " )");
                    item.Tag = entry;
                    this.AlbumList.Items.Add(item);
                    this.AlbumList.Refresh();
                }
            }
        }

        private void DeleteAlbum_Click(object sender, System.EventArgs e)
        {

            if (MessageBox.Show("Are you really sure? This is not undoable.",
                "Delete this Album", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                foreach (ListViewItem item in this.AlbumList.SelectedItems)
                {
                    PicasaEntry entry = item.Tag as PicasaEntry;
                    entry.Delete();
                    this.AlbumList.Items.Remove(item);
                    setSelection(null);
                }
            }
        }

        private void setSelection(PicasaEntry entry)
        {
            if (entry != null)
            {
                this.Cursor = Cursors.WaitCursor;
                MediaThumbnail thumb = entry.Media.Thumbnails[0];
                try
                {
                    Stream stream = this.picasaService.Query(new Uri(thumb.Attributes["url"] as string));
                    this.AlbumPicture.Image = new Bitmap(stream);
                }
                catch
                {
                    Icon error = new Icon(SystemIcons.Exclamation, 40, 40);
                    this.AlbumPicture.Image = error.ToBitmap();
                }
                Album a = new Album();
                a.AtomEntry = entry;
                this.AlbumInspector.SelectedObject = a;
                this.Cursor = Cursors.Default;
            }
            else
            {
                this.AlbumPicture.Image = null;
                this.AlbumInspector.SelectedObject = null;
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy != true)
            {
                // create a new instance of the alert form
                alert = new ProgressForm();
                // event handler for the Cancel button in AlertForm
                alert.Canceled += new EventHandler<EventArgs>(cancelAsyncButton_Click);
                alert.Show();
                // Start the asynchronous operation.
                backgroundWorker1.RunWorkerAsync();
            }
        }

        //private string CreateAlbumUploadFilesFromFolder()
        void doActualWork1(BackgroundWorker _worker, System.ComponentModel.DoWorkEventArgs e)
        {
            int numFldrs = list1.Count;
            string aURL = "";
            string aNewURL = "";
            string aID = "";


            for (int i = 0; i < numFldrs; i++)
            {
                if (_worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    progCtr += 8;
                    currAlbum = list1[i][0];
                    _worker.ReportProgress(progCtr);
                    
                    //LogToConsole("Creating album \"" + currAlbum + "\"");
                    LogToConsole(String.Format("Creating album \"{0}\" ({1} of {2})", currAlbum, i+1, numFldrs));
                    aURL = CreateAlbum(currAlbum);
                    
                    aID = Path.GetFileName(aURL);
                    PostJpegsFromFolder(list1[i][1], aID, _worker, e);

                    aNewURL = aURL.Replace("albumid", "albums").Replace(@"picasaweb.google.com/data/entry/api/user", @"plus.google.com/photos") + @"?banner=pwa";
                    list1[i][2] = aNewURL;
                }
            }

            // update js
            progCtr += 8;
            _worker.ReportProgress(progCtr);
            RefreshIncJS();
        }

        /// <summary>
        /// create a Public album with Commenting Enabled
        /// </summary>
        /// <param name="Title"></param>
        /// <returns></returns>
        private string CreateAlbum(string Title)
        {
            return (CreateAlbum(Title, Title, true, true));
        }

        private string CreateAlbum(string Title, string Summary, bool isPublic, bool isCommentingEnabled)
        {
            AlbumEntry newEntry = new AlbumEntry();
            newEntry.Title.Text = Title;
            newEntry.Summary.Text = Summary;
            AlbumAccessor ac = new AlbumAccessor(newEntry);
            ac.Access = isPublic ? "public" : "private";
            ac.CommentingEnabled = true;

            Uri feedUri = new Uri(PicasaQuery.CreatePicasaUri(Constants1.un));

            PicasaEntry createdEntry = (PicasaEntry)this.picasaService.Insert(feedUri, newEntry);
            //return (Path.GetFileName(createdEntry.SelfUri.Content));
            return (createdEntry.SelfUri.Content);
        }


        void PostJpeg(string file, string albumid)
        {
            FileInfo fileInfo = new System.IO.FileInfo(file);
            FileStream fileStream = fileInfo.OpenRead();
            Uri postUri = new Uri(PicasaQuery.CreatePicasaUri(Constants1.un, albumid));

            PicasaEntry entry = (PicasaEntry)this.picasaService.Insert(postUri, fileStream, "image/jpeg", file);

            fileStream.Close();
            fileInfo = null;
        }


        void PostJpegsFromFolder(string folder, string albumid, BackgroundWorker _bw, System.ComponentModel.DoWorkEventArgs e)
        {
            int ctr = 0;
            int numFiles = 0;
            string[] fileEntries = Directory.GetFiles(folder, "*.jpg");
            numFiles = fileEntries.Length;

            foreach (string file in fileEntries)
            {
                if (_bw.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    currFile = Path.GetFileName(file);
                    progCtr += 4;
                    _bw.ReportProgress(progCtr);


                    LogToConsole(String.Format("Uploading \"{0}\" ({1} of {2})", currFile, ++ctr, numFiles));
                    PostJpeg(file, albumid);
                }
            }
        }


        /// <summary>
        /// create new picasa album, add jpegs
        /// return number of pics addes
        /// </summary>
        /// <param name="Album"></param>
        /// <param name="Folder"></param>
        /// <returns></returns>
        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            // work has not started yet
            progCtr = 0;
            doActualWork1(worker, e);

            // consider work complete
            progCtr = 100;
            worker.ReportProgress(progCtr);
        }

        private void backgroundWorker1_ProgressChanged(object sender, System.ComponentModel.ProgressChangedEventArgs e)
        {
            // Show the progress in main form (GUI)
            string alertMsg = "";
            labelStatus.Text = (e.ProgressPercentage.ToString() + "%");
            // Pass the progress to AlertForm label and progressbar
            if (String.IsNullOrEmpty(currFile))
            {
                alertMsg = "Please wait.";
            }
            else
            {
                alertMsg = "Processing \"" + currFile + "\" (" + currAlbum + " album)... ";// +e.ProgressPercentage.ToString() + "%";
            }

            alert.Message = alertMsg;
            alert.ProgressValue = e.ProgressPercentage;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {

            if (e.Cancelled == true)
            {
                labelStatus.Text = "Canceled!";
            }
            else if (e.Error != null)
            {
                labelStatus.Text = "Error: " + e.Error.Message;
            }
            else
            {
                labelStatus.Text = "Done!";
            }
            alert.Close();
        }

        // This event handler cancels the backgroundworker, fired from Cancel button in AlertForm.
        private void cancelAsyncButton_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.WorkerSupportsCancellation == true)
            {
                // Cancel the asynchronous operation.
                backgroundWorker1.CancelAsync();
                // Close the AlertForm
                alert.Close();
            }
        }


        //    private void SaveAlbum_Click(object sender, System.EventArgs e)
        //    {
        //        foreach (ListViewItem item in this.AlbumList.SelectedItems)
        //        {
        //            PicasaEntry entry = item.Tag as PicasaEntry;
        //            string photoUri = entry.FeedUri;

        //            // Show the FolderBrowserDialog.
        //            DialogResult result = folderBrowserDialog.ShowDialog();

        //            PictureBrowser backgroundjob = new PictureBrowser(this.picasaService, true);
        //            backgroundjob.Show();

        //            if (result == DialogResult.OK)
        //            {
        //                string folderName = folderBrowserDialog.SelectedPath;
        //                if (photoUri != null)
        //                {
        //                    backgroundjob.BackupAlbum(photoUri, folderName);
        //                }
        //            }
        //        }
        //    }


        ///// <summary>
        ///// debug code
        ///// </summary>
        //bool inSpecList(string album)
        //{
        //    bool retVal = false;

        //    List<string[]> list1 = new List<string[]> {
        //         new string[]{"August Full Moon Karijini National Park","fwdaugustfullmoonkarijininatpark"}
        //         , new string[]{"Port Hedland to Carnarvon","fwdporthedlandtocarnarvon"}
        //         , new string[]{"The mighty wedge-tail Eagle","themightywedgetaileagle"}
        //         , new string[]{"Carnarvon to Kalbarri","fwdcarvarvontokalbarri"}
        //         , new string[]{ "Wildflowers @ Kalbarri National Park", "fwdwildflowerskalbarrinatpark"}
        //    };

        //    for (int i = 0; i < list1.Count; i++)
        //    {
        //        if (album == list1[i][0])
        //            return (true);
        //    }

        //    return (retVal);
        //}

        ///// <summary>
        ///// debug code
        ///// </summary>
        //void listMyAlbums()
        //{
        //    string incJS = "";
        //    string aURL = "";
        //    List<string[]> list1 = new List<string[]> {
        //         new string[]{"August Full Moon Karijini National Park","fwdaugustfullmoonkarijininatpark"}
        //         , new string[]{"Port Hedland to Carnarvon","fwdporthedlandtocarnarvon"}
        //         , new string[]{"The mighty wedge-tail Eagle","themightywedgetaileagle"}
        //         , new string[]{"Carnarvon to Kalbarri","fwdcarvarvontokalbarri"}
        //         , new string[]{ "Wildflowers @ Kalbarri National Park", "fwdwildflowerskalbarrinatpark"}
        //    };

        //    AlbumQuery query = new AlbumQuery(PicasaQuery.CreatePicasaUri(Constants1.un));

        //    PicasaFeed feed = this.picasaService.Query(query);

        //    foreach (PicasaEntry entry in feed.Entries)
        //    {
        //        if (inSpecList(entry.Title.Text))
        //        {
        //            //Console.WriteLine(entry.Title.Text);
        //            AlbumAccessor ac = new AlbumAccessor(entry);
        //            aURL = entry.SelfUri.Content.Replace("albumid", "albums").Replace(@"picasaweb.google.com/data/entry/api/user", @"plus.google.com/photos");
        //            incJS = incJS + Environment.NewLine + "    , new Array(\"" + entry.Title.Text + "\", \"" + aURL + "\")";

        //            Console.WriteLine(incJS);
        //        }
        //    }
        //}

        private void button1_Click(object sender, EventArgs e)
        {
            //listMyAlbums();
            string aURL = "";
            foreach (ListViewItem item in this.AlbumList.SelectedItems)
            {
                PicasaEntry entry = item.Tag as PicasaEntry;

                AlbumAccessor ac = new AlbumAccessor(entry);
                aURL = "\t, new Array(\"" + entry.Title.Text + "\", \"" + entry.SelfUri.Content.Replace("albumid", "albums").Replace(@"picasaweb.google.com/data/entry/api/user", @"plus.google.com/photos") + "?banner=pwa\")";
                Console.WriteLine(aURL);
            }
        }

    }
}
