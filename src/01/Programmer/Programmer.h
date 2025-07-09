#pragma once

#include "CubeProgrammer_API.h"
#include "CubeProgrammer_API_Extended.h"

#ifdef PROGRAMMER_EXPORTS
#define PROGRAMMER_API __declspec(dllexport)
#else
#define PROGRAMMER_API __declspec(dllimport)
#endif

/* -------------------------------------------------------------------------------------------- */
/*                              STLINK functions                                                */
/* -------------------------------------------------------------------------------------------- */

extern "C" PROGRAMMER_API int TryConnectStLink(int stLinkProbeIndex = 0, int shared = 0, debugConnectMode debugConnectMode = UNDER_RESET_MODE);

extern "C" PROGRAMMER_API int GetStLinkList(debugConnectParameters** stLinkList, int shared);

extern "C" PROGRAMMER_API int GetStLinkEnumerationList(debugConnectParameters** stlink_list, int shared);

extern "C" PROGRAMMER_API int ConnectStLink(debugConnectParameters debugParameters);

extern "C" PROGRAMMER_API int Reset(debugResetMode rstMode);

/* -------------------------------------------------------------------------------------------- */
/*                              Bootloader functions                                            */
/* -------------------------------------------------------------------------------------------- */

extern "C" PROGRAMMER_API int GetUsartList(usartConnectParameters** usartList);

extern "C" PROGRAMMER_API int ConnectUsartBootloader(usartConnectParameters usartParameters);

extern "C" PROGRAMMER_API int SendByteUart(int byte);

extern "C" PROGRAMMER_API int GetDfuDeviceList(dfuDeviceInfo** dfuList, int iPID, int iVID);

extern "C" PROGRAMMER_API int ConnectDfuBootloader(char* usbIndex);

extern "C" PROGRAMMER_API int ConnectDfuBootloader2(dfuConnectParameters dfuParameters);

extern "C" PROGRAMMER_API int ConnectSpiBootloader(spiConnectParameters spiParameters);

extern "C" PROGRAMMER_API int ConnectCanBootloader(canConnectParameters canParameters);

extern "C" PROGRAMMER_API int ConnectI2cBootloader(i2cConnectParameters i2cParameters);

/* -------------------------------------------------------------------------------------------- */
/*                              General purposes functions                                      */
/* -------------------------------------------------------------------------------------------- */

extern "C" PROGRAMMER_API void SetDisplayCallbacks(displayCallBacks c);

extern "C" PROGRAMMER_API void SetVerbosityLevel(int level);

extern "C" PROGRAMMER_API bool CheckDeviceConnection();

extern "C" PROGRAMMER_API generalInf* GetDeviceGeneralInf();

extern "C" PROGRAMMER_API int ReadMemory(unsigned int address, unsigned char** data, unsigned int size);

extern "C" PROGRAMMER_API int WriteMemory(unsigned int address, char* data, unsigned int size);

extern "C" PROGRAMMER_API int WriteMemoryAutoFill(unsigned int address, char* data, unsigned int size);

extern "C" PROGRAMMER_API int WriteMemoryAndVerify(unsigned int address, char* data, unsigned int size);

extern "C" PROGRAMMER_API int EditSector(unsigned int address, char* data, unsigned int size);

extern "C" PROGRAMMER_API int DownloadFile(const wchar_t* filePath, unsigned int address, unsigned int skipErase, unsigned int verify, const wchar_t* binPath);

extern "C" PROGRAMMER_API int Execute(unsigned int address);

extern "C" PROGRAMMER_API int MassErase(char* sFlashMemName = nullptr);

extern "C" PROGRAMMER_API int SectorErase(unsigned int sectors[], unsigned int sectorNbr, char* sFlashMemName = nullptr);

extern "C" PROGRAMMER_API int ReadUnprotect();

extern "C" PROGRAMMER_API int TzenRegression();

extern "C" PROGRAMMER_API int GetTargetInterfaceType();

extern "C" PROGRAMMER_API int GetCancelPointer();

extern "C" PROGRAMMER_API void* FileOpen(const wchar_t* filePath);

extern "C" PROGRAMMER_API void FreeFileData(fileData_C* data);

extern "C" PROGRAMMER_API void FreeLibraryMemory(void* ptr);

extern "C" PROGRAMMER_API int Verify(fileData_C* fileData, unsigned int address);

extern "C" PROGRAMMER_API int VerifyMemory(unsigned int address, char* data, unsigned int size);

extern "C" PROGRAMMER_API int VerifyMemoryBySegment(unsigned int address, unsigned char* data, unsigned int size);

