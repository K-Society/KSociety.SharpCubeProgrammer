#pragma once

#include "CubeProgrammer_API_Extended.h"

#ifdef PROGRAMMER_EXPORTS
#define PROGRAMMER_API_EXTENDED __declspec(dllexport)
#else
#define PROGRAMMER_API_EXTENDED __declspec(dllimport)
#endif

/* -------------------------------------------------------------------------------------------- */
/*                              EXTENDED                                                        */
/* -------------------------------------------------------------------------------------------- */

//extern "C" PROGRAMMER_API_EXTENDED const char* VersionAPI();

extern "C" PROGRAMMER_API_EXTENDED void CpuHalt();

extern "C" PROGRAMMER_API_EXTENDED void CpuRun();

extern "C" PROGRAMMER_API_EXTENDED void CpuStep();

//extern "C" PROGRAMMER_API_EXTENDED char* WindowsVersion();
