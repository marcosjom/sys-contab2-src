using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Contab {
    static class Program {


        const long APP_DELEGATE_SECS_SYNC_UPDATE_STATE = 10;
        const long APP_DELEGATE_SECS_OFFER_UPDATE_STATE = (60 * 60 * 4);
        //
        static private long wUpdaterLastAutoShown = 0;
        static private long updVerifyLast = 0;       //last time and update window was analyzed
        static private int updVerifSeqLast = 0;     //last sync-sequence in updater core
        static private System.Timers.Timer updVerifyTimer = null;      //to verify updates periodically


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
            //detect dll
            {
                string folderPath = Application.ExecutablePath;
                int lastSlashPos = folderPath.LastIndexOf('\\');
                if (lastSlashPos <= 0) {
                    //MessageBox.Show("Could not determine the current executable location folder.\n");
                } else {
                    folderPath = folderPath.Substring(0, lastSlashPos);
                    string filePath = folderPath + "\\tunnel.dll";
                    //this.btnOpen
                    Global.tunnelDllFound = File.Exists(filePath);
                }
            }
            //launch app
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                if (launchCfg.runAsManager) {
                    Application.Run(new FrmManager());
                } else {
                    //start update verif timer
                    if (updVerifyTimer == null) {
                        updVerifyTimer = new System.Timers.Timer();
                        updVerifyTimer.Elapsed += (sender, e) => { timerFiredObj(); };
                        updVerifyTimer.Interval = 1000;
                        updVerifyTimer.AutoReset = true;
                        updVerifyTimer.Start();
                    }
                    //
                    Application.Run(new FrmPrincipal());
                }
            }
        }


        private static void timerFiredObj() {
            ClsLaunchCfg launchCfg = ClsLaunchCfg.getSharedInstance();
            ClsUpdaterCore updCore = ClsUpdaterCore.getSharedInstance();
            if (updCore != null) {
                long curTime = DateTimeOffset.Now.ToUnixTimeSeconds();
                if ((updVerifyLast + APP_DELEGATE_SECS_SYNC_UPDATE_STATE) <= curTime) {
                    int curSeq = updCore.getLastSyncEventSeq();
                    if (updVerifSeqLast == curSeq) {
                        //same sequence, trigger new-sync action and analyze soon
                        updCore.syncLastVersionIfInactive();
                    } else if (!updCore.isSyncingActiveNow()) {
                        int majorI = 0, minorI = 0, compileI = 0;
                        int majorL = 0, minorL = 0, compileL = 0;
                        ulong installDateI = 0, installBootDateI = 0;
                        String protocolI = "", serverI = ""; int portI = 0; String pathI = "";
                        String channelI = "";
                        //
                        String protocolSel = "", serverSel = ""; int portSel = 0, sourceSelSeq = 0; String pathSel = "";
                        String channelSel = ""; int channelSelSeq = 0;
                        //
                        updCore.getSourceSelected(ref protocolSel, ref serverSel, ref portSel, ref pathSel, ref sourceSelSeq);
                        updCore.getSourceChannelSelected(ref channelSel, ref channelSelSeq);
                        updCore.getVersionInstalled(ref majorI, ref minorI, ref compileI, ref protocolI, ref serverI, ref portI, ref pathI, ref channelI, ref installDateI, ref installBootDateI);
                        updCore.getVersionLatest(ref majorL, ref minorL, ref compileL);
                        //
                        if (majorI == 0 && minorI == 0 && compileI == 0) {
                            //not installed, running as a portable version
                            //do not offer update.
                        } else if (Global.conn == null) {
                            //user not logged-in
                            //do not offer update.
                        } else if (majorL == 0 && minorL == 0 && compileL == 0) {
                            //remote not synced yet
                            //validate again sooner
                        } else {
                            bool updAvailable = false;
                            if (serverI.Length > 0 && portI > 0 && pathI.Length > 0 && channelI.Length > 0 && serverSel.Length > 0 && portSel > 0 && pathSel.Length > 0 && channelSel.Length > 0 && (serverI != serverSel || portI != portSel || pathI != pathSel  || channelI != channelSel)) {
                                //update server migration (changed)
                                updAvailable = true;
                            } else if ((majorL != 0 || minorL != 0 || compileL != 0) && (majorI < majorL || (majorI == majorL && minorI < minorL) || (majorI == majorL && minorI == minorL && compileI < compileL))) {
                                //explicit update available
                                updAvailable = true;
                            } else {
                                //default
                                updAvailable = false;
                            }
                            //show update window
                            if (updAvailable && (wUpdaterLastAutoShown + APP_DELEGATE_SECS_OFFER_UPDATE_STATE) <= curTime) { //each 4 hours
                                wUpdaterLastAutoShown = curTime;
                                {
                                    bool frmFound = false;
                                    FormCollection frms = Application.OpenForms;
                                    int i; for (i = 0; i < frms.Count; i++) {
                                        if (frms[i] != null && frms[i].GetType() == typeof(FrmManager)) {
                                            frmFound = true;
                                            break;
                                        }
                                    }
                                    //open window
                                    if (!frmFound) {
                                        FrmManager frm = new FrmManager();
                                        frm.Show();
                                    }
                                }
                            }
                        }
                        updVerifSeqLast = curSeq;
                        updVerifyLast = curTime;
                    }
                }
            }
        }
    }
}
