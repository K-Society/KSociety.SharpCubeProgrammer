#ifndef CUBEPROGRAMMER_API_EXTENDED_H
#define CUBEPROGRAMMER_API_EXTENDED_H

#ifdef __cplusplus
extern "C" {
#endif

#include <stdint.h>


#if (defined WIN32 || defined _WIN32 || defined WINCE)
# define CP_EXPORTS __declspec(dllexport)
#else
# define CP_EXPORTS
#endif

    const char* versionAPI();

    void Halt();

    void Run();

    void Step();

#ifdef __cplusplus
}
#endif



#endif // CUBEPROGRAMMER_API_EXTENDED_H
