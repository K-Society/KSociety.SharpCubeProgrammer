// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubePrgAPI.StLink
{
    using System.Linq;
    using SharpCubePrgAPI.User;
    using SharpCubeProgrammer.Enum;
    using SharpCubeProgrammer.Interface;
    using SharpCubeProgrammer.Struct;

    internal static class Example3
    {
        internal static int Example(ICubeProgrammerApi cubeProgrammerApi)
        {
            DisplayManager.LogMessage(MessageType.Title, "\n+++ Example 3 +++\n\n");

            DebugConnectParameters debugConnectParameters;
            var stLinkList = cubeProgrammerApi.GetStLinkEnumerationList();

            if (!stLinkList.Any())
            {
                DisplayManager.LogMessage(MessageType.Error, "No STLINK available\n");
                return 0;
            }
            else
            {
                DisplayManager.LogMessage(MessageType.Title, "\n-------- Connected ST-LINK Probes List --------");
                var probeIndex = 0;
                foreach (var stLink in stLinkList)
                {
                    DisplayManager.LogMessage(MessageType.Normal, $"\nST-LINK Probe {probeIndex} :\n");
                    DisplayManager.LogMessage(MessageType.Info, $" ST-LINK SN   : {stLink.SerialNumber} \n");
                    DisplayManager.LogMessage(MessageType.Info, $" ST-LINK FW   : {stLink.FirmwareVersion} \n");
                    probeIndex++;
                }
                DisplayManager.LogMessage(MessageType.Title, "-----------------------------------------------\n\n");
            }

            var index = 0;
            foreach (var stLink in stLinkList)
            {
                DisplayManager.LogMessage(MessageType.Title, $"\n--------------------- ");
                DisplayManager.LogMessage(MessageType.Title, $"\n ST-LINK Probe : {index} ");
                DisplayManager.LogMessage(MessageType.Title, $"\n--------------------- \n\n");

                debugConnectParameters = stLink;
                debugConnectParameters.ConnectionMode = DebugConnectionMode.UnderResetMode;
                debugConnectParameters.Shared = 0;

                /* Target connect */
                var connectStlinkFlag = cubeProgrammerApi.ConnectStLink(debugConnectParameters);
                if (connectStlinkFlag != 0)
                {
                    DisplayManager.LogMessage(MessageType.Error, "Establishing connection with the device failed\n");
                    cubeProgrammerApi.Disconnect();
                    continue;
                }
                else
                {
                    DisplayManager.LogMessage(MessageType.GreenInfo, $"\n--- Device {index} Connected --- \n");
                }
                index++;
                /* Display device informations */
                var genInfo = cubeProgrammerApi.GetDeviceGeneralInf();
                if (genInfo != null)
                {
                    DisplayManager.LogMessage(MessageType.Normal, $"\nDevice name : {genInfo?.Name} ");
                    DisplayManager.LogMessage(MessageType.Normal, $"\nDevice type : {genInfo?.Type} ");
                    DisplayManager.LogMessage(MessageType.Normal, $"\nDevice CPU : {genInfo?.Cpu} \n");
                }

                /* Download File + verification */
                const string filePath = @"..\..\..\..\..\Test\data.hex";
                uint isVerify = 1; //add verification step
                uint isSkipErase = 0; // no skip erase
                var downloadFileFlag = cubeProgrammerApi.DownloadFile(filePath, "0x08000000", isSkipErase, isVerify);
                if (downloadFileFlag != 0)
                {
                    cubeProgrammerApi.Disconnect();
                    continue;
                }

                /* Set rdp option byte */
                var sendOptionBytesCmdFlag = cubeProgrammerApi.SendOptionBytesCmd("-ob rdp=0xbb");
                if (sendOptionBytesCmdFlag != 0)
                {
                    cubeProgrammerApi.Disconnect();
                    continue;
                }

                /* Read Option bytes from target device memory */
                var ob = cubeProgrammerApi.InitOptionBytesInterface();
                if (ob == null)
                {
                    cubeProgrammerApi.Disconnect();
                    continue;
                }

                var j = 0;
                /* Display option bytes */
                foreach (var bank in ob?.Banks)
                {
                    DisplayManager.LogMessage(MessageType.Normal, $"OPTION BYTES BANK: {j}\n");
                    j++;

                    foreach (var categori in bank.Categories)
                    {
                        DisplayManager.LogMessage(MessageType.Title, $"\t{categori.Name}\n");

                        foreach (var bit in categori.Bits)
                        {
                            if (bit.Access == 0 || bit.Access == 2)
                            {
                                DisplayManager.LogMessage(MessageType.Normal, $"\t\t{bit.Name}:");
                                DisplayManager.LogMessage(MessageType.Info, $" {cubeProgrammerApi.HexConverterToString(bit.BitValue)}\n");
                            }
                        }
                    }
                }

                /* Disable readout protection */
                var readUnprotectFlag = cubeProgrammerApi.ReadUnprotect();
                if (readUnprotectFlag != 0)
                {
                    cubeProgrammerApi.Disconnect();
                    continue;
                }

                /* Display option bytes */
                ob = cubeProgrammerApi.InitOptionBytesInterface();
                if (ob == null)
                {
                    cubeProgrammerApi.Disconnect();
                    continue;
                }

                var jj = 0;
                /* Display option bytes */
                foreach (var bank in ob?.Banks)
                {
                    DisplayManager.LogMessage(MessageType.Normal, $"OPTION BYTES BANK: {j}\n");
                    jj++;

                    foreach (var categori in bank.Categories)
                    {
                        DisplayManager.LogMessage(MessageType.Title, $"\t{categori.Name}\n");

                        foreach (var bit in categori.Bits)
                        {
                            if (bit.Access == 0 || bit.Access == 2)
                            {
                                DisplayManager.LogMessage(MessageType.Normal, $"\t\t{bit.Name}:");
                                DisplayManager.LogMessage(MessageType.Info, $" {cubeProgrammerApi.HexConverterToString(bit.BitValue)}\n");
                            }
                        }
                    }
                }

                /* Apply a System Reset */
                var resetFlag = cubeProgrammerApi.Reset(DebugResetMode.SoftwareReset);
                if (resetFlag != 0)
                {
                    DisplayManager.LogMessage(MessageType.Error, "\nUnable to reset MCU!\n");
                    cubeProgrammerApi.Disconnect();
                    continue;
                }
                else
                {
                    DisplayManager.LogMessage(MessageType.GreenInfo, "\nSystem Reset is performed\n");
                }

                /* Process successfully Done */
                cubeProgrammerApi.Disconnect();

            }

            return 1;
        }
    }
}
