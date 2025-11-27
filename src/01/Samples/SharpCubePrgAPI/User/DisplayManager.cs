// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubePrgAPI.User
{
    using System;
    using System.Runtime.InteropServices;
    using System.Text;
    using SharpCubeProgrammer.Enum;

    internal static class DisplayManager
    {
        private const int WIDH = 50;
        private static uint Progress = 0;
        private static int Left = 0;
        private static int Top = 0;
        internal static VerbosityLevel VerbosityLevel { get; set; } = VerbosityLevel.VerbosityLevel0;

        internal static void LogMessage(MessageType messageType, string message)
        {
            if (String.IsNullOrEmpty(message))
            {
                return;
            }

            switch (messageType)
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

            Console.Write("{0}", message);

            Console.ForegroundColor = ConsoleColor.White;
        }

        internal static void DisplayMessage(int messageType, string message)
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

        internal static void DisplayMessageLinux(int messageType, IntPtr messageIntPtr)
        {
            var pos = 0;
            while (Marshal.ReadInt32(messageIntPtr, pos) != 0)
            {
                pos += 4;
            }

            var strbuf = new byte[pos];
            Marshal.Copy(messageIntPtr, strbuf, 0, pos);
            var message = Encoding.UTF32.GetString(strbuf);
            DisplayMessage(messageType, message);
        }

        internal static void InitProgressBar()
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

        internal static void ProgressBarUpdate(int currentProgress, int total)
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
            var counter = (uint)ratio* WIDH;

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
