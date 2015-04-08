using System;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;


namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region " other "

        private DataTable MakeNamesTable()
        {
            // Create a new DataTable titled 'Names.'
            DataTable namesTable = new DataTable("Names");

            // Add three column objects to the table.
            DataColumn idColumn = new DataColumn();
            idColumn.DataType = System.Type.GetType("System.Int32");
            idColumn.ColumnName = "id";
            idColumn.AutoIncrement = true;
            namesTable.Columns.Add(idColumn);

            DataColumn fNameColumn = new DataColumn();
            fNameColumn.DataType = System.Type.GetType("System.String");
            fNameColumn.ColumnName = "Fname";
            fNameColumn.DefaultValue = "Fname";
            namesTable.Columns.Add(fNameColumn);

            DataColumn lNameColumn = new DataColumn();
            lNameColumn.DataType = System.Type.GetType("System.String");
            lNameColumn.ColumnName = "LName";
            namesTable.Columns.Add(lNameColumn);

            // Create an array for DataColumn objects.
            DataColumn[] keys = new DataColumn[1];
            keys[0] = idColumn;
            namesTable.PrimaryKey = keys;

            // Once a table has been created, use the 
            // NewRow to create a DataRow.
            DataRow row;
            row = namesTable.NewRow();

            // Then add the new row to the collection.
            row["fName"] = "John";
            row["lName"] = "Smith";
            namesTable.Rows.Add(row);
            row = null;

            // Then add the new row to the collection.
            row = namesTable.NewRow();
            row["fName"] = "Cuong";
            row["lName"] = "Pham";
            namesTable.Rows.Add(row);
            row = null;

            // Return the new DataTable.
            return namesTable;
        }

        private void traverse_dv()
        {

            DataTable table;
            table = MakeNamesTable();

            DataView dvwAdoSale = new DataView(table);

            foreach (DataRowView objRow in dvwAdoSale)
            {
                Console.WriteLine(objRow["fName"]);
            }
        }

        public static byte[] ReadFile(string filePath)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                int length = (int)fileStream.Length;  // get file length
                buffer = new byte[length];            // create buffer
                int count;                            // actual number of bytes read
                int sum = 0;                          // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count;  // sum is a buffer offset for next reading
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }

        public string ReadFileToStr(string filePath)
        {
            StreamReader streamReader = new StreamReader(filePath);
            string text = streamReader.ReadToEnd();
            streamReader.Close();
            return (text);
        }

        private void ren_files_in_folder(string strFolder)
        {
            System.IO.DirectoryInfo objDirInfo = null;
            string old_fn = "";
            string new_fn = "";

            try
            {
                objDirInfo = new System.IO.DirectoryInfo(strFolder);
                foreach (System.IO.FileInfo objFil in objDirInfo.GetFiles("*"))
                {
                    old_fn = objFil.Name;
                    new_fn = Regex.Replace(old_fn, "[^a-zA-Z0-9_]+", "", RegexOptions.Compiled);
                    //will bombed if file len < 8
                    new_fn = new_fn.Substring(0, 8) + ".txt";

                    File.Move(strFolder + "\\" + objFil.Name, strFolder + "\\" + new_fn);
                }

            }
            catch (Exception ex)
            {
                throw ex;

            }
            finally
            {
                if ((objDirInfo != null))
                {
                    objDirInfo = null;
                }

            }
        }

        #endregion

        #region " test "

        private string re1(string userName, string pattern)
        {
            //string userName = "Neimke, Darren";
            //string pattern = "(?<surname>(\\w+)),\\s(?<firstname>(\\w+))";
            return (System.Text.RegularExpressions.Regex.Match(userName, pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase).Groups["table"].ToString());
        }

        public void create_file()
        {
            string path = @"c:\temp\MyTest.txt";

            // Delete the file if it exists.
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            // Create the file.
            using (FileStream fs = File.Create(path, 1024))
            {
                Byte[] info = new UTF8Encoding(true).GetBytes("This is some text in the file.");
                // Add some information to the file.
                fs.Write(info, 0, info.Length);
            }

            // Open the stream and read it back.
            using (StreamReader sr = File.OpenText(path))
            {
                string s = "";
                while ((s = sr.ReadLine()) != null)
                {
                    Console.WriteLine(s);
                }
            }
        }

        private string exact_string(string foo, int size)
        {
            string foo1 = "";

            foo1 = foo;//.Substring(0,1)

            if (foo1.Length < size)
            {
                for (int i = 0; i < size - foo.Length; i++)
                {
                    foo1 += "0";
                }
            }
            else
            {
                foo1 = foo1.Substring(0, size);
            }

            return (foo1);
        }

        /// <summary>
        /// Generates a random string with the given length
        /// </summary>
        /// <param name="size">Size of the string</param>
        /// <param name="lowerCase">If true, generate lowercase string</param>
        /// <returns>Random string</returns>
        private string RandomString(int size, bool exact_len, bool lowerCase)
        {
            string foo = "";
            int size1 = size;
            //Random random = new Random();

            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;

            //            size1 = random.Next(size/3, size);
            Thread.Sleep(10);
            size1 = exact_len ? size : random.Next(size / 3, size);
            for (int i = 0; i < size1; i++)
            {
                Thread.Sleep(10);
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            // append blanks
            for (int j = size1; j < size; j++)
            {
                ch = ' ';// Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }

            foo = lowerCase ? builder.ToString().ToLower() : builder.ToString();

            random = null;
            builder = null;

            return (foo);
        }

        private int RandomNumber(int min, int max)
        {
            int foo = 0;

            Random random = new Random();
            foo = random.Next(min, max);
            random = null;

            return (foo);
        }

        private string RandomStringNumber(int size, bool exact_len, int min, int max)
        {
            //int foo = 0;
            string foo1 = "";
            int size1 = size;
            Random random = new Random();

            size1 = exact_len ? size : random.Next(1, size);

            for (int i = 0; i < size1; i++)
            {
                foo1 += random.Next(0, 9).ToString();
            }

            random = null;

            return (foo1);
        }

        private string[] get_tab_cols(string input, int fil_num)
        {
            string match;
            MatchCollection match2;
            string[] tab_cols = { };
            char[] sep = { ' ' };
            string[] col1 = { };
            string col = "";
            string len_type = "";
            string pattern1 = "(?<tab_name>(.+) \\(Tab Delimited\\))";
            string pattern2 = "\n(?<col_name>([A-Z](.{2,}) (\\d{1,3}A|\\d{1,3}N|N)?))( |\\r)";//(?<et_al>(.+))

            string tab_mark = "(Tab Delimited)";
            string col_name = "";
            string col_names = "";
            int count = 3;
            int fld_count = 0;
            int fld_counter = 0;

            // works well but will be confused with "Deleted 1A 4 N"
            pattern2 = "\n(?<col_name>[A-Z].{2,}) (?<col_lt>(\\d{1,3}A|\\d{1,3}N|N))( (?<col_etal>\\w+))?\\r";

            //pattern2 = "\n(?<col_name>[A-Z].{2,}) (\\d{1,3}A|\\d{1,3}N|N)?( |\\r)";//(?<et_al>(.+))
            string rand_str = "";

            Byte[] info;
            string path = "";

            int len = 0;
            char type = 'A'; // or 'N'

            match = (100 + fil_num).ToString() + "-" + Regex.Match(input, pattern1, RegexOptions.IgnoreCase).Groups["tab_name"].ToString().Trim();
            Console.WriteLine("\n\n" + match);

            path = @"c:\MerchantRMS\conversion\input1\" + match.Substring(0, match.Length - tab_mark.Length).Replace(" ", "") + ".txt";

            // delete the file if it exists.
            if (File.Exists(path))
            {
                File.Delete(path);
            }

            // Create the file.

            col_names = "";
            using (FileStream fs = File.Create(path, 1024))
            {
                for (int i = 1; i <= count; i++)
                {
                    //info = null;
                    fld_count = Regex.Matches(input, pattern2).Count;
                    fld_counter = 0;
                    foreach (Match match1 in Regex.Matches(input, pattern2))
                    {
                        //Console.WriteLine("Found '{0}' at position {1}", match1.Value, match1.Index);

                        len = 0;
                        type = 'A'; // or 'N'
                        fld_counter++;

                        col = match1.ToString().Trim();

                        if (col.Substring(0, 5) != "MAI L")
                        {

                            len_type = col.Substring(col.LastIndexOf(" "), col.Length - col.LastIndexOf(" ")).Trim();
                            col_name = col.Substring(0, col.Length - len_type.Length - 1);
                            col_name = col_name.Replace(" ", "");

                            if (len_type.Length == 1)
                            {
                                len = 1;
                                type = len_type[0]; // or 'N'
                            }
                            else
                            {
                                len = Convert.ToInt32(len_type.Substring(0, len_type.Length - 1));
                                type = len_type[len_type.Length - 1]; // or 'N'
                            }

                            col_name = exact_string(col_name, len) + "\t";
                            if (i == 1)
                            {
                                col_names += col_name;
                            }

                            if (type == 'A')
                            {
                                rand_str = RandomString(len, (i % 3 == 0), false);
                                if (fld_counter < fld_count)
                                {
                                    rand_str = rand_str + "\t";
                                }

                                //Console.WriteLine(rand_str);
                                info = new UTF8Encoding(true).GetBytes(rand_str);
                            }
                            else
                            {
                                rand_str = RandomStringNumber(len, true, 0, 0);
                                //rand_int = RandomNumber(1, 1024);
                                //Console.WriteLine(rand_str.ToString());
                                if (fld_counter < fld_count)
                                {
                                    rand_str = rand_str + "\t";
                                }
                                info = new UTF8Encoding(true).GetBytes(rand_str);
                            }

                            // Add some information to the file.
                            fs.Write(info, 0, info.Length);
                        }
                    }
                    info = new UTF8Encoding(true).GetBytes("\r\n");
                    fs.Write(info, 0, info.Length);
                }//              for (int i = 1; i <= count; i++)

                info = new UTF8Encoding(true).GetBytes(col_names);
                fs.Write(info, 0, info.Length);
                info = new UTF8Encoding(true).GetBytes("\r\n");
                fs.Write(info, 0, info.Length);

                return (tab_cols);
            }//            using (FileStream fs = File.Create(path, 1024))
        }

        private void do_proc()
        {
            int i = 0;
            string[] tab_cols1 = { };
            int curr_start_pos = 0;
            int next_lookup_start = 22;
            int curr_end_pos = 1;

            string tab_col = "";
            string tab_cols = ReadFileToStr("C:\\MerchantRMS\\conversion\\tables1.txt");
            string first_tab = "Customer Master File";
            string tab_mark = "(Tab Delimited)";

            next_lookup_start = first_tab.Length + tab_mark.Length + 1;

            while (curr_end_pos < tab_cols.Length)
            {
                i++;

                curr_end_pos = tab_cols.IndexOf("(Tab Delimited)", curr_start_pos + next_lookup_start);
                if (curr_end_pos == -1)
                {
                    tab_col = tab_cols.Substring(curr_start_pos, tab_cols.Length - curr_start_pos - 1);
                }
                else
                {
                    tab_col = tab_cols.Substring(curr_start_pos, curr_end_pos - curr_start_pos - 1);
                }

                tab_cols1 = get_tab_cols(tab_col, i);

                if (curr_end_pos == -1)
                {
                    break;
                }

                curr_start_pos = curr_start_pos + tab_col.LastIndexOf(Environment.NewLine) + 2;
                next_lookup_start = curr_end_pos - curr_start_pos + tab_mark.Length + 1;
            }

        }

        private void do_proc_split()
        {
            int i = 0;
            string[] tab_cols1 = { };
            string[] sep = { "(Tab Delimited)" };
            int curr_start_pos = 0;
            int next_lookup_start = 22;
            int curr_end_pos = 1;

            string tab_col = "";
            string tab_cols = ReadFileToStr("C:\\MerchantRMS\\conversion\\tables1.txt");
            string first_tab = "Customer Master File";
            string tab_mark = "(Tab Delimited)";


            tab_cols1 = tab_cols.Split(sep, StringSplitOptions.None);


            next_lookup_start = first_tab.Length + tab_mark.Length + 1;

            while (curr_end_pos < tab_cols.Length)
            {
                i++;

                curr_end_pos = tab_cols.IndexOf("(Tab Delimited)", curr_start_pos + next_lookup_start);
                if (curr_end_pos == -1)
                {
                    tab_col = tab_cols.Substring(curr_start_pos, tab_cols.Length - curr_start_pos - 1);
                }
                else
                {
                    tab_col = tab_cols.Substring(curr_start_pos, curr_end_pos - curr_start_pos - 1);
                }

                tab_cols1 = get_tab_cols(tab_col, i);

                if (curr_end_pos == -1)
                {
                    break;
                }

                curr_start_pos = curr_start_pos + tab_col.LastIndexOf(Environment.NewLine) + 2;
                next_lookup_start = curr_end_pos - curr_start_pos + tab_mark.Length + 1;
            }

        }

        private string CapText1(Match m)
        {
            string strFuncNam = m.ToString().Substring(0, m.ToString().Length - m.ToString().LastIndexOf(' '));
            string replacement = strFuncNam + "                WriteLogLine(m_strErrMsg, strLogFile, , LogType.LOG_CUSTOM)\n                Return (-1)";
            return replacement;
        }

        private void do_proc1()
        {
            string pattern = null;
            string File = "C:\\Development\\0.dev\\EODServices\\clsEODTools.vb";
            string SQLStream = ReadFileToStr(File);
            pattern = "Dim strCaption As String = \"Convert \\w+\"";

            Regex rx = new Regex(pattern);
            string result = rx.Replace(SQLStream, CapText1);
            Console.WriteLine(result);
        }

        public string get_params(string code_line)
        {
            string pattern = @"\w+\((?<params>.*)\)";
            //            lngRetVal = api_Unzip("*.*", strPath & "\" & strFileToUnzip, 0, "", strPath, strPackageDatPwd)
            Match _m = Regex.Match(code_line, pattern, RegexOptions.IgnoreCase);

            return (_m.Groups["params"].Value);

        }

        public void get_comm_serv()
        {
            // read file
            //string[] sctns = {};
            string input = @"If Not api_WriteIniEntry(strFile, strSection, ""DISCONNECT"", CStr(varDetails(1))) Then GoTo ExitCreateCommServInFile";
            string input1 = @"If Not api_WriteIniEntry(strFile, strSection, ""DISCONNECT"", CStr(varDetails(1))) Then GoTo ExitCreateCommServInFile";
            string pattern = @".+strSection, (?<key>""[A-Z]+""), CStr\(varDetails\((?<key_id>\d+).+";
            string[] sep = { "strSection = \"" };
            //Match _m = Regex.Match(_input, pattern, RegexOptions.IgnoreCase);

            input = ReadFileToStr(@"C:\Users\FSoliva\Downloads\sharp\test\input\tables1.txt");

            //separate into sections
            string[] sctns = input.Split(sep, StringSplitOptions.None);

            // traverse sections
            for (int i = 0; i < sctns.Length; i++)
            {
                input1 = sctns[i];
                if (input1.Length > 64)
                {
                    MatchCollection match2 = Regex.Matches(input, pattern);
                    Console.Write(@"Dim str" + input1.Substring(0, input1.IndexOf('"')) + " As String() = {");

                    foreach (Match match1 in Regex.Matches(sctns[i], pattern))
                    {
                        Console.Write("," + match1.Groups["key"].Value);
                    }
                    Console.WriteLine("}");

                }//            if (input1.Length > 8) { 

            }//        for (int i=0; i<sctns.Length; i++) 

        }//    public void get_comm_serv()

        static void Swap<T>(ref T lhs, ref T rhs)
        {
            T temp;
            temp = lhs;
            lhs = rhs;
            rhs = temp;
        }

        void gen_unzip_param()
        {
            string _str = @"lngRetVal = api_Unzip(""*.*"", strPath & ""\"" & strFileToUnzip, 0, """", strPath, strPackageDatPwd)";
            string _params = get_params(_str);
            string[] sep = { "," };

            string[] tab_cols1 = _params.Split(sep, StringSplitOptions.None);

            Swap<string>(ref tab_cols1[0], ref tab_cols1[1]);
            Swap<string>(ref tab_cols1[3], ref tab_cols1[4]);
            Swap<string>(ref tab_cols1[2], ref tab_cols1[3]);

            string _str1 = String.Join(",", tab_cols1);

            Console.WriteLine(_str);
            Console.WriteLine("lngRetVal = objtools.Unzip(" + _str1 + ")");

        }

        #endregion


        private void Form1_Load(object sender, EventArgs e)
        {
            const string strFolder = @"D:\stuff\loads\ra\rotating";
            ClassLibrary1.ra_resize _rsz = null;

            try
            {
                _rsz = new ClassLibrary1.ra_resize();
                _rsz.resize_files_in_folder(strFolder, 480, 320, 100, 67);
            }
            finally
            {
                if (_rsz != null) _rsz = null;
            }

            System.Windows.Forms.MessageBox.Show("done, js on clipboard...");
            Application.Exit();
        }
    }
}
