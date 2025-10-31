// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubePrgAPI.Bootoader
{
    using SharpCubePrgAPI;
    using SharpCubePrgAPI.User;
    using SharpCubeProgrammer.Enum;
    using SharpCubeProgrammer.Interface;
    using SharpCubeProgrammer.Struct;

    internal static class I2cExample
    {
        private const int STM32L45xxx = 0x4A;
        private const int STM32L42xxx = 0x38;
        private const int STM32L72xxx = 0x49;
        private const int STM32L74xxx = 0x4E;

        internal static int Example(ICubeProgrammerApi cubeProgrammerApi)
        {
            DisplayManager.LogMessage(MessageType.Title, "\n+++ I2C Bootloader Example +++\n\n");

            var i2cParam = new I2cConnectParameters
            {
                add = STM32L45xxx, // Device Address
                br = 400, // Baudrate 400 KHz
                sm = 1, // FAST MODE
                am = 0, // 7 BITS ADDRESS
                af = 1, // ANALOG FILTER ENABLE
                df = 0, // DIGITAL FILTER DISABLE
                dnf = 0x00, // DIGITAL NOISE FILTER 0
                rt = 0, // RISE TIME
                ft = 0 // FALL TIME
            };

            /* Target connect */
            var canConnectFlag = cubeProgrammerApi.ConnectI2CBootloader(i2cParam);
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
