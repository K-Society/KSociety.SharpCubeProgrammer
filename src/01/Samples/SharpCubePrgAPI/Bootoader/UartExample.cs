// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubePrgAPI.Bootoader
{
    using System;
    using System.Linq;
    using SharpCubePrgAPI.User;
    using SharpCubeProgrammer.Enum;
    using SharpCubeProgrammer.Interface;

    internal static class UartExample
    {
        internal static int Example(ICubeProgrammerApi cubeProgrammerApi)
        {
            DisplayManager.LogMessage(MessageType.Title, "\n+++ UART Bootloader Example +++\n\n");

            var getUsartList = cubeProgrammerApi.GetUsartList();

            if (!getUsartList.Any())
            {
                DisplayManager.LogMessage(MessageType.Error, "NO Serial Port available\n");
                return 0;
            }
            else
            {
                DisplayManager.LogMessage(MessageType.Title, "\n------------- Serial Port List --------------\n");
                var portIndex = 0;
                foreach (var port in getUsartList)
                {
                    DisplayManager.LogMessage(MessageType.Normal, $"\nPort Name {portIndex} :");
                    DisplayManager.LogMessage(MessageType.Info, $" {port.portName} ");
                    portIndex++;
                }
                DisplayManager.LogMessage(MessageType.Title, "\n-----------------------------------------\n\n");
            }

            /* Target connect, choose the adequate Serial port by indicating its index that is already mentioned in Serial Port List above */
            var uartConnectFlag = cubeProgrammerApi.ConnectUsartBootloader(getUsartList.First());
            if (uartConnectFlag != 0)
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

            /* Option bytes programming : BOR level */
            var sendOptionBytesCmdFlag = cubeProgrammerApi.SendOptionBytesCmd("-ob BOR_LEV=1");
            if (sendOptionBytesCmdFlag != 0)
            {
                cubeProgrammerApi.Disconnect();
                return 0;
            }

            /* Read Option bytes from target device memory */
            var ob = cubeProgrammerApi.InitOptionBytesInterface();
            if (ob == null)
            {
                cubeProgrammerApi.Disconnect();
                return 0;
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

            /* Process successfully Done */
            cubeProgrammerApi.Disconnect();

            return 1;
        }
    }
}
