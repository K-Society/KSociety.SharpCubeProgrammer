#include "pch.h"
#include <stdexcept>

/* -------------------------------------------------------------------------------------------- */
/*                              STLINK functions                                                */
/* -------------------------------------------------------------------------------------------- */

int TryConnectStLink(int stLinkProbeIndex, int shared, debugConnectMode debugConnectMode)
{
    debugConnectParameters* stLinkList;
    debugConnectParameters debugParameters;

    int getStlinkListNb = getStLinkEnumerationList(&stLinkList, shared);

    if (getStlinkListNb == 0)
    {
        return -99;
    }
    else
    {
        if (stLinkProbeIndex < getStlinkListNb)
        {
            debugParameters = stLinkList[stLinkProbeIndex];
            debugParameters.connectionMode = debugConnectMode;
            debugParameters.shared = shared;

            int connectStlinkFlag = connectStLink(debugParameters);
            if (connectStlinkFlag != 0) {
                disconnect();
                return connectStlinkFlag;
            }
            else
            {
                return 0;
            }
        }
    }

    return -99;
}

int GetStLinkList(debugConnectParameters** stLinkList, int shared) 
{
	try 
	{
		return getStLinkList(stLinkList, shared);
	}
	catch (std::exception& ex)
	{
		ex;
		return -99;
	}
	catch (...) 
	{
		return -99;
	}
}

int GetStLinkEnumerationList(debugConnectParameters** stLinkList, int shared)
{
    try
    {
        return getStLinkEnumerationList(stLinkList, shared);
    }
    catch (std::exception& ex)
    {
        ex;
        return -99;
    }
    catch (...)
    {
        return -99;
    }
}

int ConnectStLink(debugConnectParameters debugParameters) 
{
	return connectStLink(debugParameters);
}

int Reset(debugResetMode rstMode) 
{
	try 
	{
		return reset(rstMode);
	}
	catch (std::exception& ex)
	{
		ex;
		return -99;
	}
	catch (...)
	{
		return -99;
	}
}

/* -------------------------------------------------------------------------------------------- */
/*                              Bootloader functions                                            */
/* -------------------------------------------------------------------------------------------- */

int GetUsartList(usartConnectParameters** usartList)
{
	return getUsartList(usartList);
}

int ConnectUsartBootloader(usartConnectParameters usartParameters)
{
	return connectUsartBootloader(usartParameters);
}

int SendByteUart(int byte)
{
	return sendByteUart(byte);
}

int GetDfuDeviceList(dfuDeviceInfo** dfuList, int iPID, int iVID)
{
	try 
	{
		return getDfuDeviceList(dfuList, iPID, iVID);
	}
	catch (std::exception& ex)
	{
		ex;
		return -99;
	}
	catch (...)
	{
		return -99;
	}
}

int ConnectDfuBootloader(char* usbIndex)
{
	try
	{
		return connectDfuBootloader(usbIndex);
	}
	catch (std::exception& ex)
	{
		ex;
		return -99;
	}
	catch (...)
	{
		return -99;
	}
}

int ConnectDfuBootloader2(dfuConnectParameters dfuParameters)
{
	return connectDfuBootloader2(dfuParameters);
}

int ConnectSpiBootloader(spiConnectParameters spiParameters)
{
	return connectSpiBootloader(spiParameters);
}

int ConnectCanBootloader(canConnectParameters canParameters)
{
	return connectCanBootloader(canParameters);
}

int ConnectI2cBootloader(i2cConnectParameters i2cParameters)
{
	return connectI2cBootloader(i2cParameters);
}


/* -------------------------------------------------------------------------------------------- */
/*                              General purposes functions                                      */
/* -------------------------------------------------------------------------------------------- */

void SetDisplayCallbacks(displayCallBacks c) 
{
	setDisplayCallbacks(c);

	return;
}

void SetVerbosityLevel(int level)
{
	setVerbosityLevel(level);
	return;
}

bool CheckDeviceConnection()
{
	try 
	{
		int result = checkDeviceConnection();
		bool output = false;
		if (result == 1) {
			output = true;
		}
		return output;
	}
	catch (std::exception& ex)
	{
		ex;
		return false;
	}
	catch (...)
	{
		return false;
	}
}

generalInf* GetDeviceGeneralInf()
{
	try
	{
		return getDeviceGeneralInf();
	}
	catch (std::exception& ex)
	{
		ex;
		return NULL;
	}
	catch (...)
	{
		return NULL;
	}
}

int ReadMemory(unsigned int address, unsigned char** data, unsigned int size)
{
	return readMemory(address, data, size);
}

