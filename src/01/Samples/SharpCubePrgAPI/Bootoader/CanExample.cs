// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubePrgAPI.Bootoader
{
    using SharpCubePrgAPI;
    using SharpCubePrgAPI.User;
    using SharpCubeProgrammer.Enum;
    using SharpCubeProgrammer.Interface;
    using SharpCubeProgrammer.Struct;

    internal static class CanExample
    {
        internal static int Example(ICubeProgrammerApi cubeProgrammerApi)
        {

            DisplayManager.LogMessage(MessageType.Title, "\n+++ CAN Bootloader Example +++\n\n");

            var canParam = new CanConnectParameters
            {
                br = 125000, // Baudrate 125KHz
                mode = 0, // NORMAL MODE
                ide = 0,  // STANDARD ID
                rtr = 0,  // DATA FRAME
                fifo = 0, // FIFO0
                fm = 0,   // MASK MODE
                fs = 1,   // 32 BIT
                fe = 1,   // FILTER ENABLE
                fbn = 0,  // FILTER BANK NUMBER 0
            };

            /* Target connect */
            var canConnectFlag = cubeProgrammerApi.ConnectCanBootloader(canParam);
            if (canConnectFlag != 0)
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
