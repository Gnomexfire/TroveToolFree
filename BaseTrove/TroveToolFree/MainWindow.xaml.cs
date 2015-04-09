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
using System.Windows.Threading;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;

namespace TroveToolFree
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        #region Declare
        public CL_UTIL _UTILIDADE_ = new CL_UTIL();
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        System.Windows.Threading.DispatcherTimer _Timer_For_Process_ = new System.Windows.Threading.DispatcherTimer();
        public int _OnTick = 6;

        STATUS_API _STATUS_API_;
        enum STATUS_API
        {
            Online,
            Offline,
            Indefinido
        }

        public static bool _IsGameEnabled;
        private string _TROVE_PROCESSO_ { get { return "trove.exe"; } }
        #endregion

        #region Start Application - And Injecting . . . - In developement Injecting +
        public MainWindow()
        {
            InitializeComponent();
            _IsGameEnabled = false;
            ImageBrush _My = new ImageBrush();
            _My.ImageSource = new
            BitmapImage(new Uri(@"pack://siteoforigin:,,,/Resources/Treasure_Trove-logo.png", UriKind.Absolute));
            this.Background = _My;
            this.Background.Opacity = 0.1;

            dispatcherTimer.Tick += new EventHandler(OnTimerTick);
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1.0);
            dispatcherTimer.Start();

            _Timer_For_Process_.Tick += _Timer_For_Process__Tick;
            _Timer_For_Process_.Interval = TimeSpan.FromSeconds(2.0);

            CHK_ATTACK_SPEED.Checked += CHK_ATTACK_SPEED_Checked;
            CHK_ATTACK_SPEED.Unchecked += CHK_ATTACK_SPEED_Unchecked;

            CHK_JUMP.Checked += CHK_JUMP_Checked;
            CHK_JUMP.Unchecked += CHK_JUMP_Unchecked;

            CHK_ENERGY.Checked += CHK_ENERGY_Checked;
            CHK_ENERGY.Unchecked += CHK_ENERGY_Unchecked;

            CHK_CAM.Checked += CHK_CAM_Checked;
            CHK_CAM.Unchecked += CHK_CAM_Unchecked;

        }

       

        void _Timer_For_Process__Tick(object sender, EventArgs e)
        {
            // Desabilitado Pelo Desenvolvedor
            if (_STATUS_API_ != STATUS_API.Online)
            {
                _Timer_For_Process_.Stop();
                return;
            }

            // GetProcess
            Process[] _Trove = Process.GetProcessesByName("Trove");
            if (_Trove.Length == 0)
            {
                LBL_STATUS.Content = "Waiting Trove . . .";
                _IsGameEnabled = false;
                return;
            }
            // Jogo Disponivel
            _IsGameEnabled = true;
            // GetMD5 - Não Injetar em versão Não suportada :P1
            if (BaseTrove.BASE_TROVE_CORE_SOURCE._API_MD5_FILE_CORE_SOURCE_ != Hash_Codifica_Decodifica.Encrypt(CL_UTIL._Get_MD5_FROM_Trove_(_Trove[0].Modules[0].FileName)))
            {
                Console.WriteLine("MD5 File Not Support") ;
                LBL_STATUS.Content = "Outdated . . .";
                _IsGameEnabled = false;
                _Timer_For_Process_.Stop();
                return;
            }
            else { 
                Console.WriteLine(". . . . ");
                LBL_STATUS.Content = "Ready . . .";
            }
            _IsGameEnabled = true;
        }

        private void MetroWindow_Loaded(object sender, RoutedEventArgs e)
        {
            _STATUS_API_ = STATUS_API.Indefinido;
            System.Windows.Forms.Application.DoEvents();
            LBL_STATUS.Content = "Get Update"  ;
            LBL_DLL_COMPILED.Content = BaseTrove.BASE_TROVE_CORE_SOURCE._GET_DLL_VERSION_CORE_;
            _KILL_CRASH_HANDLE_(); // ByPass Test
        }
        void OnTimerTick(object sender, EventArgs e)
        {
            _OnTick--;
            LBL_STATUS.Content = "Get Update . . . " + _OnTick ;
            
            if(_OnTick ==0)
            {
                Atualiza();
                dispatcherTimer.Stop();
                _Timer_For_Process_.Start();
            }
            else if(_OnTick ==1)
            {
                LBL_STATUS.Content = "Checking . . .";
            }
        }
        #endregion

        #region Update - And Disabled For Developer tool - Finalizar Chamando aplicativo que faz UPDATE + 
        
        public void Atualiza()
        {
            _UTILIDADE_.Carregar_Informacao_XML();

            //_STATUS_API_ - DISABLED API DEVELOPER
            if (BaseTrove.BASE_TROVE_CORE_SOURCE._API_STATUS_CORE_SOURCE_ == false) {
                LBL_STATUS.Content = "Disabled by the developer";
                _STATUS_API_ = STATUS_API.Offline;
                return;
            }

            if (_UTILIDADE_.Pesquisar_Nova_Atualizao(BaseTrove.BASE_TROVE_CORE_SOURCE._API_VERSION_CORE_SOURCE_))
            {
                LBL_STATUS.Content = "Updating . . .";
                // Chama Method para Baixar atualizacao
                // ----------------------------------- >  Construc method UPDATE Please :P 

                //
            }
            else
            {
                LBL_STATUS.Content = "This is the current version";
                _STATUS_API_ = STATUS_API.Online;
            }

        }
        #endregion 

        private void CMD__Click(object sender, RoutedEventArgs e)
        {
            //Atualiza();
            //Teste();
            //CL_UTIL._Task_Kill_Glyph_Crash();
            SendNop_Teste();
            //Complex._SET_NOP_CAM_();
        }

       
        #region CheckBoxe Methods - Inject 
        void CHK_SPEED_Checked(object sender, RoutedEventArgs e)
        {
            if (_IsGameEnabled != true) { return; }
        }
        void CHK_SPEED_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_IsGameEnabled != true) { return; }
        }
        
        void CHK_ATTACK_SPEED_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_IsGameEnabled != true) { return; }

        }
        void CHK_ATTACK_SPEED_Checked(object sender, RoutedEventArgs e)
        {
            if (_IsGameEnabled != true) { return; }
        }

        void CHK_CAM_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_IsGameEnabled != true) { return; }


        }
        void CHK_CAM_Checked(object sender, RoutedEventArgs e)
        {
            if (_IsGameEnabled != true) { return; }
           //Complex._SET_NOP_CAM_();
        }

        void CHK_ENERGY_Unchecked(object sender, RoutedEventArgs e)
        {

            if (_IsGameEnabled != true) { return; }
        }
        void CHK_ENERGY_Checked(object sender, RoutedEventArgs e)
        {
            if (_IsGameEnabled != true) { return; }

        }

        void CHK_JUMP_Unchecked(object sender, RoutedEventArgs e)
        {
            if (_IsGameEnabled != true) { return; }

        }
        void CHK_JUMP_Checked(object sender, RoutedEventArgs e)
        {
            if (_IsGameEnabled != true) { return; }
            
        }
        #endregion
        public void Teste()
        {
            //var _mem_nop_ = new KDMemory.KDMemoryLibrary.KDMemory(
            //      _TROVE_PROCESSO_,
            //      _TROVE_PROCESSO_,
            //      BaseTrove.BASE_TROVE_CORE_SOURCE._TROVE_CAM_FUNCION_,
            //      KDMemory.KDMemoryLibrary.WINAPI.ProcessAccessRights.PROCESS_ALL_ACCESS);

            var _mem_nop_ = new KDMemory.KDMemoryLibrary.KDMemory(
                  _TROVE_PROCESSO_,
                  _TROVE_PROCESSO_,
                  BaseTrove.BASE_TROVE_CORE_SOURCE._TROVE_CAM_POINTER_,
                  BaseTrove.BASE_TROVE_CORE_SOURCE._TROVE_CAM_OFFSET_,
                  KDMemory.KDMemoryLibrary.WINAPI.ProcessAccessRights.PROCESS_ALL_ACCESS);

            //_mem_nop_.SetTheBytes(BaseTrove.BASE_TROVE_CORE_SOURCE._TROVE_CAM_NOP_);
            foreach (var item in _mem_nop_.GetSingle())
            {
                //Console.WriteLine(item.ToString());
                _mem_nop_.SetSingle(10000f);
            }
           


        }
        public void SendNop_Teste()
        {

            var _mem_nop_ = new KDMemory.KDMemoryLibrary.KDMemory(
                 _TROVE_PROCESSO_,
                 _TROVE_PROCESSO_,
                 BaseTrove.BASE_TROVE_CORE_SOURCE._TROVE_CAM_FUNCION_,
                 KDMemory.KDMemoryLibrary.WINAPI.ProcessAccessRights.PROCESS_ALL_ACCESS);

            //_mem_nop_.SetTheBytes(BaseTrove.BASE_TROVE_CORE_SOURCE._TROVE_CAM_NOP_);
            foreach (var item in _mem_nop_.GetInt32())
            {
                Console.WriteLine(item.ToString());
                if(item != BaseTrove.BASE_TROVE_CORE_SOURCE._NOP_CONVERTIDO_BYTE_)
                {
                    _mem_nop_.SetTheBytes(BaseTrove.BASE_TROVE_CORE_SOURCE._TROVE_CAM_NOP_);
                }
                //_mem_nop_.SetSingle(10000f);
            }
        }
        public static void _KILL_CRASH_HANDLE_()
        {
            var chromeDriverProcesses = Process.GetProcesses().
                               Where(pr => pr.ProcessName == "CrashHandler");

            foreach (var process in chromeDriverProcesses)
            {
                process.Kill();
            }
        }
    }
}
