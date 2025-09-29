#include "pch.h"
#include <stdexcept>

/* -------------------------------------------------------------------------------------------- */
/*                              EXTENDED                                                        */
/* -------------------------------------------------------------------------------------------- */

//const char* VersionAPI()
//{
//    return versionAPI();
//}

void CpuHalt()
{
    Halt();
}

void CpuRun()
{
    Run();
}

void CpuStep()
{
    Step();
}

//char* WindowsVersion()
//{
//    return windows_version();
//}
