/*
 * K.I.S.S. Konsole for High Performance Software Defined Radio
 *
 * Developed from original code  Copyright 2006 (C) Phil Covington, N8VB
 * 
 * Copyright (C) 2009, 2010, 2011 Phil Harman, VK6APH
 * Copyright (C) 2009 David McQuate WA8YWQ
 * Copyright (C) 2009 Joe Martin K5SO
 * Copyright (C) 2009-2014 George Byrkit K9TRV
 * Copyright (C) 2009 Mark Amos W8XR
 * Copyright (C) 2011 Erik Anderson KE7YOA
 * Copyright (C) 2011-2012 Warren Pratt NR0V (wcpAGC code)
 * Copyright (C) 2014-2014, Jae Stutzman K5JAE (Linux/Unix interoperability)
 *
 * This program is free software; you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation; either version 2 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 * 
 * 
 */

/*
 * Change log 
 * 
 *  1 June 2009 - Released as V1.0.0
 *  4 June 2009 - Allow keypad frequency entry with period for MHz
 *              - Changed frequency display to MHz.kHz Hz
 *  5 June 2009 - Improve Grid code and show all band edges in red
 *              - Fixed large memory size
 *  6 June 2009 - Make floats region independent (',' instead of '.')
 *              - Fixed bug in mouse frequency drag code.
 * 10 June 2009 - Added split display if both wide and narrow bandscopes being displayed
 * 11 June 2009 - Release as V1.0.1
 * 12 June 2009 - Add improved KK.csv file handling by George K9TRV
 *              - Change mouse cursor when over bandscope and when left mouse button down
 * 13 June 2009 - Read mouse speed and if necessary slow it down when drag tuning
 *              - S Meter test code from George K9TRV
 *              - Added mouse click tune
 *              - Working on mouse setting of the filter bandwidth
 * 14 June 2009 - fixed bug in drag tune rate - thanks Lyle KK7P for spotting!
 *              - Removed mouse sensitivity test since no longer needed
 *              - Added bandpass shift using mouse - WIP 
 *              - Added setting filter high and low frequencies with mouse -WIP
 * 15 June 2009 - Performance tuning by George K9TRV
 *              - More SMeter code by K9TRV
 *              - Limit bandpass shift to +/-9.999kHz
 *              - Limit filter drag to +/- 9.999kHz
 * 17 June 2009 - Textural SMeter working, fixed bug in click tune - both from George K9TRV
 * 20 June 2009 - Added Graphical S Meter
 *              - Standard calibrations added - will need a user control for fine calibration.
 * 21 June 2009 - Improvements to SMeter code, only draw background once - K9TRV
 *              - Added AGC hang time control.
 *              - Started contols for max, min and step settings for bandscope, still WIP
 * 22 June 2009 - Fixed bug in keyboard entry that cleared frequency display - K1RQG spotted this!
 *              - Added users settings for ANF and NR - K9TRV
 *              - Removed need to redraw bandscope backgound each update - K9TRV
 *              - Further work on max,min and step controls  - WIP
 * 24 June 2009 - Added FM and SAM modes - K9TRV
 *              - Completed bandscope controls, settings saved to KK.csv
 * 25 June 2009 - Improved bandscope drawing - K9TRV
 *              - Updated bandscope calibration description
 *              - Added bandwidth controls for SAM and FM
 *              - Increased max filter width in CW mode to 1kHz
 * 11 July 2009 - Send C&C data to Ozy if nothing received from EP6 to force it to send data
 *              - Setup now tool strip item so will open and close correctly - Harman Jnr
 *                good to see all those college $'s were well spent!
 *              - Release as V1.0.2
 * 12 July 2009 - Start of Waterfall display or how to use all your CPU in a few lines of code!
 * 13 July 2009 - Quick hack to correct click tune for CWL - needs more work 
 * 14 July 2009 - More Waterfall work - much less CPU - K9TRV
 *              - Added colours for the different waterfall levels
 * 16 July 2009 - Added seperate pictureboxs for each display - K9TRV
 *              - Added adjustable size for bandscope windows - K9TRV
 * 17 July 2009 - Added automatic setting for Waterfall low colour i.e. 'Waterfall AGC'
 * 18 July 2009 - Release as V1.0.3
 *              - Added squelch control for bandscope
 *              - Added squelch control for filter bandwidth
 *              - Added background colour for squelch control to show status
 * 23 July 2009 - Reduced CPU load by changing ADC Overload LED method - thanks Willi SM6OMH for spotting this!
 * 25 July 2009 - Added GUI for squelch level control and compenstated for filter BW
 * 26 July 2009 - Release as V1.0.4
 * 29 July 2009 - Merged Penelope code from Dave, WA8YWQ
 * 30 July 2009 - Added Hardware configuration options and selection for Penelope
 *              - Added 20dB Mic Gain option for Penelope
 *              - Added Hardware selection for Excalibur & save setting in KK.csv
 *              - Added 10MHz clock selection & save setting in KK.csv
 * 31 July 2009 - Added Tabs to Setup form - K9TRV
 *              - Added Tx filter high and low selection and save in KK.csv
 *              - Added Tx drive control and save in KK.csv
 *              - Added Tx gain per band to setup form and save values in KK.csv
 *  1 Aug  2009 - Added Full Duplex option and save setting in KK.csv
 *              - Added MOX buttton, mute receiver audio if not full duplex
 *              - Added Tune button and % power setting on setup form and save in KK.csv
 *                  TODO: Cancel MOX/TUN if active and Start clicked
 *  2 Aug  2009 - Added CW Pitch control to setup, filter settings and save values in KK.csv
 *              - Fixed bug when opening setup form - K9TRV
 *              - Started adding J6 output controls for Penelope
 *              - Added PTT from Atlas Bus
 *  3 Aug  2009 - PTT from Atlas immediately sent back via C&C C0 to minimise delays
 *              - Added Penny Open Collector control group enable/disable and save in KK.csv
 *              - More code for Penny Open Collector controls
 *  4 Aug  2009 - Yet more code for Penny Open Collector controls - who's bright idea was it to have 154? :-)
 *  6 Aug  2009 - Completed Penny Open Collect controls - K9TRV
 *              - Added Penny Open Collector controls to C&C data
 *              - Replaced Start check box with ON/OFF button 
 *              - Interlocked ON/OFF with MOX and TUN controls
 *  7 Aug  2009 - Added independent Duplex controls for Tx and Rx
 *              - Code review and improvements - K9TRV
 *  8 Aug  2009 - Release as V1.0.5
 *              - Start support for CW - add raised cosine profile for CW note
 *  9 Aug  2009 - Completed CW raised cosine profile
 *              - Added PPT sequencer controls
 * 10 Aug  2009 - Completed CW profile and PTT sequencer
 * 11 Aug  2009 - Fixed bug in Penny Open Collector code that had bits shifted
 *              - Renamed Mercury_send() and to_Mercury[] to Data_send and to_Device[] now we have Penny support
 *              - Tidy comments up
 * 15 Aug  2009 - Fixed bug that meant SSB was always USB
 *              - Changed "120m" to "12m"
 *              - Prevented PTT on AM and FM from latching
 *              - Prevent selection of options that are only relevant with Penelope fitted
 *              - Added display of software and FPGA code versions on Setup form
 *              - Check that Penny is present if selected
 *              - Only allow PTT, MOX and TUN if Penny selected
 * 16 Aug  2009 - Fixed long term dropped audio at 48kHz bug - K9TRV
 *              - Code tidy and improvements - K9TRV
 * 19 Aug  2009 - Moved to Ozy_Janus.rbf V1.6. C0[2] = DOT, C0[1] = Dash, C0[0] = PTT
 *              - Started coding for full Duplex operation
 * 20 Aug  2009 - Added state machine to Data_send() for Duplex code
 *              - Added dot and dash key signals and temporarily linked to PTT for testing CW code.
 *              - Tested Duplex code with Mercury V2.8, works fine, now need GUI to support it.
 * 21 Aug  2009 - Provide Method for I2C mic gain so can be toggled whilst Txing
 *              - Warn user if Penny selected but no 10MHz reference selected
 *              - Fixed the bug in the 'Start' routine when after loading Ozy returns a null string - K9TRV
 * 28 Oct  2009 - Test version for Ethernet using SharpPcap.
 * 29 Oct  2009 - Added sequence number check for data from PC.
 *  4 Nov  2009 - Added receive data flag so can indicate when we loose sync.
 * 30 Jan  2010 - Test Version for Ethernet UDP/IP.
 *
 *  ******** MAKE SURE THE ARP TABLE CONTAINS AN ENTRY FOR THE MAC ON Metis  *********************
 * 18 Mar  2010 - Changed UPD/IP receiver to poll mode and time out after 1 second   
 * 
 * 13 June 2010 - Added listener.ReceiveBufferSize = 0xFFFF;   // no lost frame counts at 192kHz with this setting
 * 29 June 2010 - Removed all USB code to make this an Ethernet only version.
 * 30 June 2010 - Reads PC IP address and use this with assigned port to listen on for Ozy data.
 *  5 July 2010 - Calls ARP_update.bat when starting to force ARP entry for Metis board
 * 20 Aug  2010 - Does Metis Discovery call when selecting Start and checks that board is present
 *              - Removed call to ARP batch file when loading form since Metis now supports ARP natively
 * 10 Nov  2010 - Added Mic gain support for Metis board.
 * 12 Nov  2010 - When doing discovery get Metis IP address from IP header rather than UDP/IP Payload
 *              - Added Metis start/stop sending data command to Start() and Stop()
 * 17 Nov  2010 - Added call to Metis_start_stop() in Form1_FormClosed
 *                Added Metis MAC address display to Setupform Ethernet page.
 *                It gets its value in MetisDiscovery()
 *                Changed all references to OzyII to Metis
 * 26 Nov  2010 - Fixed error in the way that sequence number was being sent to Metis.
 *              - Removed redundant code due to changes to how IP address assigned to Metis.
 * 06 Dec  2010 - V0.0.2 - G Byrkit reorged the code to get rid of static data members and initializers.  Moved init stuff to 'start',
 *                except kept getting local computer name and ethernet ports in Form1_Load, but update ethernet ports in 'Start()'
 * 08 Dec  2010 - Added more Console debug messages during Metis Discovery
 * 11 Dec  2010 - V0.0.3 - G Byrkit: switched to IPAddress.Any for the discovery listen and listener listen bind addresses.
 *                Seems to find Metis on either of my two NICs, regardless of which NIC is used.  Both APIPA and DHCP addressing tested.
 * 12 Dec  2010 - V0.0.4 - P Harman: Reset sequence_number in Start() to eliminate error message when restarting program.
 *              - fixed typo in MAC address from Metis to Console.
 * 14 Dec  2010 - V0.0.5 - G Byrkit: efficiency improvements.  Replace all the uses of .Checked with bools that are set when the
 *                  Checked_changed event fires.  Much more efficient than referencing control properties like .Checked.
 *                  Also eliminated many buffer allocations and array copy operations within Data_send.
 *                  Other changes to ComputeDisplay to remove array allocations each time in and to remove Point allocations each
 *                  time thru the function.
 * 20 Dec  2010 - V0.0.6 - G Byrkit: efficiency improvements.  make sure sequence_number++ is done as 'unchecked' to prevent overflow
 *                exception.  Use pntr++ rather than pntr + n or pntr + x + n in Data_send.  Fix error that Ozy_thread_started wasn't
 *                being set to false when thread was terminated.  Resulted in trying to kill thread extra times (when program exited)
 *                and trying to close sockets multiple times.
 * 21 Dec  2010 - V0.0.7 - G Byrkit: efficiency improvements.
 *                1) provide int enum replacements for BandText and ModeText, so that the switch statements don't have to compare strings
 *                2) hoist TXGain and MicOutGain out of both NESTED for() loops they were in.  calculation could be done once before the
 *                loop and not inside the inner loop for each data item!
 *  *                
 *  9 Jan  2011 - V0.0.8 - P.Harman: Added 'pump prime' send to Metis at Start to select clock source. Metis now responds to the Port
 *                address that the PC sends to. Discovery uses the same Port to receive and reply on.  Port is hard coded at 1024
 *                for now. Need to determine a free port and use that in future.  Removed all code relating to Discovery socket.
 *                
 * 15 Jan  2011 - V0.0.9  - P.Harman: Now uses just one .NET socket. Code automatically allocates a free port for Metis to reply to.
 * 16 Jan  2011 - V0.0.10 - P.Harman: Metis now always listens on port 1024, changed code to suite. 
 * 29 Jan  2011 - V0.0.11 - G Byrkit, K9TRV: Add all processing as in KISS USB.  Set Copyright date to modern, rationalize USB and Ethernet
 *                  versions.  PLEASE TEST CAREFULLY AND COMPLETELY!  (support for Hermes, PennyLane, advanced audio processing, etc.)
 *                  Also set x86 as default build
 * 30 Jan  2011 - V0.0.12 - P.Harman: remove 'Test' slider
 * 30 Jan  2011 - V0.0.13 - G Byrkit, K9TRV: fix some 'gotchas' that Phil found: Setup: checking PennyLane doesn't allow ExtCtrl stuff to
 *                  be selected.  On main form: mic gain and processor sliders need to be 'touched': code to give initial values missing
 *                  in Form1() (constructor), added.
 * 31 Jan  2011 - V0.0.14 - P.Harman, moved Tx Drive Level out of nested for() loop.
 * 31 Jan  2011 - V0.0.15 - G Byrkit: fixed Setup so that PennyLane selection doesn't leave transmit controls greyed out.  Moved freq
 *                              text box up off the spectrum, just like KISS USB.
 *  2 Feb  2011 - V0.0.16 - P.Harman, enable selection of Mic boost when using PennyLane. Added constructor for Processor Gain.
 *                          Renew clipper level when selecting. Added label to VOX level control. Only display Clip LED when PTTEnable true.
 *                          Add delay to Noise Gate. 
 *  4 Feb  2011 - V0.0.17 - Added Line In selection to Setup.cs. Included in I2C and To_Ozy(). Fixed bug where not able to select Penny10MHz clock.
 *  5 Feb  2011 - V0.0.18 - P.Harman - Altered sequence of testing for mic or line-in so that mic gain changes correctly.  Fixed bug that entered . in frequency
 *                                      from keypad.
 *  5 Feb  2011 - V1.0.0  - First release.                                    
 *  6 Feb  2011 - V1.0.1  - Fix 2 problems: SMeter on Vista and Win7, if font is NOT normal size, changes to 'too big' and causes display
 *                          errors in the text.  Also, the Freq display was set to 'RightToLeft', which causes eg 18.245000 to display as
 *                          '000 18.245', which is wrong.  Changed to LeftToRight, and set text to be right-justified.
 * 14 Feb  2011 - V1.0.2  - Added PTT Delay
 *  3 Mar  2011 - V1.0.3  - P.Harman - Altered sequence of testing for mic or line-in so that mic gain changes correctly.
 *  9 Apr  2011 - V1.0.4  - P.Harman - Added support for wide band Bandscope from Metis_V1.2 code
 *  3 Jun  2011 - V1.0.5  - G.Byrkit - made multi-metis detection and selection work.  Save last Metis/Hermes MAC in KK.csv file for
 *                          next run.  Change how detection works: rather than IPAddress.Any, it attempts discovery on each ethernet port
 *                          one at a time.  This works on G.Byrkit's system, wher IPAddress.Any method has distinct flaws: won't discover
 *                          on any port but the first, but USED to work in the past.  Either a driver or windows issue, so a method that
 *                          works reliably all the time is better.  Lays foundation for detecting Metis vs Hermes and choosing accordingly.
 *                          Also remove vestiges of 'choosing an ethernet adapter' in setup.
 * 30 Jul  2011           - Added Mic AGC on/off control. Added squarelaw to Mic, VOX and Noise gate controls.
 *                        - Fixed error in I&Q levels when using PennyLane
 *                        - Added sequence numbers for wide Bandscope data
 *						  - Corrected direction of VOX level control
 *	5 Aug  2011			  - Saved Mic AGC, Noise Gate and Level to KK.CVS
 *	9 Aug  2011 - V1.1.0  - G Byrkit (K9TRV) made first 'unified' version with USB and Ethernet support in one exe.  Split Ethernet code
 *	                        and most variables out of form1.cs.  Ethernet-specific went into EthernetDevice.cs.
 *	                        Generic went into HPSDRDevice.cs.  Made USBDevice.cs from the USB-specific code.  USBDevice.cs and EthernetDevice.cs
 *	                        are derived from HPDSRDevice.cs (class inheritance).
 * 15 Aug  2011 - V1.1.1  - G Byrkit (K9TRV) add 'short height' mode to the display tab of setup form. When not checked, main form is
 *                          800 pixels tall, when checked, main form is 580 pixels tall, to better support laptops and netbooks that
 *                          have 'short' screens (600 pixels high).
 * 17 Aug  2011 - V1.1.2  - G Byrkit (K9TRV) back out changes to support 'short' displays.  What worked on WinXP failed to work on
 *                          Windows 7!  The problem was how the main form sized itself on win7 when using the 125% or 150% larger fonts.
 *                          there was no problem on XP when using 'large fonts', however.  So this effort temporarily shelved.
 *                          But DID add support for 'Alex' and 'Apollo' in KK.CSV and in the MainForm, via boolean variables.  Also,
 *                          in the data loop in USB and Ethernet code added decoding the values of AIN1-AIN6 (forward & reflected (reverse)
 *                          power, supply power volts) for use on Penelope, PennyLane, Apollo, Hermes and Alex.  These values are not yet
 *                          displayed.  Just decoded.
 * 27 Aug  2011 - V1.1.3  - G Byrkit (K9TRV) provisional Alexaires support added (antenna selection by band, atten by band, display of
 *                          forward and reverse power.
 * 28 Aug  2011 - V1.1.4  - G Byrkit (K9TRV) Wideband Spectrum display DID NOT WORK in USB (Ozy/Magister).  Fixed.
 * 28 Aug  2011 - V1.1.5  - G Byrkit (K9TRV) Waterfall display performance improvements from Erik Anderson KE7YOA
 * 01 Oct  2011 - V1.1.6  - G Byrkit (K9TRV) Phil's fixes to Alex incorporated.  Also, moved Process_data to HPSDRDevice.cs, along whith
 *                          the core of Data_send (called Data_send_core), which makes the bulk of the RX decoding and TX encoding all
 *                          common code.  Much easier to maintain!
 * 05 Jan  2012 - V1.1.7  - G Byrkit (K9TRV) Integrate Warren Pratt's (NR0V) AGC code. No 'graphical' AGC adjust yet, but show knee and
 *                          hang threshold on the panadapter (spectrum) display.  Provide to limited audience for review, correction, etc.
 * 14 Jan  2012 - V1.1.8  - G Byrkit (K9TRV) further work on NR0V Warren's AGC code.  Get Knee and Hang lines to display at correct location.
 *                          Fix problem with using LSB or other modes where filter_high is less than filter_low (like is negative as on LSB)
 *                          all of which caused NaN to be computed as log of a negative number is NOT defined.
 * 14 Jan  2012 - V1.1.9  - G Byrkit (K9TRV) AGC Knee and AGC Hang mouse drag implemented.  Sent out for testing.
 * 01 Apr  2012 - V1.1.10 - G Byrkit (K9TRV) fix in HPSDRDevice.cs in Data_send_core so that Penny OC outputs for TX are selected if
 *                          either PTT is pressed or MOX is pressed (MainForm.PTT || MainForm.MOX_set) is true.  Also in Process_data
 *                          make separate version checking and warning messages for Hermes to separate it from Mercury et al, as
 *                          firmware versions are different.
 * 01 May  2012 - V1.1.11 - G Byrkit (K9TRV) changed window type in SharpDSP for the wideband spectrum.  Was 'Hamming', now
 *                          'Blackman-Harris'.  The result is that the peaks are much sharper, according to Phil.  Other issues related
 *                          to Hermes handled in the code.  Most significantly, Phil VK6APH changed the version numbers that Hermes was
 *                          reporting, so that only the equivalent to 'Ozy version' is relevant.  That is, Penelope and Mercury versions
 *                          are NOT relevant.  So some code changed so that inappropriate versions are not reported.
 * 07 July 2012 - V1.1.12 - G Byrkit (K9TRV) bumped version number because of initozy11.bat file changes and addition of other
 *                          Ozy_janus.rbf file (.v18 thru .v22).  .v21 is the default file that is used.
 * 08 July 2012 - V1.1.13 - G Byrkit (K9TRV) Added testing whether Initozy11.bat is present in the current directory.  Provide a warning
 *                          and search for possible mis-named files that could be initozy11.bat.  Also, add a 'SkipVersionCheck' to the
 *                          main form's data, to KK.CSV, and to the first tab of the setup form.  Also, add the new Metis version 18 to
 *                          the Ethernet CheckVersions function.
 * 15 July 2012 - V1.1.14 - G Byrkit (K9TRV) Implement 16k samples for Mercury v33, Metis v19, per VK6APH's (Phil's) request
 * 17 July 2012 - V1.1.15 - G Byrkit (K9TRV) Phil reports the above works!  Now, add a checkbox to Setup to allow attempting to connect
 *                          to the previous Ethernet device (quick connect) and only if that fails do enumeration/discovery of all
 *                          Metis/Hermes devices.  If unchecked, do the discovery always when the radio is started.
 * 18 July 2012 - V1.1.16 - G Byrkit (K9TRV) Ken N9VV requested TX bandwidth on setup increased from 5000 to 8000 max for 'wideband'
 *                          strange stuff...  Also looks like the ProcessorFilter bandwidth isn't being kept in sync with the TransmitFilter
 *                          bandwidth.  checking with Phil VK6APH on this...
 * 10 Aug  2012 - V1.1.17 - G Byrkit (K9TRV) convert to VS2010 and using .Net Framework 4.0.  Add a manifest that runs 'requireAdministrator'
 *                          rather than 'asInvoker'.  Update the USB (Ozy) utils to those built with VS2010 and having manifests that 'requireAdministrator'
 *                          Also have a manifest for InitOzy11.bat (InitOzy11.bat.manifest) that also requests 'requireAdministrator',
 *                          just in case it helps.  This (use of 'requireAdministrator') is intended to help those on Vista, Win7, and the
 *                          future Win8 which is released next week.
 * 24 Aug  2012 - V1.1.18 - G Byrkit (K9TRV) make an installer using WiX.  Also make KK.CSV save into AppDataDir + "\OpenHPSDR\KISS Konsole\".  No
 *                          longer is KK.CSV part of what's downloaded or distributed.  So it won't be overwritten by new versions of KK.
 *  9 Sep  2012 - V1.1.19 - G Byrkit (K9TRV) allow a command line argument of 'appdatadir' (case insensitive) followed by a relative or absolute path
 *                          that will be used as the path to where the KK.CSV file will be stored.  If relative, the path will be under
 *                          AppDataDir + "\OpenHPSDR".  This will allow multiple instances of KK on the same system.  Otherwise, they would all share
 *                          the same KK.CSV file.  Note that the installer will only allow 1 version to be 'installed' at any one time.  but that
 *                          doesn't prevent you from making copies of the executable folder.
 * 11 Oct  2012 - V1.1.20 - G Byrkit (K9TRV) update to account for new firmware versions for Ozy/Metis/Penny/Mercury/Hermes
 * 26 Oct  2012 - v1.1.21 - G Byrkit (K9TRV) Change to .Net Framework 4.0 CLIENT PROFILE so that it runs on more computers without extra software
 *                          Also change installer to reflect this.  Also add new versions of firmware, and note that Metis 1.9, Metis 2.0 and Ozy 2.3
 *                          all have various defects and mostly should NOT be used.  Add Ozy v2.4 to installer and initozy11.bat.  Ozy v2.1 still the default...
 * 31 Oct  2012 - v1.1.22 - G Byrkit (K9TRV) Phil advised me that there is NOT YET a Hermes V1.9.  Must have been one of those Hermes/Metis confusions.
 *                          So change the code and re-release so that KK will work with Hermes 1.8 without complaining.
 * 16 Nov  2012 - v1.1.23 - G Byrkit (K9TRV) Bill Diaz discovered a problem with fast-connect to previous device when the IP addr of the previous device
 *                          had changed (likely due to DHCP handing out a different IP address).  Fixed in EthernetDevice.cs.  Also, previous version
 *                          identified itself as 1.1.21 and not as 1.1.22 as it should have.
 * 15 Dec  2012 - v1.1.24 - G Byrkit (K9TRV) Accommodate Metis 2.2, which fixes an occasional hang in previous versions of Metis.
 * 1  Jan  2013 - v1.1.25 - G Byrkit (K9TRV) Accommodate Metis 2.3.  Change copyright dates to include 2013
 * 20 Jan  2013 - v1.1.26 - G Byrkit (K9TRV) Accommodate Metis 2.4.
 * 27 Jan  2013 - v1.1.27 - G Byrkit (K9TRV) Implement 384k spectrum size for Hermes only for now.  Also remove 'Unified wcpAGC' from the title bar
 *                          version string.
 * 26 May  2013 - v1.1.28 - G Byrkit (K9TRV) Allow 384k spectrum for Metis and Ozy as well.  Add support for Mercury 3.4, Penny 1.8, Metis 2.6, Ozy 2.5.
 *                          Ozy/Magister 2.5 set as default in InitOzy11.bat
 * 09 Sep  2014 - v1.1.29 - Jae Stutzman (K5JAE) - first pass at integrating code that will build and run under Mono on Linux, yet still run under Windows
 * 05 Aug  2017 - v1.1.30 - G Byrkit (K9TRV) If Ethernet attached device, allow writing of full-spectrum data to a data file, for use monitoring during the
 *                          upcoming eclipse on 21 August 2017.
 *                          
 *    
 * 
 * TODO:        - Save IQScale in KK.CSV and set it accordingly at start
 *              - Investigate CPU usage when ANF/NR on at 192kHz sampling rate
 *              - Change USB buffer sizes as a function of sampling rate i.e. 
 *                    WriteBuf size = 1024 bytes for samples/block < 1024, 2048 otherwise
 *                    ReadBuf size = WriteBuf size * (1,2,4) for (48k, 96k, 192k) sample rates
 *              - Check 1Hz steps with mouse scroll is OK with other mice.
 *              - Review CPU usage of ANF and NR values 
 *              - Add  multi band EQ for the receiver (like the 10 band one in PowerSDR)
 *              - User calibration of frequency and amplitude
 *              - Add a second receiver within the current bandwidth
 *              - Fix click tune for CWL - need to allow for LSB filter and CW tone frequency
 *              
 *
 * 
 * 
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using SharpDSP;
using System.Diagnostics;   // use View > Output to see debug messages
using System.Globalization;
using System.Net;
using System.Net.NetworkInformation;  // so we don't have country issues with '.' and ',' in KK.csv files

namespace KISS_Konsole
{
    public partial class Form1 : Form
    {
        // put the version string early so that it can be found easily...
        string version = "V1.1.30";  // change this for each release!

        // create a delegate for the text display since may be called from another thread
        public string Ozy_version = null;  // holds version of Ozy code loaded into FX2 or Metis

        public string numberOfIPAdapters;
        public string Network_interfaces = null;  // holds a list with the description of each Network Adapter
        public int adapterSelected = 1;           // from Setup form, the number of the Network Adapter to use

        public const int rbufSize = 2048;                  // A buffer of 2048 bytes holds 1024 16-bit samples
        public const int iqsize = rbufSize / 2;
        public const int outbufferSize = iqsize;

        // no longer a const int, which can be used at compile time, but a static int, which can be
        // used at runtime, but not thru the object reference, only thru the classname reference.
        public static int EP4BufSize = 8192;                // EP4 data is 4096 16-bit samples

        public byte[] frequency = new byte[4];             // hold Penny and Mercury frequency when in simplex mode and Penny when in Duplex   
        public byte[] duplex_frequency = new byte[4];      // holds Mercury frequency when in Duplex mode

        public SharpDSP2._1.DSPState state;         // public so that the Setup form can use it
        public SharpDSP2._1.DSPBuffer SignalBuffer;
        public SharpDSP2._1.Receiver rcvr;          // public so that the Setup form can use it 

        float[] PowerSpectrumData = new float[rbufSize];    // data acquired from rcvr.PowerSpectrumSignal event

        public float[] FullBandwidthPowerSpectrumData = new float[EP4BufSize];

        int step_size; // the frequency step using the mouse scroll wheel
        public bool read_ready; // set when we have read 1024 samples from USB port
        int[] I_sample = new int[iqsize]; // holds the I samples from Ozy
        int[] Q_sample = new int[iqsize]; // holds the Q samples from Ozy

        public OperMode CurrentMode;
        public string ModeText;
        int previous_frequency_value;
        public int SampleRate;
        public int CWPitch = 700;   // used to calculate filter center frequency, mouse tuning, etc.

        // the following contain the last used VFO frequency on each band
        int set_frequency_160, set_frequency_80, set_frequency_40, set_frequency_30, set_frequency_20;
        int set_frequency_17, set_frequency_15, set_frequency_12, set_frequency_10, set_frequency_6, set_frequency_GC;
        // the following hold the last state of the preamp on each band
        bool Preamp_160, Preamp_80, Preamp_40, Preamp_30, Preamp_20, Preamp_17, Preamp_15, Preamp_12, Preamp_10;
        bool Preamp_6, Preamp_GC;

        string BandText; // holds the previous band when we change bands so we can restore the frequency we were on
        public bool PTT = false;           // true when PTT on Atlas is active
        public bool LineIn = false;        // set when line input on Penelope or PennyLane selected

        public int Merc_version;           // Version number of Mercury FPGA code
        public int Penny_version;          // Version number of Penelope FPGA code
        public int Ozy_FPGA_version;       // Version number of Ozy FPGA code

        public bool SkipVersionCheck = false;
        public bool Allow16kWidebandSamples = true;

        public int rate;            // sets bandscope update rate in F.P.S.
        public int Rate
        {
            get { return rate; }
            set
            {
                rate = value;
                if (rate <= 0)
                    timer1.Interval = 2000;
                else
                    timer1.Interval = 1000 / rate;  //timer value from 1000 to 50 i.e. 1 second to 20mS
            }
        }

        private bool showI;          // when set displays I chanel on 'scope
        public bool ShowI
        {
            get { return showI; }
            set { showI = value; SetSplitterBar(); }
        }

        private bool showQ;          // when set displays Q chanel on 'scope

        public bool ShowQ
        {
            get { return showQ; }
            set { showQ = value; SetSplitterBar(); }
        }

        public int IQScale = 6;     // sets default display scale of I and Q signals

        public int GridMax = 0;     // sets maximum value of Bandscope display
        public int GridMin = -160;  // sets minimum value of Bandscope display
        public int GridStep = 20;   // sets distance between grid lines on bandscope 

        public int WaterfallStep = 0; // sets the distance in pixels the line moves per Waterfall scan

        int delta;                  // difference between the current and previous mouse position on the bandscope
        int QuickMemory;            // holds current Quick Memory frequency
        string QuickMemoryBand;     // holds the current Quick Memory band
        public string Your_call;           // holds the users call sign
        string KeypadFrequency = null;     // holds the frequency entered using the keypad
        int keycount = 0;           // holds number of key presses of the  keypad
        bool ShowCross = false;     // when set show cross at current mouse location
        int MousePositionX;         // as it says
        int MousePositionY;         // you guessed it!
        int FilterLeft;             // left hand location of filter on bandscope
        int FilterRight;            // Hmm... now let me see...
        bool FrequencyDrag = true;  // true when we can drag tune with the mouse
        bool BandwidthShift = true; // true when can drag shift the filter
        bool HighFilter = false;    // true when we can drag high filter frequency
        bool LowFilter = false;     // true when we can drag low filter frequency
        public bool WaterfallAGC = false;  // true when Waterfall AGC is selected on the Setup form
        int max_signal = -200;      // maximum value of nPS in the array, used by  bandscope squelch
        int filter_max_signal = -200;  // maximum signal through the current filter, used by the filter, squelch
        int pixels_per_GridStep;     // how many pixels a Grid Step represents in the narrow bandscope 
        public bool IsSynced = false;      // true when we have sync from Ozy
        public bool IsInSequence = true;    // true when we have received frames in order
        public bool ADCOverload = false;     // true when ADC is overloaded
        public bool KK_on = false;           // true when KK is running
        int VOXHang = 0;

        int squelch_y_value = 0;                // Y location of the squelch threshold line 
        Rectangle squelch = new Rectangle();    // the little rectangle at the LH end of the squelch line.
        bool SquelchDrag = false;               // true when you are dragging the squelch level 

        public bool Hermes = false;         // true if Hermes is selected
        public bool PennyLane = false;      // true if PennyLane is selected
        public bool PenneyPresent = false;  // true if a Penelope card is fitted or not
        public bool Alex = false;           // true if Alex is present
        public bool Apollo = false;         // true if Apollo is present
        public bool MicGain20dB = false;    // true if 20dB mic gain is active on Penny or PennyLane
        public bool Penelope10MHz = false;  // true if 10MHz reference comes from Penny or PennyLane
        public bool Atlas10MHz = false;     // true if 10MHz reference comes from Atlas or Excalibur
        public bool Mercury10MHz = false;   // true if 10MHz reference comes from Mercury
        public bool Excalibur = false;      // true if Excalibur card fitted
        public float TxFilterHigh = 3500f;  // Transmitter high filter setting
        public float TxFilterLow = 300f;    // Transmitter low filter setting
        public float DriveGain = 0.50f;     // Determines the drive level to Penelope
        public int Gain160m = 50;           // Determines gain for Penelope on 160m
        public int Gain80m = 50;            // ditto for 80m etc
        public int Gain60m = 50;
        public int Gain40m = 50;
        public int Gain30m = 50;
        public int Gain20m = 50;
        public int Gain17m = 50;
        public int Gain15m = 50;
        public int Gain12m = 50;
        public int Gain10m = 50;
        public int Gain6m  = 50;
        public int BandGain = 50;           // Determines gain for Penelope, set by what ever band selected

        public bool Duplex = false;         // Selects full duplex operation
        public bool OnlyTxOnPTT = true;     // part of full duplex, only Tx when PTT set
        public bool MOX_set = false;               // set when PTT active
        public bool TUN_set = false;               // set when Tune active
        public int TuneLevel = 50;          // sets Tune power as a % of Drive setting
        bool previous_OnlyTxOnPTT;          // hold previous Only Tx on PTT start so we can restore 
        // Penny Open Collector settings 
        public bool PennyOC = false;        // when true enables Penny OC settings
        public int Penny160mTxOC = 0;
        public int Penny160mRxOC = 0;
        public int Penny80mTxOC = 0;
        public int Penny80mRxOC = 0;
        public int Penny60mTxOC = 0;
        public int Penny60mRxOC = 0;
        public int Penny40mTxOC = 0;
        public int Penny40mRxOC = 0;
        public int Penny30mTxOC = 0;
        public int Penny30mRxOC = 0;
        public int Penny20mTxOC = 0;
        public int Penny20mRxOC = 0;
        public int Penny17mTxOC = 0;
        public int Penny17mRxOC = 0;
        public int Penny15mTxOC = 0;
        public int Penny15mRxOC = 0;
        public int Penny12mTxOC = 0;
        public int Penny12mRxOC = 0;
        public int Penny10mTxOC = 0;
        public int Penny10mRxOC = 0;
        public int Penny6mTxOC = 0;
        public int Penny6mRxOC = 0;

        // the default alex state has TX antenna set to '1' and RX antenna set to '1'
        const int DefaultAlexState = 65;

        public int Alex160mState = DefaultAlexState;
        public int Alex80mState = DefaultAlexState;
        public int Alex60mState = DefaultAlexState;
        public int Alex40mState = DefaultAlexState;
        public int Alex30mState = DefaultAlexState;
        public int Alex20mState = DefaultAlexState;
        public int Alex17mState = DefaultAlexState;
        public int Alex15mState = DefaultAlexState;
        public int Alex12mState = DefaultAlexState;
        public int Alex10mState = DefaultAlexState;
        public int Alex6mState = DefaultAlexState;
        public int AlexGCState = DefaultAlexState;

        // amount of attenuation currently selected on Alex or equiv (like on Hermes/Apollo)
        public double AlexAtten = 0.0;

        public int FM_deviation = 2400;    // sets the +/- FM deviation limits

        public string Metis_IP_address = "";             // Start() copies above value here, so it won't change until re-starting.
        public string MetisMAC = "";

        // PTT Sequencer
        public int DelayRF  = 0; // number of mS to delay  RF after PTT activated
        public int DelayPTT = 0; // number of mS to delay release of PTT after RF stops

        public bool PTTEnable = false;

        Point MouseOld = new Point(); // keep location of mouse for drag tuning 

        public SetupForm Setup_form = null;  // get reference to the Setup form.

        NumberFormatInfo nfi = NumberFormatInfo.InvariantInfo;  // so we are region independent in terms of ',' and '.' for floats

        // these are used by the SMeter 
        int SMeterValue = 0;          // this gets passed to the SMeter code 
        Font meterFont = new Font(FontFamily.GenericSansSerif, 7);  // use smaller font for the meter

        Pen YellowPen = Pens.Yellow;
        Pen RedPen = Pens.Red;
        Pen WhitePen = Pens.White;
        Pen ChartreusePen = Pens.Chartreuse;
        Pen dotPen = (Pen)Pens.DarkOrange.Clone();

        public KKMethod KKDevice = KKMethod.Ethernet;

        public bool DoFastEthernetConnect = false;
        public string EthernetHostIPAddress = "";

        public string app_data_path = "";
        public string KKCSVName = "";
        public Form1(string appDataDir)
        {
            //Control.CheckForIllegalCrossThreadCalls = false;  // leave on so we catch these
            InitializeComponent();

            // Focus control...
            DisableFocus(Controls);

            this.labelFocus.MouseWheel += new MouseEventHandler(labelFocus_MouseWheel);
            this.labelFocus.PreviewKeyDown += new PreviewKeyDownEventHandler(labelFocus_PreviewKeyDown);
            this.Shown += new System.EventHandler(FocusGotFocus);

            Debug.Indent();                         // Indent Debug messages to make them easier to see

            // determine where a default appDataDir would be located
            app_data_path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\OpenHPSDR\\KISS Konsole\\";

            // see if the non-default appDataDir is provided, and if so, whether it is valid or can be created.
            if ((appDataDir != null) && (appDataDir.Length > 0))
            {
                bool relativePath = true;
                try
                {
                    relativePath = !Path.IsPathRooted(appDataDir);
                    if (relativePath)
                    {
                        appDataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\OpenHPSDR\\" + appDataDir;
                        appDataDir = Path.GetFullPath(appDataDir) + "\\";
                    }
                    try
                    {
                        if (!Directory.Exists(appDataDir))
                        {
                            Directory.CreateDirectory(appDataDir);
                        }
                        app_data_path = appDataDir;
                    }
                    catch
                    {
                        // failed, use the default value
                    }
                }
                catch
                {
                	// path contains invalid characters.  Ignore it!

                }

            }

            if (!Directory.Exists(app_data_path))
            {
                Directory.CreateDirectory(app_data_path);
            }

            // make the pen for the squelch line on the spectrum display have 'dot' properties
            dotPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

            // draw 
            pictureBox2.Paint += new PaintEventHandler(pictureBox2_Paint); // SMeter event handler
            pictureBox2.Height = 40;
            pictureBox2.Width = 140;

            rcvr = new SharpDSP2._1.Receiver();
            SharpDSP2._1.PowerSpectrumSignal.ps_event += new SharpDSP2._1.PowerSpectrumSignal.PSpectrumEventHandler(PowerSpectrumSignal_ps_event);
            SharpDSP2._1.OutbandPowerSpectrumSignal.ps_event += new SharpDSP2._1.OutbandPowerSpectrumSignal.PSpectrumEventHandler(OutbandPowerSpectrumSignal_ps_event);

            // set the size of the various graphics windows so the scaling the dpi does not cause an error
            splitContainer1.Height = 520;
            splitContainer1.Width = 1024;
            splitContainer1.SplitterDistance = 256;
            pictureBoxSpectrum.Height = 256;
            pictureBoxSpectrum.Width = 1024;
            pictureBoxWideband.Height = 260;
            pictureBoxWideband.Width = 1024;
            trackBarSetFrequency.Width = 1024;

            state = rcvr.DSPStateObj;       // fetch state that Receiver constructor created
            state.DSPBlockSize = iqsize;     // put in some initial values

            // Load the previous radio settings from the KK.csv file and allocate values.
            // First check that the file exists
            KKCSVName = app_data_path + "KK.csv";
            if (!File.Exists(KKCSVName))   //if file doesn't exist, create it
            {
                CreateKKCSV(KKCSVName);
            }

            // now read the lines in the config file.  Warn if action above didn't create one!
            // This allows KK.CSV to be no longer part of the SVN tree.  This means that the KK.CSV
            // file won't get mangled/updated by SVN when someone updates the code!
            if (File.Exists(KKCSVName))   //if file exists open it
            {
                ReadKKCSV(KKCSVName);

                BandSelect.Text = BandText;    // set  band to start on 
                Frequency_change();                 // check we are in band, update frequency and display 
                ChangeMode();                       // Set Bandwidth slider value based on Mode in use
                rcvr.SampleRate = SampleRate;   // set the previously saved sample rate
                QuickMemory = trackBarSetFrequency.Value;  // set a quick frequency value
                QuickMemoryBand = BandSelect.Text;  // set a quick frequency band

                // Force the Form1 Controls to the values retrieved from KK.CSV
                SetVolume(this, EventArgs.Empty);           // force Volume control to accept previous saved value
                AGCTrackBar_Scroll(this, EventArgs.Empty);  // force AGC Threshold to accept previous saved value
                Filter_squelch_CheckedChanged(this, EventArgs.Empty); // force Squelch to accept previous saved value
                Squelch_setting.Text = Squelch_level.Value.ToString();
                DriveLevel_Scroll(this, EventArgs.Empty);   // force Drive contol to accept value from KK.csv
                chkClipper_CheckedChanged(this, EventArgs.Empty); // force Processor control to update
                chkVOX_CheckedChanged(this, EventArgs.Empty); // force control to update
                chkNoiseGate_CheckedChanged(this, EventArgs.Empty); // force control to update

                MicrophoneGain_Scroll(this, EventArgs.Empty);
                NoiseGateLevel_Scroll(this, EventArgs.Empty);

                rcvr.SampleRate = SampleRate;   // set the previously saved sample rate
                AdjustFilterByMode();                  // set the bandwidth since dependent on sample rate

                ModeText = Mode.Text;
                rcvr.LOFrequency = 0.0f;
                //rcvr.BinauralMode = true;             // currently has no effect
                rcvr.PowerSpectrumUpdateRate = 50;      // Set maximum PowerSpectrum update rate
                //rcvr.PowerSpectrumCorrection = 1f;
                rcvr.WindowType = SharpDSP2._1.WindowType_e.BLACKMAN3_WINDOW;
                // set up the squelch control 
                Squelch_level.Minimum = GridMin * 10; Squelch_level.Maximum = GridMax * 10;
                Squelch_setting.Text = (Squelch_level.Value / 10).ToString();

                previous_OnlyTxOnPTT = OnlyTxOnPTT;

                SignalBuffer = new SharpDSP2._1.DSPBuffer(state);
            } 
            else
            {
                MessageBox.Show("Can't find KK.csv, should be in the same directory as KISS Konsole.exe?", "File Error");
            }
        }

        private void DisableFocus(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control.HasChildren)
                {
                    // recursive
                    DisableFocus(control.Controls);
                }


                if (control.Name != "labelFocus" && control.GetType() != typeof(MenuStrip) &&
                    control.GetType() != typeof(Label) && control.GetType() != typeof(TextBox) &&
                    control.GetType() != typeof(ComboBox))
                {
                    control.MouseClick += new MouseEventHandler(FocusMouseClick);
                }

                if (control.GetType() == typeof(ComboBox))
                {
                    ((ComboBox)control).SelectedIndexChanged += new EventHandler(FocusGotFocus);
                }

            }
        }

        void FocusMouseClick(object sender, MouseEventArgs e)
        {
            labelFocus.Focus();
        }

        void FocusGotFocus(object sender, EventArgs e)
        {
            labelFocus.Focus();
        }

        void labelFocus_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    
                    //System.Console.WriteLine("SPACE Pressed");

                    // toggle transmit
                    MOX_Click(null, new EventArgs());

                    //e.Handled = true;
                    break;
            }
        }

        void labelFocus_MouseWheel(object sender, MouseEventArgs e)
        {
            // Manually update the TrackBar using the MouseWheel event. This event is part of the Focus control
            // label specifically placed to keep focus. NOTE: Mono's implementation of the TrackBar has inverted
            // TrackBar logic, this is a bug!
            if (e.Delta > 0)
            {
                //System.Console.WriteLine("MOUSE WHEEL Up");
                if (trackBarSetFrequency.Value < trackBarSetFrequency.Maximum)
                    trackBarSetFrequency.Value ++;
            }
            else
            {
                //System.Console.WriteLine("MOUSE WHEEL Down");
                if (trackBarSetFrequency.Value > trackBarSetFrequency.Minimum)
                    trackBarSetFrequency.Value --;
            }

            trackBarSetFrequency_Scroll(null, new EventArgs());
        }

        // in-band data (EP6)
        void PowerSpectrumSignal_ps_event(object source, SharpDSP2._1.PowerSpectrumSignal.PSpectrumEvent e)
        {
#if false
            Array.Copy(e.buffer, PowerSpectrumData, rbufSize);
#else
            // G Byrkit, K9TRV: just assign the buffer.  Save the copy time
            PowerSpectrumData = e.buffer;
#endif
        }

        // wideband (full spectrum) data (EP4)
        void OutbandPowerSpectrumSignal_ps_event(object source, SharpDSP2._1.OutbandPowerSpectrumSignal.PSpectrumEvent e)
        {
#if false
            Array.Copy(e.buffer, FullBandwidthPowerSpectrumData, EP4BufSize);
#else
            // G Byrkit, K9TRV: just assign the buffer.  Save the copy time
            FullBandwidthPowerSpectrumData = e.buffer;
#endif
        }

        public void UpdateFormTitle()
        {
            this.Text = "HPSDR K.I.S.S. Konsole - " + version + " - " + Your_call;  // text on top of form 
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateFormTitle();

            this.StartPosition = FormStartPosition.CenterScreen;  // TODO save last position and restore from KK.csv

            this.Height = 800;
        }
       
        private void GetIQSamples()
        {
            float scaleIQDisplay = 20 * (float)Math.Pow(2, 15); // scale factor to convert DSP output to 16-bit short int audio output sample

            // read I & Q values from the buffer and plot the sample on the scope
            for (int i = 0; i < iqsize; i++)
            {
                I_sample[i] = (int)(scaleIQDisplay * SignalBuffer.cpx[i].real);
                Q_sample[i] = (int)(scaleIQDisplay * SignalBuffer.cpx[i].imaginary);
            }
        }

        int waterfall_low_threshold = -120;
        public int WaterfallLowThreshold
        {
            get { return waterfall_low_threshold; }
            set { waterfall_low_threshold = value; }
        }

        int waterfall_high_threshold = -40;
        public int WaterfallHighThreshold
        {
            get { return waterfall_high_threshold; }
            set { waterfall_high_threshold = value; }
        }

        Point[] IDraw;
        Point[] QDraw;
        Point[] bandDraw;
        Point[] wideBandDraw;

        // computed during one ComputeDisplay block, and used in the next.
        // May be set to WaterfallLowThreshold if not doing waterfall AGC.
        // This value, and not waterfall_low_threshold, is used in the calculations in ComputeDisplay
        int waterfallPreviousMinValue = 0;

        private void ComputeDisplay()
        {
            int xmax = pictureBoxSpectrum.Width; // max number of samples across the screen

            int BSHeight = pictureBoxSpectrum.Height;
            int WBHeight = pictureBoxWideband.Height;
            double lines = (float)(GridMax - GridMin) / (float)GridStep;
            lines = Math.Ceiling((double)lines); // if lines is a fraction round up to next integer 

            int ymax = BSHeight; // maximum y value

            int yscale = IQScale;  // held on Setup form, sets X axis gain when displaying I and Q signals

            float GainOffset = (PreampOn) ? -20.0f : 0.0f;  // if the preamp is on then remove 20dB from the signal level
            if (Alex)
            {
                GainOffset += (float)AlexAtten;
            }
            
            int xWidth = Math.Min(xmax, pictureBoxSpectrum.Width);
            int waterfall_minimum = 0;  // reset waterfall_minimum value in nPS array 
            // set to just above black for now for testing but make a user setting and save in KK.csv
            int waterfall_low_color_R = 1; int waterfall_low_color_G = 1; int waterfall_low_color_B = 1;
            int R = 0, G = 0, B = 0;
            float range;
            float off_set;
            float overall_percent; // value from 0.0 to 1.0 where 1.0 is high and 0.0 is low.
            float local_percent;

            max_signal = -200;          // reset maximum value of nPS in the array

            if ((IDraw == null) || (IDraw.Length != xWidth))
            {
                // only create these arrays (and populate them!) if not created, or if xWidth changes...
                IDraw = new Point[xWidth];
                QDraw = new Point[xWidth];
                bandDraw = new Point[xWidth];
                wideBandDraw = new Point[xWidth];

                // populate the arrays.  Only do this when the size (xWidth) changes, to minimize memory allocation
                for (int i = 0; i < xWidth; ++i)
                {
                    IDraw[i] = new Point();
                    QDraw[i] = new Point();
                    bandDraw[i] = new Point();
                    wideBandDraw[i] = new Point();
                }
            }
            
            //int xpos = 0;

            // advance to the next waterfall line and count it
            if (doWaterFall)
            {
                NextWaterfallLine();

                if (!WaterfallAGC)  // if selected then set the low threshold automatically
                    waterfallPreviousMinValue = WaterfallLowThreshold;
            }

            byte[] rawData = new byte[xWidth * 4];
            int bytePos = 0;
            int logGain = 85;  // see below as to why this value TODO: allow user to calibrate this value

            // these next 2 values, are only valid when xWidth is 1024.  That is, the width of the display
            // area is 1024 pixels.
            // this interval used to be hard-coded at 4 to correspond to an EP4BufSize of 8192.
            // if EP4BufSize = 32768, then the interval is 16, not 4.
            // the wideBand display shows the max value in a given interval, of width wideBandSpectrumInterval
            int wideBandSpectrumInterval = EP4BufSize / 2048;

            // wideBandSampleStart is 4096 when EP4BufSize is 8192
            // used to be 4095, but Phil VK6APH confirmed that was in error (a 'feature'!)
            // so if there are 8192 samples, the first half are numbered 0 thru 4095.
            // the second half are numbered (offset) 4096 thru 8191.
            int wideBandSampleStart = EP4BufSize/2;

            for (int xpos = 0; xpos < xWidth; xpos++)
            {
                float Sample;
                float SampleMax = 0.0f;
                int nI = I_sample[xpos];
                int nQ = Q_sample[xpos];

                #region How scaling the bandscope works
                /*
                 * We need to scale the bandscope display depending on the max,min and step values
                 * the uses has selected on the Setup Form.
                 * We know the number of pixels that each grid line represents since this is 
                 * calculated in DrawGrid() as vertical_grid_spacing.
                 * 
                 * Hence we can calculate the number of pixels/dB of vertical resolution from
                 * 
                 *  pixels_per_dB = vertical_grid_spacing/GridStep.
                 *  
                 * We know the signal level at the top of the bandscope since this is held in GridMax.
                 * 
                 * We need to know what each sample represents in terms of dBs.
                 * To do this we feed a known signal into Mercury with the preamp off. 
                 * 
                 * With this signal as input we  determine the maximum signal in the array and note the value.
                 * The signal processing to this point has been:
                 * 
                 *       Po =  (FFT(Pin) * G)  
                 * 
                 * where G = the overall gain from the antenna socket to here
                 *     Pin = the power level at the antenns socket in dBm and 
                 *     Po  = the  level we wish  to display on the bandscope
                 *     FFT = the fast fourier transform of the input signal 
                 *     
                 * We want the bandscope to the scaled in dBm so we take log and multiply by 10 i.e.
                 * 
                 *     10 log Po = 10log (FFT(Pin) * G)
                 *  
                 *  or 10 log Po = 10log (FFT(Pin) + 10Log G 
                 * 
                 * Since the relations between signal input and (FFT(Pin) is linear 
                 * if we know Pin and measure Po we can calculate 10log G.
                 * 
                 * For example, with my Mercury board, 0dBm gives a peak value of 85 (This value will be calibrated in future releases)
                 *
                 *  hence  85 = 0 + 10log G 
                 * 
                 * We can now calculate the dB value of every sample.  We need to map these to the screen.
                 * 
                 * For example, if we assume the top of the screen is 0dBm then we can calculate each samples
                 * pixel position as follows 
                 * 
                 *  yintPS = -(nPS - 85)* pixels_per_db  or -(nPS  - 85) * vertical_grid_spacing/GridStep
                 *  
                 * For values of GridMax other than 0dBm we need need to subtract this value, hence
                 * 
                 *  yintPS = -(nPS - 85 - GridMax) * vertical_grid_spacing / GridStep;
                 *  
                 * We do exactly the same calculations for the wide bandscope - in this case 10log G = 56
                 * 
                 */
                #endregion

                // TODO: we have 2048 samples in the FFT we could average these to 1024 pixels perhaps
                int nPS = (int)(PowerSpectrumData[2 * xpos] + GainOffset);

                // Display FullBandwidthPowerSpectrumData on the same grid.
                // FullBandwidthPowerSpectrumData are a block of 4096 contiguous undecimated samples taken at 122.88 Msps.
                // PowerSpectrumData comes from the decimated samples.
                // Decimation results in processing gain, so, while the two spectra
                // have the same scale, when plotting, their offsets differ.
                // Since we have more data points than horizontal pixels in our graph,
                // search for & display the largest signal in each group of four.

                SampleMax = -999.0f;
                for (int k = 0; k < wideBandSpectrumInterval; ++k)
                {
                    // we have 2 x 4095 samples since we use a real signal so start half way
                    // when EP4BufSize is 8192, I think that the first half of the samples are numbered 0 thru 4095,
                    // and the second half from 4096 thru 8191.
                    Sample = FullBandwidthPowerSpectrumData[wideBandSampleStart + (wideBandSpectrumInterval * xpos) + k];
                    if (Sample > SampleMax) SampleMax = Sample;
                }
                int nFBPS = (int)(SampleMax + GainOffset);

                int yintI = (int)ymax / 2 + (int)(nI >> yscale);
                int yintQ = (int)ymax / 2 + (int)(nQ >> yscale);

                int yintPS = -(nPS - logGain - GridMax) * (int)(BSHeight / lines) / GridStep;  // see explanation above

                // if the both bandscopes are in use with need to move this one down the screen by BSHeight
                int offset = 0;
                int yintFBPS = offset - (nFBPS - 56 - GridMax) * (int)(WBHeight / lines) / GridStep; // see explanation above

                // Draw the sample point by adding a point to the respective point lists
                // when we've exited the for loop, use DrawCurve for each that we are drawing
                if (ShowI)
                {
                    IDraw[xpos].X = xpos;
                    IDraw[xpos].Y = yintI;
                }
                if (ShowQ)
                {
                    QDraw[xpos].X = xpos;
                    QDraw[xpos].Y = yintQ;
                }
                if (doSpectrum)
                {
                    bandDraw[xpos].X = xpos;
                    bandDraw[xpos].Y = yintPS;
                }
                if (doWideBand)
                {
                    wideBandDraw[xpos].X = xpos;
                    wideBandDraw[xpos].Y = yintFBPS;
                }

                // this should be moved to one-time processing.  It's not needed to be done for each block of data being displayed.
                if (doWaterFall)
                {              
                    // This code is based on the waterfall in PowerSDR 

                    // from the above explanation we know the dBm value of each sample is  nPS - logGain
                    int waterfall_data = nPS - logGain;

                    // select a colour for each pixel depending on its dBm value
                    if (waterfall_data <= waterfallPreviousMinValue)
                    {
                        R = waterfall_low_color_R;
                        G = waterfall_low_color_G;
                        B = waterfall_low_color_B;
                    }
                    else if (waterfall_data >= waterfall_high_threshold)
                    {
                        R = 192;  // looks like a mauve/purple sort of colour 
                        G = 124;
                        B = 255;
                    }
                    else // value is between low and high  
                    {
                        range = waterfall_high_threshold - waterfallPreviousMinValue;
                        off_set = waterfall_data - waterfallPreviousMinValue;
                        overall_percent = off_set / range; // value from 0.0 to 1.0 where 1.0 is high and 0.0 is low.

                        if (overall_percent < (float)2 / 9) // background to blue
                        {
                            local_percent = overall_percent / ((float)2 / 9);
                            R = (int)((1.0 - local_percent) * waterfall_low_color_R);
                            G = (int)((1.0 - local_percent) * waterfall_low_color_G);
                            B = (int)(waterfall_low_color_B + local_percent * (255 - waterfall_low_color_B));
                        }
                        else if (overall_percent < (float)3 / 9) // blue to blue-green
                        {
                            local_percent = (overall_percent - (float)2 / 9) / ((float)1 / 9);
                            R = 0;
                            G = (int)(local_percent * 255);
                            B = 255;
                        }
                        else if (overall_percent < (float)4 / 9) // blue-green to green
                        {
                            local_percent = (overall_percent - (float)3 / 9) / ((float)1 / 9);
                            R = 0;
                            G = 255;
                            B = (int)((1.0 - local_percent) * 255);
                        }
                        else if (overall_percent < (float)5 / 9) // green to red-green
                        {
                            local_percent = (overall_percent - (float)4 / 9) / ((float)1 / 9);
                            R = (int)(local_percent * 255);
                            G = 255;
                            B = 0;
                        }
                        else if (overall_percent < (float)7 / 9) // red-green to red
                        {
                            local_percent = (overall_percent - (float)5 / 9) / ((float)2 / 9);
                            R = 255;
                            G = (int)((1.0 - local_percent) * 255);
                            B = 0;
                        }
                        else if (overall_percent < (float)8 / 9) // red to red-blue
                        {
                            local_percent = (overall_percent - (float)7 / 9) / ((float)1 / 9);
                            R = 255;
                            G = 0;
                            B = (int)(local_percent * 255);
                        }
                        else // red-blue to purple end
                        {
                            local_percent = (overall_percent - (float)8 / 9) / ((float)1 / 9);
                            R = (int)((0.75 + 0.25 * (1.0 - local_percent)) * 255);
                            G = (int)(local_percent * 255 * 0.5);
                            B = 255;
                        }
                    }

                    // Find the lowest signal level in the block of 1024 samples. If the automatic check box
                    // is selected then use this value for waterfall_low_threshold next time through.
                    if (waterfall_minimum > waterfall_data)
                         waterfall_minimum = waterfall_data;

                    rawData[bytePos++] = (byte)B;
                    rawData[bytePos++] = (byte)G;
                    rawData[bytePos++] = (byte)R;
                    rawData[bytePos++] = 255;
                } // end if doing waterfall display

                // Find the maximum signal level in the block of 1024 samples for use in the wideband squelch
                if (bandscopeSquelch &&  xpos > 0 && max_signal < (nPS - logGain)) //TODO: sample 0 is always 85 - why?
                    max_signal = (nPS - logGain);

            } // end for each x data point

            // copy the rawData buffer to the Bitmap, the fast way.  This uses Bitmap.LockBits and Marshal.Copy.
            Rectangle drawRect = new Rectangle(0, currentWaterfallLine, xWidth, 1);
            System.Drawing.Imaging.BitmapData rawBmpData = waterfallBitmap.LockBits(drawRect,
                System.Drawing.Imaging.ImageLockMode.WriteOnly, waterfallBitmap.PixelFormat);
            System.Runtime.InteropServices.Marshal.Copy(rawData, 0, rawBmpData.Scan0, 4 * xWidth);
            waterfallBitmap.UnlockBits(rawBmpData);

            // Now store the waterfall_minumum value, so that it is available for the next packet to use
            // Provide  some 'history' (smoothing) in the WaterfallAGC i.e. 80% of the previous value and 20% of the current
            waterfallPreviousMinValue = ((waterfallPreviousMinValue * 8) + (waterfall_minimum * 2)) / 10;
        }

        private int currentWaterfallLine = 0;
        private int numWaterfallLines = 0;

        private void ClearWaterfall()
        {
            currentWaterfallLine = 0;
            numWaterfallLines = 0;
        }

        Int32[] blackness = null;

        private void NextWaterfallLine()
        {
            if (--currentWaterfallLine < 0)
            {
                currentWaterfallLine = waterfallBitmap.Height - 1;
            }

            if (numWaterfallLines < waterfallBitmap.Height)
            {
                ++numWaterfallLines;
            }

            if (numWaterfallLines == 1)
            {
#if true
                // about to make first use!  set all to black!
                int width = waterfallBitmap.Width;
                int height = waterfallBitmap.Height;
                int length = width * height;

                if (blackness == null)
                {
                    // first create a row describing "black"
                    blackness = new Int32[length];
                    int idx;
                    for (idx = 0; idx < length; idx++)
                    {
                        blackness[idx] = unchecked((Int32)0xFF000000); // R=0, G=0, B=0, A=255
                    }
                }

                // now gain raw access to the bitmap
                Rectangle drawRect = new Rectangle(0, 0, width, height);
                System.Drawing.Imaging.BitmapData rawBmpData = waterfallBitmap.LockBits(drawRect,
                    System.Drawing.Imaging.ImageLockMode.WriteOnly, waterfallBitmap.PixelFormat);

                System.Runtime.InteropServices.Marshal.Copy(blackness, 0, rawBmpData.Scan0, length);

                // we're done, no more need for raw access
                waterfallBitmap.UnlockBits(rawBmpData);
#else
                waterfallBitmap = new Bitmap(1024, 256, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
#endif
            }
        }

        private bool agcHangEnabled = false;
        public bool AGCHangEnabled
        {
            get { return agcHangEnabled; }
            set {
                agcHangEnabled = value;
                rcvr.AGCHangEnable = value;
                if (SetupFormValid())
                {
                    Setup_form.AGCHangEnabled = value;
                }
            }
        }

        private Rectangle AGCHang = new Rectangle();
        private bool AGCHangDrag = false;
        private int AGCHang_y_value = 0;

        private Rectangle AGCKnee = new Rectangle();
        private bool AGCKneeDrag = false;
        private int AGCKnee_y_value = 0;

        private static SolidBrush pana_text_brush = new SolidBrush(Color.Khaki);
        private static Font pana_font = new Font("Tahoma", 7F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));

        private int dBToPixel(double dB)
        {
            return (int)((GridMax - dB) * pixels_per_GridStep / GridStep);
        }

        private double PixelToDb(int y)
        {
            double dB = (double)y * GridStep / pixels_per_GridStep;

            return GridMax - dB;
        }

        float cal_offset = 0.0f;

        private void DrawSpectrumGrid(Graphics g)
        {
            #region How DrawGrid works
            /* 
             * We want to display a grid spaced 5/10/20kHz apart horizontally depending on the sampling rate.
             * We also want to display a vertical grid spaced zdB apart. We also want to indicate the 
             * current filter width on the display.
             * 
             * The screen is 1024 pixels wide so filter width =  bandwidth / sampling_rate
             * 
             * To display the grid, first calculate the vertical grid spacing. We want to display from +/-XdB to -YdBm in ZdB steps 
             * which means we need 160/20 = 8 horizontal lines.
             * 
             * We know the frequency at the left hand edge of the bandscope since it will be the tuned frequency less 
             * (sampling rate/2).  This will be displayed at the top of the first vertical grid line. We want the vertical 
             * grid to have spacings of 5/10/20kHz depending on the sampling rate. We also want the frequency markings to
             * be in these increments. So we need to find the first location from the left hand side of the screen that we
             * can divide by 5/10/20 and get no remainder.  We use the % function to do this. 
             * 
             * Since the minimium grid size is 5kHz we are going to have to test in finer increments than this such that
             * frequency/5 = 0 remainder.  So we use 100Hz increments - this gives a nice smooth display when we drag it.
             * 
             * Once we know the first frequency from the left hand edge of the bandscope we can convert this to pixels. 
             * We know the bandscope is 1024 pixels wide and the frequency width = sampling rate. So we can calculate the x 
             * location of the first vertical bar from  
             * 
             *  pixel_indent = Hz offset from LHS * width of bandscope/sample rate
             *  
             * We can now format the frequency text and display the lines. 
             * 
             * 
             */

            #endregion

            // get the height of our panel
            int BSHeight = pictureBoxSpectrum.Height;

            // calculate the number of horizontal lines and the distance between them
            double lines = (float)(GridMax - GridMin) / (float)GridStep;
            lines = Math.Ceiling((double)lines); // if lines is a fraction round up to next integer 
            int x_locate = 0;
            string displayFreq;
            int display_frequency = 0;
            int offset = 15; // how many pixels from the top to start drawing the vertical lines

            // determine vertical grid spacing
            int vertical_grid_spacing = (int)(BSHeight / lines);
            pixels_per_GridStep = vertical_grid_spacing;

            // determine horizontal grid spacing
            int grid_spacing = SampleRate / 960;  // gives 50/100/200 x 100Hz
            int pixel_spacing = pictureBoxSpectrum.Width * grid_spacing * 100 / SampleRate;  // how many pixels this spacing represents
            float pixels_per_100Hz = pictureBoxSpectrum.Width * 100.0f / SampleRate; // how many pixels per 100Hz across the screen 

            // Draw the background color
            g.FillRectangle(Brushes.DarkGreen, pictureBoxSpectrum.ClientRectangle);

            Font font = new Font(FontFamily.GenericMonospace, 12);

            if (doSpectrum) // if the narrow banscope is selected display it
            {
                // Show the  filter bandwidth on the screen 
                // The screen is 1024 pixels wide so filter width =  bandwidth / sampling_rate
                FilterLeft = 512 + (int)(1024.0 * Math.Min(rcvr.FilterFrequencyLow, rcvr.FilterFrequencyHigh) / SampleRate);
                FilterRight = 512 + (int)(1024.0 * Math.Max(rcvr.FilterFrequencyLow, rcvr.FilterFrequencyHigh) / SampleRate);
                int Width = FilterRight - FilterLeft;

                // Draw the filter bandwidth in a different colour
                g.FillRectangle(Brushes.SeaGreen, FilterLeft, 0, Width, BSHeight);

                // truncate tuned frequency to 100Hz digits 
                int truncate_freq = trackBarSetFrequency.Value / 100;

                // the frequency at the left edge of the screen will be the tuned frequency - sample rate/(2 x 100Hz)
                int left_edge_frequency = truncate_freq - SampleRate / 200;

                // we now step in from the left edge frequency until we have a frequency that divides by 
                // the grid spacing with no remainder. This will be the first frequency we display
                for (display_frequency = left_edge_frequency; display_frequency < truncate_freq - 12; display_frequency++)
                {
                    if ((display_frequency % grid_spacing) == 0)
                        break;
                }

                // how many pixels is this from the left edge of the screen
                float indent_pixels = (display_frequency - left_edge_frequency) * pixels_per_100Hz;

                // draw the vertical grid lines with their associated frequency
                for (float i = indent_pixels; i < pictureBoxSpectrum.Width; i += pixels_per_100Hz)
                {
                    if (display_frequency % grid_spacing == 0)  // are we on a grid boundry
                    {
                        // draw the vertical lines
                        g.DrawLine(Pens.DarkGray, i, offset, i, BSHeight); // offset stops the line going through the text

                        displayFreq = String.Format("{0:0.000}", (float)display_frequency / 10000);
                        if (display_frequency < 100000)  // so the decimal point in the frequency aligns with the grid line - perfectionist!
                            x_locate = (int)i - 17;
                        else
                            x_locate = (int)i - 26;
                        // draw the text above each line
                        g.DrawString(displayFreq, font, Brushes.White, x_locate, 0);
                    }
                    // highlight band edge in Red
                    if (display_frequency == trackBarSetFrequency.Minimum / 100 || display_frequency == trackBarSetFrequency.Maximum / 100)
                        g.DrawLine(Pens.Red, i, offset, i, BSHeight);

                    display_frequency++; // increment frequency in 100s of Hz 
                }

                // draw a single vertical line in the middle of the screen
                g.DrawLine(Pens.Yellow, pictureBoxSpectrum.Width / 2, offset, pictureBoxSpectrum.Width / 2, BSHeight);

                // Draw the horizontal lines and signal calibration 
                // First draw the horizontal lines so they don't cover the calibration text 
                // Only draw them for the narrow bandscope
                for (int k = 0; k < BSHeight; k += vertical_grid_spacing)
                {
                    g.DrawLine(Pens.DarkGray, 0, k, pictureBoxSpectrum.Width, k);
                }

                // The dBm value for the top of the screen is held in Grid.Max, we don't indicate this but we do the next line
                // This will be Grid.Max - Grid.Step
                int textSpacing = vertical_grid_spacing;
                for (int x = GridMax - GridStep; x > GridMin; x -= GridStep)
                {
                    string level = x.ToString() + "dBm";
                    g.DrawString(level, font, Brushes.White, 0, textSpacing - 9);
                    textSpacing += vertical_grid_spacing;
                }

                // now do the AGC stuff
                AGCKnee_y_value = 0;
                AGCHang_y_value = 0;
                string agc = "";
                double hang = AGCHangLevel;
                double thresh = AGCThreshDB;
                int agc_fixed_gain = (int)AGCFixedGainDB;
                int x1 = 40;
                int x2 = pictureBoxSpectrum.Width - 40;
                int x3 = 50;
                float GainOffset = (PreampOn) ? 0.0f : 20.0f;  // if the preamp is on then remove 20dB from the signal level
                if (Alex)
                {
                    GainOffset += (float)AlexAtten;
                }

                if (rcvr.AGCMode == SharpDSP2._1.AGCType_e.agcOff)
                {
                    cal_offset = 20.0f;
                    AGCKnee_y_value = dBToPixel(-(float)agc_fixed_gain);
                    // Debug.WriteLine("agcknee_y_D:" + agcknee_y_value);
                    agc = "-F";
                }
                else
                {
                    // Warren NR0V reports that this offset change is necessary
                    cal_offset = -63.0f + GainOffset;

                    // do AGC Hang stuff, but only if AGC is on
                    AGCKnee_y_value = dBToPixel((float)thresh + cal_offset);
                    if (agcHangEnabled)
                    {
                        AGCHang_y_value = dBToPixel((float)hang + cal_offset);
                        if (AGCHang_y_value < 0)
                            AGCHang_y_value = 0;
                        else if (AGCHang_y_value >= pictureBoxSpectrum.Height)
                            AGCHang_y_value = pictureBoxSpectrum.Height - 1;

                        AGCHang.Height = 10; AGCHang.Width = 10; AGCHang.X = 50;
                        AGCHang.Y = AGCHang_y_value - (AGCHang.Height / 2);
                        g.FillRectangle(Brushes.Yellow, AGCHang);

                        using (Pen p = new Pen(Color.Yellow))
                        {
                            p.DashStyle = DashStyle.Dot;
                            g.DrawLine(p, x3, AGCHang_y_value, x2, AGCHang_y_value);
                            g.DrawString("-H", pana_font, pana_text_brush, AGCHang.X + AGCHang.Width, AGCHang.Y - AGCHang.Height);
                        }
                    }
                    agc = "-G";
                }

                if (AGCKnee_y_value < 0)
                    AGCKnee_y_value = 0;
                else if (AGCKnee_y_value >= pictureBoxSpectrum.Height)
                    AGCKnee_y_value = pictureBoxSpectrum.Height - 1;

                // do AGC Knee stuff.  IF AGC is off, this draws the 'agc fixed gain' value
                AGCKnee.Height = 10; AGCKnee.Width = 10; AGCKnee.X = 40;
                AGCKnee.Y = AGCKnee_y_value - (AGCKnee.Height / 2);
                g.FillRectangle(Brushes.YellowGreen, AGCKnee);

                using (Pen p = new Pen(Color.YellowGreen))
                {
                    p.DashStyle = DashStyle.Dot;
                    g.DrawLine(p, x1, AGCKnee_y_value, x2, AGCKnee_y_value);
                    g.DrawString(agc, pana_font, pana_text_brush, AGCKnee.X + AGCKnee.Width, AGCKnee.Y - AGCKnee.Height);
                }
            } // end DoSpectrum = true;

            // Draw horizontal lines  if we need to show I or Q and no bandscope is selected
            if (ShowI || ShowQ)
            {
                // we only need to draw the lines if showing I and/or Q and NOT showing the spectrum
                if (!doSpectrum)
                {
                    for (int k = 0; k < pictureBoxSpectrum.Height; k += vertical_grid_spacing)
                        g.DrawLine(Pens.DarkGray, 0, k, pictureBoxSpectrum.Width, k);
                }
            }

            // draw cross if required, only used on narrow bandscope at the moment. 
            if (ShowCross)
            {
                g.DrawLine(Pens.Yellow, MousePositionX, 0, MousePositionX, BSHeight); // draw cross vertical line
                if (MousePositionY < BSHeight)  // with both displays check we are within the screen boundries
                    g.DrawLine(Pens.Yellow, 0, MousePositionY, pictureBoxSpectrum.Width, MousePositionY); // draw cross horizontal line
            }
        }

        private void RefreshSpectrumScope(Graphics g)
        {
            if (ShowI && (IDraw != null)) g.DrawCurve(YellowPen, IDraw);
            if (ShowQ && (QDraw != null)) g.DrawCurve(RedPen, QDraw);
            if (doSpectrum && (bandDraw != null)) g.DrawCurve(WhitePen, bandDraw);

            // set the range of the squelch contol to the max and min bandscope values x10 to give high resolution
            Squelch_level.Minimum = GridMin * 10; Squelch_level.Maximum = GridMax * 10;

            if (filterSquelch || bandscopeSquelch)
            {
                squelch_y_value = (int)(-((Squelch_level.Value / 10) - (GridMax)) * pixels_per_GridStep / GridStep);
                
                // draw dotted line to represent squelch threshold
                g.DrawLine(dotPen, 0, squelch_y_value, pictureBoxSpectrum.Width - 1, squelch_y_value);

                // draw the little rectangle at the left side of the squelch threshold line
                squelch.Height = 10; squelch.Width = 10; squelch.X = 0;
                squelch.Y = squelch_y_value - squelch.Height / 2; // make it a square
                g.FillRectangle(Brushes.DarkOrange, squelch);   // fill the rectangle with colour
            }
        }

        private void DrawWidebandGrid(Graphics g)
        {
            #region How DrawGrid works
            /* 
             * We want to display a grid spaced 5/10/20kHz apart horizontally depending on the sampling rate.
             * We also want to display a vertical grid spaced 20dB apart. We also want to indicate the 
             * current filter width on the display.
             * 
             * The screen is 1024 pixels wide so filter width =  bandwidth / sampling_rate
             * 
             * To display the grid, first calculate the vertical grid spacing. We want to display from 0 to -160dBm in 20dB steps 
             * which means we need 160/20 = 8 horizontal lines.
             * 
             * We know the frequency at the left hand edge of the bandscope since it will be the tuned frequency less 
             * (sampling rate/2).  This will be displayed at the top of the first vertical grid line. We want the vertical 
             * grid to have spacings of 5/10/20kHz depending on the sampling rate. We also want the frequency markings to
             * be in these increments. So we need to find the first location from the left hand side of the screen that we
             * can divide by 5/10/20 and get no remainder.  We use the % function to do this. 
             * 
             * Since the minimium grid size is 5kHz we are going to have to test in finer increments than this such that
             * frequency/5 = 0 remainder.  So we use 100Hz increments - this gives a nice smooth display when we drag it.
             * 
             * Once we know the first frequency from the left hand edge of the bandscope we can convert this to pixels. 
             * We know the bandscope is 1024 pixels wide and the frequency width = sampling rate. So we can calculate the x 
             * location of the first vertical bar from  
             * 
             *  pixel_indent = Hz offset from LHS * width of bandscope/sample rate
             *  
             * We can now format the frequency text and display the lines. 
             * 
             * 
             */

            #endregion

            // get height from our pictureBox
            int WBHeight = pictureBoxWideband.Height;

            // calculate the number of horizontal lines and the distance between them
            double lines = (float)(GridMax - GridMin) / (float)GridStep;
            lines = Math.Ceiling((double)lines); // if lines is a fraction round up to next integer
            // determine grid spacing
            int vertical_grid_spacing = (int)(WBHeight / lines);

            int x_locate = 0;
            string displayFreq;
            // determine horizontal grid spacing
            int grid_spacing = SampleRate / 960;  // gives 50/100/200 x 100Hz
            int pixel_spacing = pictureBoxWideband.Width * grid_spacing * 100 / SampleRate;  // how many pixels this spacing represents
            float pixels_per_100Hz = pictureBoxWideband.Width * 100.0f / SampleRate; // how many pixels per 100Hz across the screen 

            // Draw the background color
            Font font = new Font(FontFamily.GenericMonospace, 12);

            // if wide bandscope in use draw vertical lines for it plus frequency 
            if (doWideBand)
            {
                g.FillRectangle(Brushes.DarkGreen, pictureBoxWideband.ClientRectangle);

                int start = 0;

                float MHz = 0.0f;
                for (int i = 0; i < pictureBoxWideband.Width; i += 84)  // 84 gives 5MHz line spacing
                {
                    // draw the vertical lines
                    g.DrawLine(Pens.DarkGray, i, start + 15, i, 512); // start offset stops the line going through the text

                    displayFreq = String.Format("{0:0.0}", MHz);
                    if (MHz < 10)  // so the decimal point in the frequency aligns with the grid line 
                        x_locate = (int)i - 17;
                    else
                        x_locate = (int)i - 26;
                    // draw the text on top of each line
                    g.DrawString(displayFreq, font, Brushes.White, x_locate, start);
                    MHz += 5.0f;
                }

                // Draw the horizontal lines and signal calibration 
                // First draw the horizontal lines so they dont cover the calibration text 
                // Only draw them for the wide bandscope
                for (int k = start + vertical_grid_spacing; k < pictureBoxWideband.Height; k += vertical_grid_spacing)
                    g.DrawLine(Pens.DarkGray, 0, k, pictureBoxWideband.Width, k);

                // The dBm value for the top of the screen is held in Grid.Max, we don't indicate this but we do the next line
                // This will be Grid.Max - Grid.Step
                int textSpacing = vertical_grid_spacing;
                for (int x = GridMax - GridStep; x > GridMin; x -= GridStep)
                {
                    string level = x.ToString() + "dBm";
                    g.DrawString(level, font, Brushes.White, 0, start + textSpacing - 9);
                    textSpacing += vertical_grid_spacing;
                }
            }
        }

        // format changed to PArgb (rgb values pre-multiplied by A.  Since A is 255 (max), regular RGB counts as 'pre-multiplied'.
        private Bitmap waterfallBitmap = new Bitmap(1024, 256, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

        private void RefreshWidebandScope(Graphics g)
        {
            if (doWideBand && (wideBandDraw != null))
            {
                g.DrawCurve(ChartreusePen, wideBandDraw);
            }

            if (doWaterFall)
            {
                int height = pictureBoxWideband.Height;
 
                // display the waterfall bitmap, if we have one
                // display it in two parts: from the currentWaterfallLine to the end of the array,
                // 
                if (waterfallBitmap != null)
                {
                    int firstLine = currentWaterfallLine;
                    int firstSegmentLength = waterfallBitmap.Height - currentWaterfallLine;
                    Rectangle destRect = new Rectangle(0, 0, 1024, firstSegmentLength);

                    // Create rectangle for source image.  This is the first part, from currentWaterfallLine to the end of the bitmap
                    Rectangle srcRect = new Rectangle(0, currentWaterfallLine, 1024, firstSegmentLength);
                    GraphicsUnit units = GraphicsUnit.Pixel;

                    // Draw image to screen, part 1.
                    g.DrawImage(waterfallBitmap, destRect, srcRect, units);

                    // draw rest of the image (which may be blank/black) which is the FIRST PART of the bitmap, to AFTER where
                    // the first part was drawn
                    destRect.Y = firstSegmentLength;
                    destRect.Height = currentWaterfallLine;
                    srcRect.Y = 0;
                    srcRect.Height = currentWaterfallLine;
                    g.DrawImage(waterfallBitmap, destRect, srcRect, units);
                }
            }
        }

        // this is called when we exit the program - terminate the Ethernet connection and save the 
        // current state of the Radio in the KK.csv file.
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            disableDataLogging();

            if (ourDevice != null)
            {
                ourDevice.Stop();
                ourDevice.Close();
            }

            // save the last frequency used 
            switch (BandSelect.Text)
            {
                case "160m": set_frequency_160 = trackBarSetFrequency.Value; break;
                case "80m": set_frequency_80 = trackBarSetFrequency.Value; break;
                case "40m": set_frequency_40 = trackBarSetFrequency.Value; break;
                case "30m": set_frequency_30 = trackBarSetFrequency.Value; break;
                case "20m": set_frequency_20 = trackBarSetFrequency.Value; break;
                case "17m": set_frequency_17 = trackBarSetFrequency.Value; break;
                case "15m": set_frequency_15 = trackBarSetFrequency.Value; break;
                case "12m": set_frequency_12 = trackBarSetFrequency.Value; break;
                case "10m": set_frequency_10 = trackBarSetFrequency.Value; break;
                case "6m": set_frequency_6 = trackBarSetFrequency.Value; break;
                case "GC": set_frequency_GC = trackBarSetFrequency.Value; break;
            }
            // save program settings in KK.csv file
            WriteKKCSV(KKCSVName);
        }

        private void SetSMeterGraph(double value)
        {
            // Calibrate SMeter. S9 = -73dBm = 70 on scale hence add 140
            int sValue = (int)value + 140; 
            if (sValue > 140)       // check the SMeter value is within the range we can display so we don't damage it
                sValue = 140;
            else if (sValue < 0)
                sValue = 0;

            SMeterValue = (int)sValue;
            pictureBox2.Invalidate();
        }

        private string SMeterText(double avgValue, double instantaneousValue)
        {
            if (PreampOn)  // Compensate for 20dB of Preamp gain
            {
                avgValue -= 20;
                instantaneousValue -= 20;
            }
            if (Alex)
            {
                avgValue += AlexAtten;
                instantaneousValue += AlexAtten;
            }

            // Calibrate readings. With 1kHz bandwidth noise floor is -115dBm/-135dBm  preamp on/off
            // Raw values are -45 with preamp off so need to add -70
            avgValue += -70.0f;
            instantaneousValue += -70.0f;
            // set the max signal level for use by the Squelch control
            filter_max_signal = (int)avgValue; 

            // set the bargraph (meter)
            SetSMeterGraph(instantaneousValue);

            // could also compensate with a 'global correction' factor
            return String.Format(nfi, "SMeter: Avg {0:F1} : Inst {1:F1}", avgValue, instantaneousValue);
        }

        private string SMeterText(SharpDSP2._1.SignalMeter s)
        {
            return SMeterText(s.AvgValue, s.InstValue);
        }

        private void SetSMeterText(string text)
        {
            labelSMeter.Text = text;
        }

        void DrawSMeterBackground(Graphics sg)
        {
            sg.FillRectangle(Brushes.Black, 0, 0, pictureBox2.Width, pictureBox2.Height);
            float MeterHeigth = pictureBox2.Height - 3;
            float MeterWidth = pictureBox2.Width - 5;
            pictureBox2.BackColor = Color.Black;
            Brush whiteBrush = Brushes.White;
            Brush redBrush = Brushes.Red;
            Pen whitePen = new Pen(Color.White, 2);
            Pen narrowWhitePen = new Pen(Color.White, 1);
            Pen redPen = new Pen(Color.Red, 2);
            Pen narrowRedPen = new Pen(Color.Red, 1);
            float MarkerSpacing = 14; // Meter.Width / 10.0f;
            string marker = null;

            // Draw the white items
            for (float x = 5, y = 12, z = 1; x < pictureBox2.Width / 2; x += MarkerSpacing, y += MarkerSpacing, z += 2)
            {
                marker = z.ToString();
                sg.DrawLine(narrowWhitePen, x, MeterHeigth, x, 0.9f * MeterHeigth); //small ticks
                sg.DrawLine(whitePen, y, MeterHeigth, y, 0.8f * MeterHeigth); //big ticks
                sg.DrawString(marker, meterFont, whiteBrush, y - 5, 0.45f * MeterHeigth);
            }

            // Draw the red items
            MarkerSpacing = 22; // Meter.Width / 6.5f;  // wider spacing for these itesm 

            for (float x = 67 + MarkerSpacing / 2, y = 67 + MarkerSpacing, z = 0; x < pictureBox2.Width; x += MarkerSpacing, y += MarkerSpacing, z++)
            {
                if (z == 0) marker = "+20";
                else if (z == 1) marker = "+40";
                else if (z == 2) marker = "+60";
                sg.DrawLine(narrowRedPen, x, MeterHeigth, x, 0.9f * MeterHeigth); //small ticks
                sg.DrawLine(redPen, y, MeterHeigth, y, 0.8f * MeterHeigth); //big ticks
                sg.DrawString(marker, meterFont, redBrush, y - 10, 0.45f * MeterHeigth);
            }

            // Draw horizontal lines to finish off
            sg.DrawLine(whitePen, 0, MeterHeigth, MeterWidth / 2, MeterHeigth);
            sg.DrawLine(redPen, 1 + MeterWidth / 2, MeterHeigth, MeterWidth - 1, MeterHeigth);
        }

        Image SMeterBackgroundImage;

        // Draw the SMeter and display the signal level
        void pictureBox2_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            // We only need to draw the SMeter backrough once, then use it as an image 
            // saves CPU cycles

            if (SMeterBackgroundImage == null)
            {
                SMeterBackgroundImage = new Bitmap(pictureBox2.Width, pictureBox2.Height);
                Graphics sg = Graphics.FromImage(SMeterBackgroundImage);

                DrawSMeterBackground(sg);

                pictureBox2.BackgroundImage = SMeterBackgroundImage;
            }
            else
            {
            }

            // now draw SMeter value as a vertical bar
            // scale below S9 is 7 pixels per marker and 11 above so need to scale accordingly
            Pen yellowPen = new Pen(Color.Yellow, 2);
            SMeterValue = (int)((float)SMeterValue * 1.15f) - 10;
            if (SMeterValue > 68)  // scale changes at the S9 level
                SMeterValue = (int)((float)SMeterValue * 0.98f);
            g.DrawLine(yellowPen, SMeterValue, 0, SMeterValue, pictureBox2.Height);
        }

        // Slider control sets transceiver center frequency
        private void trackBarSetFrequency_Scroll(object sender, EventArgs e)
        {
            // when the frequency slider has focus the mouse scroll wheel
            // can be used to increment the frequency by +/- step_size.

            // the scroll bar can also be moved with the arrow and Up/Down keys 
            // make the scroll bar large and small step sizes a function of the step size 
            trackBarSetFrequency.LargeChange = ((step_size + 1) * 100) - step_size;
            trackBarSetFrequency.SmallChange = (step_size + 1);

            // check we are still within the band and update the tune and display frequencies
            Frequency_change();

            UpdateGraphs();
        }

        // select the step size that the mouse scroll wheel does
        private void stepSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            // since we have alreade taken a step use (step size - 1)
            // TODO: this works OK for the mousewheel on my mouse but check this is universal
            switch (stepSize.Text)
            {
                case "1Hz": step_size = 0; break;
                case "10Hz": step_size = 9; break;
                case "100Hz": step_size = 99; break;
                case "1kHz": step_size = 999; break;
            }
        }

        public void UpdateBandGain()
        {
            switch (BandSelect.Text)
            {
                case "160m":
                    BandGain = Gain160m; // set the band gain
                    break;
                case "80m":
                    BandGain = Gain80m; // set the band gain 
                    break;
                case "40m":
                    BandGain = Gain40m; // set the band gain 
                    break;
                case "30m":
                    BandGain = Gain30m; // set the band gain 
                    break;
                case "20m":
                    BandGain = Gain20m; // set the band gain 
                    break;
                case "17m":
                    BandGain = Gain17m; // set the band gain 
                    break;
                case "15m":
                    BandGain = Gain15m; // set the band gain 
                    break;
                case "12m":
                    BandGain = Gain12m; // set the band gain 
                    break;
                case "10m":
                    BandGain = Gain10m; // set the band gain 
                    break;
                case "6m":
                    BandGain = Gain6m; // set the band gain 
                    break;
            }
        }

        public OpBand CurrentBand;

        public void BandSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            // when changing bands store the last frequency used on that band so we can return to it later
            switch (BandText)
            {
                case "160m": set_frequency_160 = trackBarSetFrequency.Value; break;
                case "80m": set_frequency_80 = trackBarSetFrequency.Value; break;
                case "40m": set_frequency_40 = trackBarSetFrequency.Value; break;
                case "30m": set_frequency_30 = trackBarSetFrequency.Value; break;
                case "20m": set_frequency_20 = trackBarSetFrequency.Value; break;
                case "17m": set_frequency_17 = trackBarSetFrequency.Value; break;
                case "15m": set_frequency_15 = trackBarSetFrequency.Value; break;
                case "12m": set_frequency_12 = trackBarSetFrequency.Value; break;
                case "10m": set_frequency_10 = trackBarSetFrequency.Value; break;
                case "6m": set_frequency_6 = trackBarSetFrequency.Value; break;
                case "GC": set_frequency_GC = trackBarSetFrequency.Value; break;
            }

            // set the band edges depending on the band selected. Restore the last used frequency from KK.csv
            switch (BandSelect.Text)
            {
                case "160m":
                    CurrentBand = OpBand.M160;
                    trackBarSetFrequency.Minimum = 1800000;  // low band limit
                    trackBarSetFrequency.Maximum = 2000000;  // high band limit
                    trackBarSetFrequency.Value = set_frequency_160; // last used frequency from KK.csv
                    if (Mode.Text == "USB")
                    {
                        Mode.Text = "LSB";                  // select the appropriate sideband
                        ChangeMode();  // reset the Bandwidth slider
                    }
                    Preamp.Checked = Preamp_160;
                    BandGain = Gain160m; // set the band gain 
                    break;

                case "80m":
                    CurrentBand = OpBand.M80;
                    trackBarSetFrequency.Minimum = 3500000;
                    trackBarSetFrequency.Maximum = 4000000;
                    trackBarSetFrequency.Value = set_frequency_80;
                    if (Mode.Text == "USB")
                    {
                        Mode.Text = "LSB";                  // select the appropriate sideband
                        ChangeMode();  // reset the Bandwidth slider
                    }
                    Preamp.Checked = Preamp_80;
                    BandGain = Gain80m;
                    break;

                case "40m":
                    CurrentBand = OpBand.M40;
                    trackBarSetFrequency.Minimum = 7000000;
                    trackBarSetFrequency.Maximum = 7300000;
                    trackBarSetFrequency.Value = set_frequency_40;
                    if (Mode.Text == "USB")
                    {
                        Mode.Text = "LSB";                  // select the appropriate sideband
                        ChangeMode();  // reset the Bandwidth slider
                    }
                    Preamp.Checked = Preamp_40;
                    BandGain = Gain40m;
                    break;

                case "30m":
                    CurrentBand = OpBand.M30;
                    trackBarSetFrequency.Minimum = 10100000;
                    trackBarSetFrequency.Maximum = 10150000;
                    trackBarSetFrequency.Value = set_frequency_30;
                    if (Mode.Text == "LSB")
                    {
                        Mode.Text = "USB";                  // select the appropriate sideband
                        ChangeMode();  // reset the Bandwidth slider
                    }
                    Preamp.Checked = Preamp_30;
                    BandGain = Gain30m;
                    break;

                case "20m":
                    CurrentBand = OpBand.M20;
                    trackBarSetFrequency.Minimum = 14000000;
                    trackBarSetFrequency.Maximum = 14350000;
                    trackBarSetFrequency.Value = set_frequency_20;
                    if (Mode.Text == "LSB")
                    {
                        Mode.Text = "USB";                  // select the appropriate sideband
                        ChangeMode();  // reset the Bandwidth slider
                    }
                    Preamp.Checked = Preamp_20;
                    BandGain = Gain20m;
                    break;

                case "17m":
                    CurrentBand = OpBand.M17;
                    trackBarSetFrequency.Minimum = 18068000;
                    trackBarSetFrequency.Maximum = 18168000;
                    trackBarSetFrequency.Value = set_frequency_17;
                    if (Mode.Text == "LSB")
                    {
                        Mode.Text = "USB";                  // select the appropriate sideband
                        ChangeMode();  // reset the Bandwidth slider
                    }
                    Preamp.Checked = Preamp_17;
                    BandGain = Gain17m;
                    break;

                case "15m":
                    CurrentBand = OpBand.M15;
                    trackBarSetFrequency.Minimum = 21000000;
                    trackBarSetFrequency.Maximum = 21450000;
                    trackBarSetFrequency.Value = set_frequency_15;
                    if (Mode.Text == "LSB")
                    {
                        Mode.Text = "USB";                  // select the appropriate sideband
                        ChangeMode();  // reset the Bandwidth slider
                    }
                    Preamp.Checked = Preamp_15;
                    BandGain = Gain15m;
                    break;

                case "12m":
                    CurrentBand = OpBand.M12;
                    trackBarSetFrequency.Minimum = 24880000;
                    trackBarSetFrequency.Maximum = 24980000;
                    trackBarSetFrequency.Value = set_frequency_12;
                    if (Mode.Text == "LSB")
                    {
                        Mode.Text = "USB";                  // select the appropriate sideband
                        ChangeMode();  // reset the Bandwidth slider
                    }
                    Preamp.Checked = Preamp_12;
                    BandGain = Gain12m;
                    break;

                case "10m":
                    CurrentBand = OpBand.M10;
                    trackBarSetFrequency.Minimum = 28000000;
                    trackBarSetFrequency.Maximum = 29700000;
                    trackBarSetFrequency.Value = set_frequency_10;
                    if (Mode.Text == "LSB")
                    {
                        Mode.Text = "USB";                  // select the appropriate sideband
                        ChangeMode();  // reset the Bandwidth slider
                    }
                    Preamp.Checked = Preamp_10;
                    BandGain = Gain10m;
                    break;

                case "6m":
                    CurrentBand = OpBand.M6;
                    trackBarSetFrequency.Minimum = 50000000;
                    trackBarSetFrequency.Maximum = 55000000;
                    trackBarSetFrequency.Value = set_frequency_6;
                    if (Mode.Text == "LSB")
                    {
                        Mode.Text = "USB";                  // select the appropriate sideband
                        ChangeMode();  // reset the Bandwidth slider
                    }
                    Preamp.Checked = Preamp_6;
                    BandGain = Gain6m;
                    break;

                case "GC":
                    CurrentBand = OpBand.GC;
                    trackBarSetFrequency.Minimum = 0;
                    trackBarSetFrequency.Maximum = 55000000;
                    trackBarSetFrequency.Value = set_frequency_GC;
                    Preamp.Checked = Preamp_GC;
                    BandGain = 0;
                    break;

                default:
                    CurrentBand = OpBand.M20;
                    trackBarSetFrequency.Minimum = 14000000;
                    trackBarSetFrequency.Maximum = 14350000;
                    break;
            }

            BandText = BandSelect.Text; // save the band we are currently on as we leave 
            Frequency_change(); // update frequency and display

            UpdateGraphs();
        }

        private void Mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeMode(); // select filter setting based on mode
        }

        private void ChangeMode() // ChangeMode() is called by Mode/BandSelect_SelectedIndexChanged()
        {
            ModeText = Mode.Text;

            switch (Mode.Text)
            {
                case "AM":  // this is 'regular' AM, not Synchronous AM (SharpDSP2._1.DSPMode_e.SAM)
                    CurrentMode = OperMode.AM;
                    state.DSPMode = SharpDSP2._1.DSPMode_e.AM;
                    BandwidthTrackBar.Minimum = 3000;
                    BandwidthTrackBar.Maximum = 12000;
                    rcvr.FilterFrequencyHigh = BandwidthTrackBar.Value;
                    rcvr.FilterFrequencyLow = -BandwidthTrackBar.Value;
                    break;

                case "FM":
                    CurrentMode = OperMode.FM;
                    rcvr.SetFMMode();
                    state.DSPMode = SharpDSP2._1.DSPMode_e.FMN;
                    BandwidthTrackBar.Minimum = 3000;
                    BandwidthTrackBar.Maximum = 12000;
                    rcvr.FilterFrequencyHigh = BandwidthTrackBar.Value;
                    rcvr.FilterFrequencyLow = -BandwidthTrackBar.Value;
                    break;

                case "SAM":
                    CurrentMode = OperMode.SAM;
                    rcvr.SetSAMMode();
                    state.DSPMode = SharpDSP2._1.DSPMode_e.SAM;
                    BandwidthTrackBar.Minimum = 3000;
                    BandwidthTrackBar.Maximum = 12000;
                    rcvr.FilterFrequencyHigh = BandwidthTrackBar.Value;
                    rcvr.FilterFrequencyLow = -BandwidthTrackBar.Value;
                    break;

                case "USB":
                    CurrentMode = OperMode.USB;
                    state.DSPMode = SharpDSP2._1.DSPMode_e.USB;
                    BandwidthTrackBar.Minimum = 1200;
                    BandwidthTrackBar.Maximum = 6000;
                    rcvr.FilterFrequencyHigh = BandwidthTrackBar.Value;
                    rcvr.FilterFrequencyLow = 200.0f;
                    break;

                case "LSB":
                    CurrentMode = OperMode.LSB;
                    state.DSPMode = SharpDSP2._1.DSPMode_e.LSB;
                    BandwidthTrackBar.Minimum = 1200;
                    BandwidthTrackBar.Maximum = 6000;
                    rcvr.FilterFrequencyHigh = -BandwidthTrackBar.Value;
                    rcvr.FilterFrequencyLow = -200.0f;
                    break;

                case "CWL":
                    CurrentMode = OperMode.CWL;
                    state.DSPMode = SharpDSP2._1.DSPMode_e.CWL;
                    BandwidthTrackBar.Minimum = 100;
                    BandwidthTrackBar.Maximum = 1100;
                    rcvr.FilterFrequencyHigh = -(CWPitch + BandwidthTrackBar.Value / 2);
                    rcvr.FilterFrequencyLow = -(CWPitch - BandwidthTrackBar.Value / 2);
                    break;

                case "CWU":
                    CurrentMode = OperMode.CWU;
                    state.DSPMode = SharpDSP2._1.DSPMode_e.CWU;
                    BandwidthTrackBar.Minimum = 100;
                    BandwidthTrackBar.Maximum = 1100;
                    rcvr.FilterFrequencyHigh = (CWPitch + BandwidthTrackBar.Value / 2);
                    rcvr.FilterFrequencyLow = (CWPitch - BandwidthTrackBar.Value / 2);
                    break;
            }

            labelFilterWidth.Text = BandwidthTrackBar.Value.ToString();

            UpdateGraphs();
        }

        // set the volume based on the VolumeTrackBar value
        public void SetVolume(object sender, EventArgs e)
        {
            // set the Receiver volume level
            setVolumeSquelched();
        }

        public void setVolumeSquelched()
        {
            // if squelch it turned on, AND 'squelched' is true, OR (MOX is set OR PTT set ) AND not full Duplex then mute audio
            if ((squelchOn && squelched) || ((MOX_set || PTT) && !Duplex))
            {
                rcvr.VolumeLeft = 0;  // send to SharpSDP
                rcvr.VolumeRight = 0;
            }
            else
            {
                // otherwise set to volume trackbar level
                // convert the track bar value to a float in the range 0 to 1, since that's what SharpDSP needs
                float v = (float)VolumeTrackBar.Value / (float)VolumeTrackBar.Maximum;

                labelVolume.Text = VolumeTrackBar.Value.ToString();

                rcvr.VolumeLeft = v;  // send to SharpSDP
                rcvr.VolumeRight = v;
           }
        }


        private void SafeUpDownValueChange(decimal newValue, NumericUpDown control)
        {
            if (newValue < control.Minimum)
                newValue = control.Minimum;
            if (newValue > control.Maximum)
                newValue = control.Maximum;

            control.Value = newValue;
        }

        private void SafeTrackBarValueChange(int newValue, TrackBar control)
        {
            if (newValue < control.Minimum)
                newValue = control.Minimum;
            if (newValue > control.Maximum)
                newValue = control.Maximum;

            control.Value = newValue;
        }

        // set the AGC Gain  based on the AGC TrackBar value
        bool noAGCTrackBarUpdate = false;
        private void AGCTrackBar_Scroll(object sender, EventArgs e)
        {
            if (noAGCTrackBarUpdate)
                return;

            if (rcvr.AGCMode == SharpDSP2._1.AGCType_e.agcOff)
            {
                rcvr.AGCFixedGainDB = AGCTrackBar.Value;    // ditto
                if (SetupFormValid())
                {
                    Setup_form.ChangeFixedGain(rcvr.AGCFixedGainDB);
                }
            }
            else
            {
                rcvr.AGCMaximumGainDB = AGCTrackBar.Value;  // TODO: make this value user adjustable and store/restore on KK.cvs
                if (SetupFormValid())
                {
                    Setup_form.ChangeMaxGain(rcvr.AGCMaximumGainDB);
                }
            }

            labelAGCGain.Text = AGCTrackBar.Value.ToString();
        }

        public double AGCMaximumGainDB
        {
            get { return rcvr.AGCMaximumGainDB; }
            set
            {
                rcvr.AGCMaximumGainDB = value;
            }
        }

        public void UpdateMaxGainAndSlider(double newGain)
        {
            noAGCTrackBarUpdate = true;
            SafeTrackBarValueChange((int)newGain, AGCTrackBar);
            rcvr.AGCMaximumGainDB = AGCTrackBar.Value;
            noAGCTrackBarUpdate = false;
        }

        public double AGCFixedGainDB
        {
            get { return rcvr.AGCFixedGainDB; }
            set
            {
                rcvr.AGCFixedGainDB = value;
            }
        }

        /// <summary>
        /// used to get/set the dB level on the spectral (panadapter) display
        /// </summary>
        public double AGCThreshDB
        {
            get { return rcvr.AGCThreshDB; }
            set
            {
                rcvr.AGCThreshDB = value;
            }
        }

        public double UserAGCHangTime = 1.0;    // 1 ms
        public double UserAGCAttackTime = 10.0;
        public double UserAGCDecayTime = 10.0;

        public double AGCHangTime
        {
            get { return rcvr.AGCHangTime * 1000; }
            set
            {
                rcvr.AGCHangTime = value / 1000.0;
            }
        }

        public int AGCHangThreshold
        {
            get { return (int)(rcvr.AGCHangThreshold * 100.0); }
            set
            {
                rcvr.AGCHangThreshold = value / 100.0;
            }
        }

        /// <summary>
        /// used to get/set where the hang threshold line is on the spectral display (panadapter)
        /// </summary>
        public double AGCHangLevel
        {
            get { return rcvr.AGCHangLevel; }
            set
            {
                rcvr.AGCHangLevel = value;
            }
        }

        public double AGCAttackTime
        {
            get { return rcvr.AGCAttackTime * 1000; }
            set
            {
                rcvr.AGCAttackTime = value / 1000.0;
            }
        }

        public double AGCDecayTime
        {
            get { return rcvr.AGCDecayTime * 1000; }
            set
            {
                rcvr.AGCDecayTime = value / 1000.0;
            }
        }

        public double AGCSlope
        {
            get { return rcvr.AGCSlope; }
            set
            {
                rcvr.AGCSlope = value;
            }
        }

        // Set the AGC speed based on the user selection 
        private void AGCSpeed_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (AGCSpeed.Text)
            {
                case "OFF": rcvr.AGCMode = SharpDSP2._1.AGCType_e.agcOff; break;
                case "Long": rcvr.AGCMode = SharpDSP2._1.AGCType_e.agcLong; break;
                case "Slow": rcvr.AGCMode = SharpDSP2._1.AGCType_e.agcSlow; break;
                case "Med": rcvr.AGCMode = SharpDSP2._1.AGCType_e.agcMedium; break;
                case "Fast": rcvr.AGCMode = SharpDSP2._1.AGCType_e.agcFast; break;
                case "User": rcvr.AGCMode = SharpDSP2._1.AGCType_e.agcUser; break;
            }

            bool CustomMode = (rcvr.AGCMode == SharpDSP2._1.AGCType_e.agcUser);

            bool HangEnabled = (rcvr.AGCMode != SharpDSP2._1.AGCType_e.agcMedium) && (rcvr.AGCMode != SharpDSP2._1.AGCType_e.agcFast);

            // if in custom AGC mode, set the times
            if (CustomMode)
            {
                // set the custom times.
                AGCHangTime = UserAGCHangTime;
                AGCAttackTime = UserAGCAttackTime;
                AGCDecayTime = UserAGCDecayTime;
            }

            UpdateAGCTimeDisplay();

            if (SetupFormValid())
            {
                Setup_form.CustomRXAGCEnabled = CustomMode;
            }

            // update whether AGC Hang is enabled, and pass the info to any valid setup form.
            AGCHangEnabled = HangEnabled;
        }

        private void UpdateAGCTimeDisplay()
        {
            if (SetupFormValid())
            {
                Setup_form.CustomRXAGCEnabled = (rcvr.AGCMode == SharpDSP2._1.AGCType_e.agcUser);
                Setup_form.ChangeAGCTimes();
            }
        }

        // helper function to set TX filter bandwidth
        public void SetTXFilterBandwidth()
        {
            if (ourDevice != null)
            {
                ourDevice.SetTxBandwidth(TxFilterLow, TxFilterHigh);
            }
        }

        // select the appropriate filter bandwidth for the Mode selected 
        public void AdjustFilterByMode()
        {
            SetTXFilterBandwidth();

            switch (Mode.Text)
            {
                case "LSB":
                    int BandwidthSBvalue = BandwidthTrackBar.Value;
                    rcvr.FilterFrequencyHigh = -BandwidthSBvalue;
                    rcvr.FilterFrequencyLow = -200.0f;
                    break;

                case "USB":
                    BandwidthSBvalue = BandwidthTrackBar.Value;
                    rcvr.FilterFrequencyHigh = BandwidthTrackBar.Value;
                    rcvr.FilterFrequencyLow = 200.0f;
                    break;

                case "CWL":
                    int BandwidthCWvalue = BandwidthTrackBar.Value;
                    rcvr.FilterFrequencyHigh = -(CWPitch + BandwidthCWvalue / 2);
                    rcvr.FilterFrequencyLow = -(CWPitch - BandwidthCWvalue / 2);
                    break;

                case "CWU":
                    BandwidthCWvalue = BandwidthTrackBar.Value;
                    rcvr.FilterFrequencyHigh = (CWPitch + BandwidthCWvalue / 2);
                    rcvr.FilterFrequencyLow = (CWPitch - BandwidthCWvalue / 2);
                    break;

                case "AM":
                    int BandwidthAMvalue = BandwidthTrackBar.Value;
                    rcvr.FilterFrequencyHigh = BandwidthAMvalue;
                    rcvr.FilterFrequencyLow = -BandwidthAMvalue;
                    break;

                case "SAM":
                    int BandwidthSAMvalue = BandwidthTrackBar.Value;
                    rcvr.FilterFrequencyHigh = BandwidthSAMvalue;
                    rcvr.FilterFrequencyLow = -BandwidthSAMvalue;
                    break;

                case "FM":
                    int BandwidthFMvalue = BandwidthTrackBar.Value;
                    rcvr.FilterFrequencyHigh = BandwidthFMvalue;
                    rcvr.FilterFrequencyLow = -BandwidthFMvalue;
                    break;
            }
        }

        // Vary the filter Bandwidth when the Bandwidth control is changed
        private void BandwidthTrackBar_Scroll(object sender, EventArgs e)
        {
            AdjustFilterByMode();
            labelFilterWidth.Text = BandwidthTrackBar.Value.ToString();
        }

        private void BandwidthTrackBar_ValueChanged(object sender, EventArgs e)
        {
            labelFilterWidth.Text = BandwidthTrackBar.Value.ToString();
        }

        public int aNFAdaptiveFilterSize = 64;
        public int ANFAdaptiveFilterSize
        {
            get { return aNFAdaptiveFilterSize; }
            set { aNFAdaptiveFilterSize = value; rcvr.InterferenceFilterAdaptiveFilterSize = value; }
        }

        public int   aNFDelay = 12;
        public int ANFDelay
        {
            get { return aNFDelay; }
            set { aNFDelay = value; rcvr.InterferenceFilterDelay = value; }
        }

        public float aNFAdaptationRate = .005F;
        public float ANFAdaptationRate
        {
            get { return aNFAdaptationRate; }
            set { aNFAdaptationRate = value; rcvr.InterferenceFilterAdaptationRate = value; }
        }

        public float aNFLeakage = .01F;
        public float ANFLeakage
        {
            get { return aNFLeakage; }
            set { aNFLeakage = value; rcvr.InterferenceFilterLeakage = value; }
        }

        // set up the Automatic Notch Filter
        private void ANF_CheckedChanged(object sender, EventArgs e)
        {
            if (ANF.Checked) 
            {
                rcvr.InterferenceFilterSwitchOn = true;
                rcvr.InterferenceFilterAdaptiveFilterSize = ANFAdaptiveFilterSize;
                rcvr.InterferenceFilterDelay = ANFDelay;
                rcvr.InterferenceFilterAdaptationRate = ANFAdaptationRate;
                rcvr.InterferenceFilterLeakage = ANFLeakage;
            }
            else
                rcvr.InterferenceFilterSwitchOn = false;
        }

        public int nRAdaptiveFilterSize = 64;
        public int NRAdaptiveFilterSize
        {
            get { return nRAdaptiveFilterSize; }
            set { nRAdaptiveFilterSize = value; rcvr.NoiseFilterAdaptiveFilterSize = value; }
        }

        public int nRDelay = 12;
        public int NRDelay
        {
            get { return nRDelay; }
            set { nRDelay = value; rcvr.NoiseFilterDelay = value; }
        }

        public float nRAdaptationRate = .005F;
        public float NRAdaptationRate
        {
            get { return nRAdaptationRate; }
            set { nRAdaptationRate = value; rcvr.NoiseFilterAdaptationRate = value; }
        }

        public float nRLeakage = .01F;
        public float NRLeakage
        {
            get { return nRLeakage; }
            set { nRLeakage = value; rcvr.NoiseFilterLeakage = value; }
        }

        // set up the Noise Reduction 
        private void NR_CheckedChanged(object sender, EventArgs e)
        {
            if (NR.Checked)  
            {
                rcvr.NoiseFilterSwitchOn = true;
                rcvr.NoiseFilterAdaptiveFilterSize = NRAdaptiveFilterSize;
                rcvr.NoiseFilterDelay = NRDelay;
                rcvr.NoiseFilterAdaptationRate = NRAdaptationRate;
                rcvr.NoiseFilterLeakage = NRLeakage;
            }
            else
                rcvr.NoiseFilterSwitchOn = false;
        }

        // Noise Blanker1
        private void NB1_CheckedChanged(object sender, EventArgs e)
        {
            rcvr.BlockNBSwitchOn = NB1.Checked;
        }

        // Noise Blanker2
        private void NB2_CheckedChanged(object sender, EventArgs e)
        {
            rcvr.AveNBSwitchOn = NB2.Checked;
        }

        private void UpdateGraphs()
        {
            pictureBoxWidebandBackground = null;

            pictureBoxSpectrum.Invalidate();
            pictureBoxWideband.Invalidate();
        }

        private void Frequency_change()
        {
            // we need to check if we are too close to the band edge to take a step 
            if (trackBarSetFrequency.Value > previous_frequency_value && trackBarSetFrequency.Value < (trackBarSetFrequency.Maximum - step_size))
                trackBarSetFrequency.Value += step_size;  // add step_size since scroll wheel has added 1 already
            else if (trackBarSetFrequency.Value < previous_frequency_value && trackBarSetFrequency.Value > (trackBarSetFrequency.Minimum + step_size + 1))
                trackBarSetFrequency.Value -= step_size; // subtract step_size since scroll wheel has subtracted 1 already
            // else use the current value

            // round frequency to step size
            int temp_frequency = trackBarSetFrequency.Value; // use a temp value so we don't have to worry about the band edges.
            temp_frequency = temp_frequency / (step_size + 1);
            temp_frequency = temp_frequency * (step_size + 1);
            trackBarSetFrequency.Value = temp_frequency;

            // display the new frequency and send to Ozy
            DisplayFrequency(trackBarSetFrequency.Value);
            // save current value so we can compare with next value so we can determine tune direction
            previous_frequency_value = trackBarSetFrequency.Value;
        }

        public float ProcGain;
        public float VOXThreshold;

        public bool received_flag = false;

        // when ever timer1 fires we update the bandscope
        private void timer1_Tick(object sender, EventArgs e)
        {
            // display the status of Sync and ADC overload on the 'LEDs'
            SyncLED.BackColor = (IsSynced & received_flag) ? Color.Green : Color.Red;
            received_flag = false;  // reset flag for next time
            ADCoverloadButton.BackColor = ADCOverload ? Color.Red : SystemColors.Control;

            if ((PenneyPresent || PennyLane || Hermes) && (BandSelect.Text != "GC"))  // enable Tx controls if Penny present and check 10MHz clock is selected
            {
                MOX.Enabled = true;
                TUN.Enabled = true;
                // If using Penny or PennyLane check we have a 10MHz clock selected,if not warn user and turn KK off
                if (!Hermes && !Penelope10MHz && !Atlas10MHz && !Excalibur && !Mercury10MHz)
                {
                    timer1.Enabled = false;
                    if (MessageBox.Show("Warning - no 10MHz clock selected", "Configuration Error!", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        == DialogResult.OK)
                    {
                        setupToolStripMenuItem_Click(this, EventArgs.Empty);  // open setup form for user
                        OnOffButton_Click(this, EventArgs.Empty);  // turn KK off 
                    }
                }
            }
            else
            {
                MOX.Enabled = false;
                TUN.Enabled = false;
            }

            setVolumeSquelched();  // see if we need to mute the audio

#if false
            // if Hermes, display the voltage supplied to the board and user inputs
            if (Hermes)
            {
                Console.WriteLine(" Volts = \t" + SupplyVolts);
                Console.WriteLine(" IO1 = \t" + IO1);
                Console.WriteLine(" IO2 = \t" + IO2);
                Console.WriteLine(" IO3 = \t" + IO3);
            }
#endif

            if (read_ready)  // update the display if data is available 
            {
                read_ready = false;

                // get a wide-bandwidth sample
                if ((ourDevice != null) && (KKDevice == KKMethod.USB))
                {
                    // we only call this in the USB code.  In the Ethernet code, this data is processed in EthernetDevice.cs
                    // on the data thread, because both EP6 and EP4 packets are received there.
                    ourDevice.ProcessWideBandData(ref ourDevice.EP4buf);
                }

                // get bandwidth samples as IQ data
                GetIQSamples();

                ComputeDisplay();

                Squelch();

                if (doSpectrum || ShowI || ShowQ)
                {
                    pictureBoxSpectrum.Invalidate(); // causes RefreshScope to run 
                }

                if (doWaterFall || doWideBand)
                {
                    if (doWaterFall)
                    {
                        // get rid of the background so that it will redraw if activated in the future
                        pictureBoxWidebandBackground = null;
                        pictureBoxWideband.BackgroundImage = null;
                    }

                    pictureBoxWideband.Invalidate(); // causes RefreshScope to run
                }

                // not a cross-thread call when called from the timer tick!
                // display SMeter reading.  Note that this is accumulated by the Receiver object
                // when it is invoked by Process_Data (G Byrkit, K9TRV, 15 June 2009)
                SetSMeterText(SMeterText(rcvr.SMeterAvgValue, rcvr.SMeterInstValue));

                if ((PenneyPresent || PennyLane || Hermes) && (Alex) && (ourDevice != null))
                {
                    // get power settings to local variables so any change on the data thread has no further effect
                    double fwdPower = ourDevice.AlexForwardPower;
                    double revPower = ourDevice.AlexReversePower;

                    if (fwdPower > 1.0)
                    {
                        textBoxForwardPower.Text = String.Format("{0:F1} W", fwdPower);
                    }
                    else
                    {
                        textBoxForwardPower.Text = String.Format("{0:F0} mW", fwdPower * 1000);
                    }

                    if (revPower > 1.0)
                    {
                        textBoxReversePower.Text = String.Format("{0:F1} W", revPower);
                    }
                    else
                    {
                        textBoxReversePower.Text = String.Format("{0:F0} mW", revPower * 1000);
                    }
                }
                else
                {
                    textBoxForwardPower.Text = "";
                    textBoxReversePower.Text = "";
                }
            }

            //            Console.WriteLine("Temp MicAGCGain = \t {0} \t TempMicPeak = \t {1}", TempMicAGC, TempMicPeak);
        }

        public void SetMicGain()
        {
            if (ourDevice != null)
            {
                ourDevice.SetMicGain();
            }
        }

        public bool squelched = false;

        // process bandscope and filter squelch.
        private void Squelch()
        {
            /* 
             * 
             * When using the filter width squelch we need to compenstate for the fact that the bandwidth is wider than
             * that when using the bandscope width. The FFT bin width (Hz) for the bandscope is 
             * 
             *      bandwidth =  sampling rate / FFT size 
             *      
             * For the filter bandwidth we can just read the filter slider.
             * Hence the  noise power will be higher in the filter bandwidth by 
             * 
             *       ratio = filter bandwidth/(sampling rate/FFT size)
             *       
             * Since we are working in dB power levels we need to take 10log of this value and apply to the signal level
             * 
             */
                        
            // if squelch is not selected then just return
            if (!bandscopeSquelch && !filterSquelch)
            {
                Squelch_level.BackColor = SystemColors.Control;
                return;
            }

            int BandWidthCorrection = 10 * (int)Math.Log10(BandwidthTrackBar.Value / (SampleRate / 1024));

            // Do Bandscope squelch
            if (bandscopeSquelch && (max_signal < Squelch_level.Value/10))
            {
                    squelched = true;
                    Squelch_level.BackColor = Color.Yellow; // change the slider background to show we have muted the audio 
            }

            // Do filter squelch - this uses the average SMeter value as its level
            else if (filterSquelch && (filter_max_signal < ((Squelch_level.Value / 10) + BandWidthCorrection)))
            {
                squelched = true;
                Squelch_level.BackColor = Color.Yellow; // change the slider background to show we have muted the audio 
            }
            else  // use the user volume setting 
            {
                squelched = false; 
                Squelch_level.BackColor = Color.Green; // change the slider background to show are above threshold
            }

            setVolumeSquelched();
        }

        private void SetSplitterBar()
        {
            int halfHeight = (splitContainer1.Height - 8) / 2;

            if ((doSpectrum || ShowI || ShowQ) && (doWaterFall || doWideBand))
            {
                splitContainer1.Panel1Collapsed = false;
                splitContainer1.Panel2Collapsed = false;

                // multi-part display.  If not yet multi-part, change the splitter point
                if (splitContainer1.IsSplitterFixed)
                {
                    splitContainer1.SplitterDistance = halfHeight; // 256;
                }
                if (doWaterFall)
                {
                    // waterfall is exactly 256 lines.  don't allow splitter bar to move
                    splitContainer1.IsSplitterFixed = true;
                    splitContainer1.SplitterDistance = halfHeight; // 256;
                }
                else
                {
                    // allow user to choose how much wideband and how much spectrum
                    splitContainer1.IsSplitterFixed = false;
                }
            }
            else if (doSpectrum || ShowI || ShowQ)
            {
                // top display only
                splitContainer1.Panel1Collapsed = false;
                splitContainer1.Panel2Collapsed = true;
                //splitContainer1.SplitterDistance = halfHeight * 2; // 512;
                splitContainer1.IsSplitterFixed = true;
            }
            else if (doWaterFall || doWideBand)
            {
                // bottom display only
                splitContainer1.Panel1Collapsed = true;
                splitContainer1.Panel2Collapsed = false;
                //splitContainer1.SplitterDistance = 0;
                splitContainer1.IsSplitterFixed = true;
            }

            UpdateGraphs();
        }

        public bool doSpectrum = false;

        private void chkSpec_CheckedChanged(object sender, EventArgs e)
        {
            doSpectrum = chkSpec.Checked;
            SetSplitterBar();
            UpdateGraphs();
        }

        public bool doWideBand = false;

        private void chkWideSpec_CheckedChanged(object sender, EventArgs e)
        {
            doWideBand = chkWideSpec.Checked;
            if (doWideBand)
            {
                chkWaterFall.Checked = false;
            }

            SetSplitterBar();
            UpdateGraphs();
        }

        bool doWaterFall = false;

        private void chkWaterFall_CheckedChanged(object sender, EventArgs e)
        {
            doWaterFall = chkWaterFall.Checked;

            if (doWaterFall)
            {
                // turning on...
                // init the waterfall data, causing the waterfallBitmap to be blanked...
                ClearWaterfall();

                chkWideSpec.Checked = false;
            }

            SetSplitterBar();
            UpdateGraphs();
        }

        public bool PreampOn = false;

        private void Preamp_CheckedChanged(object sender, EventArgs e)
        {
            PreampOn = Preamp.Checked;

            // save the preamp setting for each band
            switch (BandSelect.Text)
            {
                case "160m": Preamp_160 = PreampOn; break;
                case "80m": Preamp_80 = PreampOn; break;
                case "40m": Preamp_40 = PreampOn; break;
                case "30m": Preamp_30 = PreampOn; break;
                case "20m": Preamp_20 = PreampOn; break;
                case "17m": Preamp_17 = PreampOn; break;
                case "15m": Preamp_15 = PreampOn; break;
                case "12m": Preamp_12 = PreampOn; break;
                case "10m": Preamp_10 = PreampOn; break;
                case "6m": Preamp_6 = PreampOn; break;
                case "GC": Preamp_GC = PreampOn; break;
            }
        }

        // save the current band & frequency in a quick memory
        private void StoreFreq_Click(object sender, EventArgs e)
        {
            StoreFreq.BackColor = Color.Gray;
            QuickMemoryBand = BandSelect.Text;
            QuickMemory = trackBarSetFrequency.Value;
        }

        // restore a previously stored quick band & frequency 
        private void RecallFreq_Click(object sender, EventArgs e)
        {
            RecallFreq.BackColor = Color.Gray;
            BandSelect.Text = QuickMemoryBand;
            trackBarSetFrequency.Value = QuickMemory;
            Frequency_change();     // process the new frequency 
        }

        // process key strokes from the keyboard 
        protected override void OnKeyDown(KeyEventArgs e)
        {
            // setting suppressKeyPress to true does several things:
            // 1) it prevents the auto-display of the key
            // 2) sets e.Handled to true
            // this keeps the textbox from being corrupted by typed keys that are
            // not processed by this event handler, and makes sure that what is (eventually)
            // displayed is correct
            e.SuppressKeyPress = true;

            bool display = false;
            Debug.WriteLine("got a key \t" + e.KeyCode);

            // process key presses from the numeric keypad and keyboard
            switch (e.KeyCode)
            {
                case Keys.D0:
                case Keys.NumPad0: KeypadFrequency += "0"; keycount++; break;

                case Keys.D1:
                case Keys.NumPad1: KeypadFrequency += "1"; keycount++; break;

                case Keys.D2:
                case Keys.NumPad2: KeypadFrequency += "2"; keycount++; break;

                case Keys.D3:
                case Keys.NumPad3: KeypadFrequency += "3"; keycount++; break;

                case Keys.D4:
                case Keys.NumPad4: KeypadFrequency += "4"; keycount++; break;

                case Keys.D5:
                case Keys.NumPad5: KeypadFrequency += "5"; keycount++; break;

                case Keys.D6:
                case Keys.NumPad6: KeypadFrequency += "6"; keycount++; break;

                case Keys.D7:
                case Keys.NumPad7: KeypadFrequency += "7"; keycount++; break;

                case Keys.D8:
                case Keys.NumPad8: KeypadFrequency += "8"; keycount++; break;

                case Keys.D9:
                case Keys.NumPad9: KeypadFrequency += "9"; keycount++; break;
                case Keys.Back:
                    if (keycount > 0)  // remove the last character entered
                    {
                        KeypadFrequency = KeypadFrequency.Remove(KeypadFrequency.Length - 1);
                        --keycount;
                    }
                    break;
                case Keys.Enter:
                    if (keycount > 0)   // so we don't trigger on the main Enter Key 
                        display = true;
                    break;

                case Keys.Decimal:
                case Keys.OemPeriod:
                    if ((KeypadFrequency == null) || !KeypadFrequency.Contains("."))
                    {
                        if (keycount == 0)
                        {
                            // force a leading 0 if no chars present yet
                            KeypadFrequency += "0";
                            keycount++;
            }
                        KeypadFrequency += ".";
                        keycount++;
                    }
                    break;

                // if Up arrow increment frequency by step size
                case Keys.Up: KeypadFrequency = trackBarSetFrequency.Value++.ToString(); display = true; break;

                // if Down arrow decrement frequency by step size
                case Keys.Down: KeypadFrequency = trackBarSetFrequency.Value--.ToString(); display = true; break;

                // if not a valid key then exit
                default: display = false; return;
            }

            float temp = 0.0f;
            int setfrequency;
            if (display)  // user has pressed enter key 
            {
                try  // data format may be incorrect so check
                {
                    // must use the invariant culture here, so that the '.' is properly
                    // interpreted as a decimal separator!
                    temp = (float)Convert.ToDecimal(KeypadFrequency, nfi); // convert to a float 
                }

                catch // format error so tidy up and return
                {
                    Frequency_change();     // display the previous frequency
                    KeypadFrequency = null;
                    keycount = 0;
                    return;
                }
                float temp2 = temp * 1000000.0F;  // Multiply by 10e6 to give Hz
                setfrequency = (int)temp2; // convert to an integer
                // check frequency is valid
                if ((keycount > 0) && (keycount < 10) && (setfrequency < 55000001) && (setfrequency > 0))
                {
                    Debug.WriteLine(setfrequency);
                    BandSelect.Text = getBand(setfrequency);
                    trackBarSetFrequency.Value = setfrequency;
                    // display the new frequency and send to Ozy
                    DisplayFrequency(setfrequency);
                    KeypadFrequency = null;
                    keycount = 0;
                    return;
                }
                else  // error, abort
                {
                    Frequency_change();     // display the previous frequency
                    KeypadFrequency = null;
                    keycount = 0;
                    return;
                }
            }
            else
            {
                // display the frequency as it is entered but only the text entered on the keypad 
                display_freq.Text = KeypadFrequency;
            }
        }

        private void DisplayFrequency(int setfrequency)
        {
            // convert tune frequency to an array of bytes ready to send to Ozy
            frequency = BitConverter.GetBytes(trackBarSetFrequency.Value);
            // for now set the Duplex frequency  = simplex frequency
            duplex_frequency = BitConverter.GetBytes(trackBarSetFrequency.Value);
            // format the integer frequency to have a decimal point between the MHz digits.
            float temp_freq = (float)setfrequency / 1000000.0f;
            // convert to a string with 6 digits to the right of the decimal point 
            string freq = String.Format(nfi, "{0:F6}", (float)temp_freq);
            // strip off the last 3 digits, add a space then add them back 
            display_freq.Text = freq.Remove(freq.Length - 3) + ' ' + freq.Substring(freq.Length - 3, 3);
        }

        // find out what band we are on when user enters frequency from keypad
        private string getBand(int frequency)
        {
            if (frequency > 1800000 && frequency < 2000000) return "160m";
            else if (frequency > 3500000 && frequency < 4000000) return "80m";
            else if (frequency > 7000000 && frequency < 7300000) return "40m";
            else if (frequency > 10100000 && frequency < 10150000) return "30m";
            else if (frequency > 14000000 && frequency < 14350000) return "20m";
            else if (frequency > 18068000 && frequency < 18168000) return "17m";
            else if (frequency > 21000000 && frequency < 21450000) return "15m";
            else if (frequency > 24880000 && frequency < 24980000) return "12m";
            else if (frequency > 28000000 && frequency < 29700000) return "10m";
            else if (frequency > 50000000 && frequency < 55000000) return "6m";
            return "GC";
        }

        public bool SetupFormValid()
        {
            // if the form is null or disposed, it is NOT valid
            if (Setup_form == null || Setup_form.IsDisposed)
                return false;
            else
                return true;
        }

        private void setupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // only allow user to open form if not already open
            if (!SetupFormValid())
            {
                Setup_form = new SetupForm(this);
                Setup_form.TopMost = true;          // have the setup form appear over the main form.
                Setup_form.SampleRate.Text = SampleRate.ToString(); // pass the current Sampling Rate 
                Setup_form.Owner = this;
                Setup_form.Show();                  // display the Setup form
                Setup_form.Set_CWPitch.Value = CWPitch; // setup the initial value of cw pitch control on the setup form
                Setup_form.CustomRXAGCEnabled = (rcvr.AGCMode == SharpDSP2._1.AGCType_e.agcUser);
            }
            else  // form already open 
            {
                // handle setup form already being shown.  Make sure the user sees it...
                Setup_form.Show();                  // display the Setup form
            }
        }

        Image pictureBoxWidebandBackground = null;
        bool useWidebandBackground = true;

        private void pictureBoxWideband_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (doWideBand) // if a bandscope etc is selected then display it
            {
                if (useWidebandBackground)
                {
                    // using the wideband background image
                    if ((pictureBoxWidebandBackground == null) || (pictureBoxWideband.BackgroundImage == null))
                    {
                        pictureBoxWidebandBackground = new Bitmap(pictureBoxWideband.Width, pictureBoxWideband.Height);
                        Graphics g2 = Graphics.FromImage(pictureBoxWidebandBackground);

                        DrawWidebandGrid(g2);            // draw the background grid

                        pictureBoxWideband.BackgroundImage = null;
                        pictureBoxWideband.BackgroundImage = pictureBoxWidebandBackground;
                    }
                }
                else
                {
                    // NOT using the wideband background image
                    pictureBoxWideband.BackgroundImage = null;
                    pictureBoxWidebandBackground = null;

                    // draw the background grid using the pictureBox paint PaintEventArgs Graphics object
                    DrawWidebandGrid(g);
                }
            }
            else
            {
                pictureBoxWideband.BackgroundImage = null;
                pictureBoxWidebandBackground = null;
            }

            if (doWideBand || doWaterFall) // if a bandscope etc is selected then display it
                RefreshWidebandScope(g);
        }

        private void pictureBoxSpectrum_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;

            if (doSpectrum || ShowI || ShowQ) // if a bandscope etc is selected then display it
            {
                DrawSpectrumGrid(g);

                RefreshSpectrumScope(g);
            }
        }

        private void pictureBoxSpectrum_MouseDown(object sender, MouseEventArgs e)
        {
            // check if user has clicked over squelch level control rectangle, if so set SquelchDrag to true
            if (squelch.Contains(e.X, e.Y))  // this gives true if the mouse is inside the squelch rectangle
            {
                SquelchDrag = true;
                AGCHangDrag = false;
                AGCKneeDrag = false;
                pictureBoxSpectrum.Cursor = Cursors.HSplit;
                return;  // since we don't want any other controls active along with squench
            }
            if (AGCHang.Contains(e.X, e.Y))
            {
                if (agcHangEnabled)
                {
                    AGCHangDrag = true;
                    SquelchDrag = false;
                    AGCKneeDrag = false;
                    pictureBoxSpectrum.Cursor = Cursors.HSplit;
                    return;
                }
                else
                {
                    AGCHangDrag = false;
                }
            }
            if (AGCKnee.Contains(e.X, e.Y))
            {
                AGCKneeDrag = true;
                AGCHangDrag = false;
                SquelchDrag = false;
                pictureBoxSpectrum.Cursor = Cursors.HSplit;
                return;
            }
            
            // check if we are adjacent to the right filter location, if so change the mouse cursor to <-> and 
            // disable mouse drag tuning of the frequency and filter shift
            if (Control.MouseButtons == MouseButtons.Left && MousePositionX > FilterRight - 5 && MousePositionX < FilterRight)
            {
                pictureBoxSpectrum.Cursor = Cursors.SizeWE; // change cursor to <->
                FrequencyDrag = false;   // turn off drag tuning of frequency and filter shift 
                BandwidthShift = false;
                HighFilter = true;      // allow drag tuning of high filter frequency
                LowFilter = false;
            }
            // check if we are adjacent to the left filter location, if so change the mouse cursor to <-> and 
            // disable mouse drag tuning of the frequency and filter shift
            else if (Control.MouseButtons == MouseButtons.Left && MousePositionX > FilterLeft && MousePositionX < FilterLeft + 5)
            {
                pictureBoxSpectrum.Cursor = Cursors.SizeWE; // change cursor to <->
                FrequencyDrag = false;      // turn off drag tuning of frequency and filter shift 
                BandwidthShift = false;
                HighFilter = false;
                LowFilter = true;           // allow drag tuning of low filter frequency
            }

            // if the mouse postion is to the left or right of the filter location then change the mouse cursor to a hand
            // and enable frequency drag. If we have right clicked then ShowCross will be true in which case set the frequency  
            // to the current mouse position.
            else if (Control.MouseButtons == MouseButtons.Left && (MousePositionX < FilterLeft || MousePositionX > FilterRight))
            {
                pictureBoxSpectrum.Cursor = Cursors.Hand; // change cursor to a hand

                FrequencyDrag = true;  // we are not inside the filter bandwidth so enable drag tuning

                // if the cross is shown then set frequency to the X location of the mouse
                if (ShowCross)
                {
                    // calculate the number of Hz each pixel represents, will be  sample rate/screen bandscope width
                    int HzPerPixel = SampleRate / pictureBoxSpectrum.Width;
                    // calculate the  how many Hz the X mouse location represents from the left edge of the screen
                    int offset = MousePositionX * HzPerPixel;
                    // the frequency at the left edge of the screen will be the tuned frequency - sample rate/2
                    int left_edge_frequency = trackBarSetFrequency.Value - SampleRate / 2;
                    // add the offset to the left edge frequency
                    // do so in a way that does NOT generate an exception when going too low in frequency
                    if (left_edge_frequency + offset <= trackBarSetFrequency.Minimum)
                    {
                        trackBarSetFrequency.Value = trackBarSetFrequency.Minimum;
                    }
                    else if (left_edge_frequency + offset >= trackBarSetFrequency.Maximum)
                    {
                        trackBarSetFrequency.Value = trackBarSetFrequency.Maximum;
                    }
                    else
                    {
                        if (Mode.Text == "CWL")  // TODO: Hack so that click tune on CWL mode works - need to fix
                            trackBarSetFrequency.Value = left_edge_frequency + offset + 1000;
                        else
                            trackBarSetFrequency.Value = left_edge_frequency + offset;
                    }
                    // display the new frequency and send to Ozy
                    DisplayFrequency(trackBarSetFrequency.Value);
                }
            }
            // if the right mouse button is down then show large cross 
            else if (Control.MouseButtons == MouseButtons.Right)  // change the cursor to to a large cross
            {
                pictureBoxSpectrum.Cursor = Cursors.Cross;         // change cursor to a small cross
                // set the ShowCross flag, this is read in the DrawGrid() Method
                ShowCross = !ShowCross;                     // toggle the cross each right click
                FrequencyDrag = true;
            }

            else // we are inside the filter bandwidth so disable frequency drag tuning
            {
                FrequencyDrag = false;
                pictureBoxSpectrum.Cursor = Cursors.NoMoveHoriz;
            }
        }

        private void pictureBoxSpectrum_MouseEnter(object sender, EventArgs e)
        {
            pictureBoxSpectrum.Cursor = Cursors.Cross;
        }

        private void pictureBoxSpectrum_MouseLeave(object sender, EventArgs e)
        {
            pictureBoxSpectrum.Cursor = Cursors.Default;
            SquelchDrag = false;
            AGCHangDrag = false;
            AGCKneeDrag = false;
        }

        int mouse_delta_Y;

        private void pictureBoxSpectrum_MouseMove(object sender, MouseEventArgs e)
        {
            // get the current cursor shape so we can restore it upon leaving 
            Cursor CurrentCursor = pictureBoxSpectrum.Cursor;

            MousePositionX = e.X;   // get the mouse X and Y coordintes with respect to the bandscope
            MousePositionY = e.Y;   // these are used by the right mouse click over bandscope             
            Point MouseX = new Point();  // the mouse coordinates have a type Point
            MouseX = Control.MousePosition;  // get current mouse postion (X,Y) with respect to the screen
                                             // so we can drag tune outside the width of the bandscope area 
            delta = MouseOld.X - MouseX.X;  // get the difference between the last and current Mouse X coordinate
            int abs_delta = Math.Abs(delta);
            mouse_delta_Y = MouseOld.Y - MouseX.Y; // get the diffetence between the last and current Mouse Y coordinate
            int abs_mouse_delta_Y = Math.Abs(mouse_delta_Y);

            // check if the user has clicked on the squelch control rectangle, if so move the squelch threshold with the mouse
            if (SquelchDrag)
            {
                if (mouse_delta_Y < 0 )  // mouse has moved down 
                {     
                    if (Squelch_level.Value/10 > (GridMin + abs_mouse_delta_Y)) // check we can make a valid move
                        Squelch_level.Value -= abs_mouse_delta_Y * (512 * 3/pictureBoxSpectrum.Height);
                    //  512 * 3/pictureBoxSpectrum.Height is a magic number to get the line to move at a nice speed!
                }

                else if (mouse_delta_Y > 0) // mouse has moved up
                {
                    if(Squelch_level.Value/10 < (GridMax - abs_mouse_delta_Y))  // check we can make a valid move
                        Squelch_level.Value += abs_mouse_delta_Y * (512 * 3 / pictureBoxSpectrum.Height);
                }

                // update text box
                Squelch_level_Scroll(this, EventArgs.Empty);
                MouseOld = MouseX; // save the last mouse X location for next time the mouse moves
                return;  // so other mouse controls don't operate 
            }

            if (agcHangEnabled && AGCHangDrag)
            {
                double agc_hang_point = PixelToDb(e.Y);
                // if (agc_hang_point > GridMax) agc_hang_point = GridMax;
                // if (agc_hang_point < -121.0) agc_hang_point = -121.0;
                agc_hang_point = agc_hang_point - (double)cal_offset;

                AGCHangLevel = agc_hang_point;

                int hang_threshold = AGCHangThreshold;
                if (SetupFormValid())
                    Setup_form.ChangeHangThresh(hang_threshold);

                MouseOld = MouseX; // save the last mouse X location for next time the mouse moves
                return;
            }

            if (AGCKneeDrag)
            {
                double agc_thresh_point = PixelToDb(e.Y);
                // if (agc_thresh_point > GridMax) agc_thresh_point = GridMax;
                // if (agc_thresh_point < GridMin) agc_thresh_point = GridMin;
                agc_thresh_point = agc_thresh_point - (double)cal_offset;
                // Debug.WriteLine("agc_db_point2: " + agc_db_point);

                AGCThreshDB = agc_thresh_point;

                double agc_top = AGCMaximumGainDB;
                agc_top = Math.Round(agc_top);

                SafeTrackBarValueChange((int)agc_top, AGCTrackBar);
                AGCTrackBar_Scroll(this, EventArgs.Empty);  // force AGC Threshold to accept previous saved value

                MouseOld = MouseX; // save the last mouse X location for next time the mouse moves
                return;
            }

            if (Control.MouseButtons == MouseButtons.Left && FrequencyDrag) // do this if the left mouse button is down
            {
                // if mouse has moved left then decrease frequency but first 
                // we need to check if we are too close to the band edges to move 
                if (delta < 0)
                {
                    if (trackBarSetFrequency.Value > (trackBarSetFrequency.Minimum + abs_delta * (SampleRate / pictureBoxSpectrum.Width)))
                        trackBarSetFrequency.Value -= abs_delta * (SampleRate / pictureBoxSpectrum.Width);
                }
                // if mouse has moved right then decrease frequency but first
                // we need to check if we are too close to the band edges to move  
                else if (delta > 0)
                {
                    if (trackBarSetFrequency.Value < (trackBarSetFrequency.Maximum - abs_delta * (SampleRate / pictureBoxSpectrum.Width)))
                        trackBarSetFrequency.Value += abs_delta * (SampleRate / pictureBoxSpectrum.Width);
                }

                DisplayFrequency(trackBarSetFrequency.Value);
                UpdateGraphs();
            }

            // Allow user to alter  filter  high and low frequencies using mouse.
            // If the mouse is in the region of the left edge of the filter allow it to be drag tuned
            // Placing the mouse in this region will alter the cursor, if the left mouse button is
            // down then the filter can be altered by draging the mouse left or right.
            // If the user has pressed the left mouse button whilst in this region then LowFilter will be true

            else if (MousePositionX > FilterLeft && MousePositionX < FilterLeft + 5 || LowFilter)
            {
                pictureBoxSpectrum.Cursor = Cursors.SizeWE;            // change cursor to <->
                if (Control.MouseButtons == MouseButtons.Left)
                {
                    BandwidthShift = false; // turn off bandwith shifting with mouse
                    if (delta > 0) // then we are draging to the left  and delta is positive
                    {
                        if (Mode.Text == "LSB" || Mode.Text == "CWL")
                        {
                            if (rcvr.FilterFrequencyHigh > -9999.0f)
                                rcvr.FilterFrequencyHigh -= delta * 100;
                        }
                        else
                        {
                            if (rcvr.FilterFrequencyLow > -9999.0f)
                                rcvr.FilterFrequencyLow -= delta * 100;
                        }
                    }
                    if (delta < 0) // then we are draging to the right  and delta is negative
                    {
                        if (Mode.Text == "LSB" || Mode.Text == "CWL")
                        {
                            if (rcvr.FilterFrequencyHigh < 9999.0f)
                                rcvr.FilterFrequencyHigh -= delta * 100;
                        }
                        else
                        {
                            if (rcvr.FilterFrequencyLow < 9999.0f)
                                rcvr.FilterFrequencyLow -= delta * 100;
                        }
                    }
                }
            }

            // If the mouse is in the region of the right edge of the filter allow it to be drag tuned
            // Placing the mouse in this region will alter the cursor, if the left mouse button is
            // down then the filter can be altered by draging the mouse left or right.
            // If the user has pressed the left mouse button whilst in this region then HighFilter will be true
            else if (MousePositionX > FilterRight - 5 && MousePositionX < FilterRight || HighFilter)
            {
                pictureBoxSpectrum.Cursor = Cursors.SizeWE;        // change cursor to <->
                if (Control.MouseButtons == MouseButtons.Left)
                {
                    BandwidthShift = false; // turn off bandwith shifting with mouse
                    if (delta > 0) // then we are draging to the left and delta is positive
                    {
                        if (Mode.Text == "LSB" || Mode.Text == "CWL") // need to invert if so
                        {
                            if (rcvr.FilterFrequencyLow > -9999.0f)
                                rcvr.FilterFrequencyLow -= delta * 100;
                        }
                        else
                        {
                            if (rcvr.FilterFrequencyHigh > -9999.0f)
                                rcvr.FilterFrequencyHigh -= delta * 100;
                        }

                    }
                    if (delta < 0) // then we are dragging to the right and delta is negative
                    {
                        if (Mode.Text == "LSB" || Mode.Text == "CWL") // need to invert if so
                        {
                            if (rcvr.FilterFrequencyLow < 9999.0f)
                                rcvr.FilterFrequencyLow -= delta * 100;
                        }
                        else
                        {
                            if (rcvr.FilterFrequencyHigh < 9999.0f)
                                rcvr.FilterFrequencyHigh -= delta * 100;
                        }
                    }

                }
            }

            // If the mouse is over the filter bandwith  change the cursor. If the left mouse button is also down then 
            // drag tune the filter frequency i.e passband tuning
            else if (MousePositionX > FilterLeft + 5 && MousePositionX < FilterRight - 5 || !FrequencyDrag && BandwidthShift)
            {
                pictureBoxSpectrum.Cursor = Cursors.NoMoveHoriz;

                if (Control.MouseButtons == MouseButtons.Left && !FrequencyDrag) // and if the left mouse is down move filter
                {
                    // determine how wide the current filter is
                    int FilterWidth = Math.Abs((int)(rcvr.FilterFrequencyLow - rcvr.FilterFrequencyHigh));
                    {
                        if (delta < 0)  // then we are draging to the right and delta is negative
                        {
                            if (Mode.Text == "LSB" || Mode.Text == "CWL")
                            {
                                if (rcvr.FilterFrequencyLow < 9999.0f)  // only drag to the maximum bandwidth
                                {
                                    rcvr.FilterFrequencyLow -= delta * 100;
                                    rcvr.FilterFrequencyHigh = rcvr.FilterFrequencyLow - FilterWidth;
                                }
                            }
                            else
                            {
                                if (rcvr.FilterFrequencyHigh < 9999.0f)  // only drag to the maximum bandwidth
                                {
                                    rcvr.FilterFrequencyLow -= delta * 100;
                                    rcvr.FilterFrequencyHigh = rcvr.FilterFrequencyLow + FilterWidth; // all other modes
                                }
                            }
                        }
                        if (delta > 0)  // then we are draging to the left and delta is positive
                        {

                            if (Mode.Text == "LSB" || Mode.Text == "CWL")
                            {
                                if (rcvr.FilterFrequencyHigh > -9999.0f) // only drag to the maximum bandwidth
                                {
                                    rcvr.FilterFrequencyHigh -= delta * 100;
                                    rcvr.FilterFrequencyLow = rcvr.FilterFrequencyHigh + FilterWidth;
                                }
                            }
                            else
                            {
                                if (rcvr.FilterFrequencyLow > -9999.0f) // only drag to the maximum bandwidth
                                {
                                    rcvr.FilterFrequencyHigh -= delta * 100;
                                    rcvr.FilterFrequencyLow = rcvr.FilterFrequencyHigh - FilterWidth;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                pictureBoxSpectrum.Cursor = Cursors.Cross;  // restore cursor
            }

            MouseOld = MouseX; // save the last mouse X location for next time the mouse moves
        }

        private void pictureBoxSpectrum_MouseUp(object sender, MouseEventArgs e)
        {
            pictureBoxSpectrum.Cursor = Cursors.Cross;
            FrequencyDrag = true;
            BandwidthShift = true;
            HighFilter = false;
            LowFilter = false;
            SquelchDrag = false;
            AGCHangDrag = false;
            AGCKneeDrag = false;
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {
            UpdateGraphs();
        }

        // show the squelch level the user has set
        private void Squelch_level_Scroll(object sender, EventArgs e)
        {
            Squelch_setting.Text = (Squelch_level.Value/10).ToString();
        }

        private bool squelchOn = false;

        private bool bandscopeSquelch = false;

        private void Bandscope_squelch_CheckedChanged(object sender, EventArgs e)
        {
            bandscopeSquelch = Bandscope_squelch.Checked;

            if (bandscopeSquelch)
            {
                if (filterSquelch)
                    Filter_squelch.Checked = false;

                squelchOn = true;
            }
            else
            {
                if (!filterSquelch)
                {
                    squelchOn = false;
                    setVolumeSquelched();
                }
            }
        }

        private bool filterSquelch = false;
        
        private void Filter_squelch_CheckedChanged(object sender, EventArgs e)
        {
            filterSquelch = Filter_squelch.Checked;

            if (filterSquelch)
            {
                if (bandscopeSquelch)
                    Bandscope_squelch.Checked = false;

                squelchOn = true;
            }
            else
            {
                if (!bandscopeSquelch)
                {
                    squelchOn = false;
                    setVolumeSquelched();
                }
            }
        }
        
        // set the drive level to Penelope 
        public void DriveLevel_Scroll(object sender, EventArgs e)
        {
            Drive.Text = DriveLevel.Value.ToString();
            // apply square law to drive level control
            DriveGain = (float)Math.Pow(DriveLevel.Value, 2) / 10000;  // DriveGain is float from 0 to 1.0
        }

        // Toggle the MOX button 
        private void MOX_Click(object sender, EventArgs e)
        {
            if (KK_on) // only allow MOX to be set when radio is on 
            {
                if (MOX.BackColor == SystemColors.Control)
                {
                    MOX.BackColor = Color.Green;
                    MOX_set = true;
                }
                else
                {
                    MOX.BackColor = SystemColors.Control;
                    MOX_set = false;
                    if (TUN_set)            // Force TUN off if MOX is turning off
                    {
                        TUN.BackColor = SystemColors.Control;
                        TUN_set = false;
                        DriveLevel.Value = PreviousDrive;   // restore the previous drive level
                    }
                }
            }

            DriveLevel_Scroll(this, EventArgs.Empty); // force drive level update
            setVolumeSquelched();  // check if we need to mute the receiver audio
        }

        // Toggle the TUN (Tune) button
        int PreviousDrive;

        private void TUN_Click(object sender, EventArgs e)
        {
            if (KK_on) // only allow TUN to be set when radio is on 
            {
                if (TUN.BackColor == SystemColors.Control)
                {
                    TUN.BackColor = Color.Green;
                    TUN_set = true;
                    PreviousDrive = DriveLevel.Value;  // keep the current drive level so we can restore it
                    // reduce the drive level by the percentage set by the user on the Setup form
                    // and apply square law to the control
                    DriveLevel.Value = (int)((float)Math.Pow(DriveLevel.Value,2)/10000 * (float)(TuneLevel / 100.0f));
                    MOX_set = true;                     // Need MOX on when Tune is set 
                    MOX.BackColor = Color.Green;
                }
                else
                {
                    TUN.BackColor = SystemColors.Control;
                    TUN_set = false;
                    MOX_set = false;                   // force MOX off 
                    MOX.BackColor = SystemColors.Control;

                    if (PennyLane || Hermes)
                    {
                        // sleep if PennyLane or Hermes so that no output spik3e is observed
                        System.Threading.Thread.Sleep(500);
                    }

                    DriveLevel.Value = PreviousDrive;   // restore the previous drive level
                }
            }

            DriveLevel_Scroll(this, EventArgs.Empty); // force drive level update
        }
        
        HPSDRDevice ourDevice = null;

        //especially for MethodInvoker to use from another thread.
        public void ToggleOnOffButton()
        {
            OnOffButton_Click(this, EventArgs.Empty);
        }

        public void OnOffButton_Click(object sender, EventArgs e)
        {
            if (OnOffButton.BackColor == SystemColors.Control)
            {
                OnOffButton.BackColor = Color.Green;
                OnOffButton.Text = "ON";
                if (KKDevice == KKMethod.USB)
                {
                    ourDevice = new USBDevice(this);
                    IsInSequence = true;
                }
                else
                {
                    ourDevice = new EthernetDevice(this);
                }

                // make sure any changes made while not running are set to the new ourDevice instance
                SetTXFilterBandwidth();

                KK_on = true;
                ourDevice.Start();
            }
            else
            {
                OnOffButton.BackColor = SystemColors.Control;
                OnOffButton.Text = "OFF";                                           
                PTT = false;                                // force PTT off
                MOX_set = false;                            // force MOX off
                previous_OnlyTxOnPTT = OnlyTxOnPTT;         // save this status so we can restore it
                OnlyTxOnPTT = true;                         // force always PTT off 
                TUN.BackColor = SystemColors.Control;
                if (TUN_set)
                {
                    DriveLevel.Value = PreviousDrive;            // restore the previous drive level
                    DriveLevel_Scroll(this, EventArgs.Empty);   // force drive level update as TUN is on 
                }
                TUN_set = false;                            // force Tune off
                MOX.BackColor = SystemColors.Control;                           
                Thread.Sleep(100);                          // Delay so the PTT off is sent to Ozy
                ourDevice.Stop();
                ourDevice = null;
                OnlyTxOnPTT = previous_OnlyTxOnPTT;         // restore the previous status 
                KK_on = false;                              // set this last so PTT off gets sent
            }            
        }

        public bool ProcessorOn;
        private void chkClipper_CheckedChanged(object sender, EventArgs e)
        {
            if (chkClipper.Checked)
            {
                ProcessorOn = true;
                ProcessorGain.Enabled = true;
                ProcGaindB.Enabled = true;
            }
            else  // disable the controls
            {
                ProcessorOn = false;
                ProcessorGain.Enabled = false;
                ProcGaindB.Enabled = false;
            }
            // renew the control value
            ProcessorGainChange();
        }

        public bool BassCutOn;
        private void chkBassCut_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBassCut.Checked)
            {
                BassCutOn = true;
            }
            else
            {
                BassCutOn = false;
            }
        }

        public bool VOXOn;

        private void chkVOX_CheckedChanged(object sender, EventArgs e)
        {
            VOXOn = chkVOX.Checked;
            VOXLevel.Enabled = VOXOn;
            textBoxVOXLevel.Enabled = VOXOn;
            VOXHangTime.Enabled = VOXOn;
        }

        public bool ClipOn;
        bool ClipWasOn;

        private void HandleClip()
        {
            if (ClipOn && PTTEnable)  // only display clip LED if PTT active
            {
                labelClipLED.BackColor = Color.Green;
                ClipWasOn = true;
                ClipOn = false;
            }
            else if (ClipWasOn)
            {
                ClipWasOn = false;
                labelClipLED.BackColor = SystemColors.Control;
            }
        }

        // timer2 determines the length of time the Clip LED is on 
        private void timer2_Tick(object sender, EventArgs e)
        {
            HandleClip();
        }

        public bool VOXDelay;

        // timer 3 determines the VOX hang time
        private void timer3_Tick(object sender, EventArgs e)
        {
            timer3.Interval = (int)VOXHangTime.Value;
            if (VOXOn && !TUN_set)  // don't enable VOX when Tuning
            {
                if (VOXDelay)  // set when mic signal is > threshold
                {
                    MOX_set = true;
                    VOXDelay = false;
                }
                else MOX_set = false;
            }
        }

        public bool NoiseGateDelay;
        public bool NoiseGateSwitch;

        private void timer4_Tick(object sender, EventArgs e)
        {
            timer4.Interval = 100;
            if (NoiseGate)
            {
                if (NoiseGateDelay)         // set when mic signal is > threshold
                {
                    NoiseGateSwitch = true;
                    NoiseGateDelay = false;
                }
                else NoiseGateSwitch = false;
            }
        }

        public float MicGain;

        private void MicrophoneGain_Scroll(object sender, EventArgs e)
        {
            // apply square law to gain control and allow max 20dB of gain 
            MicGain = (float) Math.Pow(MicrophoneGain.Value, 2)/1000.00f;
            MicGain = Math.Min(MicGain, 20.0f);

            textBoxMicGain.Text = MicGain.ToString("0.0"); //MicrophoneGain.Value.ToString();    
        }

        public bool NoiseGate;

        private void chkNoiseGate_CheckedChanged(object sender, EventArgs e)
        {
            NoiseGate = chkNoiseGate.Checked;
            NoiseGateLevel.Enabled = NoiseGate;
            textBoxNoiseGate.Enabled = NoiseGate;
        }

        public float NoiseThreshold;

        private void NoiseGateLevel_Scroll(object sender, EventArgs e)
        {
            NoiseThreshold = 1.0f - (float)Math.Pow(NoiseGateLevel.Value,2) / 10000.0f;

            textBoxNoiseGate.Text = NoiseThreshold.ToString("0.0");
        }

        private void ProcessorGainChange()
        {
            // get the Processor gain
            ProcGain = ProcessorGain.Value / 10.0f;
            // display the Processor Gain in dB
            ProcGaindB.Text = (20 * Math.Log10(ProcGain)).ToString("0.0");
        }

        private void ProcessorGain_Scroll(object sender, EventArgs e)
        {
            ProcessorGainChange();
        }

        private void ProcessorGain_ValueChanged(object sender, EventArgs e)
        {
            ProcessorGainChange();
        }

        private void VOXLevelChange()
        {
            //get the VOX threshold, apply square law to the control 
            VOXThreshold = 1.0f - (float)Math.Pow(VOXLevel.Value,2) / 10000.0f;

            textBoxVOXLevel.Text = VOXThreshold.ToString("0.0");
        }

        private void VOXLevel_Scroll(object sender, EventArgs e)
        {
            VOXLevelChange();
        }

        private void VOXLevel_ValueChanged(object sender, EventArgs e)
        {
            VOXLevelChange();
        }


        public List<NetworkInterface> foundNics = new List<NetworkInterface>();

        // this will help us figure out which entries in the found Metis/Hermes/Griffin board list
        // are valid by finding out which replies are on the proper port!
        // This seems necessary because, for example, Phil VK6APH seems to have his Metis board
        // show up on BOTH ethernet ports!  Likely a wiring issue for his ethernet, but
        // if we can sort that out based on which port the discovery reply came in on, and whether
        // that port is on the same subnet with the board.
        public List<NicProperties> nicProperties = new List<NicProperties>();

        public void GetNetworkInterfaces()
        {
            // creat a string that contains the name and speed of each Network adapter 
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();

            foundNics.Clear();
            nicProperties.Clear();

            Network_interfaces = "";
            int adapterNumber = 1;

            foreach (NetworkInterface adapter in nics)
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();

                // if it's not 'up' (operational), ignore it.  (Dan Quigley, 13 Aug 2011)
                if (adapter.OperationalStatus != OperationalStatus.Up)
                    continue;

                // if it's a loopback interface, ignore it!
                if (adapter.NetworkInterfaceType == NetworkInterfaceType.Loopback)
                    continue;

                // get rid of non-ethernet addresses
                if ((adapter.NetworkInterfaceType != NetworkInterfaceType.Ethernet) && (adapter.NetworkInterfaceType != NetworkInterfaceType.GigabitEthernet))
                    continue;

                Console.WriteLine("");      // a blank line
                Console.WriteLine(adapter.Description);
                Console.WriteLine(String.Empty.PadLeft(adapter.Description.Length, '='));
                Console.WriteLine("  Interface type .......................... : {0}", adapter.NetworkInterfaceType);
                Console.WriteLine("  Physical Address ........................ : {0}", adapter.GetPhysicalAddress().ToString());
                Console.WriteLine("  Is receive only.......................... : {0}", adapter.IsReceiveOnly);
                Console.WriteLine("  Multicast................................ : {0}", adapter.SupportsMulticast);
                Console.WriteLine("  Speed    ................................ : {0}", adapter.Speed);

                // list unicast addresses
                UnicastIPAddressInformationCollection c = properties.UnicastAddresses;
                foreach (UnicastIPAddressInformation a in c)
                {
                    IPAddress addr = a.Address;

					if (a.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork) {
						Console.WriteLine ("  Unicast Addr ............................ : {0}", addr.ToString ());
						IPAddress mask = null;

						try {
							mask = a.IPv4Mask;
						} catch (NotImplementedException e) {
							// IPv4Mask does not yet exist in Mono
							String subnetMask = IPInfoTools.GetIPv4Mask (adapter.Name, addr);
							mask = IPAddress.Parse (subnetMask);
						}

						Console.WriteLine ("  Unicast Mask ............................ : {0}", (mask == null ? "null" : mask.ToString ()));

						NicProperties np = new NicProperties ();
						np.ipv4Address = a.Address;
						np.ipv4Mask = mask;

						nicProperties.Add (np);
					}
                }

                // list multicast addresses
                MulticastIPAddressInformationCollection m = properties.MulticastAddresses;
                foreach (MulticastIPAddressInformation a in m)
                {
                    IPAddress addr = a.Address;
                    Console.WriteLine("  Multicast Addr .......................... : {0}", addr.ToString());
                }

                // if the length of the network adapter name is > 31 characters then trim it, if shorter then pad to 31.
                // Need to use fixed width font - Courier New
                string speed = "  " + (adapter.Speed / 1000000).ToString() + "T";
                if (adapter.Description.Length > 31)
                {
                    Network_interfaces += adapterNumber++.ToString() + ". " + adapter.Description.Remove(31) + speed + "\n";
                }
                else
                {
                    Network_interfaces += adapterNumber++.ToString() + ". " + adapter.Description.PadRight(31, ' ') + speed + "\n";
                }

                foundNics.Add(adapter);
            }

            Console.WriteLine(Network_interfaces);

            // display number of adapters on Setup form
            numberOfIPAdapters = (adapterNumber - 1).ToString();
        }

        private void deviceConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // test whether running or not.  If running, warn and bail out
            if (KK_on)
            {
                MessageBox.Show("You press the ON button to stop data before changing device connectivity method", "KISS is ON!");
                return;
            }

            // possible way to choose Ethernet or USB ...
            if (SetupFormValid())
            {
                Setup_form.Close();
            }
            Setup_form = null;

            // show form to choose USB or Ethernet
            DeviceTypeForm dtf = new DeviceTypeForm(KKDevice);

            KKMethod oldDevice = KKDevice;

            DialogResult dr = dtf.ShowDialog();
            if (dr == DialogResult.OK)
            {
                KKDevice = dtf.GetChosenDeviceType();

                if (KKDevice != oldDevice)
                    ourDevice = null;
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KISSAboutBox k = new KISSAboutBox();
            k.ShowDialog();
        }

        private string dataFileName = null;

        public void disableDataLogging()
        {
            enableDataLoggingToolStripMenuItem.Checked = false;
            if (ourDevice != null)
            {
                ourDevice.CloseFullBandwidthDataFile();
            }
        }

        public void enableDataLogging()
        {
            enableDataLoggingToolStripMenuItem.Checked = true;
            if (ourDevice != null && (dataFileName != null))
            {
                ourDevice.OpenFullBandwidthDataFile(dataFileName);
            }
        }

        private void enableDataLoggingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (enableDataLoggingToolStripMenuItem.Checked)
            {
                disableDataLogging();
            }
            else
            {
                enableDataLogging();
            }
        }

        private void specifyFilenameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ourDevice != null)
            {
                DialogResult dr = saveFileDialogDataLogging.ShowDialog();
                if (dr == DialogResult.OK)
                {
                    ourDevice.CloseFullBandwidthDataFile();

                    dataFileName = saveFileDialogDataLogging.FileName;
                }
            }
        }
    }

    public enum OperMode : int
    {
        AM,
        FM,
        SAM,
        USB,
        LSB,
        CWU,
        CWL
    };

    public enum OpBand : int
    {
        M160,
        M80,
        M60,
        M40,
        M30,
        M20,
        M17,
        M15,
        M12,
        M10,
        M6,
        GC,
        XVRT1,
        XVRT2
    };


    public enum DeviceType : int
    {
        Metis = 0,
        Hermes = 1,
        Griffin = 2
    }

    public class MetisHermesDevice
    {
        public DeviceType deviceType;   // which type of device (currently Metis or Hermes)
        public byte codeVersion;        // reported code version type
        public bool InUse;              // whether already in use
        public string IPAddress;        // currently, an IPV4 address
        public string MACAddress;       // a physical (MAC) address
        public IPAddress hostPortIPAddress;
    }

    public class NicProperties
    {
        public IPAddress ipv4Address;
        public IPAddress ipv4Mask;
    }

    public enum KKMethod : int
    {
        USB = 0,
        Ethernet = 1
    }
}


