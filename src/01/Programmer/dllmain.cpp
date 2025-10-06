// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"

using namespace std;

DWORD dwTlsIndex; // address of shared memory

const wchar_t* loaderPath = L"st\\Programmer";

static BOOL APIENTRY InitSTEnvironment(HMODULE hModule)
{
    WCHAR dllName[MAX_PATH + 1];
    DWORD size = 0;

    size = GetModuleFileNameW(hModule, dllName, MAX_PATH);
    if (size > 0)
    {
        // Path of DLL file
        wstring dllNameStr(dllName);
        // Size of DLL File Path minus DLL File Name Size
        size_t pos = dllNameStr.size() - (sizeof(DLLFILENAME) + 7); // DLLFILENAME in Preprocessors Definitions in project properties ---->  DLLFILENAME=R"($(TargetName)$(TargetExt))"
        // Subtract File Name From Path
        dllNameStr = dllNameStr.substr(0, pos);
        dllNameStr += loaderPath;
        replace(dllNameStr.begin(), dllNameStr.end(), '\\', '/');
        const wchar_t* input = dllNameStr.c_str();

        // Count required buffer size (plus one for null-terminator).
        size_t bufferSize = (wcslen(input) + 1) * sizeof(wchar_t);
        char* buffer = new char[bufferSize];

        size_t convertedSize;
        wcstombs_s(&convertedSize, buffer, bufferSize, input, bufferSize);

        // Set device loaders path that contains FlashLoader and ExternalLoader folders.
        SetLoadersPath(buffer);

        //loaderPath = _strdup(buffer);
        delete[] buffer;

        return TRUE;
    }
    return FALSE;
}

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                       )
{
    LPVOID lpvData;
    BOOL fIgnore;
    BOOL result = FALSE;

    switch (ul_reason_for_call)
    {
        case DLL_PROCESS_ATTACH:
            if ((dwTlsIndex = TlsAlloc()) == TLS_OUT_OF_INDEXES)
            {
                return FALSE;
            }

            result = InitSTEnvironment(hModule);
            if (!result)
            {
                return FALSE;
            }
            // No break: Initialize the index for first thread.

        case DLL_THREAD_ATTACH:
            // Initialize the TLS index for this thread.
            lpvData = (LPVOID)LocalAlloc(LPTR, 256);
            if (lpvData != NULL)
            {
                fIgnore = TlsSetValue(dwTlsIndex, lpvData);
            }
            break;

        case DLL_THREAD_DETACH:
            // Release the allocated memory for this thread.
            lpvData = TlsGetValue(dwTlsIndex);
            if (lpvData != NULL)
            {
                LocalFree((HLOCAL)lpvData);
            }
            break;

        case DLL_PROCESS_DETACH:
            // Release the allocated memory for this thread.
            DeleteInterfaceList();
            lpvData = TlsGetValue(dwTlsIndex);
            if (lpvData != NULL)
            {
                LocalFree((HLOCAL)lpvData);
            }
            // Release the TLS index.
            TlsFree(dwTlsIndex);
            break;

        default:
            break;
    }

    return TRUE;
    UNREFERENCED_PARAMETER(hModule);
    UNREFERENCED_PARAMETER(lpReserved);
}
