// ProgrammerConsoleApplication.cpp : This file contains the 'main' function. Program execution begins and ends there.
//

#include <iostream>
#include "../Programmer/Programmer.h"

int main()
{
    std::cout << "Hello World!\n";
    debugConnectParameters* stLinkList;
    debugConnectParameters debugParameters;
    generalInf* genInfo;
    int getStlinkListNb = GetStLinkList(&stLinkList, 0);

    if (getStlinkListNb == 0)
    {
        std::cout << "No STLINK available\n";
        return 0;
    }
    else {
        std::cout << "\n-------- Connected ST-LINK Probes List --------";
        for (int i = 0; i < getStlinkListNb; i++)
        {
            std::cout << "\n   ST-LINK Probe " << i << sizeof(int) << " :\n" << std::endl;
            std::cout << "   ST-LINK SN   : " << stLinkList[i].serialNumber << " \n" << std::endl;
            std::cout << "   ST-LINK FW   : " << stLinkList[i].firmwareVersion << " \n" << std::endl;
        }

        std::cout << "-----------------------------------------------\n\n";
    }

    for (int index = 0; index < getStlinkListNb; index++) {

        //logMessage(Title, "\n--------------------- ");
        //logMessage(Title, "\n ST-LINK Probe : %d ", index);
        //logMessage(Title, "\n--------------------- \n\n");

        debugParameters = stLinkList[index];
        debugParameters.connectionMode = UNDER_RESET_MODE;
        debugParameters.shared = 0;
        //debugParameters.speed = 1;

        /* Target connect */
        int connectStlinkFlag = ConnectStLink(debugParameters);
        if (connectStlinkFlag != 0) {
            std::cout << "Establishing connection with the device failed: " << connectStlinkFlag << " \n" << std::endl;
            Disconnect();
            continue;
        }
        else {
            std::cout << "\n--- Device Connected --- \n" ;
        }

        /* Display device informations */
        genInfo = GetDeviceGeneralInf();
        std::cout << "\nDevice name: " << genInfo->name << std::endl;
        std::cout << "\nDevice type: " << genInfo->type << std::endl;
        std::cout << "\nDevice CPU : " << genInfo->cpu << std::endl;        
    }

    Disconnect();

    return 1;
}

// Run program: Ctrl + F5 or Debug > Start Without Debugging menu
// Debug program: F5 or Debug > Start Debugging menu

// Tips for Getting Started: 
//   1. Use the Solution Explorer window to add/manage files
//   2. Use the Team Explorer window to connect to source control
//   3. Use the Output window to see build output and other messages
//   4. Use the Error List window to view errors
//   5. Go to Project > Add New Item to create new code files, or Project > Add Existing Item to add existing code files to the project
//   6. In the future, to open this project again, go to File > Open > Project and select the .sln file
