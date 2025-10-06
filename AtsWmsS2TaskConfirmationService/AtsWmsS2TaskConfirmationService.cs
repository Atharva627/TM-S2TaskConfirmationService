using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace AtsWmsS2TaskConfirmationService
{

    public partial class AtsWmsS2TaskConfirmationService : ServiceBase
    {
        static string className = "AtsWmsS2TaskConfirmationService";
        private static readonly ILog Log = LogManager.GetLogger(className);
        public AtsWmsS2TaskConfirmationService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Log.Debug("OnStart ::  AtsWmsS2TaskConfirmationService in OnStart....");

                try
                {
                    XmlConfigurator.Configure();
                    try
                    {
                        AtsWmsS2TaskConfirmationServiceTaskThread();
                    }
                    catch (Exception ex)
                    {
                        Log.Error("OnStart :: Exception occured while  AtsWmsS2TaskConfirmationServiceTaskThread  threads task :: " + ex.Message);
                    }
                    Log.Debug("OnStart ::  AtsWmsS2TaskConfirmationServiceTaskThread in OnStart ends..!!");
                }
                catch (Exception ex)
                {
                    Log.Error("OnStart :: Exception occured in OnStart :: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                Log.Error("OnStart :: Exception occured in OnStart :: " + ex.Message);
            }
        }
        public void AtsWmsS2TaskConfirmationServiceTaskThread()
        {

            try
            {
                AtsWmsS2TaskConfirmationServiceDetails AtsWmsS2TaskConfirmationServiceDetailsInstance = new AtsWmsS2TaskConfirmationServiceDetails();
                AtsWmsS2TaskConfirmationServiceDetailsInstance.startOperation();
            }
            catch (Exception ex)
            {
                Log.Error("TestService :: Exception in  AtsWmsS2TaskConfirmationDetailseTaskThread :: " + ex.Message);
            }


        }

        protected override void OnStop()
        {
            try
            {
                Log.Debug("OnStop ::  AtsWmsS2TaskConfirmationDetails in OnStop ends..!!");
            }
            catch (Exception ex)
            {
                Log.Error("OnStop :: Exception occured in OnStop :: " + ex.Message);
            }
        }
    }
}
