// dllmain.cpp : Defines the entry point for the DLL application.
#include "pch.h"

const char* loaderPath = "./st/Programmer";

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
                       )
{
    switch (ul_reason_for_call)
    {
        case DLL_PROCESS_ATTACH:
            /* Set device loaders path that contains FlashLoader and ExternalLoader folders*/
            SetLoadersPath(loaderPath);
            break;

        case DLL_THREAD_ATTACH:
            break;

        case DLL_THREAD_DETACH:
            break;

        case DLL_PROCESS_DETACH:
            break;
    }

    return TRUE;
}

