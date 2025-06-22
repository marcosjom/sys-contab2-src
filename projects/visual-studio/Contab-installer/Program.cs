using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Contab {
    internal static class Program {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main(string[] args) {
            //Read launch params
            ClsLaunchCfg launchCfg = ClsLaunchCfg.getSharedInstance();
            {
                int i; for (i = 0; i < args.Length; i++) {
                    if (args[i] == "-runAsManager") {
                        launchCfg.runAsManager = true;
                        launchCfg.runAsDefault = false;
                    }
                }
                if (args.Length > 0) {
                    launchCfg.executablePath = args[0];
                }
            }
            //Init update-core
            {
                ClsUpdaterCore updCore = ClsUpdaterCore.getSharedInstance();
                if (updCore != null) {
                    updCore.setDefaultSourceSelected("http://", "mortegam.com", 80, "/sys/contab/");
                }
            }
            //
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmManager());
        }
    }
}
