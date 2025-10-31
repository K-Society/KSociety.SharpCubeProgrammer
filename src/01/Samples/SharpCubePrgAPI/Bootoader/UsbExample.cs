// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubePrgAPI.Bootoader
{
    using System.Linq;
    using SharpCubePrgAPI;
    using SharpCubePrgAPI.User;
    using SharpCubeProgrammer.Enum;
    using SharpCubeProgrammer.Interface;

    internal static class UsbExample
    {
        internal static int Example(ICubeProgrammerApi cubeProgrammerApi)
        {
            DisplayManager.LogMessage(MessageType.Title, "\n+++ USB Bootloader Example +++\n\n");

            var dfuList = cubeProgrammerApi.GetDfuDeviceList(0xDF11, 0x0483);

            if (!dfuList.Any())
            {
                DisplayManager.LogMessage(MessageType.Error, "No USB DFU available\n");
                return 0;
            }
            else
            {
                DisplayManager.LogMessage(MessageType.Title, "\n------------- USB DFU List --------------\n");
                var dfuIndex = 0;
                foreach (var dfu in dfuList)
                {
                    DisplayManager.LogMessage(MessageType.Normal, $"USB Port {dfuIndex} \n");
                    DisplayManager.LogMessage(MessageType.Info, $"	USB index   : {dfu.UsbIndex} \n");
                    DisplayManager.LogMessage(MessageType.Info, $"	USB SN      : {dfu.SerialNumber} \n");
                    DisplayManager.LogMessage(MessageType.Info, $"	DFU version : {dfu.DfuVersion} ");
                    dfuIndex++;
                }
                DisplayManager.LogMessage(MessageType.Title, "\n-----------------------------------------\n\n");
            }

            /* Target connect, choose the adequate USB port by indicating its index that is already mentioned in USB DFU List above */
            var usbConnectFlag = cubeProgrammerApi.ConnectDfuBootloader(dfuList.First().UsbIndex);
            if (usbConnectFlag != 0)
            {
                cubeProgrammerApi.Disconnect();
                return 0;
            }
            else
            {
                DisplayManager.LogMessage(MessageType.GreenInfo, "\n--- Device Connected --- \n");
            }

            /* Display device informations */
            Shared.DisplayDeviceInformations(cubeProgrammerApi);

            /* Download File + verification */
            const string filePath = @"..\..\..\..\..\Test\data.hex";
            uint isVerify = 1; //add verification step
            uint isSkipErase = 0; // no skip erase
            var downloadFileFlag = cubeProgrammerApi.DownloadFile(filePath, "0x08000000", isSkipErase, isVerify);
            if (downloadFileFlag != 0)
            {
                cubeProgrammerApi.Disconnect();
                return 0;
            }

            /* Reading 64 bytes from 0x08000000 */
            var readMemoryResult = Shared.ReadMemory(cubeProgrammerApi);
            if (readMemoryResult == 0)
            {
                cubeProgrammerApi.Disconnect();
                return 0;
            }

            /* Option bytes programming : BOR level */
            var sendOptionBytesCmdFlag = cubeProgrammerApi.SendOptionBytesCmd("-ob BOR_LEV=1");
            if (sendOptionBytesCmdFlag != 0)
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

            /* Process successfully Done */
            cubeProgrammerApi.Disconnect();

            return 1;
        }
    }
}
