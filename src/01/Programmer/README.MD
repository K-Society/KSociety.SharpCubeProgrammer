x64 Native Tools Command Propt for VS 2022

dumpbin /EXPORTS yourfile.dll > yourfile.exports
Paste the names of the needed functions from yourfile.exports into a new yourfile.def file. Add a line with the word EXPORTS at the top of this file.
lib /def:yourfile.def /machine:x64 /out:yourfile64.lib


dumpbin /EXPORTS CubeProgrammer_API.dll > CubeProgrammer_API.exports
lib /def:CubeProgrammer_API.def /machine:x86 /out:CubeProgrammer_API.lib
lib /def:CubeProgrammer_API.def /machine:x64 /out:CubeProgrammer_API.lib
