
This project is an implementation of the OSC Protocol for QLab 3, created for the .NET language. It's intended to be an Object Oriented interface to observe and control QLab workspaces via a UDP connection. I created it for my own uses, it's not meant to be thorough, feel free to use it as you wish.

The project contains a Visual Studio solution with two projects:

QLabConnection is a Class Library that builds to a DLL file. With the output DLL file you should also grab the Bespoke and Newsoft DLL files as they are used for OSC communication and JSON deserialization.

QLabOSCExample is simply a sandbox application to test out the functionality of the QLabConnection namespace.

QLabConnection is then comprised of the following classes:

OSCMessage: 
Is a wrapper for the Bespoke OSC interface. You could use this alone if you wanted. Simply use the OSC commands outlined by Figure53. Be sure to first either specify OSCMessage.ServerIPAddress or OSCMessage.ServerName (which will resolved the IPAddress). Both these properties are Static. Then use OSCMessage.SendOSCMessage(OSCAddress) to send a command to QLab (This is also static). Remember, if you use this alone, there is no reply handler. 

NOTE: The following classes create a threading SpinWait cycle!! This means 1) all data will be dynamic, up-to-date, pulled directly from QLab rather than the local object variables and 2) they will timeout if the connection to QLab is lost. 

QLabServer:
Creates an OSC Server. Again, be sure to specify a Server Name or IPAddress. Here you can also send custom OSC Addresses using SendOSCMessage. This time however, you can create a ResponseHandler event to receive replies.
This class also contains some general workspace commands and also a list of open Workspaces on your QLab machine.
Remember! You can only have one server at a time, so you must Disconnect or Dispose of this object before creating second.

Workspace:
An object oriented interface for the QLab workspace. This contains some general information, commands, and a list of Cues (Cue Lists are treated as Group cues).

Cue and Inherited Cue Types:
An object oriented interface for QLab cues. Audio, Fade, Mic, Video, and Animation cues are extensions of the general Cue class. All other QLab cues are simply covered by the generic Cue class.
