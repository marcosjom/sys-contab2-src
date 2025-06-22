using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;

namespace Contab {
    public class ClsAppState {

        //FrmContab state
        public class AppStateFrmContab {
            public bool showBalances;           //show accounts balances
            public bool showBalancesSep;        //show accounts balances in separated currency
            public string treeExpandedLst;      //list of expanded accounts
        }

        //FrmSelCuenta state
        public class AppStateFrmSelCuenta {
            public string accSel;               //account selected
            public string treeExpandedLst;      //list of expanded accounts
        }

        //User config
        public class AppStateRootServer {
            public string   url;            //identifier-url (will concatenate "_index.json")
            public string   lastDb;         //last Db logged-in.
            public string   lastUsername;   //last username logged-in.
            //frms
            public AppStateFrmContab frmContab;
            public AppStateFrmSelCuenta frmAccSel;
            //download
            public int      descVersion;    //last downloaded description version
            public string   descFilename;   //last downloaded description filename
            public ServerDesc descPayload;  //last downloaded description data
        }

        public class AppStateRoot {
            public string lastServerUrl;    //last server logged-in
            public AppStateRootServer[] servers;
        }

        //Server description index

        public class ServerDescIndex {
            public int version;     //current version
            public string filename;    //filename
        }

        //Server description payload

        public class ServerDescTunnelMask {
            public int seed;
        }

        public class ServerDescTunnel {
            public string server;
            public int port;
            public string[] layers;
            public ServerDescTunnelMask mask;
            //ToDo: ssl
        }

        public class ServerDesc {
            public ServerDescTunnel tunnel;
            public string[] dbs;       //exposed dbs for autofill (other dbs can be available)
            public string logoPng128;   //base64 png logo data
        }

        //Instances

        private static AppStateRoot _appState = null;

        public static string getAppDataFolderpath() {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (folderPath != null) {
                if (folderPath.Length > 0 && folderPath[folderPath.Length - 1] != '\\' && folderPath[folderPath.Length - 1] != '/') {
                    folderPath += "\\";
                }
                return folderPath + "Contab\\";
            }
            return null;
        }

        public static string getAppStateFilepath() {
            return getAppDataFolderpath() + "app-state.json";
        }

        public static ClsAppState.AppStateRoot getAppState() {
            //load (if necesary)
            if (_appState == null) {
                string filepath = getAppStateFilepath();
                if (filepath != null && filepath.Length > 0) {
                    try {
                        string jsonStr = System.IO.File.ReadAllText(filepath);
                        ClsAppState.AppStateRoot appState = JSONParser.FromJson<ClsAppState.AppStateRoot>(jsonStr);
                        _appState = appState;
                    } catch (Exception) {
                        //
                    }
                }
                //load servers
                if (_appState != null) {
                    int i; for (i = 0; i < _appState.servers.Length; i++) {
                        AppStateRootServer s = _appState.servers[i];
                        if (s.descVersion > 0 && s.descFilename != null && s.descFilename.Length > 0) {
                            string filepath2 = getAppDataFolderpath() + s.descFilename;
                            try {
                                string jsonStr = System.IO.File.ReadAllText(filepath2);
                                ClsAppState.ServerDesc descPayload = JSONParser.FromJson<ClsAppState.ServerDesc>(jsonStr);
                                s.descPayload = descPayload;
                                //Debug.WriteLine("Server desc loaded: " + jsonStr);
                            } catch (Exception) {
                                //Debug.WriteLine("Server desc failed to loaded.");
                                s.descVersion = 0;
                                s.descFilename = null;
                                s.descPayload = null;
                            }
                        } else {
                            s.descVersion = 0;
                            s.descFilename = null;
                            s.descPayload = null;
                        }
                    }
                }
                //default
                if (_appState == null) {
                    _appState = new ClsAppState.AppStateRoot();
                }
            }
            return _appState;
        }

        public static ClsAppState.AppStateRootServer getServerStateCurrent() {
            ClsAppState.AppStateRootServer r = null;
            if (_appState != null && _appState.lastServerUrl != null && _appState.lastServerUrl.Length > 0) {
                r = getServerState(_appState.lastServerUrl);
            }
            return r;
        }

        public static ClsAppState.AppStateRootServer getServerState(string serverUrl) {
            ClsAppState.AppStateRootServer r = null;
            ClsAppState.AppStateRoot state = getAppState(); //using method to force initial load
            if (state != null && state.servers != null) {
                int i; for (i = 0; i < state.servers.Length; i++) {
                    AppStateRootServer s = state.servers[i];
                    if (s.url == serverUrl) {
                        r = s;
                        break;
                    }
                }
            }
            return r;
        }

        private static bool saveServerDesc(ClsAppState.AppStateRootServer serverState, string descFileContentToSave) {
            bool r = false;
            if (serverState.url != null && serverState.url.Length > 0 && serverState.descVersion > 0 && serverState.descPayload != null && descFileContentToSave != null) {
                //create folder (if necesary)
                string folderpath = getAppDataFolderpath();
                bool folderExists = Directory.Exists(folderpath);
                if (!folderExists) {
                    try {
                        Directory.CreateDirectory(folderpath);
                        folderExists = true;
                    } catch (Exception) {
                        //
                    }
                }
                serverState.descFilename = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(serverState.url)) + ".json";
                string filepath = folderpath + serverState.descFilename;
                if (folderExists && filepath != null && filepath.Length > 0) {
                    try {
                        System.IO.File.WriteAllText(filepath, descFileContentToSave);
                        r = true;
                    } catch (Exception) {
                        //
                    }
                }
            }
            return r;
        }

