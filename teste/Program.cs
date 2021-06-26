using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using teste.Views;

namespace teste
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            if (Registry.CurrentUser.OpenSubKey("MasterZap") == null)
            {
                FrmCadastroUsuario f = new FrmCadastroUsuario();
                f.ShowDialog();

            }   
      

           

            FrmSplash load = new FrmSplash();
            load.ShowDialog();


            Application.Run(new FRMprincipal());
            
           
        }




    }
}
