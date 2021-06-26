using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Firebase.Database;
using System.Threading.Tasks;
using Firebase.Database.Query;
using Microsoft.Win32;
using System.Diagnostics;
using Firebase.Database.Extensions;

namespace teste
{
    public partial class FrmCadastroUsuario : Form
    {
        Computador comp = new Computador()
        {
            Usuario = HardwareInfo.GetAccountName(),
            BIOS_caption = HardwareInfo.GetBIOScaption(),
            Fabricante_BIOS = HardwareInfo.GetBIOSmaker(),
            serial_BIOS = HardwareInfo.GetBIOSserNo(),
            Fabricante_MOBO = HardwareInfo.GetBoardMaker(),
            ID_MOBO = HardwareInfo.GetBoardProductId(),
            Slots_RAM = HardwareInfo.GetNoRamSlots(),
            //IP_GATEWAY = HardwareInfo.GetDefaultIPGateway(),
            Fabricante_GPU = HardwareInfo.GetCPUManufacturer(),
            Serial_HD = HardwareInfo.GetHDDSerialNo(),
            OS = HardwareInfo.GetOSInformation()

        };


        string AuthSecret = "AIzaSyA3gsJjBcEoo9o-K_bo39bFQj4fqCDI_ro";
        string BasePath = "https://licencas-16699.firebaseio.com/Usuarios";





        public FrmCadastroUsuario()
        {


            InitializeComponent();
        }


        class Usuario
        {

            public string CPF { get; set; }
            public string Empresa { get; set; }
            public string DataInicio { get; set; }
            public string DataFinal { get; set; }
            public Computador computador { get; set; }
            public string bloqueado { get; set; }

        }

        public async Task SalvarUsuarioNoDatabase()
        {
            this.Hide();

            Usuario user = new Usuario()
            {
                CPF = txtCnpj.Text.Replace("/", "").Replace(" ", "").Replace("-", "").Replace(".", ""),
                DataFinal = "10/12/2020",
                DataInicio = DateTime.Now.ToString(),
                Empresa = txtNomeEstabelecimento.Text,
                computador = comp,
                bloqueado = "trial"


            };

            var auth = AuthSecret; // your app secret
            var firebaseClient = new FirebaseClient(
              BasePath,
              new FirebaseOptions
              {
                  AuthTokenAsyncFactory = () => Task.FromResult(auth)
              });


            var firebase = new FirebaseClient("https://licencas-16699.firebaseio.com/");

            await firebase.Child("usuarios").Child(user.CPF).PutAsync(user);





        }

        public void CriaRegistroDataBase()
        {

            if (string.IsNullOrEmpty(txtCnpj.Text) || string.IsNullOrEmpty(txtNomeEstabelecimento.Text))
            {
                MessageBox.Show("Preencha as informações", "ATENÇÃO", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            Task t = SalvarUsuarioNoDatabase();
            t.Wait(2000);




        }

        public void CriaRegistroRegedit()
        {

            RegistryKey key;
            key = Registry.CurrentUser.CreateSubKey("MasterZap");
            key.SetValue("CNPJ", txtCnpj.Text.Replace("/", "").Replace(" ", "").Replace("-", "").Replace(".", ""));
            key.SetValue("Estabelecimento", txtNomeEstabelecimento.Text);
            key.SetValue("DataInicio", DateTime.Now);
            key.SetValue("(Padrão)", "Trial");
            key.Close();


            foreach (var process in Process.GetProcessesByName("masterzap"))
            {
                process.Kill();
            }
            this.Close();
        }

        private void SalvaUsuarioDatabase_Click(object sender, EventArgs e)
        {
            CriaRegistroDataBase();
            CriaRegistroRegedit();
        }



        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                btnSalvaUsuarioDatabase.Enabled = true;
            }

            else
            {
                btnSalvaUsuarioDatabase.Enabled = false;
            }
        }

        private void btnCopiarEmail_Click(object sender, EventArgs e)
        {
            Clipboard.SetText("vendas_superzap@protonmail.com");
        }
    }
}
