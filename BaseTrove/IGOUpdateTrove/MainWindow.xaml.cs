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

using MahApps.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System.Threading;

namespace IGOUpdateTrove
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        #region Declare
        
        #endregion
        public string[] args = Environment.GetCommandLineArgs();
        public string _URL_;
        public Mutex _AppMutex;
        public MainWindow()
        {
            InitializeComponent();
            _GetArgsFromCommandLine();
            #region Mutex OneInstace For Application
            bool _AppInstance = false;
            _AppMutex = new Mutex(false, "IGOUpdateTrove", out _AppInstance);
            if (!_AppInstance) { App.Current.Shutdown(); }
            #endregion
        }

        public void _GetArgsFromCommandLine()
        {
            string[] args = Environment.GetCommandLineArgs();

            foreach (string arg in args)
            {
                if (arg.Substring(0, 6).ToString() == "UPDATE") // Precisa do Parametro
                {
                    _URL_ = arg.Substring(6, arg.Length - 6);
                    //Console.Write("Link : \n" + _URL_);
                }
                //else { App.Current.Shutdown(); }
            }
        }



        void Teste()
        {
           
        }
        
    }
}
