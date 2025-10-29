// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubePrgAPI.StLink
{
    using System;
    using System.Linq;
    using SharpCubePrgAPI.User;
    using SharpCubeProgrammer.Enum;
    using SharpCubeProgrammer.Interface;
    using SharpCubeProgrammer.Struct;

    internal static class Example2
    {
        internal static int Example(ICubeProgrammerApi cubeProgrammerApi)
        {
            DisplayManager.LogMessage(MessageType.Title, "\n+++ Example 2 +++\n\n");

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

                /* Reading 256 bytes from 0x08000000 */
                var size = 256;
                var startAddress = "0x08000000";

                var readMemoryFlag = cubeProgrammerApi.ReadMemory(startAddress, size);
                if (readMemoryFlag.Item1 != 0)
                {
                    cubeProgrammerApi.Disconnect();
                    continue;
                }

                DisplayManager.LogMessage(MessageType.Normal, "\nReading 32-bit memory content\n");
                DisplayManager.LogMessage(MessageType.Normal, $"  Size          : {size} Bytes\n");
                DisplayManager.LogMessage(MessageType.Normal, $"  Address:      : {startAddress}\n");

                var i = 0;
                int col;

                while (i < size)
                {
                    col = 0;
                    var hexAddress = cubeProgrammerApi.HexConverterToUint(startAddress);
                    hexAddress += (uint)i;
                    DisplayManager.LogMessage(MessageType.Normal, $"\n{cubeProgrammerApi.HexConverterToString(hexAddress)} :");
                    while (col < 4 && i < size)
                    {
                        var buffer = new byte[4];
                        Array.Copy(readMemoryFlag.Item2, i, buffer, 0, 4);
                        DisplayManager.LogMessage(MessageType.Info, $" {BitConverter.ToString(buffer.Reverse().ToArray()).Replace("-", String.Empty)} ");
                        col++;
                        i += 4;
                    }
                }
                DisplayManager.LogMessage(MessageType.Normal, "\n");

                /* Run application */
                var executeFlag = cubeProgrammerApi.Execute("0x08000000");
                if (executeFlag != 0)
                {
                    cubeProgrammerApi.Disconnect();
                    continue;
                }
                /* The system will lose the connection with bootloader when it is in running mode */

                /* Process successfully Done */
                cubeProgrammerApi.Disconnect();

            }

            return 1;
        }
    }
}