int WriteMemory(unsigned int address, char* data, unsigned int size)
{
	try 
	{
		int result = -99;

		if (size == 0)
		{
			return -99;
		}

        unsigned int remainder = size % 8U;
        if (remainder > 0)
        {
            unsigned int filling = 8U - remainder;
            unsigned int newSize = size + filling;

            char* newData = new char[newSize];

            memcpy(newData, data, size);

            for (unsigned int i = 0; i < filling; i++)
            {
                if ((size + i) < newSize)
                {
                    newData[size + i] = 0xFF;
                }
            }

            result = writeMemory(address, newData, newSize);
        }
        else
        {
            result = writeMemory(address, data, size);
        }
        return result;
	}
	catch (std::exception& ex)
	{
		ex;
		return -99;
	}
	catch (...)
	{
		return -99;
	}
}

int WriteMemoryAndVerify(unsigned int address, char* data, unsigned int size)
{
    try
    {
        int result = -99;

        result = WriteMemory(address, data, size);

        if (result == 0)
        {
            segmentData_C segmentData{};
            segmentData.address = 0;
            segmentData.size = size;
            segmentData.data = reinterpret_cast<unsigned char*>(data);

            fileData_C fileData{};
            fileData.Type = 0;
            fileData.segmentsNbr = 1;
            fileData.segments = &segmentData;

            result = verify(&fileData, address);
        } 

        return result;
    }
    catch (std::exception& ex)
    {
        ex;
        return -99;
    }
    catch (...)
    {
        return -99;
    }
}

int EditSector(unsigned int address, char* data, unsigned int size)
{
    try
    {
        int result = -99;

        if (size == 0)
        {
            return -99;
        }

        result = editSector(address, data, size);
        return result;
    }
    catch (std::exception& ex)
    {
        ex;
        return -99;
    }
    catch (...)
    {
        return -99;
    }
}

int DownloadFile(const wchar_t* filePath, unsigned int address, unsigned int skipErase, unsigned int verify, const wchar_t* binPath)
{
	try 
	{
		return downloadFile(filePath, address, skipErase, verify, binPath);
	}
	catch (std::exception& ex)
	{
		ex;
		return -99;
	}
	catch (...)
	{
		return -99;
	}	
}

int Execute(unsigned int address)
{
	return execute(address);
}

int MassErase(char* sFlashMemName)
{
	try
	{
		return massErase(sFlashMemName);
	}
	catch (std::exception& ex)
	{
		ex;
		return -99;
	}
	catch(...)
	{
		return -99;
	}
}

int SectorErase(unsigned int sectors[], unsigned int sectorNbr, char* sFlashMemName)
{
	return sectorErase(sectors, sectorNbr, sFlashMemName);
}

int ReadUnprotect()
{
	return readUnprotect();
}

int TzenRegression()
{
	//return tzenRegression();
    return -99;
}

int GetTargetInterfaceType()
{
    return getTargetInterfaceType();
}

volatile int* GetCancelPointer()
{
	return getCancelPointer();
}

void* FileOpen(const wchar_t* filePath)
{
	return fileOpen(filePath);
}

void FreeFileData(fileData_C* data)
{
	freeFileData(data);
	return;
}

void FreeLibraryMemory(void* ptr)
{
    freeLibraryMemory(ptr);
    return;
}

int Verify(fileData_C* fileData, unsigned int address)
{
	return verify(fileData, address);
}

int VerifyMemory(unsigned int address, char* data, unsigned int size)
{
	int output = -99;
	int compare = -1;

    unsigned char* verifyMemoryDataStruct = 0;

    output = readMemory(address, &verifyMemoryDataStruct, size);

	if (output == 0) 
	{
        compare = memcmp(data, verifyMemoryDataStruct, size);
	}

	if (compare == 0) 
	{
		output = 0;
	}
	return output;
}

int VerifyMemoryBySegment(unsigned int address, unsigned char* data, unsigned int size)
{
    if (size == 0)
    {
        return -99;
    }

    segmentData_C segmentData{};
    segmentData.address = 0;
    segmentData.size = size;
    segmentData.data = data;

    fileData_C fileData{};
    fileData.Type = 0;
    fileData.segmentsNbr = 1;
    fileData.segments = &segmentData;

    return verify(&fileData, address);
}

int SaveFileToFile(fileData_C* fileData, const wchar_t* sFileName)
{
	return saveFileToFile(fileData, sFileName);
}

int SaveMemoryToFile(int address, int size, const wchar_t* sFileName)
{
	return saveMemoryToFile(address, size, sFileName);
}

int Disconnect() 
{
	try
	{
		disconnect();
		return 0;
	}
	catch (std::exception& ex)
	{
		ex;
		return -99;
	}
	catch (...) 
	{
		return -99;
	}
}

void DeleteInterfaceList()
{
	deleteInterfaceList();
	return;
}

