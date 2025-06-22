using Contab;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Contab {
    public class ClsTunnel {

        //core
        IntPtr _core = IntPtr.Zero;
        Thread _thread = null;
        bool _isRunning = false;

        //
        public ClsTunnel() {
            _core = ClsTunnel.TNCoreAlloc();
        }

        ~ClsTunnel() {
            if (_core != IntPtr.Zero) {
                ClsTunnel.TNCoreRelease(_core);
                _core = IntPtr.Zero;
            }
        }

        //

        //Add one CA certificate to the global list of CAs.
        //STTNCoreCfgSslCertSrc
        /*
        * {
        *    "path": string
        *    , "pay64": string
        * }
        */
        public bool addCA(string jsonCfg) {
            bool r = false;
            if (_core != IntPtr.Zero) {
                r = ClsTunnel.TNCoreAddCA(_core, jsonCfg);
            }
            return r;
        }

        //Add one or more CAs certificates to the global list of CAs.
        //STTNCoreCfgCAs
        /*
        * {
        *    "cas": [ STTNCoreCfgSslCertSrc, ... ] (view structure at 'TNCoreAddCA' method)
        * }
        */
        public bool addCAs(string jsonCfg) {
            bool r = false;
            if (_core != IntPtr.Zero) {
                r = ClsTunnel.TNCoreAddCAs(_core, jsonCfg);
            }
            return r;
        }

        //Add one port to the core.
        //STTNCoreCfgPort
        /*
        * {
        *    "port": number
        *    , "layers": [string, ...] ("ssl", "mask", ...)
        *    , "ssl": {
        *       "cert": {
        *          "isRequested": bool
        *          , "isRequired": bool
        *          , "source": {
        *             "path": string
        *             , "pay64": string
        *             , "key": {
        *                "pass": string
        *                , "path": string
        *                , "pay64": string
        *                , "name": string
        *             }
        *          }
        *       }
        *    }
        *    , "mask": {
        *       "seed": number-0-255
        *    }
        *    , "redir": {
        *       , "server": string
        *       , "port": number
        *       , "layers": [string, ...] ("ssl", "mask", ...)
        *       , "ssl": {
        *          "cert": {
        *             "isRequested": bool
        *             , "isRequired": bool
        *             , "source": {
        *                "path": string
        *                , "pay64": string
        *                , "key": {
        *                   "pass": string
        *                   , "path": string
        *                   , "pay64": string
        *                   , "name": string
        *                }
        *             }
        *          }
        *       }
        *       , "mask": {
        *          "seed": number-0-255
        *       }
        *    }
        * }
        */
        public bool addPort(string jsonCfg) {
            bool r = false;
            if (_core != IntPtr.Zero) {
                r = ClsTunnel.TNCoreAddPort(_core, jsonCfg);
            }
            return r;
        }

        //Add one or more ports to the core.
        //STTNCoreCfgPorts
        /*
        * {
        *   "ports": [ STTNCoreCfgPort, ...] (view structure at 'TNCorePreparePort' method)
        * }
        */
        public bool addPorts(string jsonCfg) {
            bool r = false;
            if (_core != IntPtr.Zero) {
                r = ClsTunnel.TNCoreAddPorts(_core, jsonCfg);
            }
            return r;
        }


        //Finalize config.
        public bool prepare() {
            bool r = false;
            if (_core != IntPtr.Zero) {
                r = ClsTunnel.TNCorePrepare(_core);
            }
            return r;
        }

        //Starts listening port.
        public bool startListening() {
            bool r = false;
            if (_core != IntPtr.Zero) {
                r = ClsTunnel.TNCoreStartListening(_core);
            }
            return r;
        }

        //Runs at this thread (locks it)
        public bool runAtThisThread() {
            bool r = false;
            if (_core != IntPtr.Zero) {
                r = ClsTunnel.TNCoreRunAtThisThread(_core);
            }
            if (_thread != null) {
                _thread = null;
            }
            return r;
        }

        private static void run(object pObj) {
            ClsTunnel obj = (ClsTunnel)pObj;
            obj._isRunning = true;
            {
                obj.runAtThisThread();
            }
            obj._isRunning = false;
        }

        //Runs at this thread (locks it)
        public bool runAtNewThread() {
            bool r = false;
            if (_core != IntPtr.Zero && !_isRunning) {
                _thread = new Thread(ClsTunnel.run);
                _isRunning = true;
                _thread.Start(this);
                r = true;
            }
            return r;
        }

        //IsRunning
        public bool isRunning() {
            bool r = false;
            if (_core != IntPtr.Zero) {
                r = (_isRunning || ClsTunnel.TNCoreIsRunning(_core));
            }
            return r;
        }

        //Flags the core to stop and clean resources, and returns inmediately.
        public void stopFlag() {
            if (_core != IntPtr.Zero) {
                ClsTunnel.TNCoreStopFlag(_core);
            }
            /*if (_thread != null) {
                try {
                    _thread.Abort();
                } catch (Exception excp) {
                    Debug.WriteLine("_thread.Abort exeption: " + excp.Message);
                }
            }*/
        }





        //
        [DllImport("tunnel.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern IntPtr TNCoreAlloc();

        [DllImport("tunnel.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern void TNCoreRelease(IntPtr core);
        //

        //Add one CA certificate to the global list of CAs.
        //STTNCoreCfgSslCertSrc
        /*
        * {
        *    "path": string
        *    , "pay64": string
        * }
        */
        [DllImport("tunnel.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern bool TNCoreAddCA(IntPtr core, string jsonCfg);

        //Add one or more CAs certificates to the global list of CAs.
        //STTNCoreCfgCAs
        /*
        * {
        *    "cas": [ STTNCoreCfgSslCertSrc, ... ] (view structure at 'TNCoreAddCA' method)
        * }
        */
        [DllImport("tunnel.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern bool TNCoreAddCAs(IntPtr core, string jsonCfg);

        //Add one port to the core.
        //STTNCoreCfgPort
        /*
        * {
        *    "port": number
        *    , "layers": [string, ...] ("ssl", "mask", ...)
        *    , "ssl": {
        *       "cert": {
        *          "isRequested": bool
        *          , "isRequired": bool
        *          , "source": {
        *             "path": string
        *             , "pay64": string
        *             , "key": {
        *                "pass": string
        *                , "path": string
        *                , "pay64": string
        *                , "name": string
        *             }
        *          }
        *       }
        *    }
        *    , "mask": {
        *       "seed": number-0-255
        *    }
        *    , "redir": {
        *       , "server": string
        *       , "port": number
        *       , "layers": [string, ...] ("ssl", "mask", ...)
        *       , "ssl": {
        *          "cert": {
        *             "isRequested": bool
        *             , "isRequired": bool
        *             , "source": {
        *                "path": string
        *                , "pay64": string
        *                , "key": {
        *                   "pass": string
        *                   , "path": string
        *                   , "pay64": string
        *                   , "name": string
        *                }
        *             }
        *          }
        *       }
        *       , "mask": {
        *          "seed": number-0-255
        *       }
        *    }
        * }
        */
        [DllImport("tunnel.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern bool TNCoreAddPort(IntPtr core, string jsonCfg);

        //Add one or more ports to the core.
        //STTNCoreCfgPorts
        /*
        * {
        *   "ports": [ STTNCoreCfgPort, ...] (view structure at 'TNCorePreparePort' method)
        * }
        */
        [DllImport("tunnel.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern bool TNCoreAddPorts(IntPtr core, string jsonCfg);


        //Finalize config
        [DllImport("tunnel.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern bool TNCorePrepare(IntPtr core);

        //Starts listening port.
        [DllImport("tunnel.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern bool TNCoreStartListening(IntPtr core);

        //Runs at this thread (locks it)
        [DllImport("tunnel.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern bool TNCoreRunAtThisThread(IntPtr core);

        //IsRunning
        [DllImport("tunnel.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern bool TNCoreIsRunning(IntPtr core);

        //Flags the core to stop and clean resources, and returns inmediately.
        [DllImport("tunnel.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        private static extern void TNCoreStopFlag(IntPtr core);


    }
}
