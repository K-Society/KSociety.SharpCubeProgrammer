// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubePrgAPI.StLink
{
    using System;
    using System.Linq;
    using SharpCubePrgAPI.User;
    using SharpCubeProgrammer.Enum;
    using SharpCubeProgrammer.Interface;
    using SharpCubeProgrammer.Struct;

    internal static class Example1
    {
        internal static int Example(ICubeProgrammerApi cubeProgrammerApi)
        {
            DisplayManager.LogMessage(MessageType.Title, "\n+++ Example 1 +++\n\n");
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
                    DisplayManager.LogMessage(MessageType.Normal, $"\nDevice name : {genInfo?.Cpu} \n");
                }

                /* Apply mass Erase */
                var massEraseFlag = cubeProgrammerApi.MassErase();
                if (massEraseFlag != 0)
                {
                    cubeProgrammerApi.Disconnect();
                    continue;
                }

                /* Single word edition */
                var size = 4;
                var startAddress = "0x08000000";
                var data = new byte[]{ 0xAA, 0xAA, 0xBB, 0xBB, 0xCC, 0xCC, 0xDD, 0xDD };
                var writeMemoryFlag = cubeProgrammerApi.WriteMemory(startAddress, data, size);
                if (writeMemoryFlag != 0)
                {
                    cubeProgrammerApi.Disconnect();
                    continue;
                }

                /* Reading 64 bytes from 0x08000000 */
                size = 64;

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

                /* Sector Erase */
                var sectors = new uint[] { 0, 1, 2, 3 };  // we suppose that we have 4 sectors
                var sectorEraseFlag = cubeProgrammerApi.SectorErase(sectors, 1); // we will erase just the first sector
                if (sectorEraseFlag != 0)
                {
                    cubeProgrammerApi.Disconnect();
                    continue;
                }

                /* Reading 64 bytes from 0x08000000 */
                i = 0;
                size = 64;

                readMemoryFlag = cubeProgrammerApi.ReadMemory(startAddress, size);
                if (readMemoryFlag.Item1 != 0)
                {
                    cubeProgrammerApi.Disconnect();
                    continue;
                }

                DisplayManager.LogMessage(MessageType.Normal, "\nReading 32-bit memory content\n");
                DisplayManager.LogMessage(MessageType.Normal, $"  Size          : {size} Bytes\n");
                DisplayManager.LogMessage(MessageType.Normal, $"  Address:      : {startAddress}\n");

                while (i < size)
                {
                    col = 0;
                    var hexAddress = cubeProgrammerApi.HexConverterToUint(startAddress);
                    hexAddress += (uint)i;
                    DisplayManager.LogMessage(MessageType.Normal, $"\n{cubeProgrammerApi.HexConverterToString(hexAddress)} :" );
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
