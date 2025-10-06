using AtsWmsS2TaskConfirmationService.ats_tata_metallics_dbDataSetTableAdapters;
using log4net;
using OPCAutomation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using static AtsWmsS2TaskConfirmationService.ats_tata_metallics_dbDataSet;

namespace AtsWmsS2TaskConfirmationService
{
    class AtsWmsS2TaskConfirmationServiceDetails
    {
        #region Data Tables
        ats_wms_master_plc_connection_detailsDataTable ats_wms_master_plc_connection_detailsDataTableDT = null;
        ats_wms_infeed_mission_runtime_detailsDataTable ats_wms_infeed_mission_runtime_detailsDataTableDT = null;
        ats_wms_master_pallet_informationDataTable ats_wms_master_pallet_informationDataTableDT = null;
        ats_wms_current_stock_detailsDataTable ats_wms_current_stock_detailsDataTableDT = null;
        ats_wms_master_position_detailsDataTable ats_wms_master_position_detailsDataTableDT = null;
        ats_wms_outfeed_mission_runtime_detailsDataTable ats_wms_outfeed_mission_runtime_detailsDataTableDT = null;
        ats_wms_transfer_pallet_mission_runtime_detailsDataTable ats_wms_transfer_pallet_mission_runtime_detailsDataTableDT = null;
        ats_wms_transfer_pallet_mission_runtime_detailsDataTable ats_wms_transfer_pallet_mission_runtime_detailsDataTableMissionDT = null;
        ats_wms_ccm_buffer_detailsDataTable ats_wms_ccm_buffer_detailsTableAdapterDT = null;
        ats_wms_master_task_confirmation_tag_detailsDataTable ats_wms_master_task_confirmation_tag_detailsDataTableTaskCompletionDT = null;
        ats_wms_master_task_confirmation_tag_detailsDataTable ats_wms_master_task_confirmation_tag_detailsDataTableFBTaskTypeDT = null;
        ats_wms_master_task_confirmation_tag_detailsDataTable ats_wms_master_task_confirmation_tag_detailsDataTableFBTaskNoDT = null;
        ats_wms_master_task_confirmation_tag_detailsDataTable ats_wms_master_task_confirmation_tag_detailsDataTableTaskConfermationDT = null;
        ats_wms_master_task_confirmation_tag_detailsDataTable ats_wms_master_task_confirmation_tag_detailsDataTableControlModeDT = null;
        ats_wms_master_pallet_type_detailsDataTable ats_wms_master_pallet_type_detailsDataTableDT = null;

        //ats_wms_tempreture_alarm_mission_runtime_detailsDataTable ats_wms_tempreture_alarm_mission_runtime_detailsDataTableDT = null;
        #endregion

        #region Table Aadaptor
        ats_wms_master_plc_connection_detailsTableAdapter ats_wms_master_plc_connection_detailsTableAdapterInstance = new ats_wms_master_plc_connection_detailsTableAdapter();

        ats_wms_infeed_mission_runtime_detailsTableAdapter ats_wms_infeed_mission_runtime_detailsTableAdapterInstance = new ats_wms_infeed_mission_runtime_detailsTableAdapter();
        ats_wms_master_pallet_informationTableAdapter ats_wms_master_pallet_informationTableAdapterInstance = new ats_wms_master_pallet_informationTableAdapter();
        ats_wms_current_stock_detailsTableAdapter ats_wms_current_stock_detailsTableAdapterInstance = new ats_wms_current_stock_detailsTableAdapter();
        ats_wms_master_position_detailsTableAdapter ats_wms_master_position_detailsTableAdapterInstance = new ats_wms_master_position_detailsTableAdapter();
        ats_wms_outfeed_mission_runtime_detailsTableAdapter ats_wms_outfeed_mission_runtime_detailsTableAdapterInstance = new ats_wms_outfeed_mission_runtime_detailsTableAdapter();
        ats_wms_transfer_pallet_mission_runtime_detailsTableAdapter ats_wms_transfer_pallet_mission_runtime_detailsTableAdapterInstance = new ats_wms_transfer_pallet_mission_runtime_detailsTableAdapter();
        ats_wms_ccm_buffer_detailsTableAdapter ats_wms_ccm_buffer_detailsTableAdapterInstance = new ats_wms_ccm_buffer_detailsTableAdapter();
        ats_wms_master_pallet_type_detailsTableAdapter ats_wms_master_pallet_type_detailsTableAdapterInstance = new ats_wms_master_pallet_type_detailsTableAdapter();
        ats_wms_master_task_confirmation_tag_detailsTableAdapter ats_wms_master_task_confirmation_tag_detailsTableAdapterInstance = new ats_wms_master_task_confirmation_tag_detailsTableAdapter();


        // ats_wms_buffer_detailsTableAdapter ats_wms_buffer_detailsTableAdapterInstance = new ats_wms_buffer_detailsTableAdapter();
        //ats_wms_tempreture_alarm_mission_runtime_detailsTableAdapter ats_wms_tempreture_alarm_mission_runtime_detailsTableAdapterInstance = new ats_wms_tempreture_alarm_mission_runtime_detailsTableAdapter();
        #endregion

        #region Global Variables
        static string className = "AtsWmsS2TaskConfirmationServiceDetails";
        private static readonly ILog Log = LogManager.GetLogger(className);
        private System.Timers.Timer AtsWmsS2TaskConfirmationDetailsTimer = null;
        //private string IP_ADDRESS = "10.10.56.131";

        string currentDate = "";
        string currentTime = "";
        int areaId = 1;
        int positionNumberInRack = 0;
        public int stackerAreaSide = 0;
        public int stackerFloor = 0;
        public int stackerColumn = 0;
        public int destinationPositionNumberInRack = 1;
        int palletPresentOnStackerPickupPosition = 0;
        string palletCodeOnStackerPickupPosition = "";
        int stackerRightSide = 2;
        int stackerLeftSide = 1;
        int sourcePositionTagType = 0;
        int destinationPositionTagType = 1;
        int feedbackTagType = 2;
        int STACKER_2 = 2;
        int TASK_COMPLETION = 1;
        int FB_TASK_NO = 2;
        int FB_TASK_TYPE = 3;
        int TASK_CONFERMATION = 4;
        int CONTROL_MODE = 6;
        #endregion

        #region PLC PING VARIABLE   
        //private string IP_ADDRESS = System.Configuration.ConfigurationManager.AppSettings["IP_ADDRESS"]; //2
        string IP_ADDRESS = "";
        private Ping pingSenderForThisConnection = null;
        private PingReply replyForThisConnection = null;
        private Boolean pingStatus = false;
        private int serverPingStatusCount = 0;
        #endregion

        #region KEPWARE VARIABLES

        /* Kepware variable*/

        OPCServer ConnectedOpc = new OPCServer();

        Array OPCItemIDs = Array.CreateInstance(typeof(string), 100);
        Array ItemServerHandles = Array.CreateInstance(typeof(Int32), 100);
        Array ItemServerErrors = Array.CreateInstance(typeof(Int32), 100);
        Array ClientHandles = Array.CreateInstance(typeof(Int32), 100);
        Array RequestedDataTypes = Array.CreateInstance(typeof(Int16), 100);
        Array AccessPaths = Array.CreateInstance(typeof(string), 100);
        Array ItemServerValues = Array.CreateInstance(typeof(string), 100);
        OPCGroup OpcGroupNamesA1T;
        object aTC;
        object bTC;

        // Connection string
        static string plcServerConnectionString = null;

        #endregion


        public void startOperation()
        {
            try
            {
                Log.Debug("startOperation");
                AtsWmsS2TaskConfirmationDetailsTimer = new System.Timers.Timer();
                //Running the function after 1 sec 
                AtsWmsS2TaskConfirmationDetailsTimer.Interval = (1500);
                //After 1 sec timer will elapse and DataFetchDetailsOperation function will be called 
                AtsWmsS2TaskConfirmationDetailsTimer.Elapsed += new System.Timers.ElapsedEventHandler(AtsWmsS2TaskConfirmationServiceDetailsOperation);
                AtsWmsS2TaskConfirmationDetailsTimer.AutoReset = false;
                AtsWmsS2TaskConfirmationDetailsTimer.Start();
            }
            catch (Exception ex)
            {
                Log.Error("startOperation :: Exception Occure in AtsWmsS2TaskConfirmationDetailsTimer" + ex.Message);
            }
        }

