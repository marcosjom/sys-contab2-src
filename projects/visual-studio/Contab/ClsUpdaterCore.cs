using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
//
using System.Timers;
using System.Diagnostics;
//
using System.IO.Compression;    //for zip
//
using System.Runtime.InteropServices;
using static System.Windows.Forms.AxHost;
using System.Windows.Forms;
using System.Numerics;
using System.Runtime.ConstrainedExecution;
using System.Net.NetworkInformation;
using Microsoft.Win32;
using System.Security.Policy;
using Contab.Properties;
using System.Collections;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
//using System.Windows.Forms;

//
namespace Contab {
    public class STEmbbededSourceHdr {
        public string srcProtocol;
        public string srcServer;
        public int srcPort;
        public string srcPath;
        public string srcChannel;
        public string ver;
        public string defServerUrl; //for custom-installers, this optional value specifies the default server url.
    }

    class ClsUpdaterCore {

        private static ClsUpdaterCore _sharedInstance = null;

        static String CA_UPDATER_APP_CLIENT_EXE     = "contab.exe"; //must match with remote-index's value
        static String CA_UPDATER_APP_ID             = "contab"; //must match with remote-index's value
        static String CA_UPDATER_APP_FOLDER_NAME    = "MOContab"; //folder name on "Program Files" and "AppData"
        static String CA_UPDATER_APP_REGISTRY_NAME  = "MOContab"; //name on registry for uninstall 'key'
        static int CA_UPDATER_TIMER_MS_CONTINUATION = 250;      //time analyze the state of a running task
        static int CA_UPDATER_TIMER_MS_MIN          = 3000;     //time for first attempt
        static int CA_UPDATER_TIMER_MS_INCREMENT    = 10000;    //increment for next attempt after failure
        static int CA_UPDATER_TIMER_MS_MAX          = 60000;    //max time between attempts (after repeated failures)

        public static ClsUpdaterCore getSharedInstance() {
            if (_sharedInstance == null) {
                _sharedInstance = new ClsUpdaterCore();
            }
            return _sharedInstance;
        }

        struct STTimer {
            public int msPerTick; //is not lineal
            public System.Timers.Timer cur;
        }

        class STSync {
            public ClsUpdaterCore instance;
            public int curSyncDepth;
            public STTimer timer;
            //lastError
            public bool lastWasError;
            public String lastErrorDesc;
        }

        struct STSourceLocalChannelVer {
            public int major;     //1.x.x
            public int minor;     //x.1.x
            public int compile;   //x.x.1
            public bool rootFound; //folder found
            public ulong installDate;     //current version installation time
            public ulong installBootDate; //current version installation boot time (to detect need to restart)
        }
        struct STSourceLocalChannel {
            public int iSeq;  //determines if the local values changed
            public String uid;
            public STSourceLocalChannelVer version;
        }

        struct STSourceLocal {
            public int iSeq;  //determines if the local values changed
            public String protocol;
            public String server;
            public int port;
            public String path;
            public ulong curBootDate;
            //channel
            public STSourceLocalChannel channel;
        }

        struct STSourceSelectedChannel {
            public int iSeq;  //determines if the selection changed
            public String uid;
        }

        class STSourceSelected {
            public int iSeq;        //determines if the selection changed
            public bool isExplicit; //defined by user
            public String protocol;
            public String server;
            public int port;
            public String path;
            public STSourceSelectedChannel channel;
            //loaded index (from this source)
            public STLoaded loaded;
        }

        struct STLoadedChannelVerLatestVer {
            public int major;     //1.x.x
            public int minor;     //x.1.x
            public int compile;   //x.x.1
        }
        struct STLoadedChannelVerLatest {
            public String filename; //zip/dmg
            public String pass;     //zip/dmg password
            public STLoadedChannelVerLatestVer version;
        }

        struct STLoadedChannelVerPayloadVer {
            public STSourceSelected source;
            public int major;     //1.x.x
            public int minor;     //x.1.x
            public int compile;   //x.x.1
        }

        struct STLoadedChannelVerPayloadExtractedVer {
            public STSourceSelected source;
            public int major;     //1.x.x
            public int minor;     //x.1.x
            public int compile;   //x.x.1
        }

        struct STLoadedChannelVerPayloadActionVer {
            public STSourceSelected source;
            public int major;     //1.x.x
            public int minor;     //x.1.x
            public int compile;   //x.x.1
        }

        struct STLoadedChannelVerPayloadAction {
            public bool allowInstallation;    //install using remote script aciton
            public bool allowUninstallation;  //uninstall using remote script aciton (when local script is not available)
            public STLoadedChannelVerPayloadActionVer version;
        }

        struct STLoadedChannelVerPayloadExtracted {
            public bool allowExtraction;
            public String dmgPath;
            public String mountPath;        //dmg mount path (multiple dmgs could be mounted inside)
            public String installScriptPath; //sh file-path
            public String removeScriptPath; //sh file-path
            public STLoadedChannelVerPayloadExtractedVer version;
            public STLoadedChannelVerPayloadAction action;
        }

        struct STLoadedChannelVerPayload {
            public bool allowDownload;
            public STLoadedChannelVerPayloadVer version;
            public byte[] data; //zip
            public STLoadedChannelVerPayloadExtracted extracted;
        }

        struct STLoadedChannelVer {
            public STLoadedChannelVerLatest latest;
        }

        struct STLoadedChannel {
            public int iSeq;  //determines if the loaded channel index changed
            public String uid;
            public String index; //json
            public String[] versions; //NSString(filename), NSString(version), ..., NSString(id), NSString(localized-name) array
            public STLoadedChannelVer version;
        }

        struct STLoaded {
            public int iSeq;  //determines if the loaded info changed
            public String protocol;
            public String server;
            public int port;
            public String path;
            public String index; //json
            public String[] channels; //NSString(id), NSString(localized-name), ..., NSString(id), NSString(localized-name) array
            public STLoadedChannel channel;
        }

        class STSource {
            //local (loaded from system's current installation)
            public STSourceLocal local;
            //
            //remote definition (auto-selected or selected by user)
            public STSourceSelected remote = new STSourceSelected();
            //embedded definition (can be selected or embedded)
            public STSourceSelected embedded = new STSourceSelected();
            //
            //active definition (remote or embedded)
            public STSourceSelected active = null;
            //payload (downloaded, extracted, installed/uinstalled)
            public STLoadedChannelVerPayload payload;
        }

        class STState {
            public bool isInvalidated; //stopFlag
            public STSync syncRemote;  //remote request
            public STSync syncPayload; //download, extract, run-script
            public STSource source = new STSource();
        };

        //instance variables
        private STState _state = new STState();
        private string _tmpFolderWithSlah = "";
        private HttpClient _httpClt = null;
        private Task<string> _httpTask = null;
        private Task<byte[]> _httpTaskBytes = null;
        public ClsUpdaterCore() {
            //
            _state.syncRemote = new STSync();
            _state.syncRemote.instance = this;
            //
            _state.syncPayload = new STSync();
            _state.syncPayload.instance = this;
            //
            _tmpFolderWithSlah = Path.GetTempPath();
            if (_tmpFolderWithSlah.Length > 0 && _tmpFolderWithSlah[_tmpFolderWithSlah.Length - 1] != '\\') {
                _tmpFolderWithSlah += "\\";
            }
            //
            this.loadSourceLocal();
        }

        
        //To allow pnputil availability.
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool IsWow64Process(IntPtr hProcess, out bool Wow64Process);
        //To allow pnputil availability.
        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        private static extern bool Wow64DisableWow64FsRedirection(out IntPtr OldValue);

        //

        public void invalidate() {
            _state.isInvalidated = true;
        }

        public class STJsonVersion {
            public string version;
            public string channel;
            public string protocol;
            public string server;
            public int port;
            public String path;
            public ulong installDate;
            public ulong installBootDate;
        }

        /*private string getJsonValue(String jsonStr, String fieldName) {
            String val = null;
            string startTag = "\"" + fieldName + "\":";
            int startPos = jsonStr.IndexOf(startTag);
            if (startPos >= 0 && (startPos + startTag.Length) < jsonStr.Length) {
                int endPos0 = -1, endPos1 = -1, endPos2 = -1;
                / *while (startPos < loadedStr.Length && loadedStr[startPos + 1] == ' ') {
                    startPos++;
                }* /
                endPos0 = jsonStr.IndexOf(" ", startPos + startTag.Length);
                endPos1 = jsonStr.IndexOf(",", startPos + startTag.Length);
                endPos2 = jsonStr.IndexOf("}", startPos + startTag.Length);
                if (endPos0 == -1) endPos0 = endPos1;
                if (endPos0 == -1) endPos0 = endPos2;
                if (endPos0 >= 0) {
                    if (endPos1 >= 0 && endPos0 > endPos1) endPos0 = endPos1;
                    if (endPos2 >= 0 && endPos0 > endPos2) endPos0 = endPos2;
                    //get value
                    val = jsonStr.Substring(startPos + startTag.Length, endPos0 - startTag.Length - startPos);
                    //remove start spaces
                    while (val.Length > 0 && val[0] == ' ') val = val.Remove(0, 1);
                    //remove start quote
                    if (val.Length > 0 && val[0] == '\"') val = val.Remove(0, 1);
                    //remove end spaces
                    while (val.Length > 0 && val[val.Length - 1] == ' ') val = val.Remove(val.Length - 1, 1);
                }
            }
            return val;
        }*/

