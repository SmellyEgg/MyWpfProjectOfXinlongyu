using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace xinlongyuOfWpf.CustomControls.Extension
{
    /// <summary>
    /// textBoxWithPlaceHolderText.xaml 的交互逻辑
    /// </summary>
    public partial class textBoxWithPlaceHolderText : UserControl
    {

        public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register
        (
            "TipText",
            typeof(string),
            typeof(textBoxWithPlaceHolderText),
            new FrameworkPropertyMetadata("")
        );

        public string TipText
        {
            set { this.txtTips.Text = value; }
        }

        public static readonly DependencyProperty TextPropertyForTextBox =
       DependencyProperty.Register
       (
           "Text",
           typeof(string),
           typeof(textBoxWithPlaceHolderText),
           new FrameworkPropertyMetadata("")
       );

        public string Text
        {
            get { return this.txtContent.Text; }
            set { this.txtContent.Text = value; }
        }

        public textBoxWithPlaceHolderText()
        {
            InitializeComponent();
        }
    }
}
