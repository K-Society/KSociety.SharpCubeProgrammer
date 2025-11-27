// Copyright © K-Society and contributors. All rights reserved. Licensed under the K-Society License. See LICENSE.TXT file in the project root for full license information.

namespace SharpCubeProgrammer.Native
{
    internal sealed partial class ProgrammerInstanceApi
    {
        #region [Delegates]

        #region [STLINK]

        private Functions.GetStLinkList _getStLinkList;

        private Functions.GetStLinkEnumerationList _getStLinkEnumerationList;

        private Functions.ConnectStLink _connectStLink;

        private Functions.Reset _reset;

        #endregion

        #region [Bootloader]

        private Functions.GetUsartList _getUsartList;

        private Functions.ConnectUsartBootloader _connectUsartBootloader;

        private Functions.SendByteUart _sendByteUart;

        private Functions.GetDfuDeviceList _getDfuDeviceList;

        private Functions.ConnectDfuBootloader _connectDfuBootloader;

        private Functions.ConnectDfuBootloader2 _connectDfuBootloader2;

        private Functions.ConnectSpiBootloader _connectSpiBootloader;

        private Functions.ConnectCanBootloader _connectCanBootloader;

        private Functions.ConnectI2cBootloader _connectI2cBootloader;

        #endregion

        #region [General purposes]

        private Functions.SetDisplayCallbacks _setDisplayCallbacks;

        private Functions.SetDisplayCallbacksLinux _setDisplayCallbacksLinux;

        private Functions.SetVerbosityLevel _setVerbosityLevel;

        private Functions.CheckDeviceConnection _checkDeviceConnection;

        private Functions.GetDeviceGeneralInf _getDeviceGeneralInf;

        private Functions.ReadMemory _readMemory;

        private Functions.WriteMemory _writeMemory;

        private Functions.EditSector _editSector;

        private Functions.DownloadFile _downloadFile;

        private Functions.Execute _execute;

        private Functions.MassErase _massErase;

        private Functions.SectorErase _sectorErase;

        private Functions.ReadUnprotect _readUnprotect;

        private Functions.TzenRegression _tzenRegression;

        private Functions.GetTargetInterfaceType _getTargetInterfaceType;

        private Functions.GetCancelPointer _getCancelPointer;

        private Functions.FileOpen _fileOpen;

        private Functions.FreeFileData _freeFileData;

        private Functions.FreeLibraryMemory _freeLibraryMemory;

        private Functions.Verify _verify;

        private Functions.SaveFileToFile _saveFileToFile;

        private Functions.SaveMemoryToFile _saveMemoryToFile;

        private Functions.Disconnect _disconnect;

        private Functions.DeleteInterfaceList _deleteInterfaceList;

        private Functions.AutomaticMode _automaticMode;

        private Functions.SerialNumberingAutomaticMode _serialNumberingAutomaticMode;

        private Functions.GetStorageStructure _getStorageStructure;

        #endregion

        #region [Option Bytes functions]

        private Functions.SendOptionBytesCmd _sendOptionBytesCmd;

        private Functions.InitOptionBytesInterface _initOptionBytesInterface;

        private Functions.FastRomInitOptionBytesInterface _fastRomInitOptionBytesInterface;

        private Functions.ObDisplay _obDisplay;

        #endregion

        #region [Loaders functions]

        private Functions.SetLoadersPath _setLoadersPath;

        private Functions.SetExternalLoaderPath _setExternalLoaderPath;

        private Functions.SetExternalLoaderOBL _setExternalLoaderOBL;

        private Functions.GetExternalLoaders _getExternalLoaders;

        private Functions.RemoveExternalLoader _removeExternalLoader;

        private Functions.DeleteLoaders _deleteLoaders;

        #endregion

        #region [STM32WB specific functions]

        private Functions.GetUID64 _getUID64;

        private Functions.FirmwareDelete _firmwareDelete;

        private Functions.FirmwareUpgrade _firmwareUpgrade;

        private Functions.StartWirelessStack _startWirelessStack;

        private Functions.UpdateAuthKey _updateAuthKey;

        private Functions.AuthKeyLock _authKeyLock;

        private Functions.WriteUserKey _writeUserKey;

        private Functions.AntiRollBack _antiRollBack;

        private Functions.StartFus _startFus;

        private Functions.UnlockChip _unlockChip;

        #endregion

        #region [STM32MP specific functions]

        private Functions.ProgramSsp _programSsp;

        #endregion

        #region [STM32 HSM specific functions]

        private Functions.GetHsmFirmwareID _getHsmFirmwareID;

        private Functions.GetHsmCounter _getHsmCounter;

        private Functions.GetHsmState _getHsmState;

        private Functions.GetHsmVersion _getHsmVersion;

        private Functions.GetHsmType _getHsmType;

        private Functions.GetHsmLicense _getHsmLicense;

        #endregion

        #region [EXTENDED]

        private Functions.CpuHalt _cpuHalt;

        private Functions.CpuRun _cpuRun;

        private Functions.CpuStep _cpuStep;

        #endregion

        #endregion
    }
}
