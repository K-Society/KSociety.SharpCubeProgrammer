// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubePrgAPI
{
    using System;
    using System.Linq;
    using SharpCubePrgAPI.User;
    using SharpCubeProgrammer.Enum;
    using SharpCubeProgrammer.Interface;
    using SharpCubeProgrammer.Struct;

    internal static class HSMExample
    {
        /* HSM SLOT INDEX to select the plugged-in HSM */
        private const int HSM_SLOT_INDEX = 1;

        /* STLINK INDEX to select the connected STM32 device */
        private const int STLINK_INDEX = 0;

        internal static int Example(ICubeProgrammerApi cubeProgrammerApi)
        {
            DisplayManager.LogMessage(MessageType.Title, "\n+++ HSM Get Information +++\n\n");

            var fwIdentifier = cubeProgrammerApi.GetHsmFirmwareID(HSM_SLOT_INDEX);
            if (String.IsNullOrEmpty(fwIdentifier))
            {
                return -1;
            }

            _ = cubeProgrammerApi.GetHsmCounter(HSM_SLOT_INDEX);

            var hsmState = cubeProgrammerApi.GetHsmState(HSM_SLOT_INDEX);
            if (String.IsNullOrEmpty(hsmState))
            {
                return -1;
            }

            var hsmVersion = cubeProgrammerApi.GetHsmVersion(HSM_SLOT_INDEX);
            if (String.IsNullOrEmpty(hsmVersion))
            {
                return -1;
            }
                
            var hsmType = cubeProgrammerApi.GetHsmType(HSM_SLOT_INDEX);
            if (String.IsNullOrEmpty(hsmType))
            {
                return -1;
            }

            DisplayManager.LogMessage(MessageType.Title, "\n+++ HSM Generate License +++\n\n");

            DebugConnectParameters debugParameters;
            var stLinkList = cubeProgrammerApi.GetStLinkList();

            debugParameters = stLinkList.ElementAt(STLINK_INDEX);
            debugParameters.ConnectionMode = DebugConnectionMode.HotplugMode;
            debugParameters.Shared = 0;

            /* Target connect */
            var connectStlinkFlag = cubeProgrammerApi.ConnectStLink(debugParameters);
            if (connectStlinkFlag != 0)
            {
                DisplayManager.LogMessage(MessageType.Error, "Establishing connection with the device failed\n");
                cubeProgrammerApi.Disconnect();
                return -1;
            }

            /* Display device informations */
            var genInfo = cubeProgrammerApi.GetDeviceGeneralInf();
            if (genInfo != null)
            {
                DisplayManager.LogMessage(MessageType.Normal, $"\nDevice name : {genInfo?.Name} ");
                DisplayManager.LogMessage(MessageType.Normal, $"\nDevice type : {genInfo?.Type} ");
                DisplayManager.LogMessage(MessageType.Normal, $"\nDevice CPU : {genInfo?.Cpu} \n");
            }

            /* Get HSM license and save the output file in the location ..\..\..\..\..\Test\licenseFile.bin */
            var ret = cubeProgrammerApi.GetHsmLicense(HSM_SLOT_INDEX, @"..\..\..\..\..\Test\licenseFile.bin");
            if (ret != 0)
            {
                return -1;
            }

            return 0;
        }
    }
}
