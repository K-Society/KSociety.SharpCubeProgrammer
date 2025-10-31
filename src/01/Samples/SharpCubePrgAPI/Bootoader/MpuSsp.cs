// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubePrgAPI.Bootoader
{
    using System.Linq;
    using SharpCubePrgAPI;
    using SharpCubePrgAPI.User;
    using SharpCubeProgrammer.Enum;
    using SharpCubeProgrammer.Interface;

    internal static class MpuSsp
    {
        internal static int Example(ICubeProgrammerApi cubeProgrammerApi)
        {
            DisplayManager.LogMessage(MessageType.Title, "\n+++ MPU SSP +++\n\n");

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
                DisplayManager.LogMessage(MessageType.Error, "Failed to establish connection !\n");
                cubeProgrammerApi.Disconnect();
                return 0;
            }
            else
            {
                DisplayManager.LogMessage(MessageType.GreenInfo, "\n--- Device Connected --- \n");
            }

            /* Display device informations */
            Shared.DisplayDeviceInformations(cubeProgrammerApi);

            /* SSP Input binaries */
            const string sspFilePath = ""; //Indicate the SSP image path here.
            const string tfaSspFilePath = ""; //Indicate the tfa ssp path here.

            /* licenseFile  : is Empty since it is not required when using HSM.
               hsmSlotId = 0: try to change the index value in accordance with the OS settings. */

            var sspFlag = cubeProgrammerApi.ProgramSsp(sspFilePath, "", tfaSspFilePath, 0);
            if (sspFlag != 0)
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
