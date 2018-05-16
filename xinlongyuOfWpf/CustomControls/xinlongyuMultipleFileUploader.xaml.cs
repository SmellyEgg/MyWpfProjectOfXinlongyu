using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Controls;
using System.Windows.Forms;
using xinlongyuOfWpf.Controller.CommonController;
using xinlongyuOfWpf.Controller.ControlController;
using xinlongyuOfWpf.Models.ControlInfo;
using xinlongyuOfWpf.Models.Return;

namespace xinlongyuOfWpf.CustomControls
{
    /// <summary>
    /// 多文件上传控件
    /// </summary>
    public partial class xinlongyuMultipleFileUploader : System.Windows.Controls.UserControl, IControl
    {
        /// <summary>
        /// 数据源for Listbox
        /// </summary>
        private ObservableCollection<string> _allFiles;

        /// <summary>
        /// 存储文件的相对路径
        /// </summary>
        private string _folderPath = string.Empty;

        public xinlongyuMultipleFileUploader()
        {
            InitializeComponent();
            _allFiles = new ObservableCollection<string>();
            MyListBox.ItemsSource = _allFiles;
        }

        /// <summary>
        /// 插入所选文件夹中的内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnInsertClick(object sender, System.Windows.RoutedEventArgs e)
        {
            //新的方式是为了可以上传文件夹以及子文件夹里面的文件
            using (var fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    _allFiles.Clear();
                    string[] files = System.IO.Directory.GetFiles(fbd.SelectedPath, "*.*", System.IO.SearchOption.AllDirectories);
                    foreach (string str in files)
                    {
                        if (!_allFiles.Contains(str))
                        {
                            _allFiles.Add(str);
                        }
                        _folderPath = fbd.SelectedPath;
                    }
                }
            }
        }

        /// <summary>
        /// 获取文件的相对路径
        /// </summary>
        /// <param name="filespec"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        private string GetRelativePath(string filespec, string folder)
        {
            Uri pathUri = new Uri(filespec);
            // Folders must end in a slash
            if (!folder.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                folder += Path.DirectorySeparatorChar;
            }
            Uri folderUri = new Uri(folder);
            return Uri.UnescapeDataString(folderUri.MakeRelativeUri(pathUri).ToString().Replace('/', Path.DirectorySeparatorChar));
        }

        /// <summary>
        /// 删除所选项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BtnDeleteItemClick(object sender, System.Windows.RoutedEventArgs e)
        {
            _allFiles.Remove((string)MyListBox.SelectedItem);
        }

        /// <summary>
        /// 启动控件
        /// </summary>
        /// <param name="text"></param>
        public void SetA1(string text)
        {
            if (!object.Equals(_allFiles, null) && _allFiles.Count > 0)
            {
                try
                {
                    ControlDetailForPage obj = (this.Tag as ControlDetailForPage);
                    //上传文件
                    var values = new[]
                    {
                new KeyValuePair<string, string>("api_type", "upload"),
                new KeyValuePair<string, string>("sql", string.Empty),
                 };
                    SqlController slcontroller = new SqlController();
                    string sqlInsert = obj.d5;
                    //sqlInsert = DecoderAssistant.FormatSql(sqlInsert, this);
                    //bool resultbol = false;
                    //if (CommonFunction.IsFinishLoading)
                    //{
                    //    resultbol = true;
                    //    CommonFunction.IsFinishLoading = false;
                    //    CommonFunction.ShowWaitingForm();
                    //}
                    //改为传相对路径
                    foreach (string file in _allFiles)
                    {
                        var result = slcontroller.PostFile(file, values);
                        BaseReturn brj = JsonController.DeSerializeToClass<BaseReturn>(result.Result);
                        FileUploadReturn returnResult = JsonController.DeSerializeToClass<FileUploadReturn>(brj.data.ToString());
                        string fileUrl = returnResult.data.path;
                        string filename = this.GetRelativePath(file, _folderPath); ;
                        string fileName = @"\" + @filename;
                        fileName = fileName.Replace("\\", "\\\\");
                        slcontroller.ExcuteSqlWithReturn(string.Format(sqlInsert, fileUrl, fileName));
                    }
                    //if (resultbol)
                    //{
                    //    CommonFunction.IsFinishLoading = true;
                    //}
                    this.SetP9(obj.p9);
                }
                catch
                {
                    this.SetP9((this.Tag as ControlDetailForPage).p12);
                }
            }
        }
    }
}
