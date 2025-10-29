// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubePrgAPI.Bootoader
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using SharpCubePrgAPI.User;
    using SharpCubeProgrammer.Enum;
    using SharpCubeProgrammer.Interface;
    using SharpCubeProgrammer.Struct;

    internal static class UsbExample
    {
        internal static int Example(ICubeProgrammerApi cubeProgrammerApi)
        {
            DisplayManager.LogMessage(MessageType.Title, "\n+++ USB Bootloader Example +++\n\n");

            var dfuList = new List<DfuDeviceInfo>();
            var getDfuListNb = cubeProgrammerApi.GetDfuDeviceList(ref dfuList, 0xDF11, 0x0483);

            if (getDfuListNb == 0)
            {
                DisplayManager.LogMessage(MessageType.Error, "No USB DFU available\n");
                return 0;
            }
            else
            {
                DisplayManager.LogMessage(MessageType.Title, "\n------------- USB DFU List --------------\n");
                for (var j = 0; j < getDfuListNb; j++)
                {
                    DisplayManager.LogMessage(MessageType.Normal, $"USB Port {j} \n");
                    DisplayManager.LogMessage(MessageType.Info, $"	USB index   : {dfuList[j].UsbIndex} \n");
                    DisplayManager.LogMessage(MessageType.Info, $"	USB SN      : {dfuList[j].SerialNumber} \n");
                    DisplayManager.LogMessage(MessageType.Info, $"	DFU version : {dfuList[j].DfuVersion} ");
                }
                DisplayManager.LogMessage(MessageType.Title, "\n-----------------------------------------\n\n");
            }

            /* Target connect, choose the adequate USB port by indicating its index that is already mentioned in USB DFU List above */
            var usbConnectFlag = cubeProgrammerApi.ConnectDfuBootloader(dfuList[0].UsbIndex);
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
                return 0;
            }

            /* Reading 64 bytes from 0x08000000 */
            var size = 64;
            var startAddress = "0x08000000";

            var readMemoryFlag = cubeProgrammerApi.ReadMemory(startAddress, size);
            if (readMemoryFlag.Item1 != 0)
            {
                cubeProgrammerApi.Disconnect();
                return 0;
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

            return 1;
        }
    }
}