        private void AtsWmsS2TaskConfirmationServiceDetailsOperation(object sender, ElapsedEventArgs e)
        {
            try
            {

                try
                {
                    AtsWmsS2TaskConfirmationDetailsTimer.Stop();

                }
                catch (Exception ex)
                {
                    Log.Error("AtsWmsTaskConfirmationDetailsOperation :: Exception occure while stopping the timer :: " + ex.Message + "StackTrace  :: " + ex.StackTrace);
                }
                try
                {
                    //Fetching PLC data from DB by sending PLC connection IP address
                    ats_wms_master_plc_connection_detailsDataTableDT = ats_wms_master_plc_connection_detailsTableAdapterInstance.GetData();
                    if (ats_wms_master_plc_connection_detailsDataTableDT != null && ats_wms_master_plc_connection_detailsDataTableDT.Count > 0)
                    {
                        IP_ADDRESS = ats_wms_master_plc_connection_detailsDataTableDT[0].PLC_CONNECTION_IP_ADDRESS;
                        Log.Debug("2.1 :: IP_ADDRESS ::" + IP_ADDRESS);

                    }
                    else
                    {
                        Log.Debug("2.2 :: PLC Data Not found in Table");
                    }
                }
                catch (Exception ex)
                {

                    Log.Error("AtsWmsS2TaskConfirmationDetailsOperation :: Exception Occure while reading machine datasource connection details from the database :: " + ex.Message + "StackTrace :: " + ex.StackTrace);
                }


                // Check PLC Ping Status
                try
                {
                    // Checking the PLC ping status by a method
                    pingStatus = checkPlcPingRequest();
                    Log.Debug("ping" + pingStatus);
                }
                catch (Exception ex)
                {
                    Log.Error("AtsWmsS2TaskConfirmationDetailsOperation :: Exception while checking plc ping status :: " + ex.Message + " stactTrace :: " + ex.StackTrace);
                }

                if (pingStatus == true)

                //if (true)
                {

                    if (ats_wms_master_plc_connection_detailsDataTableDT != null & ats_wms_master_plc_connection_detailsDataTableDT.Count != 0)
                    {
                        //if (true)

                        Log.Debug("plc connect");
                        try
                        {
                            plcServerConnectionString = ats_wms_master_plc_connection_detailsDataTableDT[0].PLC_CONNECTION_URL;
                        }
                        catch (Exception ex)
                        {
                            Log.Error("AtsWmsS2TaskConfirmationDetailsOperation :: Exception while Checking plcServerConnectionString :: " + ex.Message + " stackTrace :: " + ex.StackTrace);
                        }
                        try
                        {
                            // Calling the connection method for PLC connection

                            OnConnectPLC();
                            Log.Debug("ONConnectPLC");

                        }
                        catch (Exception ex)
                        {
                            Log.Error("AtsWmsS2TaskConfirmationDetailsOperation :: Exception while connecting to plc :: " + ex.Message + " stackTrace :: " + ex.StackTrace);
                        }

                        // Check the PLC connected status
                        Log.Debug("0.0");
                        if (ConnectedOpc.ServerState.ToString().Equals("1"))
                        //if (true)
                        {
                            Log.Debug("0.1");
                            string taskCompletionValue = "";
                            ats_wms_master_task_confirmation_tag_detailsDataTableTaskCompletionDT = ats_wms_master_task_confirmation_tag_detailsTableAdapterInstance.GetDataBySTACKER_IDAndTAG_TYPE(STACKER_2, TASK_COMPLETION);
                            Log.Debug("0.2 :: Tag Details retrieved form Table");

                            if (ats_wms_master_task_confirmation_tag_detailsDataTableTaskCompletionDT != null && ats_wms_master_task_confirmation_tag_detailsDataTableTaskCompletionDT.Count > 0)
                            {


                                try
                                {
                                    taskCompletionValue = readTag(ats_wms_master_task_confirmation_tag_detailsDataTableTaskCompletionDT[0].TASK_CONFIRMATION_TAG_NAME);
                                    Log.Debug("0.3 :: taskCompletionValue " + taskCompletionValue);

                                    //taskCompletionValue = readTag("ATS.WMS_STACKER_2.STACKER_2_TASK_COMPLETION");
                                }
                                catch (Exception ex)
                                {
                                    Log.Debug("0.4");
                                    Log.Error("AtsWmsS2TaskConfirmationDetailsOperation ::  " + ex.Message + " Stacktrace:: " + ex.StackTrace);
                                }
                                if (taskCompletionValue.Equals("10"))
                                {
                                    Log.Debug("0.5");
                                    Log.Debug("AtsWmsS2TaskConfirmationDetailsOperation :: taskCompletionValue " + taskCompletionValue);

                                    ats_wms_master_task_confirmation_tag_detailsDataTableFBTaskNoDT = ats_wms_master_task_confirmation_tag_detailsTableAdapterInstance.GetDataBySTACKER_IDAndTAG_TYPE(STACKER_2, FB_TASK_NO);
                                    Log.Debug("1");
                                    string fbTaskNo = "";

                                    if (ats_wms_master_task_confirmation_tag_detailsDataTableFBTaskNoDT != null && ats_wms_master_task_confirmation_tag_detailsDataTableFBTaskNoDT.Count > 0)
                                    {


                                        try
                                        {
                                            //fbTaskNo = readTag("ATS.WMS_STACKER_2.STACKER_2_FB_TASK_NO");
                                            fbTaskNo = readTag(ats_wms_master_task_confirmation_tag_detailsDataTableFBTaskNoDT[0].TASK_CONFIRMATION_TAG_NAME);
                                            Log.Debug("fbTaskNo::" + fbTaskNo);
                                        }
                                        catch (Exception ex)
                                        {

                                            Log.Error("Exception occurred while reading task number :: " + ex.StackTrace);
                                        }
                                        string fbtaskType = "";
                                        ats_wms_master_task_confirmation_tag_detailsDataTableFBTaskTypeDT = ats_wms_master_task_confirmation_tag_detailsTableAdapterInstance.GetDataBySTACKER_IDAndTAG_TYPE(STACKER_2, FB_TASK_TYPE);
                                        Log.Debug("2");

                                        if (ats_wms_master_task_confirmation_tag_detailsDataTableFBTaskTypeDT != null && ats_wms_master_task_confirmation_tag_detailsDataTableFBTaskTypeDT.Count > 0)
                                        {


                                            try
                                            {


                                                //fbtaskType = readTag("ATS.WMS_STACKER_2.STACKER_2_FB_TASK_TYPE");
                                                fbtaskType = readTag(ats_wms_master_task_confirmation_tag_detailsDataTableFBTaskTypeDT[0].TASK_CONFIRMATION_TAG_NAME);
                                                Log.Debug("fbtaskType::" + fbtaskType);
                                                Log.Debug("3");
                                                Log.Debug("AtsWmsS2TaskConfirmationDetailsOperation :: fbTaskNo: " + fbTaskNo + ":: fbtaskType: " + fbtaskType);

                                            }
                                            catch (Exception ex)
                                            {

                                                Log.Error("Exception occurred while reading Task Type :: " + ex.StackTrace);
                                            }

                                            //Log.Debug("2.2");
                                            //string fbTaskCompletion = "";

                                            //try
                                            //{
                                            //    fbTaskCompletion = readTag("ATS.WMS_STACKER_2.STACKER_2_TASK_COMPLETION");
                                            //    Log.Debug("fbTaskCompletion::" + fbTaskCompletion);
                                            //    Log.Debug("3.1");
                                            //    Log.Debug("AtsWmsS2TaskConfirmationDetailsOperation :: fbTaskNo: " + fbTaskNo + ":: fbtaskType: " + fbtaskType + ":: fbTaskCompletion: " + fbTaskCompletion);

                                            //}
                                            //catch (Exception ex)
                                            //{

                                            //    Log.Error("Exception occurred while reading Task Completion :: " + ex.StackTrace);
                                            //}
                                            int fbTaskNoInt = 0;
                                            String timeNow = DateTime.Now.TimeOfDay.ToString();
                                            TimeSpan currentTimeNow = TimeSpan.Parse(timeNow);

                                            String currentDate = "";
                                            currentDate = Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd"));
                                            try
                                            {
                                                fbTaskNoInt = int.Parse(fbTaskNo);



                                                // INFEED MISSION
                                                if (fbtaskType.Equals("1"))
                                                {
                                                    Log.Debug("AtsWmsS2TaskConfirmationDetailsOperation :: INFEED MISSION :: task type id :: " + fbtaskType);

                                                    try
                                                    {
                                                        ats_wms_infeed_mission_runtime_detailsDataTableDT = ats_wms_infeed_mission_runtime_detailsTableAdapterInstance.GetDataByINFEED_MISSION_IDAndINFEED_MISSION_STATUS(fbTaskNoInt, "IN_PROGRESS");

                                                        Log.Debug(" in infeed 1");

                                                    }
                                                    catch (Exception ex)
                                                    {

                                                        Log.Error("AtsWmsS2TaskConfirmationDetailsOperation :: exception occurred while infeed mission details :: " + ex.Message);
                                                    }
                                                    try
                                                    {
                                                        Log.Debug("getting pallet information id :: " + ats_wms_infeed_mission_runtime_detailsDataTableDT[0].PALLET_INFORMATION_ID);
                                                        ats_wms_master_pallet_informationDataTableDT = ats_wms_master_pallet_informationTableAdapterInstance.GetDataByPALLET_INFORMATION_ID(ats_wms_infeed_mission_runtime_detailsDataTableDT[0].PALLET_INFORMATION_ID);

                                                        Log.Debug(" in infeed 3");

                                                        if (ats_wms_master_pallet_informationDataTableDT != null && ats_wms_master_pallet_informationDataTableDT.Count > 0)
                                                        {

                                                            string stackerControlMode = "";
                                                            ats_wms_master_task_confirmation_tag_detailsDataTableControlModeDT = ats_wms_master_task_confirmation_tag_detailsTableAdapterInstance.GetDataBySTACKER_IDAndTAG_TYPE(STACKER_2, CONTROL_MODE);
                                                            if (ats_wms_master_task_confirmation_tag_detailsDataTableControlModeDT != null && ats_wms_master_task_confirmation_tag_detailsDataTableControlModeDT.Count > 0)
                                                            {

                                                                try
                                                                {
                                                                    stackerControlMode = readTag(ats_wms_master_task_confirmation_tag_detailsDataTableControlModeDT[0].TASK_CONFIRMATION_TAG_NAME);
                                                                    //stackerControlMode = readTag("ATS.WMS_STACKER_2.STACKER_2_CONTROL_MODE");
                                                                    Log.Debug(" Read COntrol Mode " + stackerControlMode);

                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    Log.Error("AtsWmsS2TaskConfirmationDetailsOperation :: exception occurred while reading COntrol MOde Tag :: " + ex.Message);
                                                                }



                                                                if (stackerControlMode.Equals("3"))
                                                                {
                                                                    try
                                                                    {
                                                                        ats_wms_master_pallet_type_detailsDataTableDT = ats_wms_master_pallet_type_detailsTableAdapterInstance.GetDataByPALLET_TYPE_ID(ats_wms_infeed_mission_runtime_detailsDataTableDT[0].PALLET_TYPE_ID);
                                                                        Log.Debug(" in infeed 6");

                                                                        Log.Debug("GiveStackerMission :: Updating Position name against pallet information ID in Curent Pallet Stock details table");
                                                                        ats_wms_current_stock_detailsTableAdapterInstance.UpdatePALLET_CODEAndPALLET_INFORMATION_IDAndPRODUCT_VARIENT_CODEAndPRODUCT_NAMEAndCORESHOPAndCORE_SIZEAndPRODUCT_IDAndPALLET_STATUS_IDAndPALLET_STATUS_NAMEAndPALLET_TYPEAndQUANTITYAndBATCH_NUMBERAndLOAD_DATETIMEAndSTACKER_IDWherePOSITION_ID(
                                                                        ats_wms_infeed_mission_runtime_detailsDataTableDT[0].PALLET_CODE,
                                                                        ats_wms_infeed_mission_runtime_detailsDataTableDT[0].PALLET_INFORMATION_ID,
                                                                        ats_wms_master_pallet_informationDataTableDT[0].PRODUCT_VARIANT_CODE,
                                                                        ats_wms_master_pallet_informationDataTableDT[0].PRODUCT_NAME,
                                                                        ats_wms_infeed_mission_runtime_detailsDataTableDT[0].CORESHOP,
                                                                        ats_wms_infeed_mission_runtime_detailsDataTableDT[0].CORE_SIZE,
                                                                        ats_wms_infeed_mission_runtime_detailsDataTableDT[0].PRODUCT_ID,
                                                                        ats_wms_infeed_mission_runtime_detailsDataTableDT[0].PALLET_STATUS_ID,
                                                                        ats_wms_infeed_mission_runtime_detailsDataTableDT[0].PALLET_STATUS_NAME,
                                                                        ats_wms_master_pallet_type_detailsDataTableDT[0].PALLET_TYPE,
                                                                         ats_wms_infeed_mission_runtime_detailsDataTableDT[0].QUANTITY,
                                                                         ats_wms_infeed_mission_runtime_detailsDataTableDT[0].BATCH_NUMBER,
                                                                          DateTime.Now.ToString(),
                                                                         ats_wms_infeed_mission_runtime_detailsDataTableDT[0].STACKER_ID,
                                                                         ats_wms_infeed_mission_runtime_detailsDataTableDT[0].POSITION_ID
                                                                         );

                                                                        int pallet_status_id = ats_wms_infeed_mission_runtime_detailsDataTableDT[0].PALLET_STATUS_ID;

                                                                        Log.Debug(" pallet status id :: " + pallet_status_id);

                                                                        if (pallet_status_id == 3)
                                                                        {
                                                                            Log.Debug("Pallet status id recived is 3");

                                                                            //Updating pallet position details in DB
                                                                            Log.Debug("Updating values in Master Position Details");
                                                                            ats_wms_master_position_detailsTableAdapterInstance.UpdatePOSITION_IS_ALLOCATEDAndPOSITION_IS_EMPTYAndIS_MATERIAL_LOADEDWherePOSITION_ID(1, 0, 0, ats_wms_infeed_mission_runtime_detailsDataTableDT[0].POSITION_ID);


                                                                            //updating mission status to Completed in infeed mission details table against infeed mission id
                                                                            Log.Debug("Updating Mission status as :: COMPLETED IN AUTO-MODE");
                                                                            ats_wms_infeed_mission_runtime_detailsTableAdapterInstance.UpdateINFEED_MISSION_STATUSAndINFEED_MISSION_END_DATETIMEWhereINFEED_MISSION_ID("COMPLETED", (currentDate + " " + currentTimeNow), ats_wms_infeed_mission_runtime_detailsDataTableDT[0].INFEED_MISSION_ID);

                                                                        }
                                                                        else
                                                                        {
                                                                            Log.Debug("Pallet Info Id recived is Not 3");
                                                                            //Updating pallet position details in DB
                                                                            Log.Debug("Updating values in Master Position Details");
                                                                            ats_wms_master_position_detailsTableAdapterInstance.UpdatePOSITION_IS_ALLOCATEDAndPOSITION_IS_EMPTYAndIS_MATERIAL_LOADEDWherePOSITION_ID(1, 0, 1, ats_wms_infeed_mission_runtime_detailsDataTableDT[0].POSITION_ID);

                                                                            //updating mission status to Completed in infeed mission details table against infeed mission id
                                                                            Log.Debug("Updating Mission status as :: COMPLETED IN AUTO-MODE");
                                                                            ats_wms_infeed_mission_runtime_detailsTableAdapterInstance.UpdateINFEED_MISSION_STATUSAndINFEED_MISSION_END_DATETIMEWhereINFEED_MISSION_ID("COMPLETED", (currentDate + " " + currentTimeNow), ats_wms_infeed_mission_runtime_detailsDataTableDT[0].INFEED_MISSION_ID);

                                                                            //ats_wms_master_pallet_informationTableAdapterInstance.UpdateSTATION_WORKDONEWherePALLET_INFORMATION_ID(0, ats_wms_infeed_mission_runtime_detailsDataTableDT[0].PALLET_INFORMATION_ID);

                                                                        }

                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        Log.Error("GiveStackerMission :: Exception occured while updating pallet information in DB :: " + ex.Message + " stackTrace :: " + ex.StackTrace);
                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    Log.Debug("COntrol Mode not recieving as 3");
                                                                }
                                                                //else
                                                                //{
                                                                //    //Updating pallet position details in DB
                                                                //    Log.Debug("Updating values in Master Position Details in SEMI AUTO");
                                                                //    ats_wms_master_position_detailsTableAdapterInstance.UpdatePOSITION_IS_ALLOCATEDAndPOSITION_IS_EMPTYAndIS_MATERIAL_LOADEDWherePOSITION_ID(1, 0, 1, ats_wms_infeed_mission_runtime_detailsDataTableDT[0].POSITION_ID);

                                                                //    //updating mission status to Completed in infeed mission details table against infeed mission id
                                                                //    Log.Debug("Updating Mission status as :: COMPLETED");
                                                                //    ats_wms_infeed_mission_runtime_detailsTableAdapterInstance.UpdateINFEED_MISSION_STATUSAndINFEED_MISSION_END_DATETIMEWhereINFEED_MISSION_ID("COMPLETED", (currentDate + " " + currentTimeNow), ats_wms_infeed_mission_runtime_detailsDataTableDT[0].INFEED_MISSION_ID);
                                                                //    Log.Debug("AtsWmsTaskConfirmationDetailsOperation :: INFEED MISSION DETAILS : POSITION_ID: " + ats_wms_infeed_mission_runtime_detailsDataTableDT[0].POSITION_ID);
                                                                //}
                                                                try
                                                                {
                                                                    //updating buffer is deleted to 1
                                                                    // ats_wms_buffer_detailsTableAdapterInstance.UpdateBUFFER_IS_DELETEDWherePALLET_INFORMATION_ID(1, ats_wms_master_pallet_informationDataTableDT[0].PALLET_INFORMATION_ID);
                                                                    Log.Debug("buffer is deleted updated tob 1");
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    Log.Error("Exception Occured While Updating Is Buffer Deleted" + ex.Message + ex.StackTrace);
                                                                }
                                                                try
                                                                {
                                                                    Log.Debug("AtsWmsTaskConfirmationDetailsOperation :: INFEED MISSION DETAILS : writing Task confirmation 1");
                                                                    //writeTag("ATS.WMS_AREA_3.AREA_1_STACKER_3_TASK_CONFERMATION", "1");
                                                                    Log.Debug("AtsWmsTaskConfirmationDetailsOperation :: INFEED MISSION DETAILS : Task confirmation 1 Successfully written");
                                                                    //for (;;)
                                                                    //{
                                                                    //Thread.Sleep(1000);
                                                                    //writeTag("ATS.WMS_STACKER_2.STACKER_2_TASK_CONFERMATION", "1");
                                                                    //Thread.Sleep(2000);

                                                                    //if (readTag("ATS.WMS_STACKER_2.STACKER_2_DATA_REQUEST").Equals("True"))
                                                                    //{
                                                                    //    Log.Debug("AtsWmsTaskConfirmationDetailsOperation :: INFEED MISSION DETAILS : data request True");
                                                                    //    Log.Debug("AtsWmsTaskConfirmationDetailsOperation :: INFEED MISSION DETAILS : writing Task confirmation 0");
                                                                    //    writeTag("ATS.WMS_STACKER_2.STACKER_2_TASK_CONFERMATION", "0");
                                                                    //    Log.Debug("AtsWmsTaskConfirmationDetailsOperation :: INFEED MISSION DETAILS : Task confirmation 0 Successfully written");
                                                                    //    break;
                                                                    //}
                                                                    //else
                                                                    //{
                                                                    //    Log.Error("data request false");
                                                                    //}

                                                                    // changing above logic 25-11-2025

                                                                    // changing 11/12/2024

                                                                    // Check that all details updated successfully in current stock

                                                                    ats_wms_infeed_mission_runtime_detailsDataTableDT = ats_wms_infeed_mission_runtime_detailsTableAdapterInstance.GetDataByINFEED_MISSION_IDAndINFEED_MISSION_STATUS(fbTaskNoInt, "COMPLETED");


                                                                    if (ats_wms_infeed_mission_runtime_detailsDataTableDT != null && ats_wms_infeed_mission_runtime_detailsDataTableDT.Count > 0)
                                                                    {
                                                                        ats_wms_current_stock_detailsDataTableDT = ats_wms_current_stock_detailsTableAdapterInstance.GetDataByPALLET_INFORMATION_ID(ats_wms_infeed_mission_runtime_detailsDataTableDT[0].PALLET_INFORMATION_ID);
                                                                        if (ats_wms_current_stock_detailsDataTableDT != null && ats_wms_current_stock_detailsDataTableDT.Count > 0)
                                                                        {
                                                                            // Check that pallet details in master position table
                                                                            ats_wms_master_position_detailsDataTableDT = ats_wms_master_position_detailsTableAdapterInstance.GetDataByPOSITION_ID(ats_wms_infeed_mission_runtime_detailsDataTableDT[0].POSITION_ID);
                                                                            if (ats_wms_master_position_detailsDataTableDT != null && ats_wms_master_position_detailsDataTableDT.Count > 0)
                                                                            {
                                                                                if (ats_wms_master_position_detailsDataTableDT[0].POSITION_IS_EMPTY == 0 && ats_wms_master_position_detailsDataTableDT[0].POSITION_IS_ALLOCATED == 1)
                                                                                {

                                                                                    string fbTaskCompletion = "";

                                                                                    try
                                                                                    {
                                                                                        fbTaskCompletion = readTag(ats_wms_master_task_confirmation_tag_detailsDataTableTaskCompletionDT[0].TASK_CONFIRMATION_TAG_NAME);
                                                                                        //fbTaskCompletion = readTag("ATS.WMS_STACKER_2.STACKER_2_TASK_COMPLETION");
                                                                                        Log.Debug("fbTaskCompletion::" + fbTaskCompletion);

                                                                                        Log.Debug("AtsWmsS2TaskConfirmationDetailsOperation :: fbTaskNo: " + fbTaskNo + ":: fbtaskType: " + fbtaskType + ":: fbTaskCompletion: " + fbTaskCompletion);

                                                                                    }
                                                                                    catch (Exception ex)
                                                                                    {

                                                                                        Log.Error("Exception occurred while reading Task Completion :: " + ex.StackTrace);
                                                                                    }

                                                                                    ats_wms_master_task_confirmation_tag_detailsDataTableTaskConfermationDT = ats_wms_master_task_confirmation_tag_detailsTableAdapterInstance.GetDataBySTACKER_IDAndTAG_TYPE(STACKER_2, TASK_CONFERMATION);
                                                                                    Log.Debug("Retrieving Confirmation Tag from Table");
                                                                                    if (ats_wms_master_task_confirmation_tag_detailsDataTableTaskConfermationDT != null && ats_wms_master_task_confirmation_tag_detailsDataTableTaskConfermationDT.Count > 0)
                                                                                    {

                                                                                        if (fbTaskCompletion.Equals("10"))
                                                                                        {
                                                                                            Thread.Sleep(1000);
                                                                                            writeTag(ats_wms_master_task_confirmation_tag_detailsDataTableTaskConfermationDT[0].TASK_CONFIRMATION_TAG_NAME, "1");
                                                                                            Log.Debug("AtsWmsTaskConfirmationDetailsOperation :: INFEED MISSION DETAILS : Task confirmation 1 Successfully written");
                                                                                            //writeTag("ATS.WMS_STACKER_2.STACKER_2_TASK_CONFERMATION", "1");


                                                                                        }
                                                                                        else
                                                                                        {
                                                                                            Log.Debug("AtsWmsTaskConfirmationDetailsOperation :: INFEED MISSION DETAILS : task completion is not 10");
                                                                                            Log.Debug("AtsWmsTaskConfirmationDetailsOperation :: INFEED MISSION DETAILS : writing Task confirmation 0");
                                                                                            writeTag(ats_wms_master_task_confirmation_tag_detailsDataTableTaskConfermationDT[0].TASK_CONFIRMATION_TAG_NAME, "0");
                                                                                            //writeTag("ATS.WMS_STACKER_2.STACKER_2_TASK_CONFERMATION", "0");
                                                                                            Log.Debug("AtsWmsTaskConfirmationDetailsOperation :: INFEED MISSION DETAILS : Task confirmation 0 Successfully written");
                                                                                            // break;

                                                                                        }

                                                                                    }
                                                                                    else
                                                                                    {
                                                                                        Log.Debug("Task COnfirmation Tag is incorrect in Table");
                                                                                    }


                                                                                    //if (readTag("ATS.WMS_STACKER_2.STACKER_2_TASK_COMPLETION").Equals("10"))
                                                                                    //{
                                                                                    //    writeTag("ATS.WMS_STACKER_2.STACKER_2_TASK_CONFERMATION", "1");
                                                                                    //}

                                                                                }
                                                                                else
                                                                                {
                                                                                    Log.Debug("PositionIsEmpty=1 or PositionIsAllocated=0");
                                                                                }
                                                                            }
                                                                            else
                                                                            {
                                                                                Log.Debug("Cannot data for Position ID");
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            Log.Debug("Pallet ID not present in Current Stock");
                                                                        }
                                                                    }
                                                                    else
                                                                    {
                                                                        Log.Debug("NO Completed Mission for Task type");
                                                                    }


                                                                    //}
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    Log.Error("Exception occurred while writting task confirmation:: " + ex.Message);

                                                                }
                                                            }
                                                            else
                                                            {
                                                                Log.Debug("COntrol MOde Tag is incorrect in Table");
                                                            }

                                                        }
                                                        else
                                                        {
                                                            Log.Debug("Pallet ID Not present in Master Pallet Table");
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Log.Error("AtsWmsS2TaskConfirmationDetailsOperation :: exception occurred while pallet information details details :: " + ex.Message);
                                                    }
                                                }
                                                else if (fbtaskType.Equals("2"))
                                                {
                                                    //OUTFEED MISSION
                                                    Log.Debug("AtsWmsS2TaskConfirmationDetailsOperation :: OUTFEED MISSION");
                                                    try
                                                    {
                                                        Log.Debug(" 1 :: AtsWmsS2TaskConfirmationDetailsOperation :: OUTFEED MISSION");
                                                        ats_wms_outfeed_mission_runtime_detailsDataTableDT = ats_wms_outfeed_mission_runtime_detailsTableAdapterInstance.GetDataByOUTFEED_MISSION_IDAndOUTFEED_MISSION_STATUS(fbTaskNoInt, "IN_PROGRESS");
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                        Log.Error("AtsWmsS2TaskConfirmationDetailsOperation :: exception occurred while outfeed mission details :: " + ex.Message); ;
                                                    }
                                                    if (ats_wms_outfeed_mission_runtime_detailsDataTableDT != null && ats_wms_outfeed_mission_runtime_detailsDataTableDT.Count > 0)
                                                    {
                                                        //Getting pallet data from current stock details table by sending pallet information id of ready outfeed mission
                                                        try
                                                        {
                                                            Log.Debug(" 1 :: AtsWmsS2TaskConfirmationDetailsOperation :: Pallet info ID :: " + ats_wms_outfeed_mission_runtime_detailsDataTableDT[0].PALLET_INFORMATION_ID);
                                                            ats_wms_current_stock_detailsDataTableDT = ats_wms_current_stock_detailsTableAdapterInstance.GetDataByPALLET_INFORMATION_ID1(ats_wms_outfeed_mission_runtime_detailsDataTableDT[0].PALLET_INFORMATION_ID);
                                                            Log.Debug("Confirmaing Task for Pallet Code :: " + ats_wms_current_stock_detailsDataTableDT[0].PALLET_CODE);
                                                        }
                                                        catch (Exception ex)
                                                        {

                                                            Log.Error("AtsWmsS2TaskConfirmationDetailsOperation::Exception occured while getting pallet information id" + ex.Message + ex.StackTrace);
                                                        }
                                                        //Checking if the data is available in DB
                                                        if (ats_wms_current_stock_detailsDataTableDT != null && ats_wms_current_stock_detailsDataTableDT.Count > 0)
                                                        {

                                                            try
                                                            {
                                                                Log.Debug(" 3 :: ");
                                                                String timeNow1 = DateTime.Now.TimeOfDay.ToString();
                                                                TimeSpan currentTimeNow1 = TimeSpan.Parse(timeNow);

                                                                String currentDate1 = "";
                                                                currentDate1 = Convert.ToString(DateTime.Now.ToString("yyyy-MM-dd"));


                                                                Log.Debug("Updating Mission status as :: COMPLETED");

                                                                ////Update the pallet data in stock table against the pallet position id 
                                                                ats_wms_current_stock_detailsTableAdapterInstance.UpdatePALLET_CODEAndPALLET_INFORMATION_IDAndPRODUCT_VARIENT_CODEAndPRODUCT_NAMEAndCORESHOPAndCORE_SIZEAndPRODUCT_IDAndPALLET_STATUS_IDAndPALLET_STATUS_NAMEAndQUANTITYAndBATCH_NUMBERAndLOAD_DATETIMEWherePOSITION_ID(
                                                                   "NA", 0, "NA", "NA", "NA", "NA", 0, 0, "NA", 0, "NA", (currentDate1 + " " + timeNow1), ats_wms_outfeed_mission_runtime_detailsDataTableDT[0].POSITION_ID);

                                                                //Updating pallet position details in DB
                                                                Log.Debug("Updating values in Master Position Details");
                                                                ats_wms_master_position_detailsTableAdapterInstance.UpdatePOSITION_IS_ALLOCATEDAndPOSITION_IS_EMPTYAndIS_MANUAL_DISPATCHAndIS_MATERIAL_LOADEDWherePOSITION_ID(0, 1, 0,0, ats_wms_outfeed_mission_runtime_detailsDataTableDT[0].POSITION_ID);
                                                                Log.Debug("Updating in master Position for POSITION_ID :: " + ats_wms_outfeed_mission_runtime_detailsDataTableDT[0].POSITION_ID);

                                                                //updating mission status to Completed in outfeed mission details table against outfeed mission id
                                                                Log.Debug("Updating Mission status as :: COMPLETED");
                                                                ats_wms_outfeed_mission_runtime_detailsTableAdapterInstance.UpdateOUTFEED_MISSION_STATUSAndOUTFEED_MISSION_END_DATETIMEWhereOUTFEED_MISSION_ID("COMPLETED", (currentDate1 + " " + currentTimeNow1), ats_wms_outfeed_mission_runtime_detailsDataTableDT[0].OUTFEED_MISSION_ID);


                                                                // get ccm id from equiment table and insert below
                                                                Log.Debug("Updating Mission status as :: waiting 500 ms");
                                                                Thread.Sleep(1000);
                                                                Log.Debug("Updating Mission status as :: waiting 500 ms done");


                                                                Log.Debug("checking Outfeed mission end date time is Not Null ::");

                                                                ats_wms_outfeed_mission_runtime_detailsDataTableDT = ats_wms_outfeed_mission_runtime_detailsTableAdapterInstance.GetDataByOUTFEED_MISSION_ID(fbTaskNoInt);
                                                                Log.Debug(" 1 :: checking Outfeed mission end date time is Not Null ::");

                                                                if (!ats_wms_outfeed_mission_runtime_detailsDataTableDT[0].IsOUTFEED_MISSION_END_DATETIMENull())
                                                                {

                                                                    Log.Debug(" 2 :: Outfeed mission end date time is Not Null ::");

                                                                    Log.Debug("Updating Mission Deatils in Buffer Table ::");
                                                                    ats_wms_ccm_buffer_detailsTableAdapterInstance.Insert(
                                                                ats_wms_outfeed_mission_runtime_detailsDataTableDT[0].OUTFEED_MISSION_ID,
                                                                ats_wms_outfeed_mission_runtime_detailsDataTableDT[0].PALLET_CODE,
                                                                ats_wms_outfeed_mission_runtime_detailsDataTableDT[0].PALLET_INFORMATION_ID,
                                                                ats_wms_outfeed_mission_runtime_detailsDataTableDT[0].DESTINATION,
                                                                0,
                                                                ats_wms_outfeed_mission_runtime_detailsDataTableDT[0].PRODUCT_ID,
                                                                ats_wms_outfeed_mission_runtime_detailsDataTableDT[0].PRODUCT_NAME,

                                                                ats_wms_outfeed_mission_runtime_detailsDataTableDT[0].SHIFT_ID,
                                                                ats_wms_outfeed_mission_runtime_detailsDataTableDT[0].PALLET_STATUS_ID,
                                                                ats_wms_outfeed_mission_runtime_detailsDataTableDT[0].OUTFEED_MISSION_START_DATETIME,
                                                                ats_wms_outfeed_mission_runtime_detailsDataTableDT[0].OUTFEED_MISSION_END_DATETIME,
                                                                ats_wms_outfeed_mission_runtime_detailsDataTableDT[0].OUTFEED_MISSION_STATUS,
                                                                ats_wms_outfeed_mission_runtime_detailsDataTableDT[0].ORDER_ID,
                                                                0,
                                                                 ats_wms_outfeed_mission_runtime_detailsDataTableDT[0].QUANTITY
                                                                );

                                                                    Log.Debug("Updated Mission Deatils in Buffer Table ::");

                                                                    Log.Debug("Updating :: is infeed and outfeed Mission generated to 0 :: " + ats_wms_current_stock_detailsDataTableDT[0].PALLET_INFORMATION_ID);
                                                                    ats_wms_master_pallet_informationTableAdapterInstance.UpdateIS_INFEED_MISSION_GENERATEDAndIS_OUTFEED_MISSION_GENERATEDWherePALLET_INFORMATION_ID(0, 0, ats_wms_current_stock_detailsDataTableDT[0].PALLET_INFORMATION_ID);
                                                                }
                                                                else
                                                                {
                                                                    Log.Debug("Outfeed Mission End date time is Null for ID :: " + fbtaskType);
                                                                }
                                                                try
                                                                {
                                                                    ats_wms_master_task_confirmation_tag_detailsDataTableTaskConfermationDT = ats_wms_master_task_confirmation_tag_detailsTableAdapterInstance.GetDataBySTACKER_IDAndTAG_TYPE(STACKER_2, TASK_CONFERMATION);
                                                                    if (ats_wms_master_task_confirmation_tag_detailsDataTableTaskConfermationDT != null && ats_wms_master_task_confirmation_tag_detailsDataTableTaskConfermationDT.Count > 0)
                                                                    {


                                                                        try
                                                                        {
                                                                            Log.Debug("AtsWmsS2TaskConfirmationDetailsOperation :: OUTFEED MISSION DETAILS : writing Task confirmation 1");
                                                                            writeTag(ats_wms_master_task_confirmation_tag_detailsDataTableTaskConfermationDT[0].TASK_CONFIRMATION_TAG_NAME, "1");
                                                                            // writeTag("ATS.WMS_STACKER_2.STACKER_2_TASK_CONFERMATION", "1");
                                                                            Log.Debug("AtsWmsS2TaskConfirmationDetailsOperation :: OUTFEED MISSION DETAILS : Task confirmation 1 Successfully written");
                                                                        }
                                                                        catch (Exception ex)
                                                                        {

                                                                            Log.Error("AtsWmsS2TaskConfirmationDetailsOperation :: exception occurred while writing Confirmation Tag :: " + ex.Message);
                                                                        }
                                                                        //for (;;)
                                                                        //{
                                                                        //    //Thread.Sleep(1000);
                                                                        //    //writeTag("ATS.WMS_STACKER_2.STACKER_2_TASK_CONFERMATION", "1");
                                                                        //    //Thread.Sleep(2000);

                                                                        //    //if (readTag("ATS.WMS_STACKER_2.STACKER_2_DATA_REQUEST").Equals("True") && readTag("ATS.WMS_STACKER_2.STACKER_2_TASK_COMPLETION").Equals("10"))
                                                                        //    //{
                                                                        //    //    Log.Debug("AtsWmsS2TaskConfirmationDetailsOperation :: OUTFEED MISSION DETAILS : data request True");
                                                                        //    //    Log.Debug("AtsWmsS2TaskConfirmationDetailsOperation :: OUTFEED MISSION DETAILS : writing Task confirmation 0");
                                                                        //    //    writeTag("ATS.WMS_STACKER_2.STACKER_2_TASK_CONFERMATION", "0");
                                                                        //    //    Log.Debug("AtsWmsS2TaskConfirmationDetailsOperation :: OUTFEED MISSION DETAILS : Task confirmation 0 Successfully written");
                                                                        //    //    break;
                                                                        //    //}
                                                                        //    //else if(readTag("ATS.WMS_STACKER_2.STACKER_2_DATA_REQUEST").Equals("False"))
                                                                        //    //{
                                                                        //    //    Log.Debug("Data Request is not 1");
                                                                        //    //}
                                                                        //    //else
                                                                        //    //{
                                                                        //    //    Log.Debug("Data request Tag is wrong");
                                                                        //    //}

                                                                        //    string fbTaskCompletion = "";

                                                                        //    try
                                                                        //    {
                                                                        //        fbTaskCompletion = readTag("ATS.WMS_STACKER_2.STACKER_2_TASK_COMPLETION");
                                                                        //        Log.Debug("fbTaskCompletion::" + fbTaskCompletion);

                                                                        //        Log.Debug("AtsWmsS2TaskConfirmationDetailsOperation :: fbTaskNo: " + fbTaskNo + ":: fbtaskType: " + fbtaskType + ":: fbTaskCompletion: " + fbTaskCompletion);

                                                                        //    }
                                                                        //    catch (Exception ex)
                                                                        //    {

                                                                        //        Log.Error("Exception occurred while reading Task Completion :: " + ex.StackTrace);
                                                                        //    }

                                                                        //    if (fbTaskCompletion.Equals("10"))
                                                                        //    {
                                                                        //        Thread.Sleep(1000);
                                                                        //        writeTag("ATS.WMS_STACKER_2.STACKER_2_TASK_CONFERMATION", "1");
                                                                        //        Thread.Sleep(2000);

                                                                        //    }
                                                                        //    else
                                                                        //    {
                                                                        //        Log.Debug("AtsWmsTaskConfirmationDetailsOperation :: INFEED MISSION DETAILS : task completion is not 10");
                                                                        //        Log.Debug("AtsWmsTaskConfirmationDetailsOperation :: INFEED MISSION DETAILS : writing Task confirmation 0");
                                                                        //        writeTag("ATS.WMS_STACKER_2.STACKER_2_TASK_CONFERMATION", "0");
                                                                        //        Log.Debug("AtsWmsTaskConfirmationDetailsOperation :: INFEED MISSION DETAILS : Task confirmation 0 Successfully written");
                                                                        //        break;

                                                                        //    }
                                                                        //}






                                                                        try
                                                                        {
                                                                            Log.Debug(" 1 :: AtsWmsS2TaskConfirmationDetailsOperation :: OUTFEED MISSION");
                                                                            ats_wms_outfeed_mission_runtime_detailsDataTableDT = ats_wms_outfeed_mission_runtime_detailsTableAdapterInstance.GetDataByOUTFEED_MISSION_IDAndOUTFEED_MISSION_STATUS(fbTaskNoInt, "COMPLETED");
                                                                        }
                                                                        catch (Exception ex)
                                                                        {

                                                                            Log.Error("AtsWmsS2TaskConfirmationDetailsOperation :: exception occurred while outfeed mission details :: " + ex.Message); ;
                                                                        }
                                                                        if (ats_wms_outfeed_mission_runtime_detailsDataTableDT != null && ats_wms_outfeed_mission_runtime_detailsDataTableDT.Count > 0)
                                                                        {
                                                                            try
                                                                            {
                                                                                Log.Debug(" 1 :: AtsWmsS2TaskConfirmationDetailsOperation :: Pallet info ID :: " + ats_wms_outfeed_mission_runtime_detailsDataTableDT[0].PALLET_INFORMATION_ID);
                                                                                ats_wms_current_stock_detailsDataTableDT = ats_wms_current_stock_detailsTableAdapterInstance.GetDataByPALLET_INFORMATION_ID1(ats_wms_outfeed_mission_runtime_detailsDataTableDT[0].PALLET_INFORMATION_ID);
                                                                                Log.Debug("Confirmaing Task for Pallet Code :: " + ats_wms_current_stock_detailsDataTableDT[0].PALLET_CODE);
                                                                            }
                                                                            catch (Exception ex)
                                                                            {

                                                                                Log.Error("AtsWmsS2TaskConfirmationDetailsOperation::Exception occured while getting pallet information id" + ex.Message + ex.StackTrace);
                                                                            }
                                                                            //Checking if the data is available in DB
                                                                            if (ats_wms_current_stock_detailsDataTableDT != null && ats_wms_current_stock_detailsDataTableDT.Count == 0)
                                                                            {
                                                                                string fbTaskCompletion = "";

                                                                                try
                                                                                {
                                                                                    //fbTaskCompletion = readTag("ATS.WMS_STACKER_2.STACKER_2_TASK_COMPLETION");

                                                                                    fbTaskCompletion = readTag(ats_wms_master_task_confirmation_tag_detailsDataTableTaskCompletionDT[0].TASK_CONFIRMATION_TAG_NAME);
                                                                                    Log.Debug("fbTaskCompletion::" + fbTaskCompletion);

                                                                                    Log.Debug("AtsWmsS2TaskConfirmationDetailsOperation :: fbTaskNo: " + fbTaskNo + ":: fbtaskType: " + fbtaskType + ":: fbTaskCompletion: " + fbTaskCompletion);

                                                                                }
                                                                                catch (Exception ex)
                                                                                {

                                                                                    Log.Error("Exception occurred while reading Task Completion :: " + ex.StackTrace);
                                                                                }

                                                                                if (fbTaskCompletion.Equals("10"))
                                                                                {
                                                                                    try
                                                                                    {
                                                                                        Thread.Sleep(1000);
                                                                                        //writeTag("ATS.WMS_STACKER_2.STACKER_2_TASK_CONFERMATION", "1");

                                                                                        writeTag(ats_wms_master_task_confirmation_tag_detailsDataTableTaskConfermationDT[0].TASK_CONFIRMATION_TAG_NAME, "1");
                                                                                        Log.Debug("AtsWmsTaskConfirmationDetailsOperation :: INFEED MISSION DETAILS : Task confirmation 1 Successfully written");
                                                                                        Thread.Sleep(1000);
                                                                                    }
                                                                                    catch (Exception ex)
                                                                                    {
                                                                                        Log.Error("Exception occurred while writing Task Confirmation :: " + ex.StackTrace);
                                                                                    }

                                                                                }
                                                                                else
                                                                                {
                                                                                    try
                                                                                    {
                                                                                        Log.Debug("AtsWmsTaskConfirmationDetailsOperation :: INFEED MISSION DETAILS : task completion is not 10");
                                                                                        Log.Debug("AtsWmsTaskConfirmationDetailsOperation :: INFEED MISSION DETAILS : writing Task confirmation 0");
                                                                                        writeTag(ats_wms_master_task_confirmation_tag_detailsDataTableTaskConfermationDT[0].TASK_CONFIRMATION_TAG_NAME, "0");
                                                                                        //     writeTag("ATS.WMS_STACKER_2.STACKER_2_TASK_CONFERMATION", "0");
                                                                                        Log.Debug("AtsWmsTaskConfirmationDetailsOperation :: INFEED MISSION DETAILS : Task confirmation 0 Successfully written");
                                                                                        //break;
                                                                                    }
                                                                                    catch (Exception ex)
                                                                                    {
                                                                                        Log.Error("Exception occurred while writing Task Confirmation :: " + ex.StackTrace);
                                                                                    }

                                                                                }


                                                                                //if (readTag("ATS.WMS_STACKER_2.STACKER_2_TASK_COMPLETION").Equals("10"))
                                                                                //{
                                                                                //    try
                                                                                //    {
                                                                                //        Log.Debug("AtsWmsS2TaskConfirmationDetailsOperation :: OUTFEED MISSION DETAILS : writing Task confirmation 1");
                                                                                //        writeTag("ATS.WMS_STACKER_2.STACKER_2_TASK_CONFERMATION", "1");
                                                                                //        Log.Debug("AtsWmsS2TaskConfirmationDetailsOperation :: OUTFEED MISSION DETAILS : Task confirmation 1 Successfully written");
                                                                                //    }
                                                                                //    catch (Exception ex)
                                                                                //    {
                                                                                //        Log.Error("Exception occurred while writting task confirmation:: " + ex.Message);
                                                                                //    }
                                                                                //}

                                                                            }
                                                                            else
                                                                            {
                                                                                Log.Debug("Pallet ID is present in Current Stock");
                                                                            }
                                                                        }
                                                                        else
                                                                        {
                                                                            Log.Debug("Mission Not Completed in Outfeed Runtime for task no " + fbTaskNo);
                                                                        }
                                                                    }
                                                                }
                                                                catch (Exception ex)
                                                                {
                                                                    Log.Error("Exception occurred while writting task confirmation:: " + ex.Message);
                                                                }

                                                            }
                                                            catch (Exception ex)
                                                            {
                                                                Log.Error("GiveOutfeedMissionToStackerDetails :: giveMissionToStacker :: Exception occured while updating pallet information in DB :: " + ex.Message + " stackTrace :: " + ex.StackTrace);
                                                            }

                                                        }
                                                        else
                                                        {
                                                            Log.Debug("Pallet ID not retrievable from Current Stock");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        Log.Debug("NO INProgress Mission in Outfeed Runtime Table");
                                                    }
                                                }

                                                else if (fbtaskType.Equals("3"))
                                                {
                                                    Log.Debug("AtsWmsS2TaskConfirmationDetailsOperation :: Transfer pallet MISSION");
                                                    try
                                                    {
                                                        //Fetching transfer mission data by sending pallet information id and mission status as READY
                                                        ats_wms_transfer_pallet_mission_runtime_detailsDataTableDT = ats_wms_transfer_pallet_mission_runtime_detailsTableAdapterInstance.GetDataByTRANSFER_PALLET_MISSION_RUNTIME_DETAILS_IDAndTRANSFER_MISSION_STATUS(fbTaskNoInt, "IN_PROGRESS");


                                                        if (ats_wms_transfer_pallet_mission_runtime_detailsDataTableDT != null && ats_wms_transfer_pallet_mission_runtime_detailsDataTableDT.Count > 0)
                                                        {
                                                            ats_wms_master_pallet_informationDataTableDT = ats_wms_master_pallet_informationTableAdapterInstance.GetDataByPALLET_INFORMATION_ID(ats_wms_transfer_pallet_mission_runtime_detailsDataTableDT[0].PALLET_INFORMTION_ID);
                                                            if (ats_wms_master_pallet_informationDataTableDT != null && ats_wms_master_pallet_informationDataTableDT.Count > 0)
                                                            {
                                                                ats_wms_current_stock_detailsDataTableDT = ats_wms_current_stock_detailsTableAdapterInstance.GetDataByPALLET_INFORMATION_ID(ats_wms_transfer_pallet_mission_runtime_detailsDataTableDT[0].PALLET_INFORMTION_ID);
                                                                if (ats_wms_current_stock_detailsDataTableDT != null && ats_wms_current_stock_detailsDataTableDT.Count > 0)
                                                                {
                                                                    try
                                                                    {
                                                                        //updating transfered position details in current pallet stock details table
                                                                        Log.Debug("GiveStackerMission :: Updating Position name against pallet information ID in Curent Pallet Stock details table");
                                                                        ats_wms_current_stock_detailsTableAdapterInstance.UpdatePALLET_CODEAndPALLET_INFORMATION_IDAndPRODUCT_VARIENT_CODEAndPRODUCT_NAMEAndCORESHOPAndCORE_SIZEAndPRODUCT_IDAndPALLET_STATUS_IDAndPALLET_STATUS_NAMEAndQUANTITYAndBATCH_NUMBERAndLOAD_DATETIMEWherePOSITION_ID(
                                                                            ats_wms_transfer_pallet_mission_runtime_detailsDataTableDT[0].PALLET_CODE,
                                                                            ats_wms_transfer_pallet_mission_runtime_detailsDataTableDT[0].PALLET_INFORMTION_ID,
                                                                            // Convert.ToInt32(ats_wms_master_pallet_informationDataTableDT[0].SERIAL_NUMBER),
                                                                            //ats_wms_transfer_pallet_mission_runtime_detailsDataTableDT[0].PRODUCT_VARIANT_ID,
                                                                            ats_wms_transfer_pallet_mission_runtime_detailsDataTableDT[0].PRODUCT_VARIANT_CODE,
                                                                            ats_wms_transfer_pallet_mission_runtime_detailsDataTableDT[0].PRODUCT_NAME,
                                                                            ats_wms_current_stock_detailsDataTableDT[0].CORESHOP,
                                                                            ats_wms_transfer_pallet_mission_runtime_detailsDataTableDT[0].PRODUCT_NAME,
                                                                            ats_wms_master_pallet_informationDataTableDT[0].PRODUCT_ID,
                                                                            ats_wms_master_pallet_informationDataTableDT[0].PALLET_STATUS_ID,
                                                                            ats_wms_current_stock_detailsDataTableDT[0].PALLET_STATUS_NAME,
                                                                            ats_wms_current_stock_detailsDataTableDT[0].QUANTITY,
                                                                           //Convert.ToInt32(ats_wms_master_pallet_informationDataTableDT[0].QUANTITY),
                                                                           //  ats_wms_master_pallet_informationDataTableDT[0].QUALITY_STATUS,
                                                                           ats_wms_master_pallet_informationDataTableDT[0].BATCH_NUMBER,
                                                                           // ats_wms_master_pallet_informationDataTableDT[0].MODEL_NUMBER,
                                                                           //ats_wms_master_pallet_informationDataTableDT[0].LOCATION,
                                                                           DateTime.Now.ToString(),
                                                                           ats_wms_transfer_pallet_mission_runtime_detailsDataTableDT[0].TRANSFER_POSITION_ID
                                                                            );

                                                                        Thread.Sleep(500);
                                                                        ats_wms_current_stock_detailsTableAdapterInstance.UpdatePalletDetailsWherePOSITION_ID("NA", 0, 0, "NA", "NA", "NA", 0, "NA", "NA", "NA", 0,
                                                                             DateTime.Now.ToString(), ats_wms_transfer_pallet_mission_runtime_detailsDataTableDT[0].PREVIOUS_POSITION_ID);

                                                                        Thread.Sleep(500);
                                                                        //Updating pallet position details in DB
                                                                        Log.Debug("Updating values in Master Position Details at previous position");
                                                                        ats_wms_master_position_detailsTableAdapterInstance.UpdatePOSITION_IS_ALLOCATEDAndPOSITION_IS_EMPTYWherePOSITION_ID(0, 1, ats_wms_transfer_pallet_mission_runtime_detailsDataTableDT[0].PREVIOUS_POSITION_ID);


                                                                        Log.Debug("Updating values in Master Position Details at transfered position");
                                                                        if (ats_wms_master_pallet_informationDataTableDT[0].PALLET_STATUS_ID == 3)
                                                                        {
                                                                            ats_wms_master_position_detailsTableAdapterInstance.UpdatePOSITION_IS_ALLOCATEDAndPOSITION_IS_EMPTYAndIS_MATERIAL_LOADEDWherePOSITION_ID(1, 0, 0, ats_wms_transfer_pallet_mission_runtime_detailsDataTableDT[0].TRANSFER_POSITION_ID);
                                                                        }
                                                                        else if (ats_wms_master_pallet_informationDataTableDT[0].PALLET_STATUS_ID == 1 || ats_wms_master_pallet_informationDataTableDT[0].PALLET_STATUS_ID == 7)
                                                                        {
                                                                            ats_wms_master_position_detailsTableAdapterInstance.UpdatePOSITION_IS_ALLOCATEDAndPOSITION_IS_EMPTYAndIS_MATERIAL_LOADEDWherePOSITION_ID(1, 0, 1, ats_wms_transfer_pallet_mission_runtime_detailsDataTableDT[0].TRANSFER_POSITION_ID);
                                                                        }
                                                                        else if (ats_wms_master_pallet_informationDataTableDT[0].PALLET_STATUS_ID == 6)
                                                                        {
                                                                            ats_wms_master_position_detailsTableAdapterInstance.UpdatePOSITION_IS_ALLOCATEDAndPOSITION_IS_EMPTYWherePOSITION_ID(1, 0, ats_wms_transfer_pallet_mission_runtime_detailsDataTableDT[0].TRANSFER_POSITION_ID);

                                                                        }




                                                                        //updating mission status to Completed in transfer mission details table against tranfer mission id
                                                                        Log.Debug("Updating Mission status as :: COMPLETED");
                                                                        try
                                                                        {
                                                                            ats_wms_transfer_pallet_mission_runtime_detailsTableAdapterInstance.UpdateTRANSFER_MISSION_STATUSAndTRANSFER_MISSION_END_DATETIMEWhereTRANSFER_PALLET_MISSION_RUNTIME_DETAILS_ID("COMPLETED", DateTime.Now.ToString(), ats_wms_transfer_pallet_mission_runtime_detailsDataTableDT[0].TRANSFER_PALLET_MISSION_RUNTIME_DETAILS_ID);
                                                                        }
                                                                        catch (Exception ex)
                                                                        {

                                                                            Log.Error("error occured while updating mission status::" + ex.Message + ex.StackTrace);
                                                                        }

                                                                        //updatting is transfer mission generated to 0 as pallet is transfered
                                                                        Log.Debug("updating is transfer mission generated to 0 as pallet is transfered");
                                                                        ats_wms_master_pallet_informationTableAdapterInstance.UpdateIS_TRANSFER_MISSION_GENERATEDWherePALLET_INFORMATION_ID(0, ats_wms_transfer_pallet_mission_runtime_detailsDataTableDT[0].PALLET_INFORMTION_ID);

                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        Log.Error("GiveStackerMission :: Exception occured while updating pallet information in DB :: " + ex.Message + " stackTrace :: " + ex.StackTrace);
                                                                    }


                                                                    try
                                                                    {
                                                                        ats_wms_master_task_confirmation_tag_detailsDataTableTaskConfermationDT = ats_wms_master_task_confirmation_tag_detailsTableAdapterInstance.GetDataBySTACKER_IDAndTAG_TYPE(STACKER_2, TASK_CONFERMATION);
                                                                        Log.Debug("AtsWmsS2TaskConfirmationDetailsOperation :: TRANSFER MISSION DETAILS : writing Task confirmation 1");

                                                                        if (ats_wms_master_task_confirmation_tag_detailsDataTableTaskConfermationDT != null && ats_wms_master_task_confirmation_tag_detailsDataTableTaskConfermationDT.Count > 0)
                                                                        {



                                                                            try
                                                                            {
                                                                                writeTag(ats_wms_master_task_confirmation_tag_detailsDataTableTaskConfermationDT[0].TASK_CONFIRMATION_TAG_NAME, "1");
                                                                                //writeTag("ATS.WMS_STACKER_2.STACKER_2_TASK_CONFERMATION", "1");
                                                                                Log.Debug("AtsWmsS2TaskConfirmationDetailsOperation :: TRANSFER MISSION DETAILS : Task confirmation 1 Successfully written");
                                                                            }
                                                                            catch (Exception ex)
                                                                            {

                                                                                Log.Error("GiveStackerMission :: Exception occured while writing Task Confirmation Tag :: " + ex.Message + " stackTrace :: " + ex.StackTrace);

                                                                            }

                                                                            ats_wms_transfer_pallet_mission_runtime_detailsDataTableDT = ats_wms_transfer_pallet_mission_runtime_detailsTableAdapterInstance.GetDataByTRANSFER_PALLET_MISSION_RUNTIME_DETAILS_IDAndTRANSFER_MISSION_STATUS(fbTaskNoInt, "COMPLETE");

                                                                            if (ats_wms_transfer_pallet_mission_runtime_detailsDataTableDT != null && ats_wms_transfer_pallet_mission_runtime_detailsDataTableDT.Count == 0)
                                                                            {

                                                                                string fbTaskCompletion = "";

                                                                                try
                                                                                {
                                                                                    Thread.Sleep(1000);
                                                                                    //fbTaskCompletion = readTag("ATS.WMS_STACKER_2.STACKER_2_TASK_COMPLETION");
                                                                                    fbTaskCompletion = readTag(ats_wms_master_task_confirmation_tag_detailsDataTableTaskCompletionDT[0].TASK_CONFIRMATION_TAG_NAME);
                                                                                    Log.Debug("fbTaskCompletion::" + fbTaskCompletion);

                                                                                    Log.Debug("AtsWmsS2TaskConfirmationDetailsOperation :: fbTaskNo: " + fbTaskNo + ":: fbtaskType: " + fbtaskType + ":: fbTaskCompletion: " + fbTaskCompletion);

                                                                                }
                                                                                catch (Exception ex)
                                                                                {

                                                                                    Log.Error("Exception occurred while reading Task Completion :: " + ex.StackTrace);
                                                                                }

                                                                                if (fbTaskCompletion.Equals("10"))
                                                                                {
                                                                                    try
                                                                                    {
                                                                                        Thread.Sleep(1000);
                                                                                        writeTag(ats_wms_master_task_confirmation_tag_detailsDataTableTaskConfermationDT[0].TASK_CONFIRMATION_TAG_NAME, "1");
                                                                                        //writeTag("ATS.WMS_STACKER_2.STACKER_2_TASK_CONFERMATION", "1");
                                                                                        Log.Debug("AtsWmsTaskConfirmationDetailsOperation :: INFEED MISSION DETAILS : Task confirmation 1 Successfully written");
                                                                                        Thread.Sleep(1000);
                                                                                    }
                                                                                    catch (Exception ex)
                                                                                    {
                                                                                        Log.Error("Exception occurred while writing Task Confirmation :: " + ex.StackTrace);
                                                                                    }

                                                                                }
                                                                                else
                                                                                {
                                                                                    try
                                                                                    {
                                                                                        Log.Debug("AtsWmsTaskConfirmationDetailsOperation :: INFEED MISSION DETAILS : task completion is not 10");
                                                                                        Log.Debug("AtsWmsTaskConfirmationDetailsOperation :: INFEED MISSION DETAILS : writing Task confirmation 0");
                                                                                        writeTag(ats_wms_master_task_confirmation_tag_detailsDataTableTaskConfermationDT[0].TASK_CONFIRMATION_TAG_NAME, "0");
                                                                                        //writeTag("ATS.WMS_STACKER_2.STACKER_2_TASK_CONFERMATION", "0");
                                                                                        Log.Debug("AtsWmsTaskConfirmationDetailsOperation :: INFEED MISSION DETAILS : Task confirmation 0 Successfully written");
                                                                                        //break;
                                                                                    }
                                                                                    catch (Exception ex)
                                                                                    {
                                                                                        Log.Error("Exception occurred while writing Task Confirmation :: " + ex.StackTrace);
                                                                                    }

                                                                                }

                                                                            }
                                                                            else
                                                                            {
                                                                                Log.Debug(" MIssion Complete against Task No.");
                                                                            }
                                                                            //for (;;)
                                                                            //{
                                                                            //    Thread.Sleep(1000);
                                                                            //    writeTag("ATS.WMS_STACKER_2.STACKER_2_TASK_CONFERMATION", "1");
                                                                            //    Thread.Sleep(2000);

                                                                            //    if (readTag("ATS.WMS_STACKER_2.STACKER_2_DATA_REQUEST").Equals("True"))
                                                                            //    {
                                                                            //        Log.Debug("AtsWmsTaskConfirmationDetailsOperation :: TRANSFER MISSION DETAILS : data request True");
                                                                            //        Log.Debug("AtsWmsTaskConfirmationDetailsOperation :: TRANSFER MISSION DETAILS : writing Task confirmation 0");
                                                                            //        writeTag("ATS.WMS_STACKER_2.STACKER_2_TASK_CONFERMATION", "0");
                                                                            //        Log.Debug("AtsWmsS2TaskConfirmationDetailsOperation :: TRANSFER MISSION DETAILS : Task confirmation 0 Successfully written");
                                                                            //        break;
                                                                            //    }
                                                                            //}
                                                                        }
                                                                        else
                                                                        {
                                                                            Log.Debug("Task Confirmation Tag is incorrect in the Table");
                                                                        }
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        Log.Error("Exception occurred while writting task confirmation:: " + ex.Message);

                                                                    }
                                                                }
                                                                else
                                                                {
                                                                    Log.Debug("Pallet ID not retrievable from Current stock Table");
                                                                }
                                                            }
                                                            else
                                                            {
                                                                Log.Debug("Pallet ID not retrievable from Master Pallet Table");
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Log.Debug("NO InProgress Transfer Mission ");
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        Log.Error("giveMissionToStacker1Operation :: Exception occured while getting TRANSFER PALLET MISSION DETAILS details ::" + ex.Message + " StackTrace:: " + ex.StackTrace);
                                                    }


                                                }
                                                else
                                                {
                                                    // NO more Task Types
                                                }
                                            }
                                            catch (Exception ex)
                                            {
                                                Log.Error("AtsWmsS2TaskConfirmationDetailsOperation :: exception occurred while converting to int :: " + ex.Message);
                                            }

                                        }
                                        else
                                        {
                                            Log.Debug("Task Type Tag is incorrect in Table");
                                        }

                                    }
                                    else
                                    {
                                        Log.Debug("Task NO Tag is incorrect in Table");
                                    }

                                }
                                else
                                {
                                    ats_wms_master_task_confirmation_tag_detailsDataTableTaskConfermationDT = ats_wms_master_task_confirmation_tag_detailsTableAdapterInstance.GetDataBySTACKER_IDAndTAG_TYPE(STACKER_2, TASK_CONFERMATION);
                                    Log.Debug(" In else task completion where task completion is not 10");
                                    if (ats_wms_master_task_confirmation_tag_detailsDataTableTaskConfermationDT != null && ats_wms_master_task_confirmation_tag_detailsDataTableTaskConfermationDT.Count > 0)
                                    {

                                        //string taskconformation = readTag("ATS.WMS_STACKER_2.STACKER_2_TASK_CONFERMATION");
                                        string taskconformation = "";
                                        try
                                        {
                                            taskconformation = readTag(ats_wms_master_task_confirmation_tag_detailsDataTableTaskConfermationDT[0].TASK_CONFIRMATION_TAG_NAME);
                                            Log.Debug("taskconformation :: " + taskconformation);
                                        }
                                        catch (Exception ex)
                                        {

                                            Log.Error("Exception Occured while reading Task Confirmation tag");
                                        }
                                        if (taskconformation.Equals("1") || taskconformation.Equals("True"))
                                        {
                                            Log.Debug(" In if task conformation ");
                                            Log.Debug("AtsWmsTaskConfirmationDetailsOperation :: : task completion is not 10");
                                            Log.Debug("AtsWmsTaskConfirmationDetailsOperation :: : writing Task confirmation 0");

                                            writeTag(ats_wms_master_task_confirmation_tag_detailsDataTableTaskConfermationDT[0].TASK_CONFIRMATION_TAG_NAME, "0");
                                            //writeTag("ATS.WMS_STACKER_2.STACKER_2_TASK_CONFERMATION", "0");

                                            Log.Debug("AtsWmsTaskConfirmationDetailsOperation ::  : Task confirmation 0 Successfully written");
                                        }
                                        else
                                        {
                                            Log.Debug(" In else task conformation ");
                                        }
                                    }
                                    else
                                    {
                                        Log.Debug("Task Confirmation Tag incorrect in Table");
                                    }

                                }
                            }
                            else
                            {
                                Log.Debug("Completion Tag incorrect in Table");
                            }
                        }
                        else
                        {
                            //Reconnect to plc, Check Ip address, url
                        }

                    }
                    else
                    {
                        Log.Debug("PLC IP incorrect in Table");
                    }
                }
                else
                {
                    Log.Error("ping status false :");
                }
            }
            catch (Exception ex)
            {

                Log.Error("startOperation :: Exception occured while stopping timer :: " + ex.Message + " stackTrace :: " + ex.StackTrace);
            }
            finally
            {
                try
                {
                    AtsWmsS2TaskConfirmationDetailsTimer.Start();
                }
                catch (Exception ex1)
                {
                    Log.Error("startOperation :: Exception occured while stopping timer :: " + ex1.Message + " stackTrace :: " + ex1.StackTrace);
                }


            }
        }
        #region Ping funcationality

        public Boolean checkPlcPingRequest()
        {
            Log.Debug("IprodPLCMachineXmlGenOperation :: Inside checkServerPingRequest");

            try
            {
                try
                {
                    pingSenderForThisConnection = new Ping();
                    replyForThisConnection = pingSenderForThisConnection.Send(IP_ADDRESS);
                }
                catch (Exception ex)
                {
                    Log.Error("checkPlcPingRequest :: for IP :: " + IP_ADDRESS + " Exception occured while sending ping request :: " + ex.Message + " stackTrace :: " + ex.StackTrace);
                    replyForThisConnection = null;
                }

                if (replyForThisConnection != null && replyForThisConnection.Status == IPStatus.Success)
                {
                    Log.Debug("checkPlcPingRequest :: for IP :: " + IP_ADDRESS + " Ping success :: " + replyForThisConnection.Status.ToString());
                    return true;
                }
                else
                {
                    Log.Debug("checkPlcPingRequest :: for IP :: " + IP_ADDRESS + " Ping failed. ");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error("checkPlcPingRequest :: for IP :: " + IP_ADDRESS + " Exception while checking ping request :: " + ex.Message + " stackTrace :: " + ex.StackTrace);
                return false;
            }
        }

        #endregion

        #region Read and Write PLC tag

        [HandleProcessCorruptedStateExceptions]
        public string readTag(string tagName)
        {

            try
            {
                //Log.Debug("IprodPLCCommunicationOperation :: Inside readTag.");

                // Set PLC tag
                OPCItemIDs.SetValue(tagName, 1);
                //Log.Debug("readTag :: Plc tag is configured for plc group.");

                // remove all group
                ConnectedOpc.OPCGroups.RemoveAll();
                OpcGroupNamesA1T = ConnectedOpc.OPCGroups.Add("AtsWmsS2TaskConfirmationDetailsGroup");
                OpcGroupNamesA1T.DeadBand = 0;
                OpcGroupNamesA1T.UpdateRate = 500;
                OpcGroupNamesA1T.IsSubscribed = true;
                OpcGroupNamesA1T.IsActive = true;
                OpcGroupNamesA1T.OPCItems.AddItems(1, ref OPCItemIDs, ref ClientHandles, out ItemServerHandles, out ItemServerErrors, RequestedDataTypes, AccessPaths);
                OpcGroupNamesA1T.SyncRead((short)OPCAutomation.OPCDataSource.OPCDevice, 1, ref
                   ItemServerHandles, out ItemServerValues, out ItemServerErrors, out aTC, out bTC);

                //Log.Debug("readTag ::  tag name :: " + tagName + " tag value :: " + Convert.ToString(ItemServerValues.GetValue(1)));

                if (Convert.ToString(ItemServerValues.GetValue(1)).Equals("True"))
                {
                    //Log.Debug("readTag :: Found and Return True");
                    //ConnectedOpc.OPCGroups.Remove("AtsWmsStationWorkDoneDetailsGroup");
                    return "True";
                }
                else if (Convert.ToString(ItemServerValues.GetValue(1)).Equals("False"))
                {
                    //Log.Debug("readTag :: Found and Return False");
                    //ConnectedOpc.OPCGroups.Remove("AtsWmsStationWorkDoneDetailsGroup");
                    return "False";
                }
                else
                {
                    //ConnectedOpc.OPCGroups.Remove("AtsWmsStationWorkDoneDetailsGroup");
                    return Convert.ToString(ItemServerValues.GetValue(1));
                }

            }
            catch (Exception ex)
            {
                Log.Error("readTag :: Exception while reading plc tag :: " + tagName + " :: " + ex.Message);
            }

            Log.Debug("readTag :: Return False.. retun null.");

            return "False";
        }

        [HandleProcessCorruptedStateExceptions]
        public Boolean writeTag(string tagName, string tagValue)
        {

            try
            {
                Log.Debug("IprodGiveMissionToStacker :: Inside writeTag.");

                // Set PLC tag
                OPCItemIDs.SetValue(tagName, 1);
                //Log.Debug("writeTag :: Plc tag is configured for plc group.");

                // remove all group
                ConnectedOpc.OPCGroups.RemoveAll();
                OpcGroupNamesA1T = ConnectedOpc.OPCGroups.Add("AtsWmsS2TaskConfirmationDetailsGroup");
                OpcGroupNamesA1T.DeadBand = 0;
                OpcGroupNamesA1T.UpdateRate = 500;
                OpcGroupNamesA1T.IsSubscribed = true;
                OpcGroupNamesA1T.IsActive = true;
                OpcGroupNamesA1T.OPCItems.AddItems(1, ref OPCItemIDs, ref ClientHandles, out ItemServerHandles, out ItemServerErrors, RequestedDataTypes, AccessPaths);
                //Log.Debug("writeTag :: Kepware properties configuration is complete.");

                // read plc tags
                OpcGroupNamesA1T.SyncRead((short)OPCAutomation.OPCDataSource.OPCDevice, 1, ref
                   ItemServerHandles, out ItemServerValues, out ItemServerErrors, out aTC, out bTC);

                // Add tag value
                ItemServerValues.SetValue(tagValue, 1);

                // Write tag
                OpcGroupNamesA1T.SyncWrite(1, ref ItemServerHandles, ref ItemServerValues, out ItemServerErrors);
                //ConnectedOpc.OPCGroups.Remove("AtsWmsStationWorkDoneDetailsGroup");
                return true;

            }
            catch (Exception ex)
            {
                Log.Error("writeTag :: Exception while writing mission data in the plc tag :: " + tagName + " :: " + ex.Message + " stackTrace :: " + ex.StackTrace);

                OnConnectPLC();
                Thread.Sleep(1000);

                Log.Debug("writing again :: tagName" + tagName + " tagValue :: " + tagValue);
                writeTag(tagName, tagValue);
            }

            return false;

        }

        #endregion

        #region Connect and Disconnect PLC

        private void OnConnectPLC()
        {

            Log.Debug("OnConnectPLC :: inside OnConnectPLC");

            try
            {
                // Connection url
                if (!((ConnectedOpc.ServerState.ToString()).Equals("1")))
                {
                    ConnectedOpc.Connect(plcServerConnectionString, "");
                    Log.Debug("OnConnectPLC :: PLC connection successful and OPC server state is :: " + ConnectedOpc.ServerState.ToString());
                }
                else
                {
                    Log.Debug("OnConnectPLC :: Already connected with the plc.");
                }

            }
            catch (Exception ex)
            {
                Log.Error("OnConnectPLC :: Exception while connecting to plc :: " + ex.Message + " stackTrace :: " + ex.StackTrace);
            }
        }

        private void OnDisconnectPLC()
        {
            Log.Debug("inside OnDisconnectPLC");

            try
            {
                ConnectedOpc.Disconnect();
                Log.Debug("OnDisconnectPLC :: Connection with the plc is disconnected.");
            }
            catch (Exception ex)
            {
                Log.Error("OnDisconnectPLC :: Exception while disconnecting to plc :: " + ex.Message);
            }

        }

        #endregion
    }
}


