// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace QuickStart
{
    using System;
    using System.Linq;
    using System.Runtime.InteropServices;
    using SharpCubeProgrammer.Enum;
    using SharpCubeProgrammer.Struct;

    internal static class Program
    {
        private const int WIDH = 50;
        private static uint Progress = 0;
        private static int Left = 0;
        private static int Top = 0;
        internal static VerbosityLevel VerbosityLevel { get; set; } = VerbosityLevel.VerbosityLevel1;
        private static void Main()
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

            cubeProgrammerApi.SetVerbosityLevel(VerbosityLevel.VerbosityLevel1);

            #endregion

            var stLinkList = cubeProgrammerApi.GetStLinkEnumerationList();
            if (stLinkList.Any())
            {
                var stLink = stLinkList.First();
                stLink.ConnectionMode = DebugConnectionMode.UnderResetMode;
                stLink.Shared = 0;

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

                    var sendOptionBytesCmd = cubeProgrammerApi.SendOptionBytesCmd("-ob RDP=170");

                    if (sendOptionBytesCmd.Equals(CubeProgrammerError.CubeprogrammerNoError))
                    {

                    }

                    #region [DownloadFile Test]

                    //var downloadFileResult = cubeProgrammerApi.DownloadFile(
                    //    @"..\..\..\..\..\Test\NUCLEO-F401RE_Demo_V1.0.0.bin", "0x08000000");

                    var downloadFileResult = cubeProgrammerApi.DownloadFile(
                        @"..\..\..\..\..\Test\NUCLEO-F401RE_Demo_V1.0.0.hex");

                    if (downloadFileResult.Equals(CubeProgrammerError.CubeprogrammerNoError))
                    {
                        ;
                    }

                    #endregion

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

            cubeProgrammerApi.Dispose();
            Console.WriteLine("Press a button to exit.");
            Console.ReadLine();
        }
        
        private static void ReceiveMessage(int messageType, string message)
        {
            switch ((MessageType)messageType)
            {
                case MessageType.Normal:
                    Console.ForegroundColor = ConsoleColor.White;
                    break;

                case MessageType.Info:
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;

                case MessageType.GreenInfo:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;

                case MessageType.Title:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;

                case MessageType.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;

                case MessageType.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                case MessageType.Verbosity1:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;

                case MessageType.Verbosity2:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;

                case MessageType.Verbosity3:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;

                case MessageType.GreenInfoNoPopup:
                    //Console.ForegroundColor = ConsoleColor.Green;
                    break;

                case MessageType.WarningNoPopup:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;

                case MessageType.ErrorNoPopup:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;

                default:
                    break;
            }

            if ((MessageType)messageType != MessageType.Verbosity1 && (MessageType)messageType != MessageType.Verbosity2 && (MessageType)messageType != MessageType.Verbosity3 ||
                (MessageType)messageType == MessageType.Verbosity1 && VerbosityLevel >= VerbosityLevel.VerbosityLevel1 ||
                (MessageType)messageType == MessageType.Verbosity2 && VerbosityLevel >= VerbosityLevel.VerbosityLevel2 ||
                (MessageType)messageType == MessageType.Verbosity3 && VerbosityLevel == VerbosityLevel.VerbosityLevel3)
            {
                Console.WriteLine("{0}", message);
            }

            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void InitProgressBar()
        {
            if (VerbosityLevel >= VerbosityLevel.VerbosityLevel1)
            {
                for (var idx = 0; idx < WIDH; idx++)
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.OpenStandardOutput().WriteByte(177);
                }
                (Left, Top) = Console.GetCursorPosition();
                Console.Write($" {Progress}%");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write('\r');
                Progress = 0;
            }
        }

        private static void ProgressBarUpdate(int currentProgress, int total)
        {
            uint alreadyLoaded = 0;
            if (total == 0)
            {
                return;
            }

            if (currentProgress > total)
            {
                currentProgress = total;
            }

            /*Calculuate the ratio of complete-to-incomplete.*/
            var ratio = currentProgress / (float)total;
            var counter = (uint)ratio * WIDH;

            if (counter > alreadyLoaded && VerbosityLevel == VerbosityLevel.VerbosityLevel1)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                for (var Idx = Progress; Idx < counter - alreadyLoaded; Idx++)
                {
                    Console.OpenStandardOutput().WriteByte(219);
                }
            }
            Progress = counter;
            var (leftCur, topCur) = Console.GetCursorPosition();
            Console.SetCursorPosition(Left, Top);
            Console.Write($" {(int)(ratio * 100)}%");
            Console.SetCursorPosition(leftCur, topCur);
        }
    }
}