void AutomaticMode(const wchar_t* filePath, unsigned int address, unsigned int skipErase, unsigned int verify, int isMassErase, char* obCommand, int run)
{
	automaticMode(filePath, address, skipErase, verify, isMassErase, obCommand, run);
	return;
}

void SerialNumberingAutomaticMode(const wchar_t* filePath, unsigned int address, unsigned int skipErase, unsigned int verify, int isMassErase, char* obCommand, int run, int enableSerialNumbering, int serialAddress, int serialSize, char* serialInitialData)
{
    serialNumberingAutomaticMode(filePath, address, skipErase, verify, isMassErase, obCommand, run, enableSerialNumbering, serialAddress, serialSize, serialInitialData);
    return;
}

int GetStorageStructure(storageStructure** deviceStorageStruct)
{
	return getStorageStructure(deviceStorageStruct);
}

/* -------------------------------------------------------------------------------------------- */
/*                                  Option Bytes functions                                      */
/* -------------------------------------------------------------------------------------------- */

int SendOptionBytesCmd(char* command)
{
	return sendOptionBytesCmd(command);
}

peripheral_C* InitOptionBytesInterface()
{
	return initOptionBytesInterface();
}

peripheral_C* FastRomInitOptionBytesInterface(uint16_t deviceId)
{
    return fastRomInitOptionBytesInterface(deviceId);
}

int ObDisplay()
{
	return obDisplay();
}

/* -------------------------------------------------------------------------------------------- */
/*                                  Loaders functions                                           */
/* -------------------------------------------------------------------------------------------- */

void SetLoadersPath(const char* path)
{
	setLoadersPath(path);
	return;
}

void SetExternalLoaderPath(const char* path, externalLoader** externalLoaderInfo)
{
	setExternalLoaderPath(path, externalLoaderInfo);
	return;
}

void SetExternalLoaderOBL(const char* path, externalLoader** externalLoaderInfo)
{
    setExternalLoaderOBL(path, externalLoaderInfo);
    return;
}

int GetExternalLoaders(const char* path, externalStorageInfo** externalStorageNfo)
{
    try
    {
        return getExternalLoaders(path, externalStorageNfo);
    }
    catch (std::exception& ex)
    {
        ex;
        return 1;
    }
    catch (...)
    {
        return 1;
    }
}

void RemoveExternalLoader(const char* path)
{
	removeExternalLoader(path);
	return;
}

void DeleteLoaders()
{
	deleteLoaders();
	return;
}

/* -------------------------------------------------------------------------------------------- */
/*                             STM32WB specific functions                                       */
/* -------------------------------------------------------------------------------------------- */

int GetUID64(unsigned char** data)
{
	return getUID64(data);
}

int FirmwareDelete()
{
	return firmwareDelete();
}

int FirmwareUpgrade(const wchar_t* filePath, unsigned int address, unsigned int firstInstall, unsigned int startStack, unsigned int verify)
{
	return firmwareUpgrade(filePath, address, firstInstall, startStack, verify);
}

int StartWirelessStack()
{
	return startWirelessStack();
}

int UpdateAuthKey(const wchar_t* filePath)
{
	return updateAuthKey(filePath);
}

int AuthKeyLock()
{
	return authKeyLock();
}

int WriteUserKey(const wchar_t* filePath, unsigned char keyType)
{
	return writeUserKey(filePath, keyType);
}

int AntiRollBack()
{
	return antiRollBack();
}

int StartFus()
{
	return startFus();
}

int UnlockChip()
{
	return unlockchip();
}

/* -------------------------------------------------------------------------------------------- */
/*                             STM32MP specific functions                                       */
/* -------------------------------------------------------------------------------------------- */

int ProgramSsp(const wchar_t* sspFile, const wchar_t* licenseFile, const wchar_t* tfaFile, int hsmSlotId)
{
    return programSsp(sspFile, licenseFile, tfaFile, hsmSlotId);
}

/* -------------------------------------------------------------------------------------------- */
/*                             STM32 HSM specific functions                                     */
/* -------------------------------------------------------------------------------------------- */

const char* GetHsmFirmwareID(int hsmSlotId)
{
    return getHsmFirmwareID(hsmSlotId);
}

unsigned long GetHsmCounter(int hsmSlotId)
{
    return getHsmCounter(hsmSlotId);
}

const char* GetHsmState(int hsmSlotId)
{
    return getHsmState(hsmSlotId);
}

const char* GetHsmVersion(int hsmSlotId)
{
    return getHsmVersion(hsmSlotId);
}

const char* GetHsmType(int hsmSlotId)
{
    return getHsmType(hsmSlotId);
}

int GetHsmLicense(int hsmSlotId, const wchar_t* outLicensePath)
{
    return getHsmLicense(hsmSlotId, outLicensePath);
}