        public static bool setServerState(ClsAppState.AppStateRootServer serverState, string descFileContentToSave) {
            bool r = false;
            if (serverState != null && serverState.url != null && serverState.url.Length > 0) {
                ClsAppState.AppStateRoot state = getAppState(); //using method to force initial load
                if (state != null && state.servers != null) {
                    int i; for (i = 0; i < state.servers.Length; i++) {
                        AppStateRootServer s = state.servers[i];
                        if (s.url == serverState.url) {
                            s = serverState;
                            if (descFileContentToSave != null && descFileContentToSave.Length > 0) {
                                saveServerDesc(serverState, descFileContentToSave);
                            }
                            r = true;
                            break;
                        }
                    }
                }
                //add new
                if (!r) {
                    AppStateRootServer[] nArr = new AppStateRootServer[state.servers != null ? state.servers.Length + 1 : 1];
                    if (state.servers != null) {
                        int i2; for (i2 = 0; i2 < state.servers.Length; i2++) {
                            nArr[i2] = state.servers[i2];
                        }
                    }
                    nArr[nArr.Length - 1] = serverState;
                    state.servers = nArr;
                    if (descFileContentToSave != null && descFileContentToSave.Length > 0) {
                        saveServerDesc(serverState, descFileContentToSave);
                    }
                    r = true;
                }
                //save
                if (r) {
                    r = setAppState(state);
                }
            }
            return r;
        }

        public static bool setAppState(ClsAppState.AppStateRoot appState) {
            bool r = false;
            if (appState != null) {
                //create folder (if necesary)
                string folderpath = getAppDataFolderpath();
                bool folderExists = Directory.Exists(folderpath);
                if (!folderExists) {
                    try {
                        Directory.CreateDirectory(folderpath);
                        folderExists = true;
                    } catch (Exception) {
                        //
                    }
                }
                //file
                string filepath = getAppStateFilepath();
                if (folderExists && filepath != null && filepath.Length > 0) {
                    string jsonStr = "";
                    jsonStr += "{\n";
                    jsonStr += "    \"lastServerUrl\": \"" + (appState.lastServerUrl != null ? appState.lastServerUrl : "") + "\"\n";
                    jsonStr += "    , \"servers\": [\n";
                    if (appState.servers != null) {
                        int i; for (i = 0; i < appState.servers.Length; i++) {
                            AppStateRootServer s = appState.servers[i];
                            if (i != 0) jsonStr += "        ,\n";
                            jsonStr += "        {\n";
                            jsonStr += "            \"url\": \"" + (s.url != null ? s.url : "") + "\"\n";
                            jsonStr += "            , \"lastDb\": \"" + (s.lastDb != null ? s.lastDb : "") + "\"\n";
                            jsonStr += "            , \"lastUsername\": \"" + (s.lastUsername != null ? s.lastUsername : "") + "\"\n";
                            //frmContab
                            if(s.frmContab != null){
                                jsonStr += "            , \"frmContab\": {\n";
                                jsonStr += "                \"showBalances\": " + (s.frmContab.showBalances ? "true" : "false") + "\n";
                                jsonStr += "                , \"showBalancesSep\": " + (s.frmContab.showBalancesSep ? "true" : "false") + "\n";
                                if (s.frmContab.treeExpandedLst != null && s.frmContab.treeExpandedLst.Length > 0) {
                                    jsonStr += "                , \"treeExpandedLst\": \"" + s.frmContab.treeExpandedLst + "\"\n";
                                }
                                jsonStr += "            }\n";
                            }
                            if (s.frmAccSel != null) {
                                jsonStr += "            , \"frmAccSel\": {\n";
                                jsonStr += "                \"accSel\": " + (s.frmAccSel.accSel != null ? "\"" + s.frmAccSel.accSel + "\"" : "") + "\n";
                                if (s.frmAccSel.treeExpandedLst != null && s.frmAccSel.treeExpandedLst.Length > 0) {
                                    jsonStr += "                , \"treeExpandedLst\": \"" + s.frmAccSel.treeExpandedLst + "\"\n";
                                }
                                jsonStr += "            }\n";
                            }

                            //
                            jsonStr += "            , \"descVersion\": " + s.descVersion + "\n";
                            jsonStr += "            , \"descFilename\": \"" + (s.descFilename != null ? s.descFilename : "") + "\"\n";
                            jsonStr += "        }\n";
                        }
                    }
                    jsonStr += "    ]\n";
                    jsonStr += "}";
                    try {
                        System.IO.File.WriteAllText(filepath, jsonStr);
                        //
                        if (_appState != appState) {
                            _appState = appState;
                        }
                        //
                        r = true;
                    } catch (Exception) {
                        //
                    }
                }
            }
            return r;
        }

    }
}
