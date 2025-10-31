// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubePrgAPI.Bootoader
{
    using SharpCubePrgAPI;
    using SharpCubePrgAPI.User;
    using SharpCubeProgrammer.Enum;
    using SharpCubeProgrammer.Interface;
    using SharpCubeProgrammer.Struct;

    internal static class SpiExample
    {
        internal static int Example(ICubeProgrammerApi cubeProgrammerApi)
        {
            DisplayManager.LogMessage(MessageType.Title, "\n+++ SPI Bootloader Example +++\n\n");

            var spiParam = new SpiConnectParameters
            {
                baudrate = 375, // Baudrate 1 MHz
                crcPol = 7,
                direction = 0, // FULL DUPLEX
                cpha = 0,
                cpol = 0,
                crc = 0, // CRC DISABLE
                firstBit = 1, // MSB FIRST
                frameFormat = 0, // MOTOROLA
                dataSize = 1, // 1 BITS
                mode = 1, // MASTER
                nss = 1, // SOFTWARE
                nssPulse = 1,
                delay = 1
            };


            /* Target connect */
            var spiConnectFlag = cubeProgrammerApi.ConnectSpiBootloader(spiParam);
            if (spiConnectFlag != 0)
            {
                DisplayManager.LogMessage(MessageType.Error, "Establishing connection with the device failed");
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