        public void loadSourceLocal() { //loads state of system's current installation
            bool anyLocalRootExist = false;
            ulong installDateI = 0, installBootDateI = 0;
            int majorI = 0, minorI = 0, compileI = 0;
            String channelI = null; String protocolI = null; String serverI = null; int portI = 0; String pathI = null;
            {
                String progFiles = Environment.ExpandEnvironmentVariables("%ProgramW6432%");
                if (progFiles != null) {
                    String path = progFiles + "\\" + CA_UPDATER_APP_FOLDER_NAME + "\\source.json";
                    anyLocalRootExist = true;
                    if (File.Exists(path)) {
                        String loadedStr = File.ReadAllText(path);
                        if (loadedStr == null || loadedStr.Length <= 0) {
                            //empty file
                        } else {
                            try {
                                STJsonVersion loaded = loadedStr.FromJson<STJsonVersion>();
                                //version
                                String version = loaded.version; //this.getJsonValue(loadedStr, "version");
                                if (version != null && version.Length > 0) {
                                    int major = 0, minor = 0, compile = 0;
                                    if (parseVersion(version, ref major, ref minor, ref compile)){
                                        majorI = major;
                                        minorI = minor;
                                        compileI = compile;
                                    } else {
                                        Debug.WriteLine("Version (installed): version('" + version + "') is not valid.\n");
                                    }
                                }
                                //channel
                                {
                                    String v = loaded.channel; //this.getJsonValue(loadedStr, "channel");
                                    if (v != null && v.Length > 0) {
                                        channelI = v;
                                    }
                                }
                                //protocol
                                {
                                    String v = loaded.protocol; //this.getJsonValue(loadedStr, "protocol");
                                    if (v != null && v.Length > 0) {
                                        protocolI = v;
                                    }
                                }
                                //server
                                {
                                    String v = loaded.server; //this.getJsonValue(loadedStr, "server");
                                    if (v != null && v.Length > 0) {
                                        serverI = v;
                                    }
                                }
                                //port
                                {
                                    if (loaded.port > 0) {
                                        portI = loaded.port;
                                    }
                                }
                                //path
                                {
                                    String v = loaded.path; //this.getJsonValue(loadedStr, "path");
                                    if (v != null && v.Length > 0) {
                                        pathI = v;
                                    }
                                }
                                //installDate
                                {
                                    if(loaded.installDate > 0) {
                                        installDateI = loaded.installDate;
                                    }
                                }
                                //installBootDate
                                {
                                    if (loaded.installBootDate > 0){
                                        installBootDateI = loaded.installBootDate;
                                    }
                                }
                                //load from registry
                                {
                                    //installBootDate
                                    {
                                        string installBootDate = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\uninstall\" + CA_UPDATER_APP_REGISTRY_NAME, "InstallBootDate", null);
                                        if (installBootDate != null && installBootDate.Length > 0) {
                                            ulong v = ulong.Parse(installBootDate);
                                            if (v > 0) {
                                                installBootDateI = v;
                                            }
                                        }
                                    }
                                    //installDateStamp
                                    {
                                        string installDateStamp = (string)Registry.GetValue(@"HKEY_LOCAL_MACHINE\Software\Microsoft\Windows\CurrentVersion\uninstall\" + CA_UPDATER_APP_REGISTRY_NAME, "InstallDateStamp", null);
                                        if (installDateStamp != null && installDateStamp.Length > 0) {
                                            ulong v = ulong.Parse(installDateStamp);
                                            if (v > 0){
                                                installDateI = v;
                                            }
                                        }
                                    }
                                }
                            } catch (Exception e) {
                                //MessageBox.Show("Excepcion al cargar source.json: " + e);
                            }
                        }
                    }
                }
            }
            _state.source.local.channel.version.rootFound = anyLocalRootExist;
            _state.source.local.channel.version.major = majorI;
            _state.source.local.channel.version.minor = minorI;
            _state.source.local.channel.version.compile = compileI;
            _state.source.local.channel.version.installDate = installDateI;
            _state.source.local.channel.version.installBootDate = installBootDateI;
            _state.source.local.protocol = protocolI;
            _state.source.local.server = serverI;
            _state.source.local.port = portI;
            _state.source.local.path = pathI;
            _state.source.local.channel.uid = channelI;
            //auto-select
            {
                bool autoSelectedAny = false;
                //auto-select source (if not explicit yet)
                if (
                   !_state.source.remote.isExplicit && protocolI != null && protocolI.Length > 0 && serverI != null && serverI.Length > 0 && portI > 0 && pathI != null && pathI.Length > 0
                   &&
                   (
                        (_state.source.remote.protocol == null || _state.source.remote.server == null || _state.source.remote.port <= 0 || _state.source.remote.path == null)
                        || protocolI != _state.source.remote.protocol || serverI != _state.source.remote.server || portI != _state.source.remote.port || pathI != _state.source.remote.path
                    )
                   ) {
                    _state.source.remote.protocol = protocolI;
                    _state.source.remote.server = serverI;
                    _state.source.remote.port = portI;
                    _state.source.remote.path = pathI;
                    _state.source.remote.iSeq++;
                    autoSelectedAny = true;
                    Debug.WriteLine("Channel auto-selected-current (root): " + _state.source.remote.protocol + _state.source.remote.server  + ":" + _state.source.remote.port + _state.source.remote.path);
                }
                //auto-select channel (if not explicit yet)
                if ((_state.source.remote.channel.uid == null || _state.source.remote.channel.uid.Length == 0) && channelI != null && channelI.Length > 0) {
                    _state.source.remote.channel.uid = channelI;
                    _state.source.remote.channel.iSeq++;
                    autoSelectedAny = true;
                    Debug.WriteLine("Channel auto-selected-current (root): " + _state.source.remote.channel.uid);
                }
                //trigger first action
                if (autoSelectedAny) {
                    STSync sync = _state.syncRemote;
                    if (sync.timer.cur == null && sync.curSyncDepth == 0) {
                        Debug.WriteLine("LocalLoaded, iniciando timer.\n");
                        this.timerStart(sync);
                    } else {
                        Debug.WriteLine("LocalLoaded, timer ya estaba activo curSyncDepth(" + sync.curSyncDepth + ").\n");
                    }
                }
            }
        }

        public void setDefaultSourceSelected(String protocol, String server, int port, String path) {     //applies the current selected source (it will trigger the continous attempt of loading the server  and channel indexes)
            if (protocol != null && server != null && port > 0 && path != null) {
                if (
                   !_state.source.remote.isExplicit && protocol != null && protocol.Length > 0 && server != null && server.Length > 0 && port > 0 && path != null && path.Length > 0
                   &&
                   (
                       _state.source.remote.protocol == null || _state.source.remote.server == null || _state.source.remote.port <= 0 || _state.source.remote.path == null
                       || protocol != _state.source.remote.protocol || server != _state.source.remote.server || port != _state.source.remote.port || path != _state.source.remote.path
                    )
                   ) {
                    _state.source.remote.protocol = protocol;
                    _state.source.remote.server = server;
                    _state.source.remote.port = port;
                    _state.source.remote.path = path;
                    _state.source.remote.iSeq++;
                    //trigger first action
                    STSync sync = _state.syncRemote;
                    if (sync.timer.cur == null && sync.curSyncDepth == 0) {
                        Debug.WriteLine("SetDefaultSource, iniciando timer.\n");
                        this.timerStart(sync);
                    } else {
                        Debug.WriteLine("SetDefaultSource, timer ya estaba activo curSyncDepth(" + sync.curSyncDepth + ").\n");
                    }
                }
            }
        }

        public void setSourceSelected(String protocol, String server, int port, String path) {     //applies the current selected source (it will trigger the continous attempt of loading the server  and channel indexes)
            if (protocol != null && server != null && port > 0 && path != null) {
                if (
                   protocol != null && protocol.Length > 0 && server != null && server.Length > 0 && port > 0 && path != null && path.Length > 0
                   && (
                       _state.source.remote.protocol == null || _state.source.remote.server == null || _state.source.remote.port <= 0 || _state.source.remote.path == null
                       || protocol != _state.source.remote.protocol || server != _state.source.remote.server || port != _state.source.remote.port || path != _state.source.remote.path
                    )
                   ) {
                    _state.source.remote.isExplicit = true;
                    _state.source.remote.protocol = protocol;
                    _state.source.remote.server = server;
                    _state.source.remote.port = port;
                    _state.source.remote.path = path;
                    _state.source.remote.iSeq++;
                    //trigger first action
                    STSync sync = _state.syncRemote;
                    if (sync.timer.cur == null && sync.curSyncDepth == 0) {
                        Debug.WriteLine("SetSource, iniciando timer.\n");
                        this.timerStart(sync);
                    } else {
                        Debug.WriteLine("SetSource, timer ya estaba activo curSyncDepth(" + sync.curSyncDepth + ").\n");
                    }
                }
            }
        }

        public void getSourceSelected(ref String dstProtocol, ref String dstServer, ref int dstPort, ref String dstPath, ref int dstChangeSeq) {
            /*if (dstProtocol != null)*/
            dstProtocol = _state.source.remote.protocol != null ? _state.source.remote.protocol : "";
            /*if (dstServer != null)*/
            dstServer = _state.source.remote.server != null ? _state.source.remote.server : "";
            /*if (dstPort != null)*/
            dstPort = _state.source.remote.port;
            /*if (dstPath != null)*/
            dstPath = _state.source.remote.path != null ? _state.source.remote.path : "";
            /*if (dstChangeSeq != null)*/
            dstChangeSeq = _state.source.remote.iSeq;
        }

        public void setSourceChannel(String channel) { //applies the current selected channel (it will trigger the continous attempt of loading the channel index)
            if (channel != null) {
                if (
                   _state.source.remote.channel.uid == null
                   || channel != _state.source.remote.channel.uid
                   ) {
                    _state.source.remote.channel.uid = channel;
                    _state.source.remote.channel.iSeq++;
                    //trigger first action
                    STSync sync = _state.syncRemote;
                    if (sync.timer.cur == null && sync.curSyncDepth == 0) {
                        Debug.WriteLine("SetSourceChannel, iniciando timer.\n");
                        this.timerStart(sync);
                    } else {
                        Debug.WriteLine("SetSourceChannel, timer ya estaba activo curSyncDepth(" + sync.curSyncDepth + ").\n");
                    }
                }
            }
        }

        public void getSourceChannelSelected(ref String dstChannel, ref int dstChangeSeq) {
            /*if (dstChannel != null)*/
            dstChannel = (_state.source.remote.channel.uid != null ? _state.source.remote.channel.uid : "");
            /*if (dstChangeSeq != null)*/
            dstChangeSeq = _state.source.remote.channel.iSeq;
        }

        public void getSourceChannelsList(ref String[] dstPairList, ref int dstChangeSeq) {
            /*if (dstPairList != null)*/
            {
                if (_state.source.remote.loaded.channels == null) {
                    dstPairList = null;
                } else {
                    dstPairList = _state.source.remote.loaded.channels;
                }
            }
            /*if (dstChangeSeq != null)*/
            dstChangeSeq = _state.source.remote.loaded.iSeq;
        }


        public void activateInstallAction() {
            //reset error to active action
            _state.syncRemote.lastWasError = false;
            _state.syncRemote.lastErrorDesc = null;
            _state.syncPayload.lastWasError = false;
            _state.syncPayload.lastErrorDesc = null;
            //set state
            _state.source.active = _state.source.remote;
            _state.source.payload.allowDownload = true;
            _state.source.payload.extracted.allowExtraction = true;
            _state.source.payload.extracted.action.allowInstallation = true;
            _state.source.payload.extracted.action.allowUninstallation = false;
            _state.source.payload.extracted.action.version.major = 0;
            _state.source.payload.extracted.action.version.minor = 0;
            _state.source.payload.extracted.action.version.compile = 0;
            //trigger first action
            STSync sync = _state.syncRemote;
            if (sync.timer.cur == null && sync.curSyncDepth == 0) {
                Debug.WriteLine("InstallAction activada, iniciando timer.\n");
                this.timerStart(sync);
            } else {
                Debug.WriteLine("InstallAction activada, timer ya estaba activo curSyncDepth(" + sync.curSyncDepth + ").\n");
            }
        }

        public void activateInstallActionEmbedded(STEmbbededSourceHdr hdr, byte[] pkg_zip){
            STSync sync = _state.syncPayload;
            //reset error to active action
            sync.lastWasError = false;
            sync.lastErrorDesc = null;
            //set active version
            {
                _state.source.embedded.isExplicit = true;
                _state.source.embedded.protocol = hdr.srcProtocol;
                _state.source.embedded.server = hdr.srcServer;
                _state.source.embedded.port = hdr.srcPort;
                _state.source.embedded.path = hdr.srcPath;
                _state.source.embedded.channel.uid = hdr.srcChannel;
                _state.source.embedded.channel.iSeq++;
                _state.source.embedded.iSeq++;
                //artificially populate index (as if was downloaded from internet)
                {
                    _state.source.embedded.loaded.iSeq = _state.source.embedded.iSeq;
                    _state.source.embedded.loaded.index = " { } "; //embedded
                    _state.source.embedded.loaded.protocol = hdr.srcProtocol;
                    _state.source.embedded.loaded.server = hdr.srcServer;
                    _state.source.embedded.loaded.port = hdr.srcPort;
                    _state.source.embedded.loaded.path = hdr.srcPath;
                    _state.source.embedded.loaded.channel.iSeq = _state.source.embedded.channel.iSeq;
                    _state.source.embedded.loaded.channel.index = " { } "; //embedded
                    _state.source.embedded.loaded.channel.uid = hdr.srcChannel;
                    _state.source.embedded.loaded.channel.versions = new string[1];
                    _state.source.embedded.loaded.channel.versions[0] = hdr.ver;
                    _state.source.embedded.loaded.channel.version.latest.filename = "contab_embedded_" + hdr.ver + ".zip";
                    _state.source.embedded.loaded.channel.version.latest.pass = "";
                    {
                        int major = 0, minor = 0, compile = 0;
                        if (parseVersion(hdr.ver, ref major, ref minor, ref compile)) {
                            _state.source.embedded.loaded.channel.version.latest.version.major = major;
                            _state.source.embedded.loaded.channel.version.latest.version.minor = minor;
                            _state.source.embedded.loaded.channel.version.latest.version.compile = compile;
                        } else {
                            sync.lastWasError = true;
                            sync.lastErrorDesc = "Error parsing embedded version.";
                            Debug.WriteLine("Version (channel): version(" + hdr.ver + ") is not valid.\n");
                        }
                    }
                }
            }
            //set state
            _state.source.active = _state.source.embedded; //use embeded as source
            _state.source.payload.allowDownload = true;
            _state.source.payload.extracted.allowExtraction = true;
            _state.source.payload.extracted.action.allowInstallation = true;
            _state.source.payload.extracted.action.allowUninstallation = false;
            _state.source.payload.extracted.action.version.major = 0;
            _state.source.payload.extracted.action.version.minor = 0;
            _state.source.payload.extracted.action.version.compile = 0;
            //artificially populate download
            {
                _state.source.payload.version.source = _state.source.active;
                _state.source.payload.version.major = _state.source.active.loaded.channel.version.latest.version.major;
                _state.source.payload.version.minor = _state.source.active.loaded.channel.version.latest.version.minor;
                _state.source.payload.version.compile = _state.source.active.loaded.channel.version.latest.version.compile;
                _state.source.payload.data = pkg_zip;
            }
            //trigger first action
            if (sync.timer.cur == null && sync.curSyncDepth == 0){
                Debug.WriteLine("InstallAction activada, iniciando timer.\n");
                this.timerStart(sync);
            } else {
                Debug.WriteLine("InstallAction activada, timer ya estaba activo curSyncDepth(" + sync.curSyncDepth + ").\n");
            }
        }

        public void activateUninstallAction(STEmbbededSourceHdr hdr, byte[] pkg_zip) {
            bool canDoOfflineUninstall = (hdr != null && hdr.ver != null && hdr.ver.Length > 0 && pkg_zip != null);
            bool canDoOnlineUninstall = (_state.source.remote.loaded.channel.version.latest.version.major > 0 || _state.source.remote.loaded.channel.version.latest.version.minor > 0 || _state.source.remote.loaded.channel.version.latest.version.compile > 0);
            bool doOfflineInstall = false;
            bool isCanceled = false;
            //validate embedded version
            int offlineMajor = 0, offlineMinor = 0, offlineCompile = 0;
            if (canDoOfflineUninstall){
                int major = 0, minor = 0, compile = 0;
                if (parseVersion(hdr.ver, ref major, ref minor, ref compile)) {
                    offlineMajor = major;
                    offlineMinor = minor;
                    offlineCompile = compile;
                } else {
                    canDoOfflineUninstall = false;
                    Debug.WriteLine("Version (channel): version(" + hdr.ver + ") is not valid.\n");
                }
                
            }
            //
            if (canDoOfflineUninstall && canDoOnlineUninstall) {
                if (_state.source.remote.loaded.channel.version.latest.version.major > 0 && _state.source.remote.loaded.channel.version.latest.version.minor > 0 && _state.source.remote.loaded.channel.version.latest.version.compile > 0) {
                    if (offlineMajor < _state.source.remote.loaded.channel.version.latest.version.major || (offlineMajor == _state.source.remote.loaded.channel.version.latest.version.major && offlineMinor < _state.source.remote.loaded.channel.version.latest.version.minor) || (offlineMajor == _state.source.remote.loaded.channel.version.latest.version.major && offlineMinor == _state.source.remote.loaded.channel.version.latest.version.minor && offlineCompile < _state.source.remote.loaded.channel.version.latest.version.compile)){
                        switch (MessageBox.Show("A newer version of the installer is available, do you want to download and uninstall using the newer version? If you select 'No' this installer will be used.", "Download installer?", MessageBoxButtons.YesNoCancel)) {
                            case DialogResult.Yes:
                                doOfflineInstall = false;
                                break;
                            case DialogResult.No:
                                doOfflineInstall = true;
                                break;
                            default:
                                isCanceled = true;
                                break;
                        }
                    } else {
                        //offline installer is current remote version
                        doOfflineInstall = true;
                    }
                } else { 
                    switch (MessageBox.Show("Do you want to download and uninstall using the lastest version? If you select 'No' this installer will be used.", "Download installer?", MessageBoxButtons.YesNoCancel)) {
                        case DialogResult.Yes:
                            doOfflineInstall = false;
                            break;
                        case DialogResult.No:
                            doOfflineInstall = true;
                            break;
                        default:
                            isCanceled = true;
                            break;
                    }
                }
            }
            //action
            if (!isCanceled) {
                //reset error to active action
                _state.syncRemote.lastWasError = false;
                _state.syncRemote.lastErrorDesc = null;
                _state.syncPayload.lastWasError = false;
                _state.syncPayload.lastErrorDesc = null;
                if (doOfflineInstall) { 
                    //embedded installer
                    //set active version
                    {
                        _state.source.embedded.isExplicit = true;
                        _state.source.embedded.protocol = hdr.srcProtocol;
                        _state.source.embedded.server = hdr.srcServer;
                        _state.source.embedded.port = hdr.srcPort;
                        _state.source.embedded.path = hdr.srcPath;
                        _state.source.embedded.channel.uid = hdr.srcChannel;
                        _state.source.embedded.channel.iSeq++;
                        _state.source.embedded.iSeq++;
                        //artificially populate index (as if was downloaded from internet)
                        {
                            _state.source.embedded.loaded.iSeq = _state.source.embedded.iSeq;
                            _state.source.embedded.loaded.index = " { } "; //embedded
                            _state.source.embedded.loaded.protocol = hdr.srcProtocol;
                            _state.source.embedded.loaded.server = hdr.srcServer;
                            _state.source.embedded.loaded.port = hdr.srcPort;
                            _state.source.embedded.loaded.path = hdr.srcPath;
                            _state.source.embedded.loaded.channel.iSeq = _state.source.embedded.channel.iSeq;
                            _state.source.embedded.loaded.channel.index = " { } "; //embedded
                            _state.source.embedded.loaded.channel.uid = hdr.srcChannel;
                            _state.source.embedded.loaded.channel.versions = new string[1];
                            _state.source.embedded.loaded.channel.versions[0] = hdr.ver;
                            _state.source.embedded.loaded.channel.version.latest.filename = "contab_embedded_" + hdr.ver + ".zip";
                            _state.source.embedded.loaded.channel.version.latest.pass = "";
                            {
                                _state.source.embedded.loaded.channel.version.latest.version.major = offlineMajor;
                                _state.source.embedded.loaded.channel.version.latest.version.minor = offlineMinor;
                                _state.source.embedded.loaded.channel.version.latest.version.compile = offlineCompile;
                            }
                        }
                    }
                    //set state
                    _state.source.active = _state.source.embedded; //use embeded as source
                    _state.source.payload.allowDownload = true;
                    _state.source.payload.extracted.allowExtraction = true;
                    _state.source.payload.extracted.action.allowInstallation = false;
                    _state.source.payload.extracted.action.allowUninstallation = true;
                    _state.source.payload.extracted.action.version.major = 0;
                    _state.source.payload.extracted.action.version.minor = 0;
                    _state.source.payload.extracted.action.version.compile = 0;
                    //artificially populate download
                    {
                        _state.source.payload.version.source = _state.source.active;
                        _state.source.payload.version.major = _state.source.active.loaded.channel.version.latest.version.major;
                        _state.source.payload.version.minor = _state.source.active.loaded.channel.version.latest.version.minor;
                        _state.source.payload.version.compile = _state.source.active.loaded.channel.version.latest.version.compile;
                        _state.source.payload.data = pkg_zip;
                    }
                    //trigger first action
                    STSync sync = _state.syncRemote;
                    if (sync.timer.cur == null && sync.curSyncDepth == 0) {
                        Debug.WriteLine("UninstallAction activada, iniciando timer.\n");
                        this.timerStart(sync);
                    } else {
                        Debug.WriteLine("UninstallAction activada, timer ya estaba activo curSyncDepth(" + sync.curSyncDepth + ").\n");
                    }
                } else {
                    //remote installer
                    //set state
                    {
                        //uninstalling using the remote script
                        _state.source.active = _state.source.remote;
                        _state.source.payload.allowDownload = true;
                        _state.source.payload.extracted.allowExtraction = true;
                        _state.source.payload.extracted.action.allowInstallation = false;
                        _state.source.payload.extracted.action.allowUninstallation = true;
                        _state.source.payload.extracted.action.version.major = 0;
                        _state.source.payload.extracted.action.version.minor = 0;
                        _state.source.payload.extracted.action.version.compile = 0;
                    }
                    //trigger first action
                    STSync sync = _state.syncPayload;
                    if (sync.timer.cur == null && sync.curSyncDepth == 0) {
                        Debug.WriteLine("UninstallAction activada, iniciando timer.\n");
                        this.timerStart(sync);
                    } else {
                        Debug.WriteLine("UninstallAction activada, timer ya estaba activo curSyncDepth(" + sync.curSyncDepth + ").\n");
                    }
                }
            }
        }

        //

        public int getLastSyncEventSeq() { //retrieves the last syncronization request
            return _state.source.remote.iSeq;
        }

        public void syncLastVersionIfInactive() { //triggers a synchronization with remote server if no activity is running
            //trigger first action
            STSync sync = _state.syncRemote;
            STSync sync2 = _state.syncPayload;
            if (sync.timer.cur == null && sync.curSyncDepth == 0 && sync2.timer.cur == null && sync2.curSyncDepth == 0) {
                Debug.WriteLine("sync activada, iniciando timer.\n");
                _state.source.remote.iSeq++;
                this.timerStart(sync);
            }
        }

        //

        public bool isSyncingActiveNow() {    //currently connecting or parsing data.
            return (_state.syncRemote.curSyncDepth > 0 || _state.syncPayload.curSyncDepth > 0);
        }

        public bool isLastSyncingAnError() {  //last syncing result produced an error.
            return (_state.source.active == _state.source.embedded ? _state.syncPayload.lastWasError : (_state.syncRemote.lastWasError || _state.syncPayload.lastWasError));
        }

        public String getSyncLastError() {    //decription of syncing last error.
            return (_state.source.active == _state.source.embedded ? _state.syncPayload.lastErrorDesc : (_state.syncRemote.lastErrorDesc != null ? _state.syncRemote.lastErrorDesc : _state.syncPayload.lastErrorDesc));
        }

        //

        public ulong getCurBootDate() {
            ulong r = _state.source.local.curBootDate;
            if (r <= 0) {
                /*struct timeval  boottime;
                size_t size;
                int mib[MIB_SIZE];
                mib[0] = CTL_KERN;
                mib[1] = KERN_BOOTTIME;
                size = sizeof(boottime);
                if (sysctl(mib, MIB_SIZE, &boottime, &size, null, 0) != -1) {
                    r = _state.source.local.curBootDate = boottime.tv_sec; //secs since 1970
                }*/
            }
            return r;
        }

        public void getVersionInstalled(ref int major, ref int minor, ref int compile, ref String dstInstallProtocol, ref String dstInstallServer, ref int dstInstallPort, ref String strInstallPath, ref String dstInstallChannel, ref ulong dstInstallDate, ref ulong dstInstallBootDate) {
            /*if (major != null)*/
            {
                major = _state.source.local.channel.version.major;
            }
            /*if (minor != null)*/
            {
                minor = _state.source.local.channel.version.minor;
            }
            /*if (compile != null)*/
            {
                compile = _state.source.local.channel.version.compile;
            }
            /*if (dstInstallProtocol != null)*/
            {
                if (_state.source.local.protocol == null || _state.source.local.protocol.Length == 0) {
                    dstInstallProtocol = "";
                } else {
                    dstInstallProtocol = _state.source.local.protocol;
                }
            }
            /*if (dstInstallServer != null)*/
            {
                if (_state.source.local.server == null || _state.source.local.server.Length == 0) {
                    dstInstallServer = "";
                } else {
                    dstInstallServer = _state.source.local.server;
                }
            }
            /*if (dstInstallPort != null)*/
            {
                dstInstallPort = _state.source.local.port;
            }
            /*if(strInstallPath != null)*/
            {
                if (_state.source.local.path == null || _state.source.local.path.Length == 0) {
                    strInstallPath = "";
                } else {
                    strInstallPath = _state.source.local.path;
                }
            }
            /*if (dstInstallChannel != null)*/
            {
                if (_state.source.local.channel.uid == null || _state.source.local.channel.uid.Length == 0) {
                    dstInstallChannel = "";
                } else {
                    dstInstallChannel = _state.source.local.channel.uid;
                }
            }
            /*if (dstInstallDate != null)*/
            {
                dstInstallDate = _state.source.local.channel.version.installDate;
            }
            /*if (dstInstallBootDate != null)*/
            {
                dstInstallBootDate = _state.source.local.channel.version.installBootDate;
            }
        }

        public void getVersionLatest(ref int major, ref int minor, ref int compile) {
            major = _state.source.remote.loaded.channel.version.latest.version.major;
            minor = _state.source.remote.loaded.channel.version.latest.version.minor;
            compile = _state.source.remote.loaded.channel.version.latest.version.compile;
        }

        static public bool parseVersion(String ver, ref int major, ref int minor, ref int compile) {
            bool r = false;
            String[] strs = new String[3];
            strs[0] = "";
            strs[1] = "";
            strs[2] = "";
            char c; int iStr = 0; int iPos, len = ver.Length; bool isValid = true;
            for (iPos = 0; iPos < len && iStr < 3; iPos++) {
                c = ver[iPos];
                if (c == '.') {
                    iStr++;
                } else if (c < '0' || c > '9') {
                    isValid = false;
                    break;
                } else {
                    strs[iStr] += c;
                }
            }
            //parse version
            if (isValid) {
                if (strs[0].Length > 0) major = Int32.Parse(strs[0]);
                if (strs[1].Length > 0) minor = Int32.Parse(strs[1]);
                if (strs[2].Length > 0) compile = Int32.Parse(strs[2]);
                r = true;
            }
            return r;
        }

        //

        public bool rootIsSyncing() { //returns true if remote-root-record is-or-will be loaded from server.
            return (
                    _state.source.remote.iSeq != _state.source.remote.loaded.iSeq
                    /*&& !_state.syncRemote.lastWasError; retry is allowed*/
                    );
        }
        public bool channelIsSet() { //returns true if remote-channel is defined.
            return (_state.source.remote.channel.uid != null && _state.source.remote.channel.uid.Length > 0);
        }

        public bool channelIsSyncing() { //returns true if remote-channel-record is-or-will be loaded from server.
            return (
                    _state.source.remote.iSeq == _state.source.remote.loaded.iSeq
                    && _state.source.remote.channel.iSeq != _state.source.remote.loaded.channel.iSeq
                    /*&& !_state.syncRemote.lastWasError; retry is allowed*/
                    );
        }

        public bool versionPayloadDownloadIsAllowed() {
            return _state.source.payload.allowDownload;
        }

        public bool versionPayloadIsDownloading() { //returns true if active-version-payload is-or-will be loaded from server.
            return (
                    _state.source.active != null && _state.source.active.protocol != null && _state.source.active.server != null && _state.source.active.port > 0 && _state.source.active.path != null
                    && _state.source.active.iSeq == _state.source.active.loaded.iSeq
                    && _state.source.active.channel.iSeq == _state.source.active.loaded.channel.iSeq
                    && ((_state.source.active != _state.source.payload.version.source || _state.source.active.loaded.channel.version.latest.version.major != _state.source.payload.version.major || _state.source.active.loaded.channel.version.latest.version.minor != _state.source.payload.version.minor || _state.source.active.loaded.channel.version.latest.version.compile != _state.source.payload.version.compile) && _state.source.payload.allowDownload)
                    /*&& !_state.syncPayload.lastWasError; retry is allowed*/
                    );
        }

        public bool versionPayloadIsExtractIsAllowed() {
            return _state.source.payload.extracted.allowExtraction;
        }

        public bool versionPayloadIsExtracting() { //returns true if active-version-payload is-or-will be exracted from memory.
            return (
                    _state.source.active != null && _state.source.active.protocol != null && _state.source.active.server != null && _state.source.active.port > 0 && _state.source.active.path != null
                    && _state.source.active.iSeq == _state.source.active.loaded.iSeq
                    && _state.source.active.channel.iSeq == _state.source.active.loaded.channel.iSeq
                    && ((_state.source.active == _state.source.payload.version.source && _state.source.active.loaded.channel.version.latest.version.major == _state.source.payload.version.major && _state.source.active.loaded.channel.version.latest.version.minor == _state.source.payload.version.minor && _state.source.active.loaded.channel.version.latest.version.compile == _state.source.payload.version.compile) && _state.source.payload.allowDownload)
                    && ((_state.source.payload.version.source != _state.source.payload.extracted.version.source || _state.source.payload.version.major != _state.source.payload.extracted.version.major || _state.source.payload.version.minor != _state.source.payload.extracted.version.minor || _state.source.payload.version.compile != _state.source.payload.extracted.version.compile) && _state.source.payload.extracted.allowExtraction)
                    && !_state.syncPayload.lastWasError
                    );
        }

        public bool versionPayloadIsInstallIsAllowed() {
            return _state.source.payload.extracted.action.allowInstallation;
        }

        public bool versionPayloadIsInstalling() { //returns true if active-version-payload is-or-will be installed from extraction.
            return (
                    _state.source.active != null && _state.source.active.protocol != null && _state.source.active.server != null && _state.source.active.port > 0 && _state.source.active.path != null
                    && _state.source.active.iSeq == _state.source.active.loaded.iSeq
                    && _state.source.active.channel.iSeq == _state.source.active.loaded.channel.iSeq
                    && ((_state.source.active == _state.source.payload.version.source && _state.source.active.loaded.channel.version.latest.version.major == _state.source.payload.version.major && _state.source.active.loaded.channel.version.latest.version.minor == _state.source.payload.version.minor && _state.source.active.loaded.channel.version.latest.version.compile == _state.source.payload.version.compile) && _state.source.payload.allowDownload)
                    && ((_state.source.payload.version.source == _state.source.payload.extracted.version.source && _state.source.payload.version.major == _state.source.payload.extracted.version.major && _state.source.payload.version.minor == _state.source.payload.extracted.version.minor && _state.source.payload.version.compile == _state.source.payload.extracted.version.compile) && _state.source.payload.extracted.allowExtraction)
                    && ((_state.source.payload.extracted.version.source != _state.source.payload.extracted.action.version.source || _state.source.payload.extracted.version.major != _state.source.payload.extracted.action.version.major && _state.source.payload.extracted.version.minor != _state.source.payload.extracted.action.version.minor && _state.source.payload.extracted.version.compile != _state.source.payload.extracted.action.version.compile) && _state.source.payload.extracted.action.allowInstallation)
                    && !_state.syncPayload.lastWasError
                    );
        }

        public bool versionPayloadIsInstalled() { //returns true if installetion completed
            return (
                    _state.source.active != null && _state.source.active.protocol != null && _state.source.active.server != null && _state.source.active.port > 0 && _state.source.active.path != null
                    && _state.source.active.iSeq == _state.source.active.loaded.iSeq
                    && _state.source.active.channel.iSeq == _state.source.active.loaded.channel.iSeq
                    && ((_state.source.active == _state.source.payload.version.source && _state.source.active.loaded.channel.version.latest.version.major == _state.source.payload.version.major && _state.source.active.loaded.channel.version.latest.version.minor == _state.source.payload.version.minor && _state.source.active.loaded.channel.version.latest.version.compile == _state.source.payload.version.compile) && _state.source.payload.allowDownload)
                    && ((_state.source.payload.version.source == _state.source.payload.extracted.version.source && _state.source.payload.version.major == _state.source.payload.extracted.version.major && _state.source.payload.version.minor == _state.source.payload.extracted.version.minor && _state.source.payload.version.compile == _state.source.payload.extracted.version.compile) && _state.source.payload.extracted.allowExtraction)
                    && ((_state.source.payload.extracted.version.source == _state.source.payload.extracted.action.version.source && _state.source.payload.extracted.version.major == _state.source.payload.extracted.action.version.major && _state.source.payload.extracted.version.minor == _state.source.payload.extracted.action.version.minor && _state.source.payload.extracted.version.compile == _state.source.payload.extracted.action.version.compile) && _state.source.payload.extracted.action.allowInstallation)
                    && !_state.syncPayload.lastWasError
                    );
        }

        public bool versionPayloadIsUninstallIsAllowed() {
            return _state.source.payload.extracted.action.allowUninstallation;
        }

        public bool versionPayloadIsUninstalling() { //returns true if remote-version-payload is-or-will be installed from extraction.
            return (
                    _state.source.active != null && _state.source.active.protocol != null && _state.source.active.server != null && _state.source.active.port > 0 && _state.source.active.path != null
                    && _state.source.active.iSeq == _state.source.active.loaded.iSeq
                    && _state.source.active.channel.iSeq == _state.source.active.loaded.channel.iSeq
                    && ((_state.source.active == _state.source.payload.version.source && _state.source.active.loaded.channel.version.latest.version.major == _state.source.payload.version.major && _state.source.active.loaded.channel.version.latest.version.minor == _state.source.payload.version.minor && _state.source.active.loaded.channel.version.latest.version.compile == _state.source.payload.version.compile) && _state.source.payload.allowDownload)
                    && ((_state.source.payload.version.source == _state.source.payload.extracted.version.source && _state.source.payload.version.major == _state.source.payload.extracted.version.major && _state.source.payload.version.minor == _state.source.payload.extracted.version.minor && _state.source.payload.version.compile == _state.source.payload.extracted.version.compile) && _state.source.payload.extracted.allowExtraction)
                    && ((_state.source.payload.extracted.version.source != _state.source.payload.extracted.action.version.source || _state.source.payload.extracted.version.major != _state.source.payload.extracted.action.version.major && _state.source.payload.extracted.version.minor != _state.source.payload.extracted.action.version.minor && _state.source.payload.extracted.version.compile != _state.source.payload.extracted.action.version.compile) && _state.source.payload.extracted.action.allowUninstallation)
                    && !_state.syncPayload.lastWasError
                    );
        }

        public bool versionPayloadIsUninstalled() { //returns true if installetion completed
            return (
                    _state.source.active != null && _state.source.active.protocol != null && _state.source.active.server != null && _state.source.active.port > 0 && _state.source.active.path != null
                    && _state.source.active.iSeq == _state.source.active.loaded.iSeq
                    && _state.source.active.channel.iSeq == _state.source.active.loaded.channel.iSeq
                    && ((_state.source.active == _state.source.payload.version.source && _state.source.active.loaded.channel.version.latest.version.major == _state.source.payload.version.major && _state.source.active.loaded.channel.version.latest.version.minor == _state.source.payload.version.minor && _state.source.active.loaded.channel.version.latest.version.compile == _state.source.payload.version.compile) && _state.source.payload.allowDownload)
                    && ((_state.source.payload.version.source == _state.source.payload.extracted.version.source && _state.source.payload.version.major == _state.source.payload.extracted.version.major && _state.source.payload.version.minor == _state.source.payload.extracted.version.minor && _state.source.payload.version.compile == _state.source.payload.extracted.version.compile) && _state.source.payload.extracted.allowExtraction)
                    && ((_state.source.payload.extracted.version.source == _state.source.payload.extracted.action.version.source && _state.source.payload.extracted.version.major == _state.source.payload.extracted.action.version.major && _state.source.payload.extracted.version.minor == _state.source.payload.extracted.action.version.minor && _state.source.payload.extracted.version.compile == _state.source.payload.extracted.action.version.compile) && _state.source.payload.extracted.action.allowUninstallation)
                    && !_state.syncPayload.lastWasError
                    );
        }

        //timer

        private void timerStart(STSync sync) {
            //first action in sequence
            System.Timers.Timer newTimer = sync.timer.cur = new System.Timers.Timer();
            sync.timer.msPerTick = CA_UPDATER_TIMER_MS_MIN;
            newTimer.Elapsed += (sender, e) => { timerFiredObj(sync); };
            newTimer.Interval = sync.timer.msPerTick;
            newTimer.AutoReset = false;
            newTimer.Start();
            Debug.WriteLine("timerStart.\n");
        }

        private void timerContinueAgainStart(STSync sync) {
            //first action in sequence
            System.Timers.Timer newTimer = sync.timer.cur = new System.Timers.Timer();
            sync.timer.msPerTick = CA_UPDATER_TIMER_MS_MIN;
            newTimer.Elapsed += (sender, e) => { timerFiredObj(sync); };
            newTimer.Interval = CA_UPDATER_TIMER_MS_CONTINUATION;
            newTimer.AutoReset = false;
            newTimer.Start();
            Debug.WriteLine("timerContinueAgainStart.\n");
        }

        private void timerRetryAfterError(STSync sync) {
            //something went wrong, retry with time
            System.Timers.Timer newTimer = sync.timer.cur = new System.Timers.Timer();
            sync.timer.msPerTick += CA_UPDATER_TIMER_MS_INCREMENT;
            if (sync.timer.msPerTick > CA_UPDATER_TIMER_MS_MAX) {
                sync.timer.msPerTick = CA_UPDATER_TIMER_MS_MAX;
            }
            newTimer.Elapsed += (sender, e) => { timerFiredObj(sync); };
            newTimer.Interval = sync.timer.msPerTick;
            newTimer.AutoReset = false;
            newTimer.Start();
            Debug.WriteLine("timerRetryAfterError.\n");
        }

        class STJsonRootIndexApp {
            public String id;
            public String name;
        }

        class STJsonRootChannelName {
            public String lang;
            public String value;
        }

        class STJsonRootChannel {
            public String id;
            public STJsonRootChannelName[] names;
        }

        class STJsonRootIndex {
            public STJsonRootIndexApp app;
            public STJsonRootChannel[] channels;
        }

        class STJsonChannelVersion {
            public String os;
            public String filename;
            public String pass;
            public String version;
        }

        class STJsonChannel {
            public STJsonChannelVersion[] versions;
        }

        private static void timerFiredObj(STSync sync) {
            if (sync != null && sync.instance != null) {
                sync.instance.timerFired(sync);
            }
        }

        private void timerFired(STSync sync) {
            //validate only one active action
            {
                int i, enabledCount = 0; string enabledList = "";
                bool[] actions = new bool[4];
                string[] actionsNames = new string[4];
                //
                actions[0] = versionPayloadIsDownloading();
                actionsNames[0] = "downloading";
                //
                actions[1] = versionPayloadIsExtracting();
                actionsNames[1] = "extracting";
                //
                actions[2] = versionPayloadIsInstalling();
                actionsNames[2] = "installing";
                //
                actions[3] = versionPayloadIsUninstalling();
                actionsNames[3] = "uninstalling";
                //
                for (i = 0; i < actions.Length; i++) {
                    if (actions[i]) {
                        if (enabledCount != 0) enabledList += ", ";
                        enabledList += actionsNames[i];
                        enabledCount++;
                    }
                }
                if (enabledCount > 1) {
                    Debug.WriteLine("More than one action is active (program logic error): '" + enabledList + "'.\n");
                    Debug.Assert(enabledCount <= 1);
                }
            }
            //
            if (_state.isInvalidated) {
                //stopping
                Debug.WriteLine("timerFired-depth(" + sync.curSyncDepth + "): stopping (invalidated).\n");
                //consume timer (will be renewed when necesary)
                if (sync.timer.cur != null && sync.curSyncDepth == 0) {
                    sync.timer.cur = null;
                    Debug.WriteLine("Tick, timer consumido (invalidated).\n");
                }
            } else {
                bool consumedRemote = false, consumedActive = false;
                //
                //remote server actions (sync)
                //
                if(sync == _state.syncRemote) {
                    if (_state.source.remote.protocol == null || _state.source.remote.server == null || _state.source.remote.port <= 0 || _state.source.remote.path == null) {
                        //wait for 'selection' (from local-file or user-action)
                        Debug.WriteLine("timerFired-depth(" + sync.curSyncDepth + "): source not-selected.\n");
                        //consume timer (will be renewed when necesary)
                        if (sync.timer.cur != null && sync.curSyncDepth == 0) {
                            sync.timer.cur = null;
                            Debug.WriteLine("Tick, timer consumido (no source selected).\n");
                        }
                    } else {
                        doRemoteAction(ref consumedRemote);
                    }
                }
                //
                //"active" related action (remote or embedded)
                //
                if (sync == _state.syncPayload) {
                    doActiveAction(ref consumedActive);
                }
                //
                if (!consumedRemote && !consumedActive) {
                    //No operation (wait for user action)
                    Debug.WriteLine("timerFired-depth(" + sync.curSyncDepth + "): no operation (will wait for user action).\n");
                    //consume timer (will be renewed when necesary)
                    if (sync.curSyncDepth == 0) {
                        sync.timer.cur = null;
                        Debug.WriteLine("Tick, timer consumido (no action).\n");
                    }
                }
            }
        }

        void doRemoteAction(ref bool consumed) {
            STSync sync = _state.syncRemote;
            //sync root
            if (
                //incomplete
                !consumed /*&& !sync.lastWasError; retry is allowed*/
                && _state.source.remote.iSeq != _state.source.remote.loaded.iSeq
                ) {
                Debug.WriteLine("timerFired-depth(" + sync.curSyncDepth + "): syncing-root.\n");
                consumed = true;
                if (_httpTask == null) {
                    sync.curSyncDepth++;
                    //consume timer (will be renewed when necesary)
                    if (sync.curSyncDepth == 1) {
                        sync.timer.cur = null;
                        Debug.WriteLine("Tick, timer consumido (sync-root).\n");
                    }
                    //consume error
                    {
                        sync.lastWasError = false;
                        sync.lastErrorDesc = null;
                    }
                    //start request
                    _httpClt = new HttpClient();
                    _httpTask = _httpClt.GetStringAsync("" + _state.source.remote.protocol + _state.source.remote.server + ":" + _state.source.remote.port + (_state.source.remote.path != null && _state.source.remote.path.Length > 0 ? _state.source.remote.path : "/") + "_index.json");
                    //Debug.WriteLine("Root url: " + "" + _state.source.remote.protocol + _state.source.remote.server + ":" + _state.source.remote.port + (_state.source.remote.path != null && _state.source.remote.path.Length > 0 ? _state.source.remote.path : "/") + "_index.json");
                    this.timerContinueAgainStart(sync);
                } else if (_httpTask.Status == TaskStatus.Canceled || _httpTask.Status == TaskStatus.Faulted || _httpTask.Status == TaskStatus.RanToCompletion) {
                    bool r = false;
                    //consume timer (will be renewed when necesary)
                    if (sync.curSyncDepth == 1) {
                        sync.timer.cur = null;
                        Debug.WriteLine("Tick, timer consumido (sync-root).\n");
                    }
                    if (_httpTask.Status == TaskStatus.Canceled || _httpTask.Status == TaskStatus.Faulted) {
                        //error
                        sync.lastWasError = true;
                        sync.lastErrorDesc = "Download error (root).";
                        Debug.WriteLine("Error (root): " + sync.lastErrorDesc);
                    } else if (_httpTask.Status == TaskStatus.RanToCompletion) {
                        String rr = _httpTask.Result;
                        if (rr == null || rr.Length <= 0) {
                            sync.lastWasError = true;
                            sync.lastErrorDesc = "Download error (root): empty response.";
                            Debug.WriteLine("Error (root): " + sync.lastErrorDesc);
                        } else {
                            STJsonRootIndex idx = rr.FromJson<STJsonRootIndex>();
                            if (idx == null) {
                                //error
                                sync.lastWasError = true;
                                sync.lastErrorDesc = "Download error (root): index parsing error.";
                                Debug.WriteLine("Error (root): " + sync.lastErrorDesc);
                            } else if (idx.app == null || idx.app.id == null || idx.app.id.Length <= 0) {
                                //error
                                sync.lastWasError = true;
                                sync.lastErrorDesc = "Download error (root): missing app-id.";
                                Debug.WriteLine("Error (root): " + sync.lastErrorDesc);
                            } else if (idx.app.id != CA_UPDATER_APP_ID) {
                                //error
                                sync.lastWasError = true;
                                sync.lastErrorDesc = "Download error (root): app-id does not match.";
                                Debug.WriteLine("Error (root): " + sync.lastErrorDesc);
                            } else if (idx.channels == null) {
                                //error
                                sync.lastWasError = true;
                                sync.lastErrorDesc = "Download error (root): missing channels array.";
                                Debug.WriteLine("Error (root): " + sync.lastErrorDesc);
                            } else {
                                String[] channels = null;
                                int i; for (i = 0; i < idx.channels.Length; i++) {
                                    STJsonRootChannel c = idx.channels[i];
                                    if (c.id != null && c.id.Length > 0 && c.names != null) {
                                        int j; for (j = 0; j < c.names.Length; j++) {
                                            if (c.names[j].lang != null && c.names[j].lang.Length > 0 && c.names[j].value != null && c.names[j].value.Length > 0) {
                                                int k = 0;
                                                //resize array
                                                String[] channelsN = new String[(channels == null ? 0 : channels.Length) + 2];
                                                //copy old array
                                                if (channels != null) {
                                                    for (k = 0; k < channels.Length; k++) {
                                                        channelsN[k] = channels[k];
                                                    }
                                                }
                                                //add new
                                                channelsN[k++] = c.id;
                                                channelsN[k++] = c.names[j].value;
                                                channels = channelsN;
                                                //
                                                break;
                                            }
                                        }
                                    }
                                }
                                //results
                                if (channels == null || channels.Length <= 0) {
                                    //error
                                    sync.lastWasError = true;
                                    sync.lastErrorDesc = "Download error (root): no valid channels.";
                                    Debug.WriteLine("Error (root): " + sync.lastErrorDesc);
                                } else {
                                    _state.source.remote.loaded.channels = channels;
                                    _state.source.remote.loaded.iSeq = _state.source.remote.iSeq;
                                    //auto-select channel (current instalation)
                                    if (_state.source.remote.channel.uid == null || _state.source.remote.channel.uid.Length == 0) {
                                        if (_state.source.local.channel.uid != null && _state.source.local.channel.uid.Length > 0) {
                                            int i2; for (i2 = 0; i2 < channels.Length; i2 += 2) {
                                                String cId = channels[i2];
                                                if (cId == _state.source.local.channel.uid) {
                                                    _state.source.remote.channel.uid = cId;
                                                    Debug.WriteLine("Channel auto-selected-current-installed (root): " + _state.source.remote.channel.uid);
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    //auto-select channel (first-one)
                                    if (_state.source.remote.channel.uid == null || _state.source.remote.channel.uid.Length == 0) {
                                        _state.source.remote.channel.uid = (String)channels[0];
                                        Debug.WriteLine("Channel auto-selected-first (root): " + _state.source.remote.channel.uid);
                                    }
                                    //trigger channel sync
                                    _state.source.remote.channel.iSeq++;
                                    r = true;
                                }
                            }
                        }
                    }
                    _httpTask = null;
                    //results
                    if (r) {
                        //do next step inmediatly
                        this.timerFired(sync);
                    } else {
                        //schedule to retry
                        this.timerRetryAfterError(sync);
                    }
                    sync.curSyncDepth--;
                } else {
                    //tick again
                    //Debug.WriteLine("Continue (root).\n");
                    //consume timer (will be renewed when necesary)
                    if (sync.curSyncDepth == 1) {
                        sync.timer.cur = null;
                        Debug.WriteLine("Tick, timer consumido (sync-root).\n");
                    }
                    this.timerContinueAgainStart(sync);
                }
            }
            //sync channel
            if (
                //completed
                _state.source.remote.iSeq == _state.source.remote.loaded.iSeq
                //incomplete
                && !consumed /*&& !sync.lastWasError; retry is allowed*/
                && _state.source.remote.channel.iSeq != _state.source.remote.loaded.channel.iSeq
                ) {
                Debug.WriteLine("timerFired-depth(" + sync.curSyncDepth + "): syncing-channel.\n");
                consumed = true;
                if (_httpTask == null) {
                    sync.curSyncDepth++;
                    //consume timer (will be renewed when necesary)
                    if (sync.curSyncDepth == 1) {
                        sync.timer.cur = null;
                        Debug.WriteLine("Tick, timer consumido (sync-channel).\n");
                    }
                    //consume error
                    {
                        sync.lastWasError = false;
                        sync.lastErrorDesc = null;
                    }
                    //start request
                    _httpClt = new HttpClient();
                    _httpTask = _httpClt.GetStringAsync("" + _state.source.remote.protocol + _state.source.remote.server + ":" + _state.source.remote.port + (_state.source.remote.path != null && _state.source.remote.path.Length > 0 ? _state.source.remote.path : "/") + _state.source.remote.channel.uid + "/" + "_index.json");
                    //Debug.WriteLine("Channel url: " + "" + _state.source.remote.protocol + _state.source.remote.server + ":" + _state.source.remote.port + (_state.source.remote.path != null && _state.source.remote.path.Length > 0 ? _state.source.remote.path : "/") + _state.source.remote.channel.uid + "/" + "_index.json");
                    this.timerContinueAgainStart(sync);
                } else if (_httpTask.Status == TaskStatus.Canceled || _httpTask.Status == TaskStatus.Faulted || _httpTask.Status == TaskStatus.RanToCompletion) {
                    bool r = false;
                    //consume timer (will be renewed when necesary)
                    if (sync.curSyncDepth == 1) {
                        sync.timer.cur = null;
                        Debug.WriteLine("Tick, timer consumido (sync-channel).\n");
                    }
                    if (_httpTask.Status == TaskStatus.Canceled || _httpTask.Status == TaskStatus.Faulted) {
                        //error
                        sync.lastWasError = true;
                        sync.lastErrorDesc = "Download error (channel).";
                        Debug.WriteLine("Error (channel): " + sync.lastErrorDesc);
                    } else if (_httpTask.Status == TaskStatus.RanToCompletion) {
                        String rr = _httpTask.Result;
                        if (rr == null || rr.Length <= 0) {
                            sync.lastWasError = true;
                            sync.lastErrorDesc = "Download error (channel): empty response.";
                            Debug.WriteLine("Error (channel): " + sync.lastErrorDesc);
                        } else {
                            STJsonChannel idx = rr.FromJson<STJsonChannel>();
                            Debug.WriteLine("Channel response: " + rr);
                            if (idx == null) {
                                //error
                                sync.lastWasError = true;
                                sync.lastErrorDesc = "Download error (channel): index parsing error.";
                                Debug.WriteLine("Error (channel): " + sync.lastErrorDesc);
                            } else if (idx.versions == null || idx.versions.Length <= 0) {
                                //error
                                sync.lastWasError = true;
                                sync.lastErrorDesc = "Download error (channel): missing versions array.";
                                Debug.WriteLine("Error (channel): " + sync.lastErrorDesc);
                            } else {
                                //reset
                                {
                                    _state.source.remote.loaded.channel.version.latest.filename = null;
                                    _state.source.remote.loaded.channel.version.latest.version.major = 0;
                                    _state.source.remote.loaded.channel.version.latest.version.minor = 0;
                                    _state.source.remote.loaded.channel.version.latest.version.compile = 0;
                                }
                                //search
                                int i; for (i = 0; i < idx.versions.Length; i++) {
                                    STJsonChannelVersion v = idx.versions[i];
                                    if (v.os != null && v.os == "win" && v.filename != null && v.filename.Length > 0 && v.version != null && v.version.Length > 0) {
                                        int major = 0, minor = 0, compile = 0;
                                        if (parseVersion(v.version, ref major, ref minor, ref compile)) {
                                            //apply latest
                                            if (
                                                _state.source.remote.loaded.channel.version.latest.version.major < major
                                                || (_state.source.remote.loaded.channel.version.latest.version.major == major && _state.source.remote.loaded.channel.version.latest.version.minor < minor)
                                                || (_state.source.remote.loaded.channel.version.latest.version.major == major && _state.source.remote.loaded.channel.version.latest.version.minor == minor && _state.source.remote.loaded.channel.version.latest.version.compile < compile)
                                                )
                                            {
                                                //latest
                                                _state.source.remote.loaded.channel.version.latest.filename = v.filename;
                                                _state.source.remote.loaded.channel.version.latest.version.major = major;
                                                _state.source.remote.loaded.channel.version.latest.version.minor = minor;
                                                _state.source.remote.loaded.channel.version.latest.version.compile = compile;
                                                if (v.pass != null && v.pass.Length > 0) {
                                                    _state.source.remote.loaded.channel.version.latest.pass = v.pass;
                                                } else {
                                                    _state.source.remote.loaded.channel.version.latest.pass = "";
                                                }
                                                //payload
                                                //_state.source.payload.allowDownload = false;
                                                //
                                                _state.source.remote.loaded.channel.iSeq = _state.source.remote.channel.iSeq;
                                                _state.source.remote.loaded.channel.uid = _state.source.remote.channel.uid;
                                                _state.source.remote.loaded.protocol = _state.source.remote.protocol;
                                                _state.source.remote.loaded.server = _state.source.remote.server;
                                                _state.source.remote.loaded.port = _state.source.remote.port;
                                                _state.source.remote.loaded.path = _state.source.remote.path;
                                                //TMP: auto-download
                                                {
                                                    //this.activateInstallAction();
                                                }
                                                r = true;
                                            } else {
                                                Debug.WriteLine("Version (channel): version(" + v.version + ") is not valid.\n");
                                            }
                                        }
                                    }
                                }
                                //result
                                if (!r) {
                                    //error
                                    sync.lastWasError = true;
                                    sync.lastErrorDesc = "Download error (channel): no valid version found.";
                                    Debug.WriteLine("Error (root): " + sync.lastErrorDesc);
                                } else {
                                    Debug.WriteLine("Latest version (channel): " + _state.source.remote.loaded.channel.version.latest.version.major + "." + _state.source.remote.loaded.channel.version.latest.version.minor + "." + _state.source.remote.loaded.channel.version.latest.version.compile + ": '" + _state.source.remote.loaded.channel.version.latest.filename + "'.\n");
                                }
                            }
                        }
                    }
                    _httpTask = null;
                    //results
                    if (r) {
                        //do next step inmediatly
                        this.timerFired(sync);
                    } else {
                        //schedule to retry
                        this.timerRetryAfterError(sync);
                    }
                    //
                    sync.curSyncDepth--;
                } else {
                    //tick again
                    //Debug.WriteLine("Continue (channel).\n");
                    //consume timer (will be renewed when necesary)
                    if (sync.curSyncDepth == 1) {
                        sync.timer.cur = null;
                        Debug.WriteLine("Tick, timer consumido (sync-channel).\n");
                    }
                    this.timerContinueAgainStart(sync);
                }
            }
            //Trigger download action
            if (
                //completed
                _state.source.remote.iSeq == _state.source.remote.loaded.iSeq
                //incomplete
                && !consumed && !sync.lastWasError
                && _state.source.remote.channel.iSeq == _state.source.remote.loaded.channel.iSeq
                //
                && _state.source.remote.loaded.channel.version.latest.filename != null
                && (_state.source.remote.loaded.channel.version.latest.version.major > 0 || _state.source.remote.loaded.channel.version.latest.version.minor > 0 || _state.source.remote.loaded.channel.version.latest.version.compile > 0)
                && _state.source.payload.allowDownload
                )
            {
                Debug.WriteLine("timerFired-depth(" + sync.curSyncDepth + "): triggering download.\n");
                STSync sync2 = _state.syncPayload;
                if (sync2.timer.cur == null && sync2.curSyncDepth == 0){
                    Debug.WriteLine("LocalLoaded, iniciando timer.\n");
                    this.timerStart(sync2);
                } else {
                    Debug.WriteLine("LocalLoaded, timer ya estaba activo curSyncDepth(" + sync2.curSyncDepth + ").\n");
                }
            }
        }
        void doActiveAction(ref bool consumed) {
            STSync sync = _state.syncPayload;
            //download file
            if (
                //completed
                _state.source.active != null && _state.source.active.protocol != null && _state.source.active.server != null && _state.source.active.port > 0 && _state.source.active.path != null
                && _state.source.active.iSeq == _state.source.active.loaded.iSeq
                && _state.source.active.channel.iSeq == _state.source.active.loaded.channel.iSeq
                //incomplete
                && !consumed && !sync.lastWasError
                && ((_state.source.active != _state.source.payload.version.source || _state.source.active.loaded.channel.version.latest.version.major != _state.source.payload.version.major || _state.source.active.loaded.channel.version.latest.version.minor != _state.source.payload.version.minor || _state.source.active.loaded.channel.version.latest.version.compile != _state.source.payload.version.compile) && _state.source.payload.allowDownload)
                ) {
                Debug.WriteLine("timerFired-depth(" + sync.curSyncDepth + "): downloading-file.\n");
                consumed = true;
                if (_httpTaskBytes == null) {
                    sync.curSyncDepth++;
                    //consume timer (will be renewed when necesary)
                    if (sync.curSyncDepth == 1) {
                        sync.timer.cur = null;
                        Debug.WriteLine("Tick, timer consumido (download).\n");
                    }
                    //consume error
                    {
                        sync.lastWasError = false;
                        sync.lastErrorDesc = null;
                    }
                    //start request
                    _httpClt = new HttpClient();
                    _httpTaskBytes = _httpClt.GetByteArrayAsync("" + _state.source.active.protocol + _state.source.active.server + ":" + _state.source.active.port + (_state.source.active.path != null && _state.source.active.path.Length > 0 ? _state.source.active.path : "/") + _state.source.active.channel.uid + "/" + _state.source.active.loaded.channel.version.latest.filename);
                    //Debug.WriteLine("" + _state.source.active.protocol + _state.source.active.server + ":" + _state.source.active.port + (_state.source.active.path != null && _state.source.active.path.Length > 0 ? _state.source.active.path : "/") + _state.source.active.channel.uid + "/" + _state.source.active.loaded.channel.version.latest.filename);
                    this.timerContinueAgainStart(sync);
                } else if (_httpTaskBytes.Status == TaskStatus.Canceled || _httpTaskBytes.Status == TaskStatus.Faulted || _httpTaskBytes.Status == TaskStatus.RanToCompletion) {
                    bool r = false;
                    //consume timer (will be renewed when necesary)
                    if (sync.curSyncDepth == 1) {
                        sync.timer.cur = null;
                        Debug.WriteLine("Tick, timer consumido (download).\n");
                    }
                    if (_httpTaskBytes.Status == TaskStatus.Canceled || _httpTaskBytes.Status == TaskStatus.Faulted) {
                        //error
                        sync.lastWasError = true;
                        sync.lastErrorDesc = "Download error (file).";
                        Debug.WriteLine("Error (root): " + sync.lastErrorDesc);
                    } else if (_httpTaskBytes.Status == TaskStatus.RanToCompletion) {
                        byte[] rr = _httpTaskBytes.Result;
                        if (rr == null || rr.Length <= 0) {
                            sync.lastWasError = true;
                            sync.lastErrorDesc = "Download error (file): empty response.";
                            Debug.WriteLine("Error (root): " + sync.lastErrorDesc);
                        } else {
                            _state.source.payload.version.source = _state.source.active;
                            _state.source.payload.version.major = _state.source.active.loaded.channel.version.latest.version.major;
                            _state.source.payload.version.minor = _state.source.active.loaded.channel.version.latest.version.minor;
                            _state.source.payload.version.compile = _state.source.active.loaded.channel.version.latest.version.compile;
                            _state.source.payload.data = rr;
                            //
                            //_state.source.payload.extracted.allowExtraction = false;
                            //TMP: auto-extract
                            {
                                //_state.source.payload.extracted.allowExtraction = true;
                            }
                            Debug.WriteLine("Latest-downloaded: " + (int)(rr.Length / 1024) + " KBs.\n");
                            r = true;
                        }
                    }
                    _httpTaskBytes = null;
                    //results
                    if (r) {
                        //do next step inmediatly
                        this.timerFired(sync);
                    } else {
                        //schedule to retry
                        this.timerRetryAfterError(sync);
                    }
                    //
                    sync.curSyncDepth--;
                } else {
                    //tick again
                    //consume timer (will be renewed when necesary)
                    if (sync.curSyncDepth == 1) {
                        sync.timer.cur = null;
                        Debug.WriteLine("Tick, timer consumido (download).\n");
                    }
                    this.timerContinueAgainStart(sync);
                }
            }
            //extract file
            if (
                //completed
                _state.source.active != null && _state.source.active.protocol != null && _state.source.active.server != null && _state.source.active.port > 0 && _state.source.active.path != null
                && _state.source.active.iSeq == _state.source.active.loaded.iSeq
                && _state.source.active.channel.iSeq == _state.source.active.loaded.channel.iSeq
                && ((_state.source.active == _state.source.payload.version.source && _state.source.active.loaded.channel.version.latest.version.major == _state.source.payload.version.major && _state.source.active.loaded.channel.version.latest.version.minor == _state.source.payload.version.minor && _state.source.active.loaded.channel.version.latest.version.compile == _state.source.payload.version.compile) && _state.source.payload.allowDownload)
                //incomplete
                && !consumed && !sync.lastWasError
                && ((_state.source.payload.version.source != _state.source.payload.extracted.version.source || _state.source.payload.version.major != _state.source.payload.extracted.version.major || _state.source.payload.version.minor != _state.source.payload.extracted.version.minor || _state.source.payload.version.compile != _state.source.payload.extracted.version.compile) && _state.source.payload.extracted.allowExtraction)
                ) {
                bool r = false;
                Debug.WriteLine("timerFired-depth(" + sync.curSyncDepth + "): extracting-latest-version.\n");
                consumed = true;
                sync.curSyncDepth++;
                //consume timer (will be renewed when necesary)
                if (sync.curSyncDepth == 1) {
                    sync.timer.cur = null;
                    Debug.WriteLine("Tick, timer consumido (extract-file).\n");
                }
                //consume error
                {
                    sync.lastWasError = false;
                    sync.lastErrorDesc = null;
                }
                //
                if (_state.source.payload.data == null || _state.source.payload.data.Length <= 0) {
                    //error
                    sync.lastWasError = true;
                    sync.lastErrorDesc = "Extraction error (file): no content found.\n";
                    Debug.WriteLine("Error (latest-extract): " + sync.lastErrorDesc);
                } else {
                    String dmgPath = _tmpFolderWithSlah + "contab-" + _state.source.active.loaded.channel.version.latest.filename;
                    String mountPath = _tmpFolderWithSlah + "contab-" + _state.source.active.loaded.channel.version.latest.filename + "-mount";
                    bool fileWritten = false;
                    //verify previous-download matches
                    if (!fileWritten) {
                        if (File.Exists(dmgPath)) {
                            FileInfo info = new FileInfo(dmgPath);
                            if (info != null) {
                                if (info.Length != _state.source.payload.data.Length){
                                    Debug.WriteLine("Previous download extract file size does not matches, will try to write again.\n");
                                } else {
                                    Debug.WriteLine("Previous download extract file size matches, wont write again.\n");
                                    fileWritten = true;
                                }
                            }
                        }
                    }
                    //write fresh-file
                    if (!fileWritten) {
                        try {
                            File.WriteAllBytes(dmgPath, _state.source.payload.data);
                            fileWritten = true;
                        } catch (Exception e) {
                            //error
                            sync.lastWasError = true;
                            sync.lastErrorDesc = "Extraction error (latest-download): could not write file.\n";
                            Debug.WriteLine("Error (latest-extract): " + sync.lastErrorDesc);
                        }
                    }
                    //mount
                    if (fileWritten) {
                        try {
                            ZipFile.ExtractToDirectory(dmgPath, mountPath);
                        } catch (Exception e) {
                            //allowed to fail (when already exracted in a previous execution)
                        }
                        //
                        if (!Directory.Exists(mountPath)) {
                            //error
                            sync.lastWasError = true;
                            sync.lastErrorDesc = "Extraction error (latest-download): final folder not found.\n";
                            Debug.WriteLine("Error (latest-extract): " + sync.lastErrorDesc);
                        } else {
                            //search for install and remove scripts (use the last one found)
                            String installScriptPath = null, removeScriptPath = null;
                            //at mount folder
                            {
                                String installPath = mountPath + "\\manage.bat";
                                String removePath = mountPath + "\\manage.bat";
                                if (File.Exists(installPath) && File.Exists(removePath)) {
                                    installScriptPath = installPath;
                                    removeScriptPath = removePath;
                                    Debug.WriteLine("Install/remove scripts found at extraction root.\n");
                                }
                            }
                            //at subfolder
                            {
                                DirectoryInfo dir = new DirectoryInfo(mountPath);
                                DirectoryInfo[] dirs = dir.GetDirectories();
                                int i; for (i = 0; i < dirs.Length; i++) {
                                    String folderName = dirs[i].Name;
                                    String installPath = mountPath + "\\" + folderName + "\\manage.bat";
                                    String removePath = mountPath + "\\" + folderName + "\\manage.bat";
                                    String removeFolder = mountPath + "\\" + folderName;
                                    if (File.Exists(installPath) && File.Exists(removePath)) {
                                        installScriptPath = installPath;
                                        removeScriptPath = removePath;
                                        Debug.WriteLine("Install/remove scripts found at extraction subfolder '" + folderName + "'.\n");
                                    }
                                }
                            }
                            //results
                            if (installScriptPath == null || removeScriptPath == null) {
                                sync.lastWasError = true;
                                sync.lastErrorDesc = "Extraction error (latest-download): could not find instalation and removal scripts in extraction folder.\n";
                                Debug.WriteLine("Error (latest-extract): " + sync.lastErrorDesc);
                            } else {
                                //sucess
                                _state.source.payload.extracted.dmgPath = dmgPath;
                                _state.source.payload.extracted.mountPath = mountPath;
                                _state.source.payload.extracted.installScriptPath = installScriptPath;
                                _state.source.payload.extracted.removeScriptPath = removeScriptPath;
                                _state.source.payload.extracted.version.source = _state.source.payload.version.source;
                                _state.source.payload.extracted.version.major = _state.source.payload.version.major;
                                _state.source.payload.extracted.version.minor = _state.source.payload.version.minor;
                                _state.source.payload.extracted.version.compile = _state.source.payload.version.compile;
                                //
                                //_state.source.payload.extracted.action.allowInstallation = false;
                                //_state.source.payload.extracted.action.allowUninstallation = false;
                                //TMP: auto-install
                                {
                                    //_state.source.payload.extracted.action.allowInstallation = true;
                                    //_state.source.payload.extracted.action.allowUninstallation = false;
                                }
                                Debug.WriteLine("Latest-extracted.\n");
                                r = true;
                            }
                        }
                    }
                }
                //ouside completion-handler
                if (r) {
                    //do next step inmediatly
                    this.timerFired(sync);
                } else {
                    //schedule to retry
                    this.timerRetryAfterError(sync);
                }
                //
                sync.curSyncDepth--;
            }
            //action (install/uninstall)
            if (
                //completed
                _state.source.active != null && _state.source.active.protocol != null && _state.source.active.server != null && _state.source.active.port > 0 && _state.source.active.path != null
                && _state.source.active.iSeq == _state.source.active.loaded.iSeq
                && _state.source.active.channel.iSeq == _state.source.active.loaded.channel.iSeq
                && ((_state.source.active == _state.source.payload.version.source && _state.source.active.loaded.channel.version.latest.version.major == _state.source.payload.version.major && _state.source.active.loaded.channel.version.latest.version.minor == _state.source.payload.version.minor && _state.source.active.loaded.channel.version.latest.version.compile == _state.source.payload.version.compile) && _state.source.payload.allowDownload)
                && ((_state.source.payload.version.source == _state.source.payload.extracted.version.source && _state.source.payload.version.major == _state.source.payload.extracted.version.major && _state.source.payload.version.minor == _state.source.payload.extracted.version.minor && _state.source.payload.version.compile == _state.source.payload.extracted.version.compile) && _state.source.payload.extracted.allowExtraction)
                //incomplete
                && !consumed && !sync.lastWasError
                && ((_state.source.payload.extracted.version.source != _state.source.payload.extracted.action.version.source || _state.source.payload.extracted.version.major != _state.source.payload.extracted.action.version.major || _state.source.payload.extracted.version.minor != _state.source.payload.extracted.action.version.minor || _state.source.payload.extracted.version.compile != _state.source.payload.extracted.action.version.compile) && (_state.source.payload.extracted.action.allowInstallation || _state.source.payload.extracted.action.allowUninstallation))
                )
            {
                bool r = false;
                Debug.WriteLine("timerFired-depth(" + sync.curSyncDepth + "): run-action.\n");
                consumed = true;
                sync.curSyncDepth++;
                //consume timer (will be renewed when necesary)
                if (sync.curSyncDepth == 1) {
                    sync.timer.cur = null;
                    Debug.WriteLine("Tick, timer consumido (install).\n");
                }
                //consume error
                {
                    sync.lastWasError = false;
                    sync.lastErrorDesc = null;
                }
                //
                if (_state.source.payload.extracted.installScriptPath == null || _state.source.payload.extracted.installScriptPath.Length <= 0 || _state.source.payload.extracted.removeScriptPath == null || _state.source.payload.extracted.removeScriptPath.Length <= 0) {
                    //error
                    sync.lastWasError = true;
                    sync.lastErrorDesc = "Installation error (install): no path for install/remove scripts.\n";
                    Debug.WriteLine("Error (latest-install): " + sync.lastErrorDesc);
                } else {
                    //execute
                    r = this.runActionScript(_state.source.payload.extracted.action.allowInstallation, _state.source.payload.extracted.action.allowUninstallation, _state.source.payload.extracted.installScriptPath, _state.source.payload.extracted.removeScriptPath, _tmpFolderWithSlah + "contab-" + _state.source.active.loaded.channel.version.latest.filename, _state.source.payload.extracted.mountPath);
                    //results
                    if (!r) {
                        //error
                        sync.lastWasError = true;
                        if (sync.lastErrorDesc == null || sync.lastErrorDesc.Length <= 0) {
                            sync.lastErrorDesc = "Installation error (install): could not run scripts.\n";
                            Debug.WriteLine("Error (latest-install): " + sync.lastErrorDesc);
                        }
                    } else {
                        _state.source.payload.extracted.action.version.source = _state.source.payload.extracted.version.source;
                        _state.source.payload.extracted.action.version.major = _state.source.payload.extracted.version.major;
                        _state.source.payload.extracted.action.version.minor = _state.source.payload.extracted.version.minor;
                        _state.source.payload.extracted.action.version.compile = _state.source.payload.extracted.version.compile;
                        //
                        //read from local files
                        this.loadSourceLocal();
                        //reset state
                        {
                            _state.source.payload.extracted.allowExtraction = false;
                            _state.source.payload.extracted.action.allowInstallation = false;
                            _state.source.payload.extracted.action.allowUninstallation = false;
                            _state.source.payload.extracted.mountPath = null;
                            _state.source.payload.extracted.installScriptPath = null;
                            _state.source.payload.extracted.removeScriptPath = null;
                            _state.source.payload.extracted.version.source = null;
                            _state.source.payload.extracted.version.minor = 0;
                            _state.source.payload.extracted.version.major = 0;
                            _state.source.payload.extracted.version.compile = 0;
                        }
                        //
                        Debug.WriteLine("Installation completed.\n");
                    }
                }
                //ouside completion-handler
                if (r) {
                    //do next step inmediatly
                    this.timerFired(sync);
                } else {
                    //schedule to retry
                    this.timerRetryAfterError(sync);
                }
                //
                sync.curSyncDepth--;
            }
        }

        public static string getDefaultServerUrl() {
            string r = null;
            //read registry (currently installed)
            if(r == null || r.Length <= 0){
                try {
                    r = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows\\CurrentVersion\\uninstall\\" + CA_UPDATER_APP_REGISTRY_NAME, "DefaultServerUrl", null);
                } catch (Exception) {
                    //
                }
            }
            //read installer pkg (only available inside installer-app, not updater)
            {
                System.Resources.ResourceManager mngr = Resources.ResourceManager;
                if (mngr != null) {
                    byte[] srcHead = (byte[])mngr.GetObject("src_head", Resources.Culture);
                    if (srcHead != null) {
                        String srcHeadStr = System.Text.Encoding.Default.GetString(srcHead);
                        try {
                            STEmbbededSourceHdr loaded = srcHeadStr.FromJson<STEmbbededSourceHdr>();
                            r = loaded.defServerUrl;
                        } catch (Exception) {
                            //
                        }
                    }
                }
            }
            return r;
        }

        bool runActionScript(bool isInstallAction, bool isUninstallAction, String installScriptPath, String uninstallScriptPath, String downloadedFilePath, String extractionRootPath){
            bool r = false;
            STSync sync = _state.syncPayload;
            //build script
            {
                String scriptActionPath = _tmpFolderWithSlah + "contab-action.bat";
                String scriptAction = "";
                if (isInstallAction && installScriptPath != null && installScriptPath.Length > 0) {
                    //Keep track of current defaultServerUrl (before the registry gets deleted)
                    string defServerUrl = getDefaultServerUrl();
                    //
                    scriptAction += "setlocal EnableDelayedExpansion\n";
                    //detect architecture
                    scriptAction += "SET CA_ARCH=\n";
                    scriptAction += "IF DEFINED PROCESSOR_ARCHITEW6432 (\n";
                    scriptAction += "  SET CA_ARCH=%PROCESSOR_ARCHITEW6432%\n";
                    scriptAction += ") ELSE (\n";
                    scriptAction += "  SET CA_ARCH=%PROCESSOR_ARCHITECTURE%\n";
                    scriptAction += ")\n";
                    //call uninstall-script
                    if (uninstallScriptPath != null && uninstallScriptPath.Length > 0) {
                        scriptAction += "call " + uninstallScriptPath + " -uninstall\n";
                    }
                    //call install-script
                    {
                        scriptAction += "call " + installScriptPath + " -install\n";
                    }
                    //version-descriptor
                    {
                        //registry
                        {
                            scriptAction += "reg add \"HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\" + CA_UPDATER_APP_REGISTRY_NAME  + "\" /v InstallBootDate /t REG_SZ /d 0 >NUL 2>&1\n";
                            scriptAction += "reg add \"HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\" + CA_UPDATER_APP_REGISTRY_NAME + "\" /v InstallDateStamp /t REG_SZ /d " + DateTimeOffset.Now.ToUnixTimeSeconds() + " >NUL 2>&1\n";
                            //default server (optional)
                            if (defServerUrl != null && defServerUrl.Length > 0) {
                                scriptAction += "reg add \"HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\" + CA_UPDATER_APP_REGISTRY_NAME + "\" /v DefaultServerUrl /t REG_SZ /d \"" + defServerUrl + "\" >NUL 2>&1\n";
                            }
                        }
                    }
                } else if (isUninstallAction && uninstallScriptPath != null && uninstallScriptPath.Length > 0) {
                    scriptAction += "setlocal EnableDelayedExpansion\n";
                    //detect architecture
                    scriptAction += "SET CA_ARCH=\n";
                    scriptAction += "IF DEFINED PROCESSOR_ARCHITEW6432 (\n";
                    scriptAction += "  SET CA_ARCH=%PROCESSOR_ARCHITEW6432%\n";
                    scriptAction += ") ELSE (\n";
                    scriptAction += "  SET CA_ARCH=%PROCESSOR_ARCHITECTURE%\n";
                    scriptAction += ")\n";
                    //call uninstall-script
                    {
                        scriptAction += "call " + uninstallScriptPath + " -uninstall\n";
                    }
                }
                //add finalization steps
                if (scriptAction != null || scriptAction.Length > 0) {
                    if (extractionRootPath != null && extractionRootPath.Length > 0) {
                        //delete
                        {
                            scriptAction += "rmdir \"" + extractionRootPath + "\" /s /q >NUL 2>&1\n";
                        }
                    }
                    //append downloaded dmg actions (delete)
                    if (downloadedFilePath != null && downloadedFilePath.Length > 0) {
                        scriptAction += "del \""+ downloadedFilePath + "\" >NUL 2>&1\n";
                    }
                    //append delete-itself-cmd
                    {
                        //scriptAction += "del \"" + scriptActionPath + "\" >NUL 2>&1\n";
                    }
                    //reopen app
                    {
                        bool shouldReopenApp = true;
                        String curExePath = System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
                        if (curExePath != null && curExePath.Length > 0) {
                            String appExe = System.IO.Path.GetFileName(curExePath);
                            if (appExe != null && appExe.Length > 0) {
                                if(appExe.ToLower() != CA_UPDATER_APP_CLIENT_EXE.ToLower()) {
                                    //no need to reopen this app,
                                    //name does not match the installer
                                    //automatically closed exe(s).
                                    shouldReopenApp = false;
                                }
                            }
                        }
                        if (shouldReopenApp){
                            scriptAction += "\"" + System.Reflection.Assembly.GetExecutingAssembly().Location + "\"\n";
                        }
                    }
                    //final message
                    scriptAction += "echo ----------------------------------------.\n";
                    scriptAction += "echo End-of-script, you can close this window.\n";
                    scriptAction += "echo ----------------------------------------.\n";
                }
                //write to file and execute
                if (scriptAction == null || scriptAction.Length <= 0) {
                    if (sync.lastErrorDesc == null) {
                        sync.lastWasError = true;
                        sync.lastErrorDesc = "Missing script.\n";
                        Debug.WriteLine("Error (latest-install): " + sync.lastErrorDesc);
                    }
                } else {
                    bool written = false; //bat
                    try {
                        File.WriteAllText(scriptActionPath, scriptAction);
                        written = true;
                    } catch (Exception e) {
                        if (sync.lastErrorDesc == null) {
                            sync.lastWasError = true;
                            sync.lastErrorDesc = "Could not write bat-script.\n";
                            Debug.WriteLine("Error (action): " + sync.lastErrorDesc);
                        }
                    }
                    //Execute
                    if (written) {
                        //using cmd
                        {
                            Debug.WriteLine("Executing script.");
                            ProcessStartInfo psi = new ProcessStartInfo();
                            psi.UseShellExecute = true;
                            psi.FileName = "cmd.exe";
                            psi.Arguments = "/C \"" + scriptActionPath + "\"";
                            psi.Verb = "runas";
                            try {
                                Process proc = new Process();
                                proc.StartInfo = psi;
                                proc.Start();
                                Debug.WriteLine("Executing scripts.");
                                proc.WaitForExit();
                                Debug.WriteLine("Executed scripts.");
                                r = true;
                            } catch (Exception) {
                                Debug.WriteLine("Failed to execute scripts.");
                            }
                        }
                    }
                }
            }
            return r;
        }

    }
}