extern "C" PROGRAMMER_API int SaveFileToFile(fileData_C* fileData, const wchar_t* sFileName);

extern "C" PROGRAMMER_API int SaveMemoryToFile(int address, int size, const wchar_t* sFileName);

extern "C" PROGRAMMER_API int Disconnect();

extern "C" PROGRAMMER_API void DeleteInterfaceList();

extern "C" PROGRAMMER_API void AutomaticMode(const wchar_t* filePath, unsigned int address, unsigned int skipErase, unsigned int verify, int isMassErase, char* obCommand, int run);

extern "C" PROGRAMMER_API void SerialNumberingAutomaticMode(const wchar_t* filePath, unsigned int address, unsigned int skipErase, unsigned int verify, int isMassErase, char* obCommand, int run, int enableSerialNumbering, int serialAddress, int serialSize, char* serialInitialData);

extern "C" PROGRAMMER_API int GetStorageStructure(storageStructure** deviceStorageStruct);

/* -------------------------------------------------------------------------------------------- */
/*                                  Option Bytes functions                                      */
/* -------------------------------------------------------------------------------------------- */

extern "C" PROGRAMMER_API int SendOptionBytesCmd(char* command);

extern "C" PROGRAMMER_API peripheral_C* InitOptionBytesInterface();

extern "C" PROGRAMMER_API peripheral_C* FastRomInitOptionBytesInterface(uint16_t deviceId);

extern "C" PROGRAMMER_API int ObDisplay();

/* -------------------------------------------------------------------------------------------- */
/*                                  Loaders functions                                           */
/* -------------------------------------------------------------------------------------------- */

extern "C" PROGRAMMER_API void SetLoadersPath(const char* path);

extern "C" PROGRAMMER_API void SetExternalLoaderPath(const char* path, externalLoader** externalLoaderInfo);

extern "C" PROGRAMMER_API void SetExternalLoaderOBL(const char* path, externalLoader** externalLoaderInfo);

extern "C" PROGRAMMER_API int GetExternalLoaders(const char* path, externalStorageInfo** externalStorageNfo);

extern "C" PROGRAMMER_API void RemoveExternalLoader(const char* path);

extern "C" PROGRAMMER_API void DeleteLoaders();

/* -------------------------------------------------------------------------------------------- */
/*                             STM32WB specific functions                                       */
/* -------------------------------------------------------------------------------------------- */

extern "C" PROGRAMMER_API int GetUID64(unsigned char** data);

extern "C" PROGRAMMER_API int FirmwareDelete();

extern "C" PROGRAMMER_API int FirmwareUpgrade(const wchar_t* filePath, unsigned int address, unsigned int firstInstall, unsigned int startStack, unsigned int verify);

extern "C" PROGRAMMER_API int StartWirelessStack();

extern "C" PROGRAMMER_API int UpdateAuthKey(const wchar_t* filePath);

extern "C" PROGRAMMER_API int AuthKeyLock();

extern "C" PROGRAMMER_API int WriteUserKey(const wchar_t* filePath, unsigned char keyType);

extern "C" PROGRAMMER_API int AntiRollBack();

extern "C" PROGRAMMER_API int StartFus();

extern "C" PROGRAMMER_API int UnlockChip();

/* -------------------------------------------------------------------------------------------- */
/*                             STM32MP specific functions                                       */
/* -------------------------------------------------------------------------------------------- */

extern "C" PROGRAMMER_API int ProgramSsp(const wchar_t* sspFile, const wchar_t* licenseFile, const wchar_t* tfaFile, int hsmSlotId);

/* -------------------------------------------------------------------------------------------- */
/*                             STM32 HSM specific functions                                     */
/* -------------------------------------------------------------------------------------------- */

extern "C" PROGRAMMER_API const char* GetHsmFirmwareID(int hsmSlotId);

extern "C" PROGRAMMER_API unsigned long GetHsmCounter(int hsmSlotId);

extern "C" PROGRAMMER_API const char* GetHsmState(int hsmSlotId);

extern "C" PROGRAMMER_API const char* GetHsmVersion(int hsmSlotId);

extern "C" PROGRAMMER_API const char* GetHsmType(int hsmSlotId);

extern "C" PROGRAMMER_API int GetHsmLicense(int hsmSlotId, const wchar_t* outLicensePath);

/* -------------------------------------------------------------------------------------------- */
/*                              EXTENDED                                                        */
/* -------------------------------------------------------------------------------------------- */

//extern "C" PROGRAMMER_API const char* VersionAPI();
extern "C" PROGRAMMER_API void CpuHalt();

extern "C" PROGRAMMER_API void CpuRun();

extern "C" PROGRAMMER_API void CpuStep();
