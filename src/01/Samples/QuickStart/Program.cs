// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace QuickStart
{
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;
    using System.Text.RegularExpressions;
    using SharpCubeProgrammer.Enum;
    using SharpCubeProgrammer.Struct;

    internal class Program
    {
        static void Main(string[] args)
        {
            var cubeProgrammerApi = new SharpCubeProgrammer.CubeProgrammerApi();

            Console.WriteLine("Press a button to start.");
            Console.ReadLine();

            #region [Logging]

            var displayCallBacks = new DisplayCallBacks
            {
                InitProgressBar = InitProgressBar,
                LogMessage = ReceiveMessage,
                LoadBar = ProgressBarUpdate
            };

            cubeProgrammerApi.SetDisplayCallbacks(displayCallBacks);

            cubeProgrammerApi.SetVerbosityLevel(CubeProgrammerVerbosityLevel.CubeprogrammerVerLevelDebug);

            #endregion

            var stLinkList = cubeProgrammerApi.GetStLinkEnumerationList();
            if (stLinkList.Any())
            {
                var stLink = stLinkList.First();
                stLink.ConnectionMode = DebugConnectionMode.UnderResetMode;
                stLink.Shared = 0;

                Console.WriteLine("Speed: {0}", stLink.Speed);
                var connectionResult = cubeProgrammerApi.ConnectStLink(stLink);

                if (connectionResult.Equals(CubeProgrammerError.CubeprogrammerNoError))
                {
                    var generalInfo = cubeProgrammerApi.GetDeviceGeneralInf();
                    if (generalInfo != null)
                    {
                        Console.WriteLine("INFO: \n" +
                                          "Board: {0} \n" +
                                          "Bootloader Version: {1} \n" +
                                          "Cpu: {2} \n" +
                                          "Description: {3} \n" +
                                          "DeviceId: {4} \n" +
                                          "FlashSize: {5} \n" +
                                          "RevisionId: {6} \n" +
                                          "Name: {7} \n" +
                                          "Series: {8} \n" +
                                          "Type: {9}",
                            generalInfo.Value.Board,
                            generalInfo.Value.BootloaderVersion,
                            generalInfo.Value.Cpu,
                            generalInfo.Value.Description,
                            generalInfo.Value.DeviceId,
                            generalInfo.Value.FlashSize,
                            generalInfo.Value.RevisionId,
                            generalInfo.Value.Name,
                            generalInfo.Value.Series,
                            generalInfo.Value.Type);
                    }

                    var storageStructure = cubeProgrammerApi.GetStorageStructure();

                    if (storageStructure.Item1.Equals(CubeProgrammerError.CubeprogrammerNoError))
                    {
                        var storage = storageStructure.Item2;
                        Console.WriteLine("Storage structure: \n" +
                                          "BanksNumber: {0} \n",
                                                storageStructure.Item2.BanksNumber);

                        for (var i = 0; i < storageStructure.Item2.BanksNumber; i++)
                        {
                            var bank = storageStructure.Item2.Banks[i];
                            Console.WriteLine("Bank [{0}] \n" +
                                              "Sector number: {1}", i, bank.SectorsNumber);

                            for (var ii = 0; ii < bank.SectorsNumber; ii++)
                            {
                                var sector = bank.Sectors[ii];
                                Console.WriteLine("Sector [{0}] \n" +
                                                  "Sector address: {1} \n" +
                                                  "Sector index: {2} \n" +
                                                  "Sector size: {3} \n", ii, cubeProgrammerApi.HexConverterToString(sector.Address), sector.Index, sector.Size);
                            }
                        }
                    }

                    var sendOptionBytesCmd = cubeProgrammerApi.SendOptionBytesCmd("-ob RDP=170");

                    if (sendOptionBytesCmd.Equals(CubeProgrammerError.CubeprogrammerNoError))
                    {

                    }

                    #region [DownloadFile Test]

                    var downloadFileResult = cubeProgrammerApi.DownloadFile(
                        @"..\..\..\..\..\Test\NUCLEO-WBA52CG.bin", "0x08000000");

                    if (downloadFileResult.Equals(CubeProgrammerError.CubeprogrammerNoError))
                    {

                    }

                    #endregion

                    var peripheral = cubeProgrammerApi.InitOptionBytesInterface();

                    if (peripheral.HasValue)
                    {
                        Console.WriteLine("PeripheralC: \n" +
                                          "Name: {0} \n" +
                                          "Description: {1} \n" +
                                          "Banks Nbr: {2} \n",
                            peripheral.Value.Name,
                            peripheral.Value.Description,
                            peripheral.Value.BanksNbr);
                    }

                    var targetInterfaceType = cubeProgrammerApi.GetTargetInterfaceType();

                    Console.WriteLine("TargetInterfaceType: {0}", targetInterfaceType);

                    cubeProgrammerApi.Disconnect();
                }
                else
                {
                    cubeProgrammerApi.Disconnect();
                    Console.WriteLine(connectionResult.ToString());
                }
            }
            else
            {
                Console.WriteLine("No ST-Link found!");
            }
            cubeProgrammerApi.DeleteInterfaceList();
            cubeProgrammerApi.Dispose();
            Console.WriteLine("Press a button to exit.");
            Console.ReadLine();
        }

        private static void ReceiveMessage(int messageType, [MarshalAs(UnmanagedType.LPWStr)] string message)
        {
            message = Regex.Replace(message, "(?<!\r)\n", "");
            if (String.IsNullOrEmpty(message))
            {
                return;
            }

            switch ((MessageType)messageType)
            {
                case MessageType.Normal:
                    Console.WriteLine("Message: {0}", message);
                    break;

                case MessageType.Info:
                    Console.WriteLine("Message: {0}", message);
                    break;

                case MessageType.GreenInfo:
                    Console.WriteLine("Message: {0}", message);
                    break;

                case MessageType.Title:
                    Console.WriteLine("Message: {0}", message);
                    break;

                case MessageType.Warning:
                    Console.WriteLine("Message: {0}", message);
                    break;

                case MessageType.Error:
                    Console.WriteLine("Message: {0}", message);
                    break;

                case MessageType.Verbosity1:
                case MessageType.Verbosity2:
                case MessageType.Verbosity3:
                    Console.WriteLine("Message: {0}", message);
                    break;

                case MessageType.GreenInfoNoPopup:
                case MessageType.WarningNoPopup:
                case MessageType.ErrorNoPopup:
                    Console.WriteLine("Message: {0}", message);
                    break;

                default:
                    break;
            }
        }

        private static void InitProgressBar()
        {

        }

        private static void ProgressBarUpdate(int currentProgress, int total)
        {
            if (total > 0)
            {
                //can use current variable to set a progress bar, I have noticed that currentProgress is advancing only by write or download 
                //operation, erase operation does not produce advance
                var current = (currentProgress * 100F) / total;

                Console.WriteLine("ProgressBarUpdate: {0}", current);
            }
        }
    }
}