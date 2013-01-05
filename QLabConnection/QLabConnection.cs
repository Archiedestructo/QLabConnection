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

        public static object __ReturnValueHolder = null;
        public static string __ReturnAddressHolder = null;
        public static string __ReplyAddressHolder = null;

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
        public List<Workspace> Workspaces
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage("/workspaces");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);

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
        public void SendOSCMessage(string Address)
        {
            OSCMessage.SendOSCMessage(Address);
        }
        public void SendOSCMessages(List<string> Addresses)
        {
            OSCMessage.SendOSCMessages(Addresses);
        }

        public bool CurrentWorkspace_Connect()
        {
            SendOSCMessage("/connect");
            return true;
        }
        public bool CurrentWorkspace_Connect(int passcode)
        {
            QLabServer.__ReturnValueHolder = null;
            SendOSCMessage("/connect " + passcode);
            System.Threading.SpinWait.SpinUntil(() =>
            {
                return (QLabServer.__ReturnValueHolder != null);
            }, 3000);

            if (QLabServer.__ReturnValueHolder.Equals("ok"))
                return true;
            return false;
        }

        public void CurrentWorkspace_Disconnect()
        {
            SendOSCMessage("/disconnect");
            server.Stop();
        }

        public void CurrentWorkspace_Go()
        {
            SendOSCMessage("/go");
        }

        public void CurrentWorkspace_Stop()
        {
            SendOSCMessage("/stop");
        }

        public Workspace GetWorkspace(string ID)
        {
            foreach (Workspace workspace in Workspaces)
                if (workspace.uniqueID.Equals(ID))
                    return workspace;
            return null;
        }

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

            return returnList;



            JContainer jCueLists = (JContainer)QLabServer.__ReturnValueHolder;

            foreach (JObject jCueList in jCueLists.Children())
            {
                Cue cueList = JsonConvert.DeserializeObject<Cue>(jCueList.ToString());
                cueList.cues.Clear();

                JContainer jCues = (JContainer)jCueList["cues"];

                foreach (JObject jCue in jCues.Children())
                {
                    Cue cue = JsonConvert.DeserializeObject<Cue>(jCue.ToString());
                    if (cue.type.Equals("Audio"))
                        cue = new AudioCue(cue);
                    if (cue.type.Equals("Fade"))
                        cue = new FadeCue(cue);
                    if (cue.type.Equals("Mic"))
                        cue = new MicCue(cue);
                    cue.workspaceID = WorkspaceID;
                    returnList.Add(cue);
                }
            }
            return returnList;
        }
        #endregion

        #region Event Handlers
        public event WorkspaceUpdatedHandler WorkspaceUpdated;
        public event CueUpdatedHandler CueUpdated;
        public event OSCResponseHandler OSCResponse;
        #endregion

        #region Events
        void server_PacketReceived(object sender, OscPacketReceivedEventArgs e)
        {
            if (OSCResponse != null)
            {
                bool doContinue = OSCResponse.Invoke(e.Packet.Address, e.Packet.Data[0].ToString());
                if (!doContinue)
                    return;
            }

            __ReplyAddressHolder = e.Packet.Address;
            if (e.Packet.Address.StartsWith("/reply")) //General Workspace Request
            {
                Dictionary<string, object> response = JsonConvert.DeserializeObject<Dictionary<string, object>>(e.Packet.Data[0].ToString());

                __ReturnAddressHolder = response["address"].ToString();
                __ReturnValueHolder = response["data"];
            }
            if (e.Packet.Address.StartsWith("/update")) //General Workspace Request
            {
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
        public string uniqueID
        {
            get { return _uniqueID; }
            set { _uniqueID = value; }
        }
        public string _displayName = null;
        public string displayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }
        public bool _hasPasscode = false;
        public bool hasPasscode
        {
            get { return _hasPasscode; }
            set { _hasPasscode = value; }
        }
        bool doUpdate = false;
        #endregion

        #region Dynamic Variables
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
                }, 3000);

                if (QLabServer.__ReturnValueHolder != null)
                {
                    return QLabServer.ParseJSONCues(uniqueID, QLabServer.__ReturnValueHolder.ToString());
                }

                throw new QLabOSCTimeout();
            }
        }

        public List<Cue> SelectedCues
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage("/workspace/" + uniqueID + "/selectedCues");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);

                if (QLabServer.__ReturnValueHolder != null)
                {
                    return QLabServer.ParseJSONCues(uniqueID, QLabServer.__ReturnValueHolder.ToString());
                }

                throw new QLabOSCTimeout();
            }
        }

        public List<Cue> RunningCues
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage("/workspace/" + uniqueID + "/runningCues");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);

                if (QLabServer.__ReturnValueHolder != null)
                {
                    return QLabServer.ParseJSONCues(uniqueID, QLabServer.__ReturnValueHolder.ToString());
                }

                throw new QLabOSCTimeout();
            }
        }

        public List<Cue> RunningOrPausedCues
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage("/workspace/" + uniqueID + "/runningOrPausedCues");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);

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
        public void SendOSCMessage(string Address)
        {
            OSCMessage.SendOSCMessage(Address);
        }
        public void SendOSCMessages(List<string> Addresses)
        {
            OSCMessage.SendOSCMessages(Addresses);
        }

        public void Connect()
        {
            SendOSCMessage("/workspace/" + uniqueID + "/connect");
        }

        System.ComponentModel.BackgroundWorker bgw = new System.ComponentModel.BackgroundWorker();
        public void Connect(int Passcode)
        {
            SendOSCMessage("/workspace/" + uniqueID + "/connect " + Passcode);
            bgw.DoWork += new System.ComponentModel.DoWorkEventHandler(bgw_DoWork);
            bgw.RunWorkerAsync(uniqueID);
        }

        void bgw_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            while (true)
            {
                System.Threading.Thread.Sleep(30 * 1000);
                OSCMessage.SendOSCMessage("/workspace/" + e.Argument + "/thump");
            }
        }

        public void Disconnect()
        {
            SendOSCMessage("/workspace/" + uniqueID + "/disconnect");
        }

        public void Update(bool DoUpdates)
        {
            doUpdate = DoUpdates;
            SendOSCMessage("/workspace/" + uniqueID + "/updates " + (doUpdate == true ? "1" : "0"));
        }

        public void Thump()
        {
            SendOSCMessage("/workspace/" + uniqueID + "/thump");
        }

        public void Go()
        {
            SendOSCMessage("/workspace/" + uniqueID + "/go");
        }

        public void Stop()
        {
            SendOSCMessage("/workspace/" + uniqueID + "/stop");
        }

        public void Pause()
        {
            SendOSCMessage("/workspace/" + uniqueID + "/pause");
        }

        public void Resume()
        {
            SendOSCMessage("/workspace/" + uniqueID + "/resume");
        }

        public void Reset()
        {
            SendOSCMessage("/workspace/" + uniqueID + "/reset");
        }

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

        #region Events

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
        public string workspaceID
        {
            get { return _workspaceID; }
            set { _workspaceID = value; }
        }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string _uniqueID = null;
        public string uniqueID
        {
            get { return _uniqueID; }
            set { _uniqueID = value; }
        }
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public string _type = null;
        public string type
        {
            get { return _type; }
            set { _type = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        public List<Cue> _Cues = new List<Cue>();
        public List<Cue> cues
        {
            get { return _Cues; }
            set { _Cues = value; }
        }
        #endregion

        #region Dynamic Variables - Read Only
        public bool hasFileTargets
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/hasFileTargets");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToBoolean(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
        }
        public bool hasCueTargets
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/hasCueTargets");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToBoolean(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
        }
        public bool allowsEditingDuration
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/allowsEditingDuration");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToBoolean(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
        }
        public bool isLoaded
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/isLoaded");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToBoolean(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
        }
        public bool isRunning
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/isRunning");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToBoolean(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
        }
        public bool isPaused
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/isPaused");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToBoolean(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
        }
        public bool isBroken
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/isBroken");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToBoolean(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
        }

        public double preWaitElapsed
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/preWaitElapsed");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToDouble(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
        }
        public double actionElapsed
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/actionElapsed");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToDouble(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
        }
        public double postWaitElapsed
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/postWaitElapsed");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToDouble(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
        }
        public double percentPreWaitElapsed
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/percentPreWaitElapsed");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToDouble(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
        }
        public double percentActionElapsed
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/percentActionElapsed");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToDouble(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
        }
        public double percentPostWaitElapsed
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/percentPostWaitElapsed");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToDouble(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
        }
        #endregion

        #region Dynamic Variables
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
                }, 3000);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToString(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
            set
            {
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/number " + value);
            }
        }
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
                }, 3000);
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
        public string notes
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/notes");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToString(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
            set
            {
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/notes " + value);
            }
        }
        public string cueTargetNumber
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/cueTargetNumber");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToString(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
            set
            {
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/cueTargetNumber " + value);
            }
        }
        public string cueTargetID
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/cueTargetID");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToString(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
            set
            {
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/cueTargetID " + value);
            }
        }
        public double preWait
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/preWait");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToDouble(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
            set
            {
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/preWait " + value);
            }
        }
        public double duration
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/duration");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToDouble(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
            set
            {
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/duration " + value);
            }
        }
        public double postWait
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/postWait");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToDouble(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
            set
            {
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/postWait " + value);
            }
        }
        public ContinueMode continueMode
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage((uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/continueMode");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);
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
                }, 3000);
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
                }, 3000);
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
                }, 3000);
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
        /// </summary> 
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        /// <param name="Address">OSC Address (i.e. /cue/1/start)</param>
        public void SendOSCMessages(List<string> Addresses)
        {
            OSCMessage.SendOSCMessages(Addresses);
        }
        /// <summary>
        /// Assumes a uniqueID or number has already been specified.
        /// </summary> 
        public void Go()
        {
            SendOSCMessage((workspaceID != null ? "/workspace/" + workspaceID : "") + (uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/start");
        }
        /// <summary>
        /// Assumes a uniqueID or number has already been specified.
        /// </summary> 
        public void Stop()
        {
            SendOSCMessage((workspaceID != null ? "/workspace/" + workspaceID : "") + (uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/stop");
        }
        /// <summary>
        /// Assumes a uniqueID or number has already been specified.
        /// </summary> 
        public void Pause()
        {
            SendOSCMessage((workspaceID != null ? "/workspace/" + workspaceID : "") + (uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/pause");
        }
        /// <summary>
        /// Assumes a uniqueID or number has already been specified.
        /// </summary> 
        public void Resume()
        {
            SendOSCMessage((workspaceID != null ? "/workspace/" + workspaceID : "") + (uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/resume");
        }
        /// <summary>
        /// Assumes a uniqueID or number has already been specified.
        /// </summary> 
        public void Load()
        {
            SendOSCMessage((workspaceID != null ? "/workspace/" + workspaceID : "") + (uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/load");
        }
        /// <summary>
        /// Assumes a uniqueID or number has already been specified.
        /// </summary> 
        public void LoadAt(int time)
        {
            SendOSCMessage((workspaceID != null ? "/workspace/" + workspaceID : "") + (uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/load " + time);
        
        /// <summary>
        /// Assumes a uniqueID or number has already been specified.
        /// </summary> 
        public void Preview()
        {
            SendOSCMessage((workspaceID != null ? "/workspace/" + workspaceID : "") + (uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/preview");
        }
        /// <summary>
        /// Assumes a uniqueID or number has already been specified.
        /// </summary> 
        public void Reset()
        {
            SendOSCMessage((workspaceID != null ? "/workspace/" + workspaceID : "") + (uniqueID != null ? "/cue_id/" + uniqueID : "/cue/" + _number) + "/reset");
        }
        /// <summary>
        /// Assumes a uniqueID or number has already been specified.
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
        public int patch
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/patch");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToInt16(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
            set
            {
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/patch " + value);
            }
        }
        public double startTime
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/startTime");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToDouble(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
            set
            {
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/startTime " + value);
            }
        }
        public double endTime
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/endTime");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToDouble(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
            set
            {
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/endTime " + value);
            }
        }
        public int playCount
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/playCount");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToInt16(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
            set
            {
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/playCount " + value);
            }
        }
        public bool infiniteLoop
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/infiniteLoop");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToBoolean(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
            set
            {
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/infiniteLoop " + (value == false ? 0 : 1));
            }
        }
        public double rate
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/rate");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToDouble(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
            set
            {
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/rate " + value);
            }
        }
        public bool doFade
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/doFade");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);
                if (QLabServer.__ReturnValueHolder != null)
                    return Convert.ToBoolean(QLabServer.__ReturnValueHolder);
                throw new QLabOSCTimeout();
            }
            set
            {
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/doFade " + (value == false ? 0 : 1));
            }
        }
        public List<double> sliderLevels
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/sliderLevels");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);
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
            }, 3000);
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
            }, 3000);
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
        public List<double> sliderLevels
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/sliderLevels");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);
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
        public void SetVolumeLevel(int channelNumber, double decibelNumber)
        {
            SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/sliderLevel " + channelNumber + " " + decibelNumber);
        }
        public double GetVolumeLevel(int channelNumber)
        {
            QLabServer.__ReturnValueHolder = null;
            SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/sliderLevel " + channelNumber);
            System.Threading.SpinWait.SpinUntil(() =>
            {
                return (QLabServer.__ReturnValueHolder != null);
            }, 3000);
            if (QLabServer.__ReturnValueHolder != null)
            {
                return Convert.ToDouble(QLabServer.__ReturnValueHolder);
            }
            throw new QLabOSCTimeout();
        }
        public void SetMasterVolume(double decibelNumber)
        {
            SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/sliderLevel 0 " + decibelNumber);
        }
        public double GetMasterVolume()
        {
            QLabServer.__ReturnValueHolder = null;
            SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/sliderLevel 0");
            System.Threading.SpinWait.SpinUntil(() =>
            {
                return (QLabServer.__ReturnValueHolder != null);
            }, 3000);
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
        public List<double> sliderLevels
        {
            get
            {
                QLabServer.__ReturnValueHolder = null;
                SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/sliderLevels");
                System.Threading.SpinWait.SpinUntil(() =>
                {
                    return (QLabServer.__ReturnValueHolder != null);
                }, 3000);
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
        public void SetVolumeLevel(int channelNumber, double decibelNumber)
        {
            SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/sliderLevel " + channelNumber + " " + decibelNumber);
        }
        public double GetVolumeLevel(int channelNumber)
        {
            QLabServer.__ReturnValueHolder = null;
            SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/sliderLevel " + channelNumber);
            System.Threading.SpinWait.SpinUntil(() =>
            {
                return (QLabServer.__ReturnValueHolder != null);
            }, 3000);
            if (QLabServer.__ReturnValueHolder != null)
            {
                return Convert.ToDouble(QLabServer.__ReturnValueHolder);
            }
            throw new QLabOSCTimeout();
        }
        public void SetMasterVolume(double decibelNumber)
        {
            SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/sliderLevel 0 " + decibelNumber);
        }
        public double GetMasterVolume()
        {
            QLabServer.__ReturnValueHolder = null;
            SendOSCMessage("/cue/" + (uniqueID != null ? uniqueID : number) + "/sliderLevel 0");
            System.Threading.SpinWait.SpinUntil(() =>
            {
                return (QLabServer.__ReturnValueHolder != null);
            }, 3000);
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
