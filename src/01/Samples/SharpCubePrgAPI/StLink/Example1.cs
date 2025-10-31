// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubePrgAPI.StLink
{
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
                Shared.DisplayDeviceInformations(cubeProgrammerApi);

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
                var readMemoryResult = Shared.ReadMemory(cubeProgrammerApi);
                if (readMemoryResult == 0)
                {
                    cubeProgrammerApi.Disconnect();
                    return 0;
                }

                /* Sector Erase */
                var sectors = new uint[] { 0, 1, 2, 3 };  // we suppose that we have 4 sectors
                var sectorEraseFlag = cubeProgrammerApi.SectorErase(sectors, 1); // we will erase just the first sector
                if (sectorEraseFlag != 0)
                {
                    cubeProgrammerApi.Disconnect();
                    continue;
                }

                /* Reading 64 bytes from 0x08000000 */
                readMemoryResult = Shared.ReadMemory(cubeProgrammerApi);
                if (readMemoryResult == 0)
                {
                    cubeProgrammerApi.Disconnect();
                    return 0;
                }

                /* Read Option bytes from target device memory */
                var readOptionBytesResult = Shared.ReadOptionBytes(cubeProgrammerApi);
                if (readOptionBytesResult == 0)
                {
                    cubeProgrammerApi.Disconnect();
                    return 0;
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
