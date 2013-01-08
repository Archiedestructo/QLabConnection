using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.ComponentModel;
using System.Diagnostics;

using System.Net;
using Bespoke.Common;
using Bespoke.Common.Osc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace QLabConnection
{
    #region Delegates
    public delegate void WorkspaceUpdatedHandler(string WorkspaceID);
    public delegate void CueUpdatedHandler(string WorkspaceID, string CueID);
    public delegate bool OSCResponseHandler(string Address, string Response);
    #endregion

    #region OSC Message
    public class OSCMessage
    {
        private static IPAddress serverIPAddress = null;
        /// <summary>
        /// The Server's IP Address in the form of a string (i.e. "10.3.10.168)
        /// </summary>
        public static string ServerIPAddress
        {
            get
            {
                return serverIPAddress.ToString();
            }
            set
            {
                serverIPAddress = IPAddress.Parse(value);
            }
        }
        private static string serverName = null;
        /// <summary>
        /// The Server's Host Name. This will also resolve the IP Address
        /// </summary>
        public static string ServerName
        {
            get
            {
                return serverName;
            }
            set
            {
                serverName = value;
                IPAddress[] addresslist = Dns.GetHostAddresses(value);
                if (addresslist.Count() > 0)
                    serverIPAddress = addresslist[0];
            }
        }

        #region Bespoke Wrapper
        static IPEndPoint sourceEndPoint = null;
        public static void SendOSCMessage(string Address)
        {
            sourceEndPoint = new IPEndPoint(serverIPAddress, 53000);
            OscBundle bundle = new OscBundle(sourceEndPoint);
            bundle.Append(OSCMessage.CreateMessage(Address));
            bundle.Send(sourceEndPoint);
        }
        public static void SendOSCMessages(string[] Addresses)
        {
            sourceEndPoint = new IPEndPoint(serverIPAddress, 53000);
            OscBundle bundle = new OscBundle(sourceEndPoint);
            foreach (string Address in Addresses)
                bundle.Append(OSCMessage.CreateMessage(Address));
            bundle.Send(sourceEndPoint);
        }
        public static void SendOSCMessages(List<string> Addresses)
        {
            sourceEndPoint = new IPEndPoint(serverIPAddress, 53000);
            OscBundle bundle = new OscBundle(sourceEndPoint);
            foreach (string Address in Addresses)
                bundle.Append(OSCMessage.CreateMessage(Address));
            bundle.Send(sourceEndPoint);
        }

        /// <summary>
        /// Prepare an OSC Message to be send to QLab. This will doctor any type formats etc
        /// </summary>
        /// <param name="Address">The OSC Address formated in line with the QLab specifications</param>
        /// <returns>A Bespoke.OscMessage object</returns>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static OscMessage CreateMessage(string Address)
        {
            Address = Address.Trim();
            string Value = null;
            if (Address.IndexOf(" ") > 0)
            {
                Value = Address.Substring(Address.IndexOf(" ") + 1);
                Address = Address.Substring(0, Address.IndexOf(" "));
            }

            OscMessage msg = new OscMessage(sourceEndPoint, Address);
            if (Value != null)
            {
                if (Address.EndsWith("updates") ||
                    Address.EndsWith("loadAt") ||
                    Address.EndsWith("preWait") ||
                    Address.EndsWith("duration") ||
                    Address.EndsWith("postWait") ||
                    Address.EndsWith("continueMode") ||
                    Address.EndsWith("flagged") ||
                    Address.EndsWith("armed") ||
                    Address.EndsWith("patch") ||
                    Address.EndsWith("startTime") ||
                    Address.EndsWith("endTime") ||
                    Address.EndsWith("playCount") ||
                    Address.EndsWith("infiniteLoop") ||
                    Address.EndsWith("rate") ||
                    Address.EndsWith("doFade") ||
                    Address.EndsWith("surfaceID") ||
                    Address.EndsWith("fullScreen") ||
                    Address.EndsWith("preserveAspectRatio") ||
                    Address.EndsWith("translationX") ||
                    Address.EndsWith("translationY") ||
                    Address.EndsWith("scaleX") ||
                    Address.EndsWith("scaleY") ||
                    Address.EndsWith("doEffect") ||
                    Address.EndsWith("effect"))
                {
                    msg.Append<float>((float)Convert.ToDouble(Value));
                }
                else if (Address.EndsWith("sliderLevel"))
                {
                    if (Value.Contains(" "))
                    {
                        msg.Append<float>((float)Convert.ToDouble(Value.Substring(0, Value.IndexOf(" "))));
                        Value = Value.Substring(Value.IndexOf(" ") + 1);
                    }
                    msg.Append<float>((float)Convert.ToDouble(Value));
                }
                else
                {
                    msg.Append<string>(Value);
                }
            }
            return msg;
        }
        #endregion
    }
    #endregion

    #region Error Handlers
    public class QLabOSCTimeout : Exception
    {
        public QLabOSCTimeout()
            : base("QLab did not return an OSC message in the allotted amount of time")
        {
        }
    }
    public class QLabOSCIPAddress : Exception
    {
        public QLabOSCIPAddress()
            : base("QLabServer / OSCServer has not been given an IPAddress or Server Name")
        {
        }
    }
    #endregion

    #region QLab Connection
    public class QLabServer
    {
        #region Variables
        OscServer server;

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static object __ReturnValueHolder = null;

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static string __ReturnAddressHolder = null;

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static string __ReplyAddressHolder = null;

        static int _OSCTimeout = 1500;
        /// <summary>
        /// The amount of time to wait for a QLab response
        /// <para>If this time elapses, an error will be thrown</para>
        /// </summary>
        public static int OSCTimeout
        {
            get { return _OSCTimeout; }
            set { _OSCTimeout = value; }
        }

        /// <summary>
        /// The Server's IP Address in the form of a string (i.e. "10.3.10.168)
        /// </summary>
        /// <remarks>This variable is directly connected to the static OSCMessage.ServerIPAddress</remarks>
        public string ServerIPAddress
        {
            get
            {
                return OSCMessage.ServerIPAddress;
            }
            set
            {
                OSCMessage.ServerIPAddress = value;
            }
        }

        /// <summary>
        /// The Server's Host Name. This will also resolve the IP Address
        /// </summary>
        /// <remarks>This variable is directly connected to the static OSCMessage.ServerName</remarks>
        public string ServerName
        {
            get
            {
                return OSCMessage.ServerName;
            }
            set
            {
                OSCMessage.ServerName = value;
            }
        }
        #endregion

        #region Dynamic Variables
        /// <summary>
        /// A List of the Workspaces that are currently open on the specified server
        /// </summary>
        public List<Workspace> Workspaces
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage("/workspaces");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);

                string address = QLabServer.__ReturnAddressHolder;
                string reply = QLabServer.__ReplyAddressHolder;
                if (QLabServer.__ReturnValueHolder != null)
                {
                    List<Workspace> returnList = new List<Workspace>();

                    returnList = JsonConvert.DeserializeObject<List<Workspace>>(QLabServer.__ReturnValueHolder.ToString());

                    return returnList;
                }

                throw new Exception("QLab did not return an OSC message in the allotted amount of time");
            }
        }
        #endregion

        #region Constructors
        public QLabServer()
        {
            StartServer();
        }
        private void StartServer()
        {
            //Start a Listening server populated with computer's own IP address and port 53001
            string host = Dns.GetHostName();
            IPHostEntry ip = Dns.GetHostEntry(host);
            server = new OscServer(TransportType.Udp, ip.AddressList[1], 53001); //ME on 53001

            server.PacketReceived += new EventHandler<OscPacketReceivedEventArgs>(server_PacketReceived);

            server.Start();
        }
        #endregion

        #region Methods
        /// <summary>
        /// Send an OSC Message to QLab. 
        /// Assumes a Server Name or IP Address has already been specified.
        /// </summary> 
        /// <param name="Address">OSC Address (i.e. /cue/1/start)</param>
        public void SendOSCMessage(string Address)
        {
            OSCMessage.SendOSCMessage(Address);
        }
        /// <summary>
        /// Send an OSC Message to QLab. 
        /// Assumes a Server Name or IP Address has already been specified.
        /// <param name="Address">OSC Address (i.e. /cue/1/start)</param>
        /// </summary> 
        public void SendOSCMessages(List<string> Addresses)
        {
            OSCMessage.SendOSCMessages(Addresses);
        }

        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary>
        /// <returns>Whether or not the Connection was successful</returns>
        public bool CurrentWorkspace_Connect()
        {
            QLabServer.__ReturnValueHolder = null;
            SendOSCMessage("/connect");
            System.Threading.SpinWait.SpinUntil(() =>
            {
                return (QLabServer.__ReturnValueHolder != null);
            }, QLabServer.OSCTimeout);

            if (QLabServer.__ReturnValueHolder.Equals("ok"))
                return true;
            return false;
        }
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary>
        /// <param name="passcode">The Integer passcode specified in the QLab workspace</param>
        /// <returns>Whether or not the Connection was successful</returns>
        public bool CurrentWorkspace_Connect(int passcode)
        {
            QLabServer.__ReturnValueHolder = null;
            SendOSCMessage("/connect " + passcode);
            System.Threading.SpinWait.SpinUntil(() =>
            {
                return (QLabServer.__ReturnValueHolder != null);
            }, QLabServer.OSCTimeout);

            if (QLabServer.__ReturnValueHolder.Equals("ok"))
                return true;
            return false;
        }

        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary>
        public void CurrentWorkspace_Disconnect()
        {
            SendOSCMessage("/disconnect");
            server.Stop();
        }

        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary>
        public void CurrentWorkspace_Go()
        {
            SendOSCMessage("/go");
        }

        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary>
        public void CurrentWorkspace_Stop()
        {
            SendOSCMessage("/stop");
        }

        /// <summary>
        /// Searched for a specific Workspace in the server's open Workspaces
        /// </summary>
        /// <param name="uniqueID">The Workspace's uniqueID number</param>
        /// <returns>A Workspace if it was found, otherwise null</returns>
        public Workspace GetWorkspace(string ID)
        {
            foreach (Workspace workspace in Workspaces)
                if (workspace.uniqueID.Equals(ID))
                    return workspace;
            return null;
        }

        /// <summary>
        /// Searched for a specific Cue in the Cue List
        /// </summary>
        /// <param name="uniqueID">The Cue's uniqueID number</param>
        /// <returns>A Cue if it was found, otherwise null</returns>
        public Cue GetCue(string uniqueID)
        {
            foreach (Workspace workspace in Workspaces)
            {
                foreach (Cue cue in workspace.Cues)
                {
                    Cue returnCue = cue.GetCue(uniqueID);
                    if (returnCue != null)
                        return returnCue;
                }
            }
            return null;
        }
        /// <summary>
        /// Converts a QLab JSON Reply to an Object Oriented list of Cues.
        /// <para>This method is recursive so will traverse through the entire listing of cues</para>
        /// </summary>
        /// <param name="WorkspaceID">Specify the uniqueID of the containing Workspace</param>
        /// <param name="jsonString">The actual JSON string reply from QLab</param>
        /// <returns>Returns a List of Cues</returns>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static List<Cue> ParseJSONCues(string WorkspaceID, string jsonString)
        {
            List<Cue> returnList = new List<Cue>();

            //Adapt to deserialize to private variables
            jsonString = jsonString.Replace("\"number\":", "\"_number\":");
            jsonString = jsonString.Replace("\"name\":", "\"_name\":");
            jsonString = jsonString.Replace("\"type\":", "\"_type\":");
            jsonString = jsonString.Replace("\"colorName\":", "\"_colorName\":");
            jsonString = jsonString.Replace("\"flagged\":", "\"_flagged\":");
            jsonString = jsonString.Replace("\"armed\":", "\"_armed\":");
            jsonString = jsonString.Replace("\"cues\":", "\"_cues\":");


            returnList = JsonConvert.DeserializeObject<List<Cue>>(jsonString);

            ConvertCues(returnList, WorkspaceID);

            return returnList;
        }
        /// <summary>
        /// Traverses through all the Cues in a list, adding the WorkspaceID and converting the cues into their inherited types
        /// </summary>
        /// <param name="cues">The List of Cues</param>
        /// <param name="WorkspaceID">The WorkspaceID</param>
        private static void ConvertCues(List<Cue> cues, string WorkspaceID)
        {
            if (cues == null)
                return;

            for (int x=0; x < cues.Count; x++)
            {
                Cue cue = cues[x];
                
                if (cue.type.Equals("Audio"))
                    cue = new AudioCue(cue);
                if (cue.type.Equals("Fade"))
                    cue = new FadeCue(cue);
                if (cue.type.Equals("Mic"))
                    cue = new MicCue(cue);
                if (cue.type.Equals("Video"))
                    cue = new VideoCue(cue);
                if (cue.type.Equals("Animation"))
                    cue = new AnimationCue(cue);

                cue.workspaceID = WorkspaceID;

                cues[x] = cue;

                ConvertCues(cue.cues, WorkspaceID);
            }
        }

        /// <summary>
        /// Disconnect and get rid of the current Server
        /// </summary>
        public void Dispose()
        {
            this.CurrentWorkspace_Disconnect();
        }
        #endregion

        #region Event Handlers
        public event WorkspaceUpdatedHandler WorkspaceUpdated;
        public event CueUpdatedHandler CueUpdated;
        public event OSCResponseHandler OSCResponse;
        #endregion

        #region Events
        /// <summary>
        /// Server Response from QLab
        /// </summary>
        void server_PacketReceived(object sender, OscPacketReceivedEventArgs e)
        {
            //If the designer has created an event handler, return base information
            if (OSCResponse != null)
            {
                bool doContinue = this.OSCResponse.Invoke(e.Packet.Address, e.Packet.Data[0].ToString());
                if (!doContinue)
                    return;
            }

            //Set the static ReplyAddressHold to be found by the caller
            __ReplyAddressHolder = e.Packet.Address;
            if (e.Packet.Address.StartsWith("/reply")) //General Workspace Request
            {
                Dictionary<string, object> response = JsonConvert.DeserializeObject<Dictionary<string, object>>(e.Packet.Data[0].ToString());

                //Set the static ReplyValueHolder to found by the caller and end the SpinWait lock
                __ReturnAddressHolder = response["address"].ToString();
                __ReturnValueHolder = response["data"];
            }
            if (e.Packet.Address.StartsWith("/update")) //General Workspace Request
            {
                //Update command has been called. Determine the appropriate response
                //And if the designer has created an event handler, invoke the handler
                if (e.Packet.Address.Contains("/cue_id/"))
                {
                    if (CueUpdated != null)
                    {
                        string workspaceID = e.Packet.Address.Replace("/update/workspace/", "");
                        workspaceID = workspaceID.Substring(0, workspaceID.IndexOf("/"));

                        string cueID = e.Packet.Address.Replace("/update/workspace/" + workspaceID + "/cue_id/", "");

                        CueUpdated.Invoke(workspaceID, cueID);
                    }
                }
                else
                {
                    if (WorkspaceUpdated != null)
                    {
                        string workspaceID = e.Packet.Address.Replace("/update/workspace/", "");
                        WorkspaceUpdated.Invoke(workspaceID);
                    }
                }
            }
        }
        #endregion
    }
    #endregion

    #region Workspace
    public class Workspace
    {
        #region Variables
        private string _uniqueID = null;
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public string uniqueID
        {
            get { return _uniqueID; }
            set { _uniqueID = value; }
        }
        public string _displayName = null;
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public string displayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }
        public bool _hasPasscode = false;
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public bool hasPasscode
        {
            get { return _hasPasscode; }
            set { _hasPasscode = value; }
        }
        bool doUpdate = false;
        #endregion

        #region Dynamic Variables
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public List<Cue> Cues
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage("/workspace/" + uniqueID + "/cueLists");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);

                if (QLabServer.__ReturnValueHolder != null)
                {
                    return QLabServer.ParseJSONCues(uniqueID, QLabServer.__ReturnValueHolder.ToString());
                }

                throw new QLabOSCTimeout();
            }
        }
        
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public List<Cue> SelectedCues
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage("/workspace/" + uniqueID + "/selectedCues");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);

                if (QLabServer.__ReturnValueHolder != null)
                {
                    return QLabServer.ParseJSONCues(uniqueID, QLabServer.__ReturnValueHolder.ToString());
                }

                throw new QLabOSCTimeout();
            }
        }
        
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public List<Cue> RunningCues
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage("/workspace/" + uniqueID + "/runningCues");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);

                if (QLabServer.__ReturnValueHolder != null)
                {
                    return QLabServer.ParseJSONCues(uniqueID, QLabServer.__ReturnValueHolder.ToString());
                }

                throw new QLabOSCTimeout();
            }
        }
        
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public List<Cue> RunningOrPausedCues
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage("/workspace/" + uniqueID + "/runningOrPausedCues");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);

                if (QLabServer.__ReturnValueHolder != null)
                {
                    return QLabServer.ParseJSONCues(uniqueID, QLabServer.__ReturnValueHolder.ToString());
                }

                throw new QLabOSCTimeout();
            }
        }
        #endregion

        #region Constructors
        public Workspace()
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Send an OSC Message to QLab. 
        /// Assumes a Server Name or IP Address has already been specified.
        /// </summary> 
        /// <param name="Address">OSC Address (i.e. /cue/1/start)</param>
        public void SendOSCMessage(string Address)
        {
            OSCMessage.SendOSCMessage(Address);
        }
        /// <summary>
        /// Send an OSC Message to QLab. 
        /// Assumes a Server Name or IP Address has already been specified.
        /// <param name="Address">OSC Address (i.e. /cue/1/start)</param>
        /// </summary> 
        public void SendOSCMessages(List<string> Addresses)
        {
            OSCMessage.SendOSCMessages(Addresses);
        }

        /// <summary>
        /// Open a connection with the QLab Workspace
        /// <remarks>Assumes a uniqueID has already been specified.</remarks>
        /// </summary> 
        public void Connect()
        {
            SendOSCMessage("/workspace/" + uniqueID + "/connect");
        }

        System.ComponentModel.BackgroundWorker bgw = new System.ComponentModel.BackgroundWorker();
        /// <summary>
        /// Open a connection with the QLab Workspace
        /// <remarks>Assumes a uniqueID has already been specified.</remarks>
        /// </summary> 
        /// <param name="Passcode">The 4 Digit Integer specified in QLab</param>
        public void Connect(int Passcode)
        {
            SendOSCMessage("/workspace/" + uniqueID + "/connect " + Passcode);

            //Create a BackgroundWorker to handle the necessary UDP Thump command to maintain an open connection
            //This is only needed for a Connect with Passcode command
            //Thump must happen every 31 seconds
            bgw = new System.ComponentModel.BackgroundWorker();
            bgw.DoWork += new System.ComponentModel.DoWorkEventHandler(bgw_DoWork);
            bgw.RunWorkerAsync(uniqueID);
        }

        void bgw_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            //BackgroundWorker to handle the necessary UDP Thump command to maintain an open connection
            //This is only needed for a Connect with Passcode command
            //Thump must happen every 31 seconds
            while (true)
            {
                System.Threading.Thread.Sleep(30 * 1000);
                OSCMessage.SendOSCMessage("/workspace/" + e.Argument + "/thump");
            }
        }

        /// <summary>
        /// Close the connection with the QLab Workspace
        /// <remarks>Assumes a uniqueID has already been specified.</remarks>
        /// </summary> 
        public void Disconnect()
        {
            //and dispose of the BackgroundWorker
            bgw.Dispose();
            SendOSCMessage("/workspace/" + uniqueID + "/disconnect");
        }

        /// <summary>
        /// Tell QLab whether or not to send Update requests
        /// <remarks>Assumes a uniqueID has already been specified.</remarks>
        /// </summary> 
        public void Update(bool DoUpdates)
        {
            doUpdate = DoUpdates;
            SendOSCMessage("/workspace/" + uniqueID + "/updates " + (doUpdate == true ? "1" : "0"));
        }

        /// <summary>
        /// Send a reminder to QLab that we're still connected
        /// <remarks>This is only needed if the workspace requires a Passcode. Also, the Connect command with a Passcode specified will begin the necessary Thump sequence</remarks>
        /// <remarks>Assumes a uniqueID has already been specified.</remarks>
        /// </summary> 
        public void Thump()
        {
            SendOSCMessage("/workspace/" + uniqueID + "/thump");
        }

        /// <summary>
        /// TBD with final OSC Documentation
        /// <remarks>Assumes a uniqueID has already been specified.</remarks>
        /// </summary> 
        public void Go()
        {
            SendOSCMessage("/workspace/" + uniqueID + "/go");
        }

        /// <summary>
        /// TBD with final OSC Documentation
        /// <remarks>Assumes a uniqueID has already been specified.</remarks>
        /// </summary> 
        public void Stop()
        {
            SendOSCMessage("/workspace/" + uniqueID + "/stop");
        }

        /// <summary>
        /// TBD with final OSC Documentation
        /// <remarks>Assumes a uniqueID has already been specified.</remarks>
        /// </summary> 
        public void Pause()
        {
            SendOSCMessage("/workspace/" + uniqueID + "/pause");
        }

        /// <summary>
        /// TBD with final OSC Documentation
        /// <remarks>Assumes a uniqueID has already been specified.</remarks>
        /// </summary> 
        public void Resume()
        {
            SendOSCMessage("/workspace/" + uniqueID + "/resume");
        }

        /// <summary>
        /// TBD with final OSC Documentation
        /// <remarks>Assumes a uniqueID has already been specified.</remarks>
        /// </summary> 
        public void Reset()
        {
            SendOSCMessage("/workspace/" + uniqueID + "/reset");
        }

        /// <summary>
        /// TBD with final OSC Documentation
        /// <remarks>Assumes a uniqueID has already been specified.</remarks>
        /// </summary> 
        public void Panic()
        {
            SendOSCMessage("/workspace/" + uniqueID + "/panic");
        }


        public Cue GetCue(string uniqueID)
        {
            foreach (Cue cue in Cues)
            {
                Cue returnCue = cue.GetCue(uniqueID);
                if (returnCue != null)
                    return returnCue;
            }
            return null;
        }
        #endregion
    }
    #endregion

    #region Cues
    public class Cue
    {
        #region Variables
        
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string _workspaceID = null;
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public string workspaceID
        {
            get { return _workspaceID; }
            set { _workspaceID = value; }
        }
        
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string _uniqueID = null;
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public string uniqueID
        {
            get { return _uniqueID; }
            set { _uniqueID = value; }
        }
        
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string _type = null;
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public string type
        {
            get { return _type; }
            set { _type = value; }
        }
        
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public List<Cue> _Cues = new List<Cue>();
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public List<Cue> cues
        {
            get { return _Cues; }
            set { _Cues = value; }
        }
        #endregion

        #region Dynamic Variables - Read Only
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public bool hasFileTargets
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/hasFileTargets");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToBoolean(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
        }
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public bool hasCueTargets
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/hasCueTargets");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToBoolean(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
        }
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public bool allowsEditingDuration
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/allowsEditingDuration");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToBoolean(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
        }
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public bool isLoaded
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/isLoaded");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToBoolean(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
        }
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public bool isRunning
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/isRunning");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToBoolean(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
        }
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public bool isPaused
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/isPaused");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToBoolean(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
        }
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public bool isBroken
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/isBroken");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToBoolean(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
        }
        
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public double preWaitElapsed
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/preWaitElapsed");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToDouble(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
        }
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public double actionElapsed
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/actionElapsed");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToDouble(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
        }
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public double postWaitElapsed
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/postWaitElapsed");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToDouble(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
        }
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public double percentPreWaitElapsed
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/percentPreWaitElapsed");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToDouble(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
        }
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public double percentActionElapsed
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/percentActionElapsed");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToDouble(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
        }
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public double percentPostWaitElapsed
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/percentPostWaitElapsed");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToDouble(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
        }
        #endregion

        #region Dynamic Variables
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string _number = null;
        public string number
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/number");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToString(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
            set
            {
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/number " + value);
            }
        }
        
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string _name = null;
        public string name
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/name");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                {
                    _name = Convert.ToString(QLabServer.__ReturnValueHolder);
                    return _name;
                }
                throw new QLabOSCTimeout();
            }
            set
            {
                _name = value;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/name " + value); 
            }
        }
        
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public string notes
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/notes");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToString(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
            set
            {
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/notes " + value);
            }
        }
        
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public string cueTargetNumber
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/cueTargetNumber");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToString(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
            set
            {
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/cueTargetNumber " + value);
            }
        }
        
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public string cueTargetID
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/cueTargetID");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToString(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
            set
            {
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/cueTargetID " + value);
            }
        }
        
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public double preWait
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/preWait");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToDouble(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
            set
            {
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/preWait " + value);
            }
        }
        
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public double duration
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/duration");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToDouble(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
            set
            {
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/duration " + value);
            }
        }
        
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public double postWait
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/postWait");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToDouble(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
            set
            {
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/postWait " + value);
            }
        }
        
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public ContinueMode continueMode
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/continueMode");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                {
                    int mode = Convert.ToInt16(QLabServer.__ReturnValueHolder);
                    if (mode == 0)
                        return ContinueMode.NoContinue;
                    if (mode == 1)
                        return ContinueMode.AutoContinue;
                    if (mode == 2)
                        return ContinueMode.AutoFollow;
                }

                throw new QLabOSCTimeout();
            }
            set
            {
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/continueMode " + (int)value);
            }
        }
        
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool _flagged = false;
        public bool flagged
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/flagged");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                {
                    _flagged = Convert.ToInt16(QLabServer.__ReturnValueHolder) == 1;
                    return _flagged;
                }
                throw new QLabOSCTimeout();
            }
            set
            {
                _flagged = value;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/flagged " + (value == false ? 0 : 1));
            }
        }
        
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool _armed = false;
        public bool armed
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/armed");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                {
                    _armed = Convert.ToInt16(QLabServer.__ReturnValueHolder) == 1;
                    return _armed;
                }
                throw new QLabOSCTimeout();
            }
            set
            {
                _armed = value;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/armed " + (value == false ? 0 : 1));
            }
        }

        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string _colorName = null;
        public string colorName
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/colorName");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                {
                    _colorName = Convert.ToString(QLabServer.__ReturnValueHolder);
                    return _colorName;
                }
                throw new QLabOSCTimeout();
            }
            set
            {
                _colorName = value;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/colorName " + value);
            }
        }
        #endregion

        #region Constructors
        public Cue()
        {
        }
        #endregion

        #region Methods
        /// <summary>
        /// Send an OSC Message to QLab. 
        /// Assumes a Server Name or IP Address has already been specified.
        /// </summary> 
        /// <param name="Address">OSC Address (i.e. /cue/1/start)</param>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SendOSCMessage(string Address)
        {
            OSCMessage.SendOSCMessage(Address);
        }
        /// <summary>
        /// Send an OSC Message to QLab. 
        /// Assumes a Server Name or IP Address has already been specified.
        /// <param name="Address">OSC Address (i.e. /cue/1/start)</param>
        /// </summary> 
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public void SendOSCMessages(List<string> Addresses)
        {
            OSCMessage.SendOSCMessages(Addresses);
        }
        /// <summary>
        /// TBD with final OSC Documentation
        /// <remarks>Assumes a uniqueID or number has already been specified.</remarks>
        /// </summary> 
        public void Go()
        {
            SendOSCMessage((workspaceID != null ? "/workspace/" + workspaceID : "") + (uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/start");
        }
        /// <summary>
        /// TBD with final OSC Documentation
        /// <remarks>Assumes a uniqueID or number has already been specified.</remarks>
        /// </summary> 
        public void Stop()
        {
            SendOSCMessage((workspaceID != null ? "/workspace/" + workspaceID : "") + (uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/stop");
        }
        /// <summary>
        /// TBD with final OSC Documentation
        /// <remarks>Assumes a uniqueID or number has already been specified.</remarks>
        /// </summary> 
        public void Pause()
        {
            SendOSCMessage((workspaceID != null ? "/workspace/" + workspaceID : "") + (uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/pause");
        }
        /// <summary>
        /// TBD with final OSC Documentation
        /// <remarks>Assumes a uniqueID or number has already been specified.</remarks>
        /// </summary> 
        public void Resume()
        {
            SendOSCMessage((workspaceID != null ? "/workspace/" + workspaceID : "") + (uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/resume");
        }
        /// <summary>
        /// TBD with final OSC Documentation
        /// <remarks>Assumes a uniqueID or number has already been specified.</remarks>
        /// </summary> 
        public void Load()
        {
            SendOSCMessage((workspaceID != null ? "/workspace/" + workspaceID : "") + (uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/load");
        }
        /// <summary>
        /// TBD with final OSC Documentation
        /// <remarks>Assumes a uniqueID or number has already been specified.</remarks>
        /// <param name="time">The time to which you would like to load (In Seconds)</param>
        /// </summary> 
        public void LoadAt(double time)
        {
            SendOSCMessage((workspaceID != null ? "/workspace/" + workspaceID : "") + (uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/load " + time);
        }
        /// <summary>
        /// TBD with final OSC Documentation
        /// <remarks>Assumes a uniqueID or number has already been specified.</remarks>
        /// </summary> 
        public void Preview()
        {
            SendOSCMessage((workspaceID != null ? "/workspace/" + workspaceID : "") + (uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/preview");
        }
        /// <summary>
        /// TBD with final OSC Documentation
        /// <remarks>Assumes a uniqueID or number has already been specified.</remarks>
        /// </summary> 
        public void Reset()
        {
            SendOSCMessage((workspaceID != null ? "/workspace/" + workspaceID : "") + (uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/reset");
        }
        /// <summary>
        /// TBD with final OSC Documentation
        /// <remarks>Assumes a uniqueID or number has already been specified.</remarks>
        /// </summary> 
        public void Panic()
        {
            SendOSCMessage((workspaceID != null ? "/workspace/" + workspaceID : "") + (uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/panic");
        }
        
        /// <summary>
        /// Get a cue from the list of children cues
        /// </summary> 
        /// <param name="id">Specify the uniqueID of the cue</param>
        public Cue GetCue(string id)
        {
            if (uniqueID.Equals(id))
                return this;

            foreach (Cue cue in cues)
            {
                if (cue.uniqueID.Equals(id))
                    return cue;

                Cue returnCue = cue.GetCue(id);
                if (returnCue != null)
                    return returnCue;
            }

            return null;
        }
        #endregion
    }

    public class AudioCue : Cue
    {
        public AudioCue(Cue cue)
        {
            _workspaceID = cue._workspaceID;
            _uniqueID = cue._uniqueID;
            _number = cue._number;
            _name = cue._name;
            _type = cue._type;
            _colorName = cue._colorName;
            _flagged = cue._flagged;
            _armed = cue._armed;
        }

        #region Dynamic Variables
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public int patch
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/patch");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToInt16(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
            set
            {
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/patch " + value);
            }
        }
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public double startTime
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/startTime");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToDouble(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
            set
            {
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/startTime " + value);
            }
        }
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public double endTime
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/endTime");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToDouble(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
            set
            {
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/endTime " + value);
            }
        }
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public int playCount
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/playCount");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToInt16(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
            set
            {
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/playCount " + value);
            }
        }
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public bool infiniteLoop
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/infiniteLoop");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToBoolean(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
            set
            {
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/infiniteLoop " + (value == false ? 0 : 1));
            }
        }
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public double rate
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/rate");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToDouble(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
            set
            {
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/rate " + value);
            }
        }
        /// <summary>
        /// TBD with final OSC Documentation
        /// </summary> 
        public bool doFade
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/doFade");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToBoolean(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
            set
            {
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/doFade " + (value == false ? 0 : 1));
            }
        }
        /// <summary>
        /// Returns of 49 Volume Levels (Channel 0 is the Master Volume)
        /// </summary> 
        public List<double> sliderLevels
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/sliderLevels");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                {
                    List<double> returnList = new List<double>();
                    foreach (double val in (Newtonsoft.Json.Linq.JArray)QLabServer.__ReturnValueHolder)
                        returnList.Add(val);
                    return returnList;
                }
                throw new QLabOSCTimeout();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adjust the volume level of a specific channel (Channel 0 is the Master Volume)
        /// </summary> 
        /// <param name="channelNumber">Specify a Channel Number (0-48)</param>
        /// <param name="decibelNumber">Specify the Volume in decibels (Typically -60 to +12)</param>
        public void SetVolumeLevel(int channelNumber, double decibelNumber)
        {
            SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/sliderLevel " + channelNumber + " " + decibelNumber);
        }

        /// <summary>
        /// Get the volume level of a specific channel (Channel 0 is the Master Volume)
        /// </summary> 
        /// <param name="channelNumber">Specify a Channel Number (0-48)</param>
        public double GetVolumeLevel(int channelNumber)
        {
            QLabServer.__ReturnValueHolder = null;
            SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/sliderLevel " + channelNumber);
            System.Threading.SpinWait.SpinUntil(() =>
            {
                return (QLabServer.__ReturnValueHolder != null);
            }, QLabServer.OSCTimeout);
            if (QLabServer.__ReturnValueHolder != null)
            {
                return Convert.ToDouble(QLabServer.__ReturnValueHolder);
            }
            throw new QLabOSCTimeout();
        }

        /// <summary>
        /// Adjust the volume level of a the Master Volume
        /// </summary> 
        /// <param name="decibelNumber">Specify the Volume in decibels (Typically -60 to +12)</param>
        public void SetMasterVolume(double decibelNumber)
        {
            SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/sliderLevel 0 " + decibelNumber);
        }

        /// <summary>
        /// Get the volume level of a the Master Volume
        /// </summary> 
        public double GetMasterVolume()
        {
            QLabServer.__ReturnValueHolder = null;
            SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/sliderLevel 0");
            System.Threading.SpinWait.SpinUntil(() =>
            {
                return (QLabServer.__ReturnValueHolder != null);
            }, QLabServer.OSCTimeout);
            if (QLabServer.__ReturnValueHolder != null)
            {
                return Convert.ToDouble(QLabServer.__ReturnValueHolder);
            }
            throw new QLabOSCTimeout();
        }
        #endregion
    }
    public class FadeCue : Cue
    {
        public FadeCue(Cue cue)
        {
            _workspaceID = cue._workspaceID;
            _uniqueID = cue._uniqueID;
            _number = cue._number;
            _name = cue._name;
            _type = cue._type;
            _colorName = cue._colorName;
            _flagged = cue._flagged;
            _armed = cue._armed;
        }

        #region Dynamic Variables
        /// <summary>
        /// Returns of 49 Volume Levels (Channel 0 is the Master Volume)
        /// </summary> 
        public List<double> sliderLevels
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/sliderLevels");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                {
                    List<double> returnList = new List<double>();
                    foreach (double val in (Newtonsoft.Json.Linq.JArray)QLabServer.__ReturnValueHolder)
                        returnList.Add(val);
                    return returnList;
                }
                throw new QLabOSCTimeout();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adjust the volume level of a specific channel (Channel 0 is the Master Volume)
        /// </summary> 
        /// <param name="channelNumber">Specify a Channel Number (0-48)</param>
        /// <param name="decibelNumber">Specify the Volume in decibels (Typically -60 to +12)</param>
        public void SetVolumeLevel(int channelNumber, double decibelNumber)
        {
            SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/sliderLevel " + channelNumber + " " + decibelNumber);
        }
        
        /// <summary>
        /// Get the volume level of a specific channel (Channel 0 is the Master Volume)
        /// </summary> 
        /// <param name="channelNumber">Specify a Channel Number (0-48)</param>
        public double GetVolumeLevel(int channelNumber)
        {
            QLabServer.__ReturnValueHolder = null;
            SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/sliderLevel " + channelNumber);
            System.Threading.SpinWait.SpinUntil(() =>
            {
                return (QLabServer.__ReturnValueHolder != null);
            }, QLabServer.OSCTimeout);
            if (QLabServer.__ReturnValueHolder != null)
            {
                return Convert.ToDouble(QLabServer.__ReturnValueHolder);
            }
            throw new QLabOSCTimeout();
        }
        
        /// <summary>
        /// Adjust the volume level of a the Master Volume
        /// </summary> 
        /// <param name="decibelNumber">Specify the Volume in decibels (Typically -60 to +12)</param>
        public void SetMasterVolume(double decibelNumber)
        {
            SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/sliderLevel 0 " + decibelNumber);
        }
        
        /// <summary>
        /// Get the volume level of a the Master Volume
        /// </summary> 
        public double GetMasterVolume()
        {
            QLabServer.__ReturnValueHolder = null;
            SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/sliderLevel 0");
            System.Threading.SpinWait.SpinUntil(() =>
            {
                return (QLabServer.__ReturnValueHolder != null);
            }, QLabServer.OSCTimeout);
            if (QLabServer.__ReturnValueHolder != null)
            {
                return Convert.ToDouble(QLabServer.__ReturnValueHolder);
            }
            throw new QLabOSCTimeout();
        }
        #endregion
    }

    public class MicCue : Cue
    {
        public MicCue(Cue cue)
        {
            _workspaceID = cue._workspaceID;
            _uniqueID = cue._uniqueID;
            _number = cue._number;
            _name = cue._name;
            _type = cue._type;
            _colorName = cue._colorName;
            _flagged = cue._flagged;
            _armed = cue._armed;
        }

        #region Dynamic Variables
        /// <summary>
        /// Returns of 49 Volume Levels (Channel 0 is the Master Volume)
        /// </summary> 
        public List<double> sliderLevels
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/sliderLevels");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, QLabServer.OSCTimeout);
                if (QLabServer.__ReturnValueHolder != null)
                {
                    List<double> returnList = new List<double>();
                    foreach (double val in (Newtonsoft.Json.Linq.JArray)QLabServer.__ReturnValueHolder)
                        returnList.Add(val);
                    return returnList;
                }
                throw new QLabOSCTimeout();
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Adjust the volume level of a specific channel (Channel 0 is the Master Volume)
        /// </summary> 
        /// <param name="channelNumber">Specify a Channel Number (0-48)</param>
        /// <param name="decibelNumber">Specify the Volume in decibels (Typically -60 to +12)</param>
        public void SetVolumeLevel(int channelNumber, double decibelNumber)
        {
            SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/sliderLevel " + channelNumber + " " + decibelNumber);
        }
        
        /// <summary>
        /// Get the volume level of a specific channel (Channel 0 is the Master Volume)
        /// </summary> 
        /// <param name="channelNumber">Specify a Channel Number (0-48)</param>
        public double GetVolumeLevel(int channelNumber)
        {
            QLabServer.__ReturnValueHolder = null;
            SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/sliderLevel " + channelNumber);
            System.Threading.SpinWait.SpinUntil(() =>
            {
                return (QLabServer.__ReturnValueHolder != null);
            }, QLabServer.OSCTimeout);
            if (QLabServer.__ReturnValueHolder != null)
            {
                return Convert.ToDouble(QLabServer.__ReturnValueHolder);
            }
            throw new QLabOSCTimeout();
        }
        
        /// <summary>
        /// Adjust the volume level of a the Master Volume
        /// </summary> 
        /// <param name="decibelNumber">Specify the Volume in decibels (Typically -60 to +12)</param>
        public void SetMasterVolume(double decibelNumber)
        {
            SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/sliderLevel 0 " + decibelNumber);
        }
        
        /// <summary>
        /// Get the volume level of a the Master Volume
        /// </summary> 
        public double GetMasterVolume()
        {
            QLabServer.__ReturnValueHolder = null;
            SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/sliderLevel 0");
            System.Threading.SpinWait.SpinUntil(() =>
            {
                return (QLabServer.__ReturnValueHolder != null);
            }, QLabServer.OSCTimeout);
            if (QLabServer.__ReturnValueHolder != null)
            {
                return Convert.ToDouble(QLabServer.__ReturnValueHolder);
            }
            throw new QLabOSCTimeout();
        }
        #endregion
    }

    public class VideoCue : Cue
    {

        public VideoCue(Cue cue)
        {
            _workspaceID = cue._workspaceID;
            _uniqueID = cue._uniqueID;
            _number = cue._number;
            _name = cue._name;
            _type = cue._type;
            _colorName = cue._colorName;
            _flagged = cue._flagged;
            _armed = cue._armed;
        }
    }

    public class AnimationCue : Cue
    {

        public AnimationCue(Cue cue)
        {
            _workspaceID = cue._workspaceID;
            _uniqueID = cue._uniqueID;
            _number = cue._number;
            _name = cue._name;
            _type = cue._type;
            _colorName = cue._colorName;
            _flagged = cue._flagged;
            _armed = cue._armed;
        }
    }
    #endregion

    #region Enums
    public enum ContinueMode
    {
        NoContinue = 0,
        AutoContinue,
        AutoFollow
    }
    #endregion
}
