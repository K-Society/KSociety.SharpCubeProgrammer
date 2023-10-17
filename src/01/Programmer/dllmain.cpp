// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"
//#define BUFSIZE MAX_PATH

extern unsigned int verbosityLevel;
const char* loaderPath = "../../st/Programmer";

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                     )
{
    switch (ul_reason_for_call)
    {
        case DLL_PROCESS_ATTACH:
            /*TCHAR Buffer[BUFSIZE];
            DWORD dwRet;
            dwRet = GetCurrentDirectory(BUFSIZE, Buffer);
            lstrcat(Buffer, L"\\Programmer");
            SetDllDirectory(Buffer);*/
            //logMessage(Normal, "\nDllMain DLL_PROCESS_ATTACH\n");
            /* Set device loaders path that contains FlashLoader and ExternalLoader folders*/
            SetLoadersPath(loaderPath);

            /* Set the progress bar and message display functions callbacks */
            displayCallBacks vsLogMsg;
            vsLogMsg.logMessage = DisplayMessage;
            vsLogMsg.initProgressBar = InitPBar;
            vsLogMsg.loadBar = lBar;
            SetDisplayCallbacks(vsLogMsg); 
            

            /* Set DLL verbosity level */
            SetVerbosityLevel(verbosityLevel = VERBOSITY_LEVEL_0);
            break;

        case DLL_THREAD_ATTACH:
            break;

        case DLL_THREAD_DETACH:
            break;

        case DLL_PROCESS_DETACH:
            DeleteInterfaceList();
            //logMessage(Normal, "\nDLL_PROCESS_DETACH\n");
            break;
    }

    return TRUE;
}

