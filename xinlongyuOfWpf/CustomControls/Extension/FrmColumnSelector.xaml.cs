using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using xinlongyuOfWpf.Models.OtherModel;
using System.Linq;

namespace xinlongyuOfWpf.CustomControls.Extension
{
    /// <summary>
    /// FrmColumnSelector.xaml 的交互逻辑
    /// </summary>
    public partial class FrmColumnSelector : Window
    {
        public ObservableCollection<BoolStringClass> TheList { get; set; }

        public FrmColumnSelector()
        {
            InitializeComponent();
            TheList = new ObservableCollection<BoolStringClass>();

            //TheList.Add(new BoolStringClass { IsSelected = false, TheText = "Some text for item #7" });
            this.DataContext = this;

        }

        /// <summary>
        /// 是否全选
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBoxChanged(object sender, RoutedEventArgs e)
        {
            if ((sender as CheckBox).IsChecked == true)
            {
                //全选
                var listobjects = TheList.ToList();
                listobjects.ForEach(p => p.IsSelected = true);
                TheList.Clear();
                listobjects.ForEach(p => TheList.Add(p));
            }
            else
            {
                //全不选
                var listobjects = TheList.ToList();
                listobjects.ForEach(p => p.IsSelected = false);
                TheList.Clear();
                listobjects.ForEach(p => TheList.Add(p));
            }
        }


        private void btnOkClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnCancelClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
