using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contab {
    class ClsLaunchCfg {

        public String executablePath = "";  //first argument in main() call
        public bool runAsDefault = true;            //no "-runAs*" option was specified
        //public bool runsAsServiceCompetitor = false; //"-runsAsServiceCompetitor" open as user-agent (service) with other posibles instances.
                                            //                           Asumes multiple instances could be running, runs until (a stopSignal)
                                            //                           or (take of port/lock ownership by this instance + it runs the service) and exits.
        //public bool runAsService = false;            //"-runAsService" open as system-service as administrator.
                                            //                Tries to run as the unnique instance of
                                            //                the service in the system, exits if fails.
        public bool runAsManager = false;            //"-runAsManager" open installer window first/only
        
        private static ClsLaunchCfg _sharedInstance = null;

        private ClsLaunchCfg() { 
            //nothing
        }

        public static ClsLaunchCfg getSharedInstance() {
            if (_sharedInstance == null) {
                _sharedInstance = new ClsLaunchCfg();
                _sharedInstance.executablePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            }
            return _sharedInstance;
        }

    }
}
